/// <list type="table">
/// <listheader><term>Animation.cs</term><description>
///     Class representing an animated sprite
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     May 28, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 3, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Merged with dev for @emlowry Sprite refactor
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Animation : Sprite
    {
        // Time since last animation frame
        int m_iElapsedTime;

        // Time until next animation frame
        int m_iFrameTime;

        // Frame Count
        int m_iFrameCount;

        // Frame Index
        int m_iCurrentFrame;

        // repeat animation when finished
        public bool m_bLooping;

        public void Initialize(Texture2D a_tTexture, Vector2 a_v2Pos, int a_iFrameWidth, int a_iFrameHeight,
                               int a_iFrameCount, int a_iFrameTime,
                               Color a_cColor, float a_fScale, bool a_bLooping)
        {
            Tint = a_cColor;
            SliceWidth = a_iFrameWidth;
            SliceHeight = a_iFrameHeight;
            this.m_iFrameCount = a_iFrameCount;
            this.m_iFrameTime = a_iFrameTime;
            HorizontalScale = VerticalScale = a_fScale;

            m_bLooping = a_bLooping;
            Position = a_v2Pos;
            Texture = a_tTexture;
        }

        public void Update(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }

            m_iElapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (m_iElapsedTime > m_iFrameTime)
            {
                // Next Frame
                m_iCurrentFrame++;

                if (m_iCurrentFrame == m_iFrameCount)
                {
                    m_iCurrentFrame = 0;

                    if (!m_bLooping)
                    {
                        Visible = false;
                    }
                }


                // Reset elapsed time
                m_iElapsedTime = 0;

            }


            SliceLeft = m_iCurrentFrame * SliceWidth;

        }
    }
}
