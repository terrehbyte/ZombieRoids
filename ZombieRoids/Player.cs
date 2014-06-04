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
///     Elizabeth Lowry
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
        private TimeSpan m_tsLastShot;
        private TimeSpan m_tsShotDelay = TimeSpan.FromSeconds(0.2);

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
            Left = MathHelper.Clamp(Left, a_oContext.viewport.Left,
                                          a_oContext.viewport.Right - Width);
            Top = MathHelper.Clamp(Top, a_oContext.viewport.Top,
                                        a_oContext.viewport.Bottom - Height);

            if (Alive)
            {
                // Calculate aim rotation
                FaceCursor();
                
                // Fire bullets
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Fire(a_oContext.time);
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
                        HitPoints -= oEnemy.Damage;
                        oEnemy.Alive = false;
                        if (!Alive)
                        {
                            break;
                        }
                    }
                }
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
                m_tsLastShot = a_gtGameTime.TotalGameTime;
                
                // Search for an inactive Bullet
                Bullet bulTemp = null;

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
        /// Draw the player and all bullets shot by the player
        /// </summary>
        /// <param name="a_sbSpriteBatch"></param>
        public override void Draw(SpriteBatch a_sbSpriteBatch)
        {
            if (Alive)
            {
                base.Draw(a_sbSpriteBatch);
            }
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
