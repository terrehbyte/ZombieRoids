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
///     June 14, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Particle System
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
    public class Animation : Sprite
    {
        /// <summary>
        /// Delegate type for functions called to handle animation-related events
        /// </summary>
        /// <param name="a_oOrigin"></param>
        /// <param name="a_oContext"></param>
        public delegate void EventHandler(Animation a_oOrigin, GameState.Context a_oContext);

        /// <summary>
        /// Called when a non-looping animation tries to pass its last frame
        /// </summary>
        public EventHandler OnComplete { get; set; }

        /// <summary>
        /// Called when a looping animation restarts
        /// </summary>
        public EventHandler OnLoop { get; set; }

        /// <summary>
        /// Called every update, so things like gradual tint change can be added
        /// without creating a new class
        /// </summary>
        public EventHandler OnUpdate { get; set; }

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
        /// Number of rows in the sprite sheet
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Number of columns in the sprite sheet
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// If true, the animation repeats once it passes the final frame
        /// </summary>
        public bool Looping { get; set; }

        /// <summary>
        /// Number of completed animation loops
        /// </summary>
        public int CompletedLoops { get; set; }

        /// <summary>
        /// Has this animation, if non-looping, completed?
        /// </summary>
        public bool Complete
        {
            get { return !Looping && CompletedLoops > 0; }
        }

        /// <summary>
        /// Total time this animation has played.
        /// </summary>
        public TimeSpan PlayTime { get; private set; }

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
        /// <param name="a_iColumns">Width in frames of the sprite sheet</param>
        /// <param name="a_iRows">Height in frames of the sprite sheet</param>
        /// <param name="a_iFrameCount">Number of frames</param>
        /// <param name="a_fFPS">Frames per second</param>
        /// <param name="a_cColor">Frame tint color</param>
        /// <param name="a_fScale">Scale of sprite in both dimensions</param>
        /// <param name="a_bLooping">True if animation repeats</param>
        public virtual void Initialize(Texture2D a_tTexture, Vector2 a_v2Pos,
                                       int a_iColumns, int a_iRows,
                                       int a_iFrameCount, float a_fFPS,
                                       Color a_cColor, Vector2 a_v2Scale,
                                       bool a_bLooping)
        {
            Tint = a_cColor;
            Columns = a_iColumns;
            Rows = a_iRows;
            FrameCount = a_iFrameCount;
            FPS = a_fFPS;
            Scale = a_v2Scale;
            Looping = a_bLooping;
            m_iCurrentFrame = 0;
            CompletedLoops = 0;
            PlayTime = TimeSpan.Zero;
            Position = a_v2Pos;
            Texture = a_tTexture;
            SliceWidth = Texture.Width / Columns;
            SliceHeight = Texture.Height / Rows;
            OnComplete = null;
            OnUpdate = null;
            Visible = true;
        }
        public virtual void Initialize(Texture2D a_tTexture, Vector2 a_v2Pos)
        {
            Initialize(a_tTexture, a_v2Pos, 1, 1, 1, 0, Color.White, Vector2.One, true);
        }

        /// <summary>
        /// Add an update handler that hides the animation after it has played
        /// for the given amount of time.
        /// </summary>
        /// <param name="a_tsLifetime">How long the particle should last</param>
        public void ApplyLifetime(TimeSpan a_tsLifetime)
        {
            OnUpdate +=
                (oParticle, oContext) =>
                {
                    if (null != oParticle && oParticle.PlayTime >= a_tsLifetime)
                    {
                        oParticle.Visible = false;
                    }
                };
        }

        /// <summary>
        /// Add an update handler that gives the animation the given ballistic
        /// trajectory.
        /// </summary>
        /// <param name="a_v2Position">Initial position of the particle</param>
        /// <param name="a_v2Velocity">Initial velocity of the particle</param>
        /// <param name="a_v2Acceleration">Acceleration of the particle</param>
        public void FollowBallisticTrajectory(Vector2 a_v2Position,
                                              Vector2 a_v2Velocity,
                                              Vector2 a_v2Acceleration)
        {
            OnUpdate +=
                (oParticle, oContext) =>
                {
                    if (null != oParticle)
                    {
                        float fTime = (float)oParticle.PlayTime.TotalSeconds;
                        oParticle.Position = a_v2Position + (a_v2Velocity * fTime) +
                                             (a_v2Acceleration * fTime * fTime / 2);
                    }
                };
        }

        /// <summary>
        /// Add an animation completion handler that hides the particle when
        /// animation finishes
        /// </summary>
        public void HideWhenComplete()
        {
            OnComplete += (oParticle, oContext) => { oParticle.Visible = false; };
        }

        /// <summary>
        /// Update animation frame based on time since last update
        /// </summary>
        /// <param name="gameTime">Current/elapsed time</param>
        public virtual void Update(GameState.Context a_oContext)
        {
            // If already complete, don't bother
            if (Complete)
            {
                return;
            }

            // Add time since last update to elapsed time
            PlayTime += a_oContext.time.ElapsedGameTime;

            // If there's only one frame, just call the update event handler
            // and return.
            if (1 >= FrameCount)
            {
                if (null != OnUpdate)
                {
                    OnUpdate(this, a_oContext);
                }
                return;
            }

            // Update time since last frame
            m_iElapsedMilliseconds +=
                (int)a_oContext.time.ElapsedGameTime.TotalMilliseconds;
            int iLoopsCompleted = 0;

            if (m_iElapsedMilliseconds > m_iMillisecondsPerFrame)
            {
                // Next Frame
                m_iCurrentFrame += (0 == m_iMillisecondsPerFrame ? 1 :
                    (m_iElapsedMilliseconds / m_iMillisecondsPerFrame));
                iLoopsCompleted = m_iCurrentFrame / FrameCount;

                // If past the last frame,
                if (m_iCurrentFrame >= FrameCount)
                {
                    if (Looping && FrameCount != 0)
                    {
                        // loop if looping
                        CompletedLoops += iLoopsCompleted;
                        m_iCurrentFrame %= FrameCount;
                    }
                    else
                    {
                        // set to last frame if not looping
                        CompletedLoops = 1;
                        m_iCurrentFrame = FrameCount - 1;
                    }
                }

                // Reset elapsed time
                if (0 != m_iMillisecondsPerFrame)
                {
                    m_iElapsedMilliseconds %= m_iMillisecondsPerFrame;
                }

            }

            // Update texture slice to current frame
            SliceLeft = (m_iCurrentFrame % Columns) * SliceWidth;
            SliceTop = (m_iCurrentFrame / Columns) * SliceHeight;

            // call update event handler
            if (null != OnUpdate)
            {
                OnUpdate(this, a_oContext);
            }

            if (iLoopsCompleted >= 1)
            {
                // If looping, call loop completion handler
                if (Looping && null != OnLoop)
                {
                    // Call once for every loop completed this update.
                    for(int i = 0; i < iLoopsCompleted; ++i)
                    {
                        OnLoop(this, a_oContext);
                    }
                }
                // Otherwise, call animation completion event handler
                else if (!Looping && null != OnComplete)
                {
                    OnComplete(this, a_oContext);
                }
            }
        }
    }
}
