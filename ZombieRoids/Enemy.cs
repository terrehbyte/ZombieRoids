using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Enemy
    {
        public Animation m_aniEnemyAnimation;

        public Vector2 m_v2Pos;
        public bool m_bActive;

        public int m_iHealth;
        public int m_iDamage;
        public int m_iValue;
        public int m_iWidth
        {
            get
            {
                return m_aniEnemyAnimation.m_iFrameWidth;
            }
        }

        public int m_iHeight
        {
            get
            {
                return m_aniEnemyAnimation.m_iFrameHeight;
            }
        }

        float m_fEnemyMoveSpeed;

        public void Initialize(Animation a_aniAnim, Vector2 a_v2Pos)
        {
            m_aniEnemyAnimation = a_aniAnim;

            m_v2Pos = a_v2Pos;

            m_bActive = true;

            m_iHealth = 10;
            m_iDamage = 10;
            m_fEnemyMoveSpeed = 6f;
            m_iValue = 100;

        }

        public void Update(GameTime gameTime)
        {
            m_v2Pos.X -= m_fEnemyMoveSpeed;

            m_aniEnemyAnimation.m_v2Pos = m_v2Pos;

            m_aniEnemyAnimation.Update(gameTime);

            if (m_v2Pos.X < -m_iWidth || m_iHealth <= 0)
            {
                m_bActive = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            m_aniEnemyAnimation.Draw(spriteBatch);

        }
    }
}
