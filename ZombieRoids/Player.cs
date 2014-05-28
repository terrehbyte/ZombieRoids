using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Player
    {
        public Animation m_aniPlayerAnimation;
        public Texture2D m_tPlayerTex;
        public Vector2 m_v2Pos;
        public bool m_bActive;
        public int m_iHealth;
        public int m_iWidth
        {
            get
            {
                return m_aniPlayerAnimation.m_iFrameWidth;
            }
        }
        public int iHeight
        {
            get
            {
                return m_aniPlayerAnimation.m_iFrameHeight;
            }
        }

        public void Initialize(Animation a_animation, Vector2 a_v2Pos)
        {
            m_aniPlayerAnimation = a_animation;
            m_v2Pos = a_v2Pos;
            m_bActive = true;
            m_iHealth = 100;
        }

        public void Update(GameTime gameTime)
        {
            m_aniPlayerAnimation.m_v2Pos = m_v2Pos;
            m_aniPlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(tPlayerTex, v2Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            m_aniPlayerAnimation.Draw(spriteBatch);
        }
    }
}
