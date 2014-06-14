/// <list type="table">
/// <listheader><term>ParticleAnimationExtensions.cs</term><description>
///     Provides extension methods for particle animation objects
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

namespace ZombieRoids
{
    namespace ParticleAnimationExtensions
    {
        /// <remarks>
        /// Provides extension methods for Animation objectss managed by
        /// ParticleEmitter objects
        /// </remarks>
        public static class ParticleAnimationExtensions
        {
            /// <summary>
            /// Returns the particle emitter, if any, that updates and draws the
            /// given animation
            /// </summary>
            /// <param name="a_oParticle"></param>
            /// <returns></returns>
            public static ParticleEmitter Emitter(Animation a_oParticle)
            {
                return ParticleEmitter.EmitterOf(a_oParticle);
            }

            /// <summary>
            /// Is the given animation a particle updated and drawn by a particle
            /// emitter?
            /// </summary>
            /// <param name="a_oParticle"></param>
            /// <returns></returns>
            public static bool IsActiveParticle(this Animation a_oParticle)
            {
                return ParticleEmitter.IsActiveParticle(a_oParticle);
            }

            /// <summary>
            /// Is the given animation a particle in a particle emitter's buffer of
            /// recycled particles?
            /// </summary>
            /// <param name="a_oParticle">The animation to check</param>
            /// <returns></returns>
            public static bool IsRecycledParticle(this Animation a_oParticle)
            {
                return ParticleEmitter.IsRecycledParticle(a_oParticle);
            }

            /// <summary>
            /// Recycle the given animation so it can be reused later by its emitter
            /// </summary>
            /// <param name="a_oParticle">The animation to recycle</param>
            public static void Recycle(this Animation a_oParticle)
            {
                ParticleEmitter.Recycle(a_oParticle);
            }

            /// <summary>
            /// Does the given particle require an explicit Recycle call in order to
            /// be recycled, instead of being recycled automatically when off screen
            /// or invisible?
            /// </summary>
            /// <param name="a_oParticle"></param>
            /// <returns></returns>
            public static bool RequiresExplicitRecycling(this Animation a_oParticle)
            {
                return ParticleEmitter.RequiresExplicitRecycling(a_oParticle);
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
            public static void SetExplicitRecyclingRequired(this Animation a_oParticle,
                                                            bool a_bRequireExplicitRecycling = true)
            {
                ParticleEmitter.SetExplicitRecyclingRequired(a_oParticle,
                                                             a_bRequireExplicitRecycling);
            }
        }
    }
}
