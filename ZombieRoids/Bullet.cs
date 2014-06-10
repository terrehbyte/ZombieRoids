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
///     June 5, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Added constant rotation speed applied every frame
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
        // Time to Cull Bullet
        private TimeSpan m_tsBulletDeathtime;

        // Bullet sound effect
        SoundEffect bulletSound;

        /// <summary>
        /// Constructs a new bullet fired by the given entity
        /// </summary>
        /// <param name="a_oShooter">Entity firing the bullet</param>
        /// <param name="a_tTexture">Bullet image</param>
        /// <param name="a_oContext">Current game context</param>
        public Bullet(Entity a_oShooter, Texture2D a_tTexture, Game1.Context a_oContext)
        {
            Initialize(a_tTexture, a_oShooter.Position);
            if (null != a_oShooter)
            {
                Fire(a_oShooter, a_oContext);
            }
        }

        /// <summary>
        /// Fires the bullet from the given entity at the given speed
        /// </summary>
        /// <param name="a_oShooter">Entity firing the bullet</param>
        /// <param name="a_oContext">Current game context</param>
        public void Fire(Entity a_oShooter, Game1.Context a_oContext)
        {
            Active = true;
            Alive = true;
            Position = a_oShooter.Position;
            Rotation = a_oShooter.Rotation;
            AngularVelocity = GameConsts.BulletSpin;
            Velocity = a_oShooter.Forward * GameConsts.BulletSpeed;
            m_tsBulletDeathtime = a_oContext.time.TotalGameTime + GameConsts.BulletLifetime;
            bulletSound = a_oContent.Load<SoundEffect>(GameConsts.PlayerShootSoundName);
        }

        /// <summary>
        /// Updates bullet position and recycles bullets that leave the screen
        /// </summary>
        /// <param name="a_oContext"></param>
        public override void Update(Game1.Context a_oContext)
        {
            // Only update if active
            if (Active)
            {
                base.Update(a_oContext);

                // Check for collision with enemy
                foreach (Enemy oEnemy in a_oContext.enemies.Where(enemy => enemy.Alive))
                {
                    if (Collision.CheckCollision(this, oEnemy))
                    {
                        a_oContext.score.Value += oEnemy.Value;
                        oEnemy.HitPoints -= GameConsts.BulletDamage;
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
    }
}
