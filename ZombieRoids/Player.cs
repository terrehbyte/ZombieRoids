using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RotatedRectangleCollisions;

namespace ZombieRoids
{
    class Player : Entity
    {
        public RotatedBoxCollider m_rotrctCollider
        {
            get;
            private set;
        }

        private Vector2 m_v2Target;
        public int m_iSpeed = 10;
        private TimeSpan m_tsLastShot;
        private TimeSpan m_tsShotDelay = TimeSpan.FromSeconds(0.1);

        public List<Bullet> m_lbulBulletList = new List<Bullet>();

        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            m_bActive = true;
            m_bAlive = true;
            m_iHealth = 100;
            base.Initialize(a_tTex, a_v2Pos);
        }

        void UpdateCollider()
        {
            Rectangle rctTempRect = new Rectangle((int)m_v2Pos.X,
                                        (int)m_v2Pos.Y,
                                        (int)m_v2Dims.X,
                                        (int)m_v2Dims.X);

            m_rotrctCollider = new RotatedBoxCollider(rctTempRect, m_fRotRads);
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
                m_v2Vel = MoveInput();

                // Calculate new position
                m_v2Pos += m_v2Vel;

                // Calculate aim rotation
                m_fRotRads = AimInput();
                

                // Check for death
                if (m_iHealth <= 0)
                {
                    // Player is dead
                    m_bAlive = false;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    Fire(a_gtGameTime);
                }

                // Update Bullets
                // null references here, terry
                for (int i = 0; i < m_lbulBulletList.Count; i++)
                {
                    m_lbulBulletList[i].Update(a_gtGameTime);
                }   

                //Console.WriteLine("HP" + m_iHealth.ToString());
            }

            UpdateCollider();
        }

        /// <summary>
        /// handle user input for movement
        /// </summary>
        /// <returns>Horizontal and vertical change, respectively</returns>
        Vector2 MoveInput()
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
            Vector2 v2CurPos = new Vector2(m_v2Pos.X, m_v2Pos.Y);
            Vector2 v2Input = new Vector2(mCurState.Position.X, mCurState.Position.Y);

            Vector2 dir = v2Input - v2CurPos;

            fRotVal = (float)Math.Atan2(dir.Y, dir.X);

            m_v2Target = v2Input;

            return fRotVal;
        }

        void Fire(GameTime a_gtGameTime)
        {
            if (a_gtGameTime.TotalGameTime - m_tsLastShot > m_tsShotDelay)
            {
                m_tsLastShot = a_gtGameTime.TotalGameTime;

                Bullet bulTemp = new Bullet(Engine.Instance.Instantiate("Graphics\\Laser", m_v2Pos));
                bulTemp.m_fRotRads = m_fRotRads;
                Vector2 v2BulletVel = m_v2Pos - m_v2Target;
                
                v2BulletVel.Normalize();
                v2BulletVel *= -m_iSpeed;

                bulTemp.m_v2Vel = v2BulletVel;

                m_lbulBulletList.Add(bulTemp);
            }
        }

        public override void Draw(SpriteBatch a_sbSpriteBatch)
        {
            base.Draw(a_sbSpriteBatch);

            // null references here, terry
            for (int i = 0; i < m_lbulBulletList.Count; i++)
            {
                m_lbulBulletList[i].Draw(a_sbSpriteBatch);
            }
        }
    }
}
