#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace ZombieRoids
{
    public partial class MovingObject
    {
        // Function this object should call to draw itself
        public delegate void DrawHandler(MovingObject a_oSelf, GameTime gameTime);
        public DrawHandler OnDraw { get; set; }

        // Function this object should call if there is a collision
        public delegate void CollisionHandler(MovingObject a_oSelf,
                                              MovingObject a_oObstacle,
                                              GameTime gameTime);
        public CollisionHandler OnCollision { get; set; }

        // Function this object should call before updating position
        public delegate void UpdateHandler(MovingObject a_oSelf, GameTime gameTime);
        public UpdateHandler OnUpdate { get; set; }

        // Object that this one is a component of
        public object Owner { get; set; }

        // Factory that manages this object
        protected Factory m_oFactory;

        // Physics properties
        public double Radius { get; set; }
        public Vector2 Position { get; set; }
        protected Vector2 m_v2Velocity;
        public Vector2 Velocity
        {
            get { return m_v2Velocity; }
            set
            {
                if (value != m_v2Velocity)
                {
                    m_v2Velocity = value;
                    if (Vector2.Zero != value)
                    {
                        m_dRotation = Math.Atan2(value.Y, value.X);
                    }
                }
            }
        }
        protected double m_dRotation;
        public double Rotation
        {
            get { return m_dRotation; }
            set
            {
                if (value != m_dRotation)
                {
                    m_dRotation = value;
                    if (Vector2.Zero != m_v2Velocity)
                    {
                        m_v2Velocity =
                            new Vector2(m_v2Velocity.Length() * (float)Math.Cos(value),
                                        m_v2Velocity.Length() * (float)Math.Sin(value));
                    }
                }
            }
        }
        public Vector2 Direction
        {
            get
            {
                if (Vector2.Zero == Velocity)
                {
                    return new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                }
                Vector2 oDirection = Velocity;
                oDirection.Normalize();
                return oDirection;
            }
            set
            {
                if (Vector2.Zero != value)
                {
                    Rotation = Math.Atan2(value.Y, value.X);
                }
            }
        }

        // Rectangular boundary
        public Rectangle Boundary
        {
            get
            {
                return new Rectangle((int)(Position.X - Radius),
                                     (int)(Position.Y - Radius),
                                     (int)(2 * Radius),
                                     (int)(2 * Radius));
            }
        }

        // Is this object in use somewhere?
        public bool InUse
        {
            get { return m_oFactory.InUse(this); }
            set
            {
                if (!value && m_oFactory.InUse(this) != value)
                {
                    Recycle();
                }
            }
        }

        // Is this object still moving and colliding?
        public bool Moving
        {
            get { return m_oFactory.IsMoving(this); }
            set { m_oFactory.SetMovement(this, value); }
        }

        // Is this object still drawn on screen?
        public bool Visible
        {
            get { return m_oFactory.IsVisible(this); }
            set { m_oFactory.SetVisibility(this, value); }
        }

        // Constructor is only called when there are no recycled objects to reuse
        private MovingObject(Factory a_oFactory,
                             object a_oOwner,
                             double a_dRadius,
                             Vector2 a_v2Position,
                             Vector2 a_v2Velocity,
                             DrawHandler a_pDrawFunction,
                             CollisionHandler a_pCollisionFunction,
                             UpdateHandler a_pUpdateFunction)
        {
            m_oFactory = a_oFactory;
            Owner = a_oOwner;
            Radius = a_dRadius;
            Position = a_v2Position;
            Velocity = a_v2Velocity;
            OnDraw = a_pDrawFunction;
            OnCollision = a_pCollisionFunction;
            OnUpdate = a_pUpdateFunction;
        }

        // Call the collision handler, if there is one
        protected void HandleCollision(MovingObject a_oObstacle,
                                       GameTime gameTime)
        {
            if (null != OnCollision)
            {
                OnCollision(this, a_oObstacle, gameTime);
            }
        }

        // Call the draw handler, if there is one
        protected void HandleDraw(GameTime gameTime)
        {
            if (null != OnDraw)
            {
                OnDraw(this, gameTime);
            }
        }

        // Call the update handler, if there is one
        protected void HandleUpdate(GameTime gameTime)
        {
            if (null != OnUpdate)
            {
                OnUpdate(this, gameTime);
            }
        }

        // Recycle an object
        public void Recycle()
        {
            m_oFactory.Recycle(this);
        }

        // Are the given two objects colliding?
        public static bool Colliding(MovingObject a_oObject1,
                                     MovingObject a_oObject2)
        {
            if (null == a_oObject1 || null == a_oObject2 ||
                Object.ReferenceEquals(a_oObject1, a_oObject2))
            {
                return false;
            }
            return (a_oObject1.Radius + a_oObject2.Radius) >
                (a_oObject1.Position - a_oObject2.Position).Length();
        }
    }
}
