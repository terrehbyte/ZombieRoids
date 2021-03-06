﻿/// <list type="table">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    /// <remarks>
    /// Represents a bullet
    /// </remarks>
    public class Bullet : Entity
    {
        // Time to Cull Bullet
        private TimeSpan m_tsLifeRemaining;

        /// <summary>
        /// Constructs a new bullet fired by the given entity
        /// </summary>
        /// <param name="a_oShooter">Entity firing the bullet</param>
        /// <param name="a_tTexture">Bullet image</param>
        /// <param name="a_oContext">Current game context</param>
        public Bullet(Entity a_oShooter, Texture2D a_tTexture, GameState.Context a_oContext)
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
        public void Fire(Entity a_oShooter, GameState.Context a_oContext)
        {
            // Set up bullet
            Active = true;
            Alive = true;
            Position = a_oShooter.Position;
            Rotation = a_oShooter.Rotation;
            AngularVelocity = GameConsts.BulletSpin;
            Velocity = a_oShooter.Forward * GameConsts.BulletSpeed;

            m_tsLifeRemaining = GameConsts.BulletLifetime;

            // Play firing sound
            GameAssets.PlayerShootSound.Play();
        }

        /// <summary>
        /// Updates bullet position and recycles bullets that leave the screen
        /// </summary>
        /// <param name="a_oContext"></param>
        public override void Update(GameState.Context a_oContext)
        {
            // Only update if active
            if (Active)
            {
                base.Update(a_oContext);

                // Check for collision with enemy
                if (a_oContext.state is PlayState)
                {
                    PlayState oState = a_oContext.state as PlayState;
                    foreach (Enemy oEnemy in oState.Enemies.Where(enemy => enemy.Alive))
                    {
                        if (Collision.CheckCollision(this, oEnemy))
                        {
                            oState.Score += oEnemy.Value;
                            oEnemy.HitPoints -= GameConsts.BulletDamage;
                            Active = false;
                            break;
                        }
                    }
                }

                // If passed culling time, then cull/deactivate
                m_tsLifeRemaining -= a_oContext.time.ElapsedGameTime;
                if (TimeSpan.Zero >= m_tsLifeRemaining)
                {
                    Active = false;
                }
            }
        }
    }
}
