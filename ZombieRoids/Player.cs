using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    class Player : Entity
    {
        public int m_iSpeed = 10;



        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            m_bActive = true;
            m_bAlive = true;
            m_iHealth = 100;
            base.Initialize(a_tTex, a_v2Pos);
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
                //m_fRotRads = AimInput();
                

                // Check for death
                if (m_iHealth <= 0)
                {
                    // Player is dead
                    m_bAlive = false;
                }
            }
        }

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

        float AimInput()
        {
            MouseState mCurState = Mouse.GetState();
            float fRotVal;
            Vector2 v2CurPos = new Vector2(m_v2Pos.X, m_v2Pos.Y);
            Vector2 v2Input = new Vector2(mCurState.Position.X, mCurState.Position.Y);

            Vector2 dir = v2Input - v2CurPos;

            fRotVal = (float)Math.Atan2(dir.Y, dir.X);
            
            return fRotVal;
        }
    }
}
