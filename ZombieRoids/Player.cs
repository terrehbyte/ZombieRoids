/// <list type="table">
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
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Game1 class
/// </description></item>
/// </list>

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    /// <remarks>
    /// Represents the player on the screen
    /// </remarks>
    public class Player : Entity
    {
        private const int mc_iSpeed = 150;
        private const int mc_iBulletSpeed = 210;

        // Time of Last Fire
        private TimeSpan m_tsLastShot;

        // Time Delay Between Shots
        private TimeSpan m_tsShotDelay = TimeSpan.FromSeconds(-1.0);

        // Time When Invuln Ends
        private TimeSpan m_tsInvulnEnd;

        // Time Duration of Invuln
        private TimeSpan m_tsInvulnDuration = TimeSpan.FromSeconds(2.0);

        public bool m_bInvuln = false;

        // Number of Lives
        public int m_iLives;

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
                        v2Input.Y -= mc_iSpeed;
                    }

                    // To move down, press S, Down, or 2 (Down) on the number
                    // pad
                    if (kbCurKeys.IsKeyDown(Keys.S) ||
                        kbCurKeys.IsKeyDown(Keys.Down) ||
                        kbCurKeys.IsKeyDown(Keys.NumPad2))
                    {
                        v2Input.Y += mc_iSpeed;
                    }

                    // To move left, press A, Left, or 4 (Left) on the number
                    // pad
                    if (kbCurKeys.IsKeyDown(Keys.A) ||
                        kbCurKeys.IsKeyDown(Keys.Left) ||
                        kbCurKeys.IsKeyDown(Keys.NumPad4))
                    {
                        v2Input.X -= mc_iSpeed;
                    }

                    // To move right, press D, Right, or 6 (Right) on the number
                    // pad
                    if (kbCurKeys.IsKeyDown(Keys.D) ||
                        kbCurKeys.IsKeyDown(Keys.Right) ||
                        kbCurKeys.IsKeyDown(Keys.NumPad6))
                    {
                        v2Input.X += mc_iSpeed;
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
        public override void Initialize(Texture2D a_tTexture, Vector2 a_v2Position)
        {
            base.Initialize(a_tTexture, a_v2Position);
            HitPoints = 100;
            Active = true;
        }

        /// <summary>
        /// Perform Player game logic
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(Game1.Context a_oContext)
        {
            // Update position
            base.Update(a_oContext);

            if (Active)
            {
                // Update Player
                if (Alive)
                {
                    // Calculate aim rotation
                    FaceCursor();

                    // Fire if Left-Click
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Fire(a_oContext.time);
                    }

                    // Check for Invuln End
                    // refactor as property?
                    if (m_bInvuln)
                    {
                        if (!(a_oContext.time.TotalGameTime < m_tsInvulnEnd))
                        {
                            m_bInvuln = false;
                        }
                    }
                    else
                    {
                        // Declare dead if no hp left
                        if (HitPoints <= 0)
                        {
                            Alive = false;
                        }
                    }
                }
                // If the player's dead, check for respawn
                else
                {
                    // Respawn if lives remaining
                    if (m_iLives > 0)
                    {
                        Spawn(a_oContext.time);
                        m_iLives--;
                        Console.WriteLine("Lives Remaining " + m_iLives);
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
            if (Alive)
            {
                foreach (Enemy oEnemy in a_oContext.enemies)
                {
                    if (Collision.CheckCollision(this, oEnemy))
                    {
                        if (!m_bInvuln)
                        {
                            HitPoints -= oEnemy.Damage;
                        }
                        
                        //oEnemy.Alive = false;

                        // Stop if the player isn't alive anymore
                        if (!Alive)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process input for other actions performed by player
        /// </summary>
        private void OtherActions(Game1.Context a_oContext)
        {
            KeyboardState kbCurKeys = Keyboard.GetState();
            if (kbCurKeys.IsKeyDown(Keys.Q))
            {
                Teleport(a_oContext.viewport);
            }
        }

        /// <summary>
        /// Handle user input for aiming
        /// </summary>
        private void FaceCursor()
        {
            // Get the mouse position
            MouseState mCurState = Mouse.GetState();
            Vector2 v2Input = new Vector2(mCurState.Position.X, mCurState.Position.Y);

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
        private void Fire(GameTime a_gtGameTime)
        {
            // If it's been long enough since the last shot,
            if (a_gtGameTime.TotalGameTime - m_tsLastShot > m_tsShotDelay)
            {
                // Record new threshold for firing a bullet
                m_tsLastShot = a_gtGameTime.TotalGameTime;
                
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
                    bulTemp = new Bullet(this, BulletTexture, mc_iBulletSpeed);
                    m_lbulBullets.Add(bulTemp);
                }

                else
                {
                    // Otherwise, fire the preexisting bullet
                    bulTemp.Fire(this, mc_iBulletSpeed);
                }
            }
        }

        /// <summary>
        /// Randomly dumps the player somewhere on-screen
        /// </summary>
        void Teleport(Rectangle a_oDisplayArea)
        {
            Random rngGennie = new Random();

            // Obtain randomized position
            Vector2 v2NewPos = new Vector2(rngGennie.Next(0, (int)new Vector2(a_oDisplayArea.Width, a_oDisplayArea.Height).X),
                                           rngGennie.Next(0, (int)new Vector2(a_oDisplayArea.Width, a_oDisplayArea.Height).Y));

            // Reassign position to randomized location
            Position = v2NewPos;
        }

        /// <summary>
        /// (Re)spawn the player in-place as alive and invulnerable
        /// </summary>
        /// <param name="a_gtGameTime">GameTime</param>
        public void Spawn(GameTime a_gtGameTime)
        {
            // Set time to be invulnerable
            m_tsInvulnEnd = a_gtGameTime.TotalGameTime + m_tsInvulnDuration;

            Alive = true;
            m_bInvuln = true;
        }

        /// <summary>
        /// Draw the player and all bullets shot by the player
        /// </summary>
        /// <param name="a_sbSpriteBatch">SpriteBatch</param>
        public override void Draw(SpriteBatch a_sbSpriteBatch)
        {
            // Only draw if alive
            if (Alive)
            {
                base.Draw(a_sbSpriteBatch);
            }

            // Draw bullets
            for (int i = 0; i < m_lbulBullets.Count; i++)
            {
                if (null != m_lbulBullets)
                {
                    m_lbulBullets[i].Draw(a_sbSpriteBatch);
                }
            }
        }
    }
}
