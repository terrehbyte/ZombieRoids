using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Enemy : Entity
    {
        public Rectangle m_rctCollider;

        public int m_iValue;    // score value
        public int m_iDivisions = 2; // How many will this break into

        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            base.Initialize(a_tTex, a_v2Pos);

            m_bAlive = true;
            m_bActive = true;
            m_iValue = 100;
        }

        public void UpdateCollider()
        {
            m_rctCollider = new Rectangle((int)m_v2Pos.X,
                            (int)m_v2Pos.Y,
                            (int)m_v2Dims.X,
                            (int)m_v2Dims.X);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_bActive)
            {
                m_v2Pos += m_v2Vel;

                // Disable if dead
                if (!m_bAlive)
                {
                    m_bActive = false;
                }

                UpdateCollider();
            }
        }
    }
}
