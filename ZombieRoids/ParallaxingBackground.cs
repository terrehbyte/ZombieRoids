/// <list type="table">
/// <listheader><term>ParallaxingBackground.cs</term><description>
///     A class representing a tiled background that scrolls at a given speed
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     May 28, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Adding comments
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    /// <summary>
    /// Scrolling background
    /// </summary>
    public class ParallaxingBackground
    {
        // Tile texture
        private Texture2D m_tTexture;

        // Tile positions
        private Vector2[] m_av2Positions;

        // Scroll speed
        private int m_iSpeed;

        // Screen area
        private int m_iHeight;
        private int m_iWidth;

        /// <summary>
        /// Sets up the background using a given tile texture, screen size, and
        /// scroll speed
        /// </summary>
        /// <param name="a_cmContent">Content manager for loading texture</param>
        /// <param name="a_sTexturePath">Texture name</param>
        /// <param name="a_iScreenWidth">Screen width</param>
        /// <param name="a_iScreenHeight">Screen Height</param>
        /// <param name="a_iSpeed">Scroll speed</param>
        public void Initialize(Texture2D a_tTexture,
                               int a_iScreenWidth, int a_iScreenHeight, int a_iSpeed)
        {
            // Save screen size
            m_iHeight = a_iScreenHeight;
            m_iWidth = a_iScreenWidth;

            // Load BG Tex
            m_tTexture = a_tTexture;

            // Set Scroll Speed
            m_iSpeed = a_iSpeed;

            // Store positions for as many tiles as needed to cover the entire
            // screen at all points during scrolling
            m_av2Positions =
                new Vector2[Math.Max(a_iScreenWidth / m_tTexture.Width, 1) + 1];
            for (int i = 0; i < m_av2Positions.Length; i++)
            {
                m_av2Positions[i] = new Vector2(i * m_tTexture.Width, 0);
            }
        }

        /// <summary>
        /// Scroll the texture according to elapsed time
        /// </summary>
        /// <param name="gameTime">Current/elapsed time</param>
        public void Update(GameTime gameTime)
        {
            // Update the BG positions
            for (int i = 0; i < m_av2Positions.Length; i++)
            {
                // move position according to speed and elapsed time
                m_av2Positions[i].X +=
                    m_iSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // if moving left, loop tiles past the left edge of the screen
                // back around to the right side
                if (m_iSpeed <= 0)
                {
                    while (m_av2Positions[i].X <= -m_tTexture.Width)
                    {
                        m_av2Positions[i].X +=
                            m_tTexture.Width * m_av2Positions.Length;
                    }
                }

                // if moving right, loop tiles past the right edge of the screen
                // back around to the left side
                else
                {
                    while (m_av2Positions[i].X >=
                           m_tTexture.Width * (m_av2Positions.Length - 1))
                    {
                        m_av2Positions[i].X -=
                            m_tTexture.Width * m_av2Positions.Length;
                    }
                }
            }
        }

        /// <summary>
        /// Draw all the tiles
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < m_av2Positions.Length; i++)
            {
                Rectangle rectBg = new Rectangle((int)m_av2Positions[i].X,
                                                 (int)m_av2Positions[i].Y,
                                                 m_iWidth,
                                                 m_iHeight);

                spriteBatch.Draw(m_tTexture, rectBg, Color.White);
            }
        }

        
    }
}
