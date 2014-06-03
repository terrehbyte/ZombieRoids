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
///     June 3, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Sprite class
/// </description></item>
/// </list>

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RotatedRectangleCollisions;

namespace ZombieRoids
{
    class Player : Entity
    {
        #region Props and Vars
        public RotatedBoxCollider m_rotrctCollider
        {
            get;
            private set;
        }

        private Vector2 m_v2Target;
        public int m_iSpeed = 5;
        public int m_iBulletSpeed = 7;
        private TimeSpan m_tsLastShot;
        private TimeSpan m_tsShotDelay = TimeSpan.FromSeconds(-1.0);
        private TimeSpan m_tsInvulnEnd;
        private TimeSpan m_tsInvulnDuration = TimeSpan.FromSeconds(5.0);

        public bool m_bInvuln = false;

        public int m_iLives;

        public List<Bullet> m_lbulBullets = new List<Bullet>();

        #endregion

        #region Entity Members
        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            m_bActive = true;
            m_bAlive = true;
            base.Initialize(a_tTex, a_v2Pos);
        }

        void UpdateCollider()
        {
            m_rotrctCollider = new RotatedBoxCollider(Boundary, Rotation);
        }

        /// <summary>
        /// Perform Player game logic
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public override void Update(GameTime a_gtGameTime)
        {
            base.Update(a_gtGameTime);

            if (m_bAlive)
            {
                m_v2Vel = MoveInput(a_gtGameTime);

                // Calculate new position
                Position += m_v2Vel;

                // Calculate aim rotation

                Rotation = AimInput();

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Fire(a_gtGameTime);
                }

                if (m_bInvuln)
                {
                    if (a_gtGameTime.TotalGameTime < m_tsInvulnEnd)
                    {
                        Console.WriteLine("INVULN");
                    }
                    else
                    {
                        m_bInvuln = false;
                    }
                }
            }
            else
            {
                if (m_iLives > 0)
                {
                    Spawn(a_gtGameTime);
                }
            }

            for (int i = 0; i < m_lbulBullets.Count; i++)
            {
                m_lbulBullets[i].Update(a_gtGameTime);
            }   

            UpdateCollider();
        }
        public override void Draw(SpriteBatch a_sbSpriteBatch)
        {
            base.Draw(a_sbSpriteBatch);

            // null references here, terry
            for (int i = 0; i < m_lbulBullets.Count; i++)
            {
                m_lbulBullets[i].Draw(a_sbSpriteBatch);
            }
        }
        #endregion

        /// <summary>
        /// handle user input for movement
        /// </summary>
        /// <returns>Horizontal and vertical change, respectively</returns>
        Vector2 MoveInput(GameTime a_gtGameTime)
        {
            // grab Keyboard Input and stuff it into a vector
            KeyboardState kbCurKeys = Keyboard.GetState();
            Vector2 v2Input = new Vector2();

            if (kbCurKeys.IsKeyDown(Keys.W))
            {
                v2Input.Y -= m_iSpeed;
            }
            if (kbCurKeys.IsKeyDown(Keys.S))
            {
                v2Input.Y += m_iSpeed;
            }

            if (kbCurKeys.IsKeyDown(Keys.A))
            {
                v2Input.X -= m_iSpeed;
            }
            if (kbCurKeys.IsKeyDown(Keys.D))
            {
                v2Input.X += m_iSpeed;
            }

            if (kbCurKeys.IsKeyDown(Keys.Q))
            {
                Position = Teleport();
            }

            if (kbCurKeys.IsKeyDown(Keys.E))
            {
                Spawn(a_gtGameTime);
            }

            return v2Input;
        }

        /// <summary>
        /// Handle user input for aiming
        /// </summary>
        /// <returns>Radians to rotate</returns>
        float AimInput()
        {
            MouseState mCurState = Mouse.GetState();
            float fRotVal;
            Vector2 v2Input = new Vector2(mCurState.Position.X, mCurState.Position.Y);

            Vector2 dir = v2Input - Position;

            fRotVal = (Vector2.Zero == dir ? Rotation : (float)Math.Atan2(dir.Y, dir.X));

            m_v2Target = v2Input;

            return fRotVal;
        }

        void Fire(GameTime a_gtGameTime)
        {
            if (a_gtGameTime.TotalGameTime - m_tsLastShot > m_tsShotDelay)
            {
                m_tsLastShot = a_gtGameTime.TotalGameTime;
                
                // Search for Bullet
                Bullet bulTemp = null;

                for (int i = 0; i < m_lbulBullets.Count; i++)
                {
                    if (!m_lbulBullets[i].m_bActive)
                    {
                        bulTemp = m_lbulBullets[i];
                        m_lbulBullets[i].m_bActive = true;
                        m_lbulBullets[i].Position = Position;
                        break;
                    }
                }
                
                // If a bullet couldn't be found
                if (bulTemp == null)
                {
                    bulTemp = new Bullet(Engine.Instance.Instantiate("Graphics\\Laser", Position));
                    m_lbulBullets.Add(bulTemp);
                }

                // Assign Attributes to Bullet
                bulTemp.m_tsBulletDeathTime = a_gtGameTime.TotalGameTime + bulTemp.m_tsBulletLifetime;
                bulTemp.Rotation = Rotation;
                Vector2 v2BulletVel = Position - m_v2Target;
                v2BulletVel.Normalize();
                v2BulletVel *= -m_iBulletSpeed;

                bulTemp.m_v2Vel = v2BulletVel;


            }
        }
        Vector2 Teleport()
        {
            Random rngGennie = new Random();
            // gen two nums
            Vector2 v2NewPos = new Vector2(rngGennie.Next(0, (int)Game1.v2ScreenDims.X),
                                           rngGennie.Next(0, (int)Game1.v2ScreenDims.Y));

            return v2NewPos;
        }

        public void Spawn(GameTime a_gtGameTime)
        {
            // Set time to be invulnerable
            m_tsInvulnEnd = a_gtGameTime.TotalGameTime + m_tsInvulnDuration;
            m_bAlive = true;
            m_bInvuln = true;
        }
    }
}
