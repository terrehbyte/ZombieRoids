using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Enemy : Entity
    {
        public Rectangle m_rctCollider;

        public int m_iDamage;   // damage dealt to other thigns
        public int m_iValue;    // score value

        int m_iSpeed = 10;

        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            base.Initialize(a_tTex, a_v2Pos);

            m_bActive = true;

            m_iHealth = 10;
            m_iDamage = 10;
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
            m_v2Pos.X -= m_iSpeed;

            // TODO: Add recycling
            if (m_v2Pos.X < -m_v2Dims.X || m_iHealth <= 0)
            {
                m_bActive = false;
            }

            UpdateCollider();
        }
    }
}
