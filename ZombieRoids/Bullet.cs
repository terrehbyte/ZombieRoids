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
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 3, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Sprite class
/// </description></item>
/// </list>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RotatedRectangleCollisions;

namespace ZombieRoids
{
    class Bullet : Entity
    {
        public TimeSpan m_tsBulletLifetime = TimeSpan.FromSeconds(1.0);
        public TimeSpan m_tsBulletDeathTime;

        public RotatedBoxCollider m_rotrctCollider
        {
            get;
            private set;
        }

        public Bullet(Entity a_bulSource)
        {
            Initialize(a_bulSource.Texture, a_bulSource.Position);
            m_v2Vel = a_bulSource.m_v2Vel;
            Rotation = a_bulSource.Rotation;
        }

        void UpdateCollider()
        {
            m_rotrctCollider = new RotatedBoxCollider(Boundary, Rotation);
        }

        public override void Initialize(Texture2D a_tTex,Vector2 a_v2Pos)
        {
            m_bActive = true;
            m_bAlive = true;

            base.Initialize(a_tTex, a_v2Pos);
        }

        public override void Update(GameTime a_gtGameTime)
        {
            if (m_bActive)
            {
                base.Update(a_gtGameTime);

                // Calculate new position
                Position += m_v2Vel;

                if (a_gtGameTime.TotalGameTime > m_tsBulletDeathTime)
                {
                    m_bActive = false;
                }

                UpdateCollider();
            }
        }
    }
}
