using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class ParallaxingBackground
    {
        Texture2D m_tTex;
        Vector2[] m_av2Pos;

        int m_iSpeed;

        int m_iBGHeight;
        int m_iBGWidth;

        public void Initialize(ContentManager a_cmContent, String a_sTexturePath, int a_iScreenWidth, int a_iScreenHeight, int a_iSpeed)
        {
            m_iBGHeight = a_iScreenHeight;
            m_iBGWidth = a_iScreenWidth;

            // Load BG Tex
            m_tTex = a_cmContent.Load<Texture2D>(a_sTexturePath);

            // Set Scroll Speed
            m_iSpeed = a_iSpeed;

            m_av2Pos = new Vector2[a_iScreenWidth / m_tTex.Width + 1];

            for (int i = 0; i < m_av2Pos.Length; i++)
            {
                m_av2Pos[i] = new Vector2(i * m_tTex.Width, 0);
            }
        }

        public void Update(GameTime gameTime)
        {
            // Update the BG positions
            for (int i = 0; i < m_av2Pos.Length; i++)
            {
                m_av2Pos[i].X += m_iSpeed;

                // if bg going left

                if (m_iSpeed <= 0)
                {
                    if (m_av2Pos[i].X <= -m_tTex.Width)
                    {
                        m_av2Pos[i].X = m_tTex.Width * (m_av2Pos.Length - 1);
                    }
                }

                // bg going right
                else
                {
                    if (m_av2Pos[i].X >= m_tTex.Width * (m_av2Pos.Length - 1))
                    {
                        m_av2Pos[i].X = -m_tTex.Width;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < m_av2Pos.Length; i++)
            {
                Rectangle rectBg = new Rectangle((int)m_av2Pos[i].X,
                                                 (int)m_av2Pos[i].Y,
                                                 m_iBGWidth,
                                                 m_iBGHeight);

                spriteBatch.Draw(m_tTex, rectBg, Color.White);
            }
        }

        
    }
}
