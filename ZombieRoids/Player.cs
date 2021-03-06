﻿/// <list type="table">
/// <listheader><term>Player.cs</term><description>
///     Class representing the player on the screen
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     May 27, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 14, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Particle System
/// </description></item>
/// </list>

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ZombieRoids
{
    /// <remarks>
    /// Represents the player on the screen
    /// </remarks>
    public class Player : Entity
    {
        // Is the teleport key being pressed?
        private bool m_bTeleportKeyDown = false;

        // Time since Last Fire
        private TimeSpan m_tsTimeSinceLastShot;

        // Time Until Invuln Ends
        private TimeSpan m_tsInvulnTimeRemaining;

        // For dropping smoke bombs when teleporting
        private ParticleEmitter m_oEmitter = new ParticleEmitter();

        /// <summary>
        /// Is the player currently immune to damage?
        /// </summary>
        public bool Invulnerable
        {
            get { return m_bInvulnerable; }
            set
            {
                if (value != m_bInvulnerable)
                {
                    m_bInvulnerable = value;
                    Tint = (value ? GameConsts.PlayerInvulnerableTint
                                  : GameConsts.PlayerNormalTint);
                }
            }
        }
        private bool m_bInvulnerable = false;

        // Number of Lives
        public int Lives { get; set; }

        public List<Bullet> m_lbulBullets = new List<Bullet>();
        public Texture2D BulletTexture { get; set; }       

        /// <summary>
        /// handle user input for movement
        /// </summary>
        public override Vector2 Velocity
        {
            get
            {
                // grab Keyboard Input and stuff it into a vector
                KeyboardState kbCurKeys = Keyboard.GetState();
                Vector2 v2Input = new Vector2();

                // Only move if alive
                if (Alive)
                {
                    // To move up, press W, Up, or 8 (Up) on the number pad
                    if (kbCurKeys.IsKeyDown(Keys.W) ||
                        kbCurKeys.IsKeyDown(Keys.Up) ||
                        kbCurKeys.IsKeyDown(Keys.NumPad8))
                    {
                        v2Input.Y -= GameConsts.PlayerSpeed;
                    }

                    // To move down, press S, Down, or 2 (Down) on the number
                    // pad
                    if (kbCurKeys.IsKeyDown(Keys.S) ||
                        kbCurKeys.IsKeyDown(Keys.Down) ||
                        kbCurKeys.IsKeyDown(Keys.NumPad2))
                    {
                        v2Input.Y += GameConsts.PlayerSpeed;
                    }

                    // To move left, press A, Left, or 4 (Left) on the number
                    // pad
                    if (kbCurKeys.IsKeyDown(Keys.A) ||
                        kbCurKeys.IsKeyDown(Keys.Left) ||
                        kbCurKeys.IsKeyDown(Keys.NumPad4))
                    {
                        v2Input.X -= GameConsts.PlayerSpeed;
                    }

                    // To move right, press D, Right, or 6 (Right) on the number
                    // pad
                    if (kbCurKeys.IsKeyDown(Keys.D) ||
                        kbCurKeys.IsKeyDown(Keys.Right) ||
                        kbCurKeys.IsKeyDown(Keys.NumPad6))
                    {
                        v2Input.X += GameConsts.PlayerSpeed;
                    }
                }

                // Return calculated movement
                return v2Input;
            }
        }

        /// <summary>
        /// Sets up the player entity with 100 hitpoints
        /// </summary>
        /// <param name="a_tTexture">Player image</param>
        /// <param name="a_v2Position">Player starting position</param>
        public override void Initialize(Texture2D a_tTexture, Vector2 a_v2Position,
                                       int a_iColumns, int a_iRows,
                                       int a_iFrameCount, float a_fFPS,
                                       Color a_cColor, Vector2 a_v2Scale,
                                       bool a_bLooping)
        {
            base.Initialize(a_tTexture, a_v2Position, a_iColumns, a_iRows,
                            a_iFrameCount, a_fFPS, a_cColor, a_v2Scale,
                            a_bLooping);
            Reset(a_v2Position);
        }

        /// <summary>
        /// Sets up the player at the start of a new game
        /// </summary>
        /// <param name="a_v2Position">Position to put the player in</param>
        public void Reset(Vector2 a_v2Position)
        {
            foreach (Bullet oBullet in m_lbulBullets)
            {
                oBullet.Active = false;
            }
            Position = a_v2Position;
            HitPoints = GameConsts.PlayerHP;
            Lives = GameConsts.PlayerLives;
            Invulnerable = false;
            m_tsInvulnTimeRemaining = TimeSpan.Zero;
            m_tsTimeSinceLastShot = GameConsts.PlayerFireInterval;
            Active = true;
            m_bTeleportKeyDown = false;
            m_oEmitter.RecycleAll();
        }

        /// <summary>
        /// Perform Player game logic
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameState.Context a_oContext)
        {
            // Update position
            base.Update(a_oContext);

            // Update smoke bomb animations
            m_oEmitter.Update(a_oContext);

            if (Active)
            {
                // Update Player
                if (Alive)
                {
                    // Check for teleport
                    OtherActions(a_oContext);

                    // Calculate aim rotation
                    FaceCursor();

                    // Fire if Left-Click
                    m_tsTimeSinceLastShot += a_oContext.time.ElapsedGameTime;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Fire(a_oContext);
                    }

                    // Check for Invuln End
                    // refactor as property?
                    if (Invulnerable)
                    {
                        m_tsInvulnTimeRemaining -= a_oContext.time.ElapsedGameTime;
                        if (TimeSpan.Zero >= m_tsInvulnTimeRemaining)
                        {
                            Invulnerable = false;
                        }
                    }
                }
                // If the player's dead, check for respawn
                else
                {
                    // Respawn if lives remaining
                    if (Lives > 0)
                    {
                        Spawn(a_oContext.time);
                        Lives--;
                        Console.WriteLine("Lives Remaining " + Lives);
                    }
                    // Otherwise deactive player
                    else
                    {
                        Active = false;
                    }
                }
            }
            
            // Update Bullets
            for (int i = 0; i < m_lbulBullets.Count; i++)
            {
                if (null != m_lbulBullets[i])
                {
                    m_lbulBullets[i].Update(a_oContext);
                }   
            }

            // Check for collision with enemies
            if (Alive && a_oContext.state is PlayState)
            {
                foreach (Enemy oEnemy in (a_oContext.state as PlayState).Enemies)
                {
                    if (Collision.CheckCollision(this, oEnemy))
                    {
                        // If player not invulnerable, damage the player
                        if (!Invulnerable)
                        {
                            HitPoints -= oEnemy.Damage;
                        }

                        // Stop if the player isn't alive anymore
                        if (!Alive)
                        {
                            GameAssets.PlayerDeathSound.Play();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process input for other actions performed by player
        /// </summary>
        private void OtherActions(GameState.Context a_oContext)
        {
            // Grab Keyboard State
            KeyboardState kbCurKeys = Keyboard.GetState();

            // If Q, Home, or 7 (Home) in the num pad is pressed, teleport when
            // key is released
            if (kbCurKeys.IsKeyDown(Keys.Q) ||
                kbCurKeys.IsKeyDown(Keys.Home) ||
                kbCurKeys.IsKeyDown(Keys.NumPad7))
            {
                m_bTeleportKeyDown = true;
            }
            else if (m_bTeleportKeyDown)
            {
                m_bTeleportKeyDown = false;
                Teleport(a_oContext);
            }
        }

        /// <summary>
        /// Handle user input for aiming
        /// </summary>
        private void FaceCursor()
        {
            // Get the mouse position
            MouseState mCurState = Mouse.GetState();
            Vector2 v2Input = new Vector2(mCurState.Position.X,
                                          mCurState.Position.Y);

            // Rotate to face the cursor position
            if (v2Input != Position)
            {
                Forward = v2Input - Position;
            }
        }

        /// <summary>
        /// Fire a bullet
        /// </summary>
        /// <param name="a_gtGameTime">Current/elapsed time</param>
        private void Fire(GameState.Context a_oContext)
        {
            // If it's been long enough since the last shot,
            if (m_tsTimeSinceLastShot > GameConsts.PlayerFireInterval)
            {
                // Record new threshold for firing a bullet
                m_tsTimeSinceLastShot = TimeSpan.Zero;
                
                Bullet bulTemp = null;

                // Search for an inactive Bullet
                for (int i = 0; i < m_lbulBullets.Count; i++)
                {
                    if (null != m_lbulBullets[i] && !m_lbulBullets[i].Active)
                    {
                        bulTemp = m_lbulBullets[i];
                        break;
                    }
                }

                if (bulTemp == null)
                {
                    // If a bullet couldn't be found, create a new one
                    bulTemp = new Bullet(this, BulletTexture, a_oContext);
                    m_lbulBullets.Add(bulTemp);
                }
                else
                {
                    // Otherwise, fire the preexisting bullet
                    bulTemp.Fire(this, a_oContext);
                    // Play bullet sound

                }
            }
        }

        /// <summary>
        /// Randomly dumps the player somewhere on-screen
        /// </summary>
        /// <param name="a_oContext">Current game context</param>
        private void Teleport(GameState.Context a_oContext)
        {
            // Drop smoke bomb at old location
            DropSmokeBomb();

            // Reassign position to randomized location
            Position =
                new Vector2(a_oContext.random.Next(a_oContext.viewport.Left,
                                                   a_oContext.viewport.Right),
                            a_oContext.random.Next(a_oContext.viewport.Top,
                                                   a_oContext.viewport.Bottom));

            // Drop smoke bomb at new location
            DropSmokeBomb();

            // Play Sound
            GameAssets.PlayerTeleportSound.Play();
        }

        /// <summary>
        /// Drop a smoke bomb at the player's current location
        /// </summary>
        private void DropSmokeBomb()
        {
            Animation oBomb = m_oEmitter.Emit(GameAssets.PlayerTeleportTexture,
                                              Position,
                                              GameConsts.PlayerTeleportColumns,
                                              GameConsts.PlayerTeleportRows,
                                              GameConsts.PlayerTeleportFrames,
                                              GameConsts.PlayerTeleportFPS,
                                              Color.White, Vector2.One, false);
            oBomb.HideWhenComplete();
        }

        /// <summary>
        /// (Re)spawn the player in-place as alive and invulnerable with full health
        /// </summary>
        /// <param name="a_gtGameTime">GameTime</param>
        public void Spawn(GameTime a_gtGameTime)
        {
            // Set time to be invulnerable
            m_tsInvulnTimeRemaining = GameConsts.PlayerInvulnerabilityDuration;
            Invulnerable = true;

            // Reset HP
            HitPoints = GameConsts.PlayerHP;

            // Play sound
            GameAssets.PlayerSpawnSound.Play();
        }

        /// <summary>
        /// Draw the player and all bullets shot by the player
        /// </summary>
        /// <param name="a_sbSpriteBatch">SpriteBatch</param>
        public override void Draw(SpriteBatch a_sbSpriteBatch)
        {
            // Draw smoke bombs
            m_oEmitter.Draw(a_sbSpriteBatch);

            // Draw bullets
            for (int i = 0; i < m_lbulBullets.Count; i++)
            {
                if (null != m_lbulBullets)
                {
                    m_lbulBullets[i].Draw(a_sbSpriteBatch);
                }
            }

            // Only draw if alive
            if (Alive)
            {
                base.Draw(a_sbSpriteBatch);
            }
        }
    }
}
