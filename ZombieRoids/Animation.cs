using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Animation
    {
        // Spritesheet for Animation
        Texture2D m_tSpriteSheet;

        // Strip Scale
        float m_fScale;

        // Time since last animation frame
        int m_iElapsedTime;

        // Time until next animation frame
        int m_iFrameTime;

        // Frame Count
        int m_iFrameCount;

        // Frame Index
        int m_iCurFrame;

        // Frame Color
        Color m_cColor;

        // Source Rect to display
        Rectangle m_rctSourceRect = new Rectangle();
        Rectangle m_rctDestRect = new Rectangle();

        // Frame Dims
        public int m_iFrameWidth;
        public int m_iFrameHeight;

        public bool m_bActive;
        public bool m_bLooping;

        public Vector2 m_v2Pos;

        public void Initialize(Texture2D a_tTexture, Vector2 a_v2Pos, int a_iFrameWidth, int a_iFrameHeight,
                               int a_iFrameCount, int a_iFrameTime,
                               Color a_cColor, float a_fScale, bool a_bLooping)
        {
            this.m_cColor = a_cColor;
            this.m_iFrameWidth = a_iFrameWidth;
            this.m_iFrameHeight = a_iFrameHeight;
            this.m_iFrameCount = a_iFrameCount;
            this.m_iFrameTime = a_iFrameTime;
            this.m_fScale = a_fScale;

            m_bLooping = a_bLooping;
            m_v2Pos = a_v2Pos;
            m_tSpriteSheet = a_tTexture;

            m_bActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!m_bActive)
            {
                return;
            }

            m_iElapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (m_iElapsedTime > m_iFrameTime)
            {
                // Next Frame
                m_iCurFrame++;

                if (m_iCurFrame == m_iFrameCount)
                {
                    m_iCurFrame = 0;

                    if (!m_bLooping)
                    {
                        m_bActive = false;
                    }
                }


                // Reset elapsed time
                m_iElapsedTime = 0;

            }


            m_rctSourceRect = new Rectangle(m_iCurFrame * m_iFrameWidth, 0, m_iFrameWidth, m_iFrameHeight);

            m_rctDestRect = new Rectangle((int)m_v2Pos.X - (int)(m_iFrameWidth * m_fScale) / 2,
                                        (int)m_v2Pos.Y - (int)(m_iFrameHeight * m_fScale) / 2,
                                        (int)(m_iFrameWidth * m_fScale),
                                        (int)(m_iFrameHeight * m_fScale));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (m_bActive)
            {
                spriteBatch.Draw(m_tSpriteSheet, m_rctDestRect, m_rctSourceRect, m_cColor);
            }
        }
    }
}
