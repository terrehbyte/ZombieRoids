using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZombieRoids
{
    namespace Boundaries
    {
        class Rectangle : Boundary
        {
            // Dimensions are always positive
            private Vector2 m_v2Size;
            public Vector2 Size
            {
                get { return m_v2Size; }
                set
                {
                    m_v2Size = new Vector2(Math.Abs(value.X), Math.Abs(value.Y));
                }
            }
            public float Width
            {
                get { return m_v2Size.X; }
                set { m_v2Size.X = Math.Abs(value); }
            }
            public float Height
            {
                get { return m_v2Size.Y; }
                set { m_v2Size.Y = Math.Abs(value); }
            }
            public float AspectRatio
            {
                get
                {
                    return (0 != m_v2Size.Y) ? m_v2Size.X / m_v2Size.Y :
                           (0 != m_v2Size.X) ? float.PositiveInfinity :
                           /* 0/0 is undefined */ float.NaN;
                }
                set
                {
                    if (float.NaN == value)
                    {
                        m_v2Size.X = 0;
                        m_v2Size.Y = 0;
                    }
                    else if (float.PositiveInfinity == value ||
                        float.NegativeInfinity == value)
                    {
                        m_v2Size.Y = 0;
                    }
                    else if (0 == value)
                    {
                        m_v2Size.X = 0;
                    }
                    else if (0 != m_v2Size.Y && 0 != m_v2Size.X)
                    {
                        float ratio = (float)Math.Sqrt((double)Math.Abs(value) / AspectRatio);
                        m_v2Size.X *= ratio;
                        m_v2Size.Y /= ratio;
                    }
                }
            }
            public float Area
            {
                get { return m_v2Size.X * m_v2Size.Y; }
                set
                {
                    if (0 == m_v2Size.X || 0 == m_v2Size.Y)
                    {
                        m_v2Size.X = m_v2Size.Y =
                            (float)Math.Sqrt(Math.Abs((double)value));
                    }
                    else
                    {
                        float ratio = (float)Math.Sqrt(Math.Abs((double)value) /
                                                 (m_v2Size.X * m_v2Size.Y));
                        m_v2Size.X *= ratio;
                        m_v2Size.Y *= ratio;
                    }
                }
            }

            // register collision handlers
            static Rectangle()
            {
                RegisterCollisionDetector(typeof(Rectangle), typeof(Rectangle),
                    ((boundary1, boundary2) =>
                    {
                        if (null == boundary1 || !(boundary1 is Rectangle) ||
                            null == boundary2 || !(boundary2 is Rectangle))
                        {
                            return false;
                        }
                        Rectangle rect1 = boundary1 as Rectangle;
                        Rectangle rect2 = boundary2 as Rectangle;
                        //TODO
                        return false;
                    }));
            }

            // Constructors
            public Rectangle(Vector2 a_v2Size, Vector2 a_v2Center)
            {
                Size = a_v2Size;
                Center = a_v2Center;
            }
            public Rectangle(Vector2 a_v2Size, float a_fX = 0, float a_fY = 0)
                : this(a_v2Size, new Vector2(a_fX, a_fY)) { }
            public Rectangle(float a_fWidth, float a_fHeight, Vector2 a_v2Center)
                : this(new Vector2(a_fWidth, a_fHeight), a_v2Center) { }
            public Rectangle(float a_fWidth, float a_fHeight,
                             float a_fX, float a_fY)
                : this(new Vector2(a_fWidth, a_fHeight),
                       new Vector2(a_fX, a_fY)) { }
            public Rectangle(float a_fSideLength, float a_fX, float a_fY)
                : this(new Vector2(a_fSideLength, a_fSideLength),
                       new Vector2(a_fX, a_fY)) { }
            public Rectangle(float a_fWidth, float a_fHeight)
                : this(new Vector2(a_fWidth, a_fHeight)) { }
            public Rectangle() : this(Vector2.Zero) { }
        }
    }
}
