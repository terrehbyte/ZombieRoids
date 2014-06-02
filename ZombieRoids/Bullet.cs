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
        public RotatedBoxCollider m_rotrctCollider
        {
            get;
            private set;
        }

        public Bullet(Entity a_bulSource)
        {
            Initialize(a_bulSource.m_tTex, a_bulSource.m_v2Pos);
            m_v2Vel = a_bulSource.m_v2Vel;
            m_fRotRads = a_bulSource.m_fRotRads;
        }

        void UpdateCollider()
        {
            Rectangle rctTempRect = new Rectangle((int)m_v2Pos.X,
                                        (int)m_v2Pos.Y,
                                        (int)m_v2Dims.X,
                                        (int)m_v2Dims.X);

            m_rotrctCollider = new RotatedBoxCollider(rctTempRect, m_fRotRads);
        }

        public override void Initialize(Texture2D a_tTex,Vector2 a_v2Pos)
        {
            m_bActive = true;
            m_bAlive = true;



            base.Initialize(a_tTex, a_v2Pos);
        }

        public override void Update(GameTime a_gtGameTime)
        {
            base.Update(a_gtGameTime);

            // Calculate new position
            m_v2Pos += m_v2Vel;

            UpdateCollider();
        }
    }
}
