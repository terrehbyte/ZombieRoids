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
                m_v2Vel = Input();

                // Calculate new position
                m_v2Pos += m_v2Vel;

                // Check for death
                if (m_iHealth <= 0)
                {
                    // Player is dead
                    m_bAlive = false;
                }
            }
        }

        Vector2 Input()
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
    }
}
