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
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 3, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Animation class
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    /// <remarks>
    /// A sprite that animates through multiple frames
    /// </remarks>
    class Animation : Sprite
    {
        /// <summary>
        /// Animation frames per second
        /// </summary>
        public float FPS
        {
            get
            {
                return (0 == m_iMillisecondsPerFrame
                        ? float.PositiveInfinity
                        : 1000f / m_iMillisecondsPerFrame);
            }
            set
            {
                m_iMillisecondsPerFrame =
                    (0 == value ? int.MaxValue
                                : (float.PositiveInfinity == value ||
                                   float.NegativeInfinity == value) ? 0
                                : float.NaN != value ? (int)(1000f / value)
                                : m_iMillisecondsPerFrame);
            }
        }
        private int m_iMillisecondsPerFrame;

        /// <summary>
        /// The number of frames in the animation
        /// </summary>
        public int FrameCount { get; set; }

        /// <summary>
        /// If true, the animation repeats once it passes the final frame
        /// </summary>
        public bool Looping { get; set; }

        // Time since last animation frame
        private int m_iElapsedMilliseconds;

        // Frame Index
        private int m_iCurrentFrame;

        /// <summary>
        /// Set up an animation
        /// </summary>
        /// <param name="a_tTexture">Sprite map containing the frames of the
        ///                          animation in a horizontal strip</param>
        /// <param name="a_v2Pos">Position of the animated sprite</param>
        /// <param name="a_iFrameWidth">Width of animation frames</param>
        /// <param name="a_iFrameHeight">Height of animation frames</param>
        /// <param name="a_iFrameCount">Number of frames</param>
        /// <param name="a_fFPS">Frames per second</param>
        /// <param name="a_cColor">Frame tint color</param>
        /// <param name="a_fScale">Scale of sprite in both dimensions</param>
        /// <param name="a_bLooping">True if animation repeats</param>
        public void Initialize(Texture2D a_tTexture, Vector2 a_v2Pos,
                               int a_iFrameWidth, int a_iFrameHeight,
                               int a_iFrameCount, float a_fFPS,
                               Color a_cColor, float a_fScale, bool a_bLooping)
        {
            Tint = a_cColor;
            SliceWidth = a_iFrameWidth;
            SliceHeight = a_iFrameHeight;
            FrameCount = a_iFrameCount;
            FPS = a_fFPS;
            HorizontalScale = VerticalScale = a_fScale;
            Looping = a_bLooping;
            Position = a_v2Pos;
            Texture = a_tTexture;
        }

        /// <summary>
        /// Update animation frame based on time since last update
        /// </summary>
        /// <param name="gameTime">Current/elapsed time</param>
        public void Update(GameTime gameTime)
        {
            // If not visible, don't bother
            if (!Visible)
            {
                return;
            }

            // Add time since last update to elapsed time
            m_iElapsedMilliseconds +=
                (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (m_iElapsedMilliseconds > m_iMillisecondsPerFrame)
            {
                // Next Frame
                m_iCurrentFrame += (0 == m_iMillisecondsPerFrame ? 1 :
                    (m_iElapsedMilliseconds / m_iMillisecondsPerFrame));

                // If past the last frame,
                if (m_iCurrentFrame >= FrameCount)
                {
                    if (Looping && FrameCount != 0)
                    {
                        // loop if looping
                        m_iCurrentFrame %= FrameCount;
                    }
                    else
                    {
                        // hide if not looping
                        m_iCurrentFrame = 0;
                        Visible = false;
                    }
                }

                // Reset elapsed time
                if (0 != m_iMillisecondsPerFrame)
                {
                    m_iElapsedMilliseconds %= m_iMillisecondsPerFrame;
                }

            }

            // Update texture slice to current frame
            SliceLeft = m_iCurrentFrame * SliceWidth;
        }
    }
}
