using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZombieRoids
{
    namespace Boundaries
    {
        public class Circle : Boundary
        {
            // Radius is always positive
            private float m_fRadius;
            public float Radius
            {
                get { return m_fRadius; }
                set { m_fRadius = Math.Abs(value); }
            }
            public float Diameter
            {
                get { return m_fRadius * 2; }
                set { m_fRadius = Math.Abs(value / 2); }
            }
            public float Area
            {
                get { return (float)(Math.PI * m_fRadius * m_fRadius); }
                set { m_fRadius = (float)Math.Sqrt(Math.Abs((double)value / Math.PI)); }
            }

            // register collision detection
            static Circle()
            {
                RegisterCollisionDetector(typeof(Circle), typeof(Circle),
                    ((boundary1, boundary2) =>
                    {
                        if (null == boundary1 || !(boundary1 is Circle) ||
                            null == boundary2 || !(boundary2 is Circle))
                        {
                            return false;
                        }
                        Circle circle1 = boundary1 as Circle;
                        Circle circle2 = boundary2 as Circle;
                        double collisionDistance = circle1.Radius + circle2.Radius;
                        return ((circle2.Center - circle1.Center).LengthSquared() <
                                (collisionDistance * collisionDistance));
                    }));
            }

            // constructors
            public Circle(float a_fRadius, Vector2 a_v2Center)
            {
                Radius = Math.Abs(a_fRadius);
                Center = a_v2Center;
            }
            public Circle(float a_fRadius = 0, float a_fX = 0, float a_fY = 0)
                : this(a_fRadius, new Vector2(a_fX, a_fY)) { }
        }
    }
}
