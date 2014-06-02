using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    class Player : Entity
    {
        public bool m_bActive;
        public int m_iHealth;
        public int m_iSpeed = 10;

        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            m_tTex = a_tTex;
            m_v2Pos = a_v2Pos;
            m_bActive = true;
            m_iHealth = 100;
        }

        public override void Update(GameTime gameTime)
        {
            m_v2Vel = Input();

            // Calculate new position
            m_v2Pos += m_v2Vel;
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
