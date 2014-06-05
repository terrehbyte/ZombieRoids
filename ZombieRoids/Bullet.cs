/// <list type="table">
/// <listheader><term>Bullet.cs</term><description>
///     Represents a bullet fired by the player
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 2, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Merged with dev for @emlowry Refactoring Game1 class
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
    /// Represents a bullet
    /// </remarks>
    public class Bullet : Entity
    {
        // Duration of Bullet Life
        TimeSpan m_tsBulletLifetime = TimeSpan.FromSeconds(1.0);

        // Time to Cull Bullet
        TimeSpan m_tsBulletDeathtime;

        /// <summary>
        /// Constructs a new bullet fired by the given entity at the given speed
        /// </summary>
        /// <param name="a_oShooter">Entity firing the bullet</param>
        /// <param name="a_tTexture">Bullet image</param>
        /// <param name="a_fSpeed">Bullet speed</param>
        public Bullet(Entity a_oShooter, Texture2D a_tTexture, float a_fSpeed)
        {
            Initialize(a_tTexture, a_oShooter.Position);
            if (null != a_oShooter)
            {
                Fire(a_oShooter, a_fSpeed);
            }
        }

        /// <summary>
        /// Fires the bullet from the given entity at the given speed
        /// </summary>
        /// <param name="a_oShooter">Entity firing the bullet</param>
        /// <param name="a_fSpeed">Bullet speed</param>
        public void Fire(Entity a_oShooter, float a_fSpeed)
        {
            Active = true;
            Alive = true;
            Position = a_oShooter.Position;
            Rotation = a_oShooter.Rotation;
            Velocity = a_oShooter.Forward * a_fSpeed;
        }



        /// <summary>
        /// Updates bullet position and recycles bullets that leave the screen
        /// </summary>
        /// <param name="a_oContext"></param>
        public override void Update(Game1.Context a_oContext)
        {
            base.Update(a_oContext);

            // Set death time 
            // @terrehbyte: this isn't ideal but I don't want to break anything before
            //              refactoring is complete
            //              - Maybe make Time a global thing? static of Game1?
            if (m_tsBulletDeathtime == TimeSpan.Zero)
            {
                m_tsBulletDeathtime = a_oContext.time.TotalGameTime + m_tsBulletLifetime;
            }



            // If active, check for collision with an an enemy
            if (Active)
            {
                foreach (Enemy oEnemy in a_oContext.enemies)
                {
                    if (Collision.CheckCollision(this, oEnemy))
                    {
                        oEnemy.Alive = false;
                        Active = false;
                        break;
                    }
                }

                // If passed culling time, then cull/deactivate
                if (a_oContext.time.TotalGameTime > m_tsBulletDeathtime)
                {
                    Active = false;
                }
            }
        }

        public override void OnInactive()
        {
            base.OnInactive();

            // Reset deathtime to zero so it can be set again later
            m_tsBulletDeathtime = TimeSpan.Zero;
        }
    }
}
