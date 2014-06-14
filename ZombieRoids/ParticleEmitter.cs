/// <list type="table">
/// <listheader><term>ParticleEmitter.cs</term><description>
///     Class that creates and manages particles
/// </description></listheader>
/// <item><term>Author</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 14, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 14, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Creation
/// </description></item>
/// </list>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    /// <remarks>
    /// Object that emits particles in the form of animation objects that it
    /// draws and updates.
    /// </remarks>
    public class ParticleEmitter
    {
        // keep track of which emitters manage which particles
        private static Dictionary<Animation, ParticleEmitter> sm_oLookup =
            new Dictionary<Animation, ParticleEmitter>();

        // keep track of all particles managed by this emitter
        private HashSet<Animation> m_oActive = new HashSet<Animation>();
        private Stack<Animation> m_oRecycled = new Stack<Animation>();
        private HashSet<Animation> m_oExplicitRecyclingRequired = new HashSet<Animation>();

        /// <summary>
        /// How many particles are drawn and updated by this emitter?
        /// </summary>
        public int ActiveParticles { get { return m_oActive.Count; } }

        /// <summary>
        /// How many particles are in this emitter's buffer of recycled particles?
        /// </summary>
        public int RecycledParticles { get { return m_oRecycled.Count; } }

        /// <summary>
        /// When destroying a particle emitter, be sure to remove all references
        /// to it in the particle system lookup
        /// </summary>
        ~ParticleEmitter()
        {
            RecycleAll();
            while (0 < m_oRecycled.Count)
            {
                sm_oLookup.Remove(m_oRecycled.Pop());
            }
        }

        /// <summary>
        /// Grabs a particle from the Recycled stack (if it has any) or creates
        /// a new one, then adds it to the Active and, if appropriate,
        /// RequiresExplicitRecycling sets.
        /// </summary>
        /// <param name="a_bRequireExplicitRecycling">Does the given particle
        /// require an explicit Recycle call in order to be recycled, instead
        /// of being recycled automatically when off screen or invisible?</param>
        /// <returns></returns>
        protected Animation Emit(bool a_bRequireExplicitRecycling = false)
        {
            // If the recycled stack is empty, recycle a newly-created animation
            if (0 == m_oRecycled.Count)
            {
                ManageAndRecycle(new Animation());
            }

            // Move a particle from the recycled stack to the active set
            Animation oParticle = m_oRecycled.Pop();
            m_oActive.Add(oParticle);

            // If required, add it to the set of particles requiring explicit recycle calls
            if (a_bRequireExplicitRecycling)
            {
                m_oExplicitRecyclingRequired.Add(oParticle);
            }

            // Return the particle
            return oParticle;
        }

        /// <summary>
        /// Emit a stationary single-frame particle of eternal duration
        /// </summary>
        /// <param name="a_tTexture">Particle texture</param>
        /// <param name="a_oPosition">Particle position</param>
        /// <param name="a_bRequireExplicitRecycling">Does the given particle
        /// require an explicit Recycle call in order to be recycled, instead
        /// of being recycled automatically when off screen or invisible?</param>
        /// <returns></returns>
        public virtual Animation Emit(Texture2D a_tTexture,
                                      Vector2 a_v2Position,
                                      bool a_bRequireExplicitRecycling = false)
        {
            Animation oParticle = Emit(a_bRequireExplicitRecycling);
            oParticle.Initialize(a_tTexture, a_v2Position);
            return oParticle;
        }

        /// <summary>
        /// Emit a stationary animated particle of eternal duration
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
        /// <param name="a_bRequireExplicitRecycling">Does the given particle
        /// require an explicit Recycle call in order to be recycled, instead
        /// of being recycled automatically when off screen or invisible?</param>
        /// <returns></returns>
        public virtual Animation Emit(Texture2D a_tTexture,
                                      Vector2 a_v2Position,
                                      int a_iColumns, int a_iRows,
                                      int a_iFrameCount, float a_fFPS,
                                      Color a_cColor, Vector2 a_v2Scale,
                                      bool a_bLooping,
                                      bool a_bRequireExplicitRecycling = false)
        {
            Animation oParticle = Emit(a_bRequireExplicitRecycling);
            oParticle.Initialize(a_tTexture, a_v2Position, a_iColumns, a_iRows,
                                 a_iFrameCount, a_fFPS, a_cColor, a_v2Scale,
                                 a_bLooping);
            return oParticle;
        }

        /// <summary>
        /// Draw all particles managed by this ParticleEmitter using the given
        /// already-begun SpriteBatch object.
        /// </summary>
        /// <param name="a_oSpriteBatch">The SpriteBatch to use for drawing</param>
        public void Draw(SpriteBatch a_oSpriteBatch)
        {
            foreach (Animation oParticle in m_oActive)
            {
                if (null != oParticle)
                {
                    oParticle.Draw(a_oSpriteBatch);
                }
            }
        }

        /// <summary>
        /// Set this ParticleEmitter as the one updating and drawing the given particle
        /// </summary>
        /// <param name="a_oParticle">The particle to add</param>
        public void Manage(Animation a_oParticle)
        {
            // Only manage non-null particles
            if (null == a_oParticle)
            {
                return;
            }

            // If this is already the emitter of the particle, nothing else needs to be done
            ParticleEmitter oPreviousEmitter = EmitterOf(a_oParticle);
            if (Object.ReferenceEquals(this, oPreviousEmitter))
            {
                return;
            }

            // If another particle emitter is currently managing the particle,
            if (null != oPreviousEmitter)
            {
                // preserve the particle's active/recycled state
                if (IsActiveParticle(a_oParticle))
                {
                    m_oActive.Add(a_oParticle);
                    if (RequiresExplicitRecycling(a_oParticle))
                    {
                        m_oExplicitRecyclingRequired.Add(a_oParticle);
                    }
                }
                else if (!m_oRecycled.Contains(a_oParticle))
                {
                    m_oRecycled.Push(a_oParticle);
                }

                // and tell the current emitter of the particle to stop managing it.
                oPreviousEmitter.UnManage(a_oParticle);
            }
            // Otherwise,
            else
            {
                // Determine active/recycled state by visibility
                if (a_oParticle.Visible)
                {
                    m_oActive.Add(a_oParticle);
                }
                else
                {
                    m_oRecycled.Push(a_oParticle);
                }
            }

            // Note this particle emitter as the one managing the particle
            sm_oLookup[a_oParticle] = this;
        }

        /// <summary>
        /// Recycle all particles managed by this emitter
        /// </summary>
        public void RecycleAll()
        {
            foreach(Animation oParticle in m_oActive)
            {
                if (null != oParticle && !m_oRecycled.Contains(oParticle))
                {
                    m_oRecycled.Push(oParticle);
                }
            }
            m_oExplicitRecyclingRequired.Clear();
            m_oActive.Clear();
        }

        /// <summary>
        /// Recycle the given animation so it can be reused later by this emitter
        /// </summary>
        /// <param name="a_oParticle">The particle to recycle</param>
        public void ManageAndRecycle(Animation a_oParticle)
        {
            if (null != a_oParticle)
            {
                Manage(a_oParticle);
                m_oActive.Remove(a_oParticle);
                m_oExplicitRecyclingRequired.Remove(a_oParticle);
                if (!m_oRecycled.Contains(a_oParticle))
                {
                    m_oRecycled.Push(a_oParticle);
                }
            }
        }

        /// <summary>
        /// Remove an animation from the set of particles managed by this
        /// ParticleEmitter
        /// </summary>
        /// <param name="a_oParticle">The particle to remove</param>
        public void UnManage(Animation a_oParticle)
        {
            // Remove from active set
            m_oActive.Remove(a_oParticle);
            m_oExplicitRecyclingRequired.Remove(a_oParticle);

            // Remove from Recycled stack
            if (m_oRecycled.Contains(a_oParticle))
            {

                m_oRecycled =
                    new Stack<Animation>(
                        m_oRecycled.Where(oParticle =>
                            !Object.ReferenceEquals(oParticle, a_oParticle)));
            }

            // Remove from lookup, if this is indeed the manager of the particle
            ParticleEmitter oEmitter = EmitterOf(a_oParticle);
            if (Object.ReferenceEquals(oEmitter, this))
            {
                sm_oLookup.Remove(a_oParticle);
            }
        }

        /// <summary>
        /// Update all particles managed by this ParticleEmitter
        /// </summary>
        /// <param name="a_oContext"></param>
        public void Update(GameState.Context a_oContext)
        {
            // Update active particles
            HashSet<Animation> oCurrentlyActive = new HashSet<Animation>(m_oActive);
            foreach (Animation oParticle in oCurrentlyActive)
            {
                if (null != oParticle)
                {
                    oParticle.Update(a_oContext);
                }
            }

            // Recycle particles that are now invisible or off-screen
            HashSet<Animation> oToRecycle =
                new HashSet<Animation>(
                    m_oActive.Except(m_oExplicitRecyclingRequired)
                             .Where(oParticle =>
                                    (null != oParticle &&
                                     (!oParticle.Visible ||
                                      !a_oContext.viewport.Intersects(oParticle.Boundary)))));
            foreach (Animation oParticle in oToRecycle)
            {
                ManageAndRecycle(oParticle);
            }
        }

        //
        // For use by the particle animation extension methods
        //

        /// <summary>
        /// Returns the particle emitter, if any, that updates and draws the
        /// given animation
        /// </summary>
        /// <param name="a_oParticle"></param>
        /// <returns></returns>
        public static ParticleEmitter EmitterOf(Animation a_oParticle)
        {
            if (null == a_oParticle || !sm_oLookup.ContainsKey(a_oParticle))
            {
                return null;
            }
            return sm_oLookup[a_oParticle];
        }

        /// <summary>
        /// Is the given animation a particle updated and drawn by a particle
        /// emitter?
        /// </summary>
        /// <param name="a_oParticle"></param>
        /// <returns></returns>
        public static bool IsActiveParticle(Animation a_oParticle)
        {
            if (null == a_oParticle)
            {
                return false;
            }
            ParticleEmitter oEmitter = EmitterOf(a_oParticle);
            return (null == oEmitter ? false :
                oEmitter.m_oActive.Contains(a_oParticle));
        }

        /// <summary>
        /// Is the given animation a particle in a particle emitter's buffer of
        /// recycled particles?
        /// </summary>
        /// <param name="a_oParticle">The animation to check</param>
        /// <returns></returns>
        public static bool IsRecycledParticle(Animation a_oParticle)
        {
            if (null == a_oParticle)
            {
                return false;
            }
            ParticleEmitter oEmitter = EmitterOf(a_oParticle);
            return (null == oEmitter ? false :
                oEmitter.m_oRecycled.Contains(a_oParticle));
        }

        /// <summary>
        /// Recycle the given animation so it can be reused later by its emitter
        /// </summary>
        /// <param name="a_oParticle">The animation to recycle</param>
        public static void Recycle(Animation a_oParticle)
        {
            ParticleEmitter oEmitter = EmitterOf(a_oParticle);
            if (null != oEmitter)
            {
                oEmitter.ManageAndRecycle(a_oParticle);
            }
        }

        /// <summary>
        /// Does the given particle require an explicit Recycle call in order to
        /// be recycled, instead of being recycled automatically when off screen
        /// or invisible?
        /// </summary>
        /// <param name="a_oParticle"></param>
        /// <returns></returns>
        public static bool RequiresExplicitRecycling(Animation a_oParticle)
        {
            if (null == a_oParticle)
            {
                return false;
            }
            ParticleEmitter oEmitter = EmitterOf(a_oParticle);
            return (null == oEmitter ? false :
                oEmitter.m_oExplicitRecyclingRequired.Contains(a_oParticle));
        }

        /// <summary>
        /// Set whether or not the given animation, if an active particle,
        /// requires an explicit Recycle call to be recycled instead of being
        /// recycled automatically when invisible or off screen.
        /// </summary>
        /// <param name="a_oParticle">The particle to set the value for</param>
        /// <param name="a_bRequireExplicitRecycling">Does the given particle
        /// require an explicit Recycle call in order to be recycled, instead
        /// of being recycled automatically when off screen or invisible?</param>
        public static void SetExplicitRecyclingRequired(Animation a_oParticle,
                                                        bool a_bRequireExplicitRecycling = true)
        {
            // Don't bother if the animation is null
            if (null != a_oParticle)
            {
                // Don't bother if the animation isn't an active particle
                ParticleEmitter oEmitter = EmitterOf(a_oParticle);
                if (null != oEmitter && !oEmitter.m_oRecycled.Contains(a_oParticle))
                {
                    // Set whether or not to require explicit recycling
                    if (a_bRequireExplicitRecycling)
                    {
                        oEmitter.m_oExplicitRecyclingRequired.Add(a_oParticle);
                    }
                    else
                    {
                        oEmitter.m_oExplicitRecyclingRequired.Remove(a_oParticle);
                    }
                }
            }
        }
    }
}
