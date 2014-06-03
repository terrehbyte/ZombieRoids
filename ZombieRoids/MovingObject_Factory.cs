#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace ZombieRoids
{
    public partial class MovingObject
    {
        // Factory
        public class Factory
        {
            // Track which objects are in use, moving according to velocity,
            // visible on screen, or available for reuse.
            private HashSet<MovingObject> m_oInUse;
            private HashSet<MovingObject> m_oMoving;
            private HashSet<MovingObject> m_oVisible;
            private Stack<MovingObject> m_oRecycled;

            // bounds to stay within
            public Rectangle Boundary { get; set; }

            // Constructor
            public Factory(int a_iStartQuantity = 0)
            {
                m_oInUse = new HashSet<MovingObject>();
                m_oMoving = new HashSet<MovingObject>();
                m_oVisible = new HashSet<MovingObject>();
                m_oRecycled = new Stack<MovingObject>();
                while (a_iStartQuantity > 0)
                {
                    Create().Recycle();
                    --a_iStartQuantity;
                }
            }

            // Create or refurbish a fresh object
            public MovingObject Create(object a_oOwner,
                                       double a_dRadius,
                                       Vector2 a_v2Position,
                                       Vector2 a_v2Velocity,
                                       DrawHandler a_pDrawFunction = null,
                                       CollisionHandler a_pCollisionFunction = null,
                                       UpdateHandler a_pUpdateFunction = null,
                                       bool a_bMoving = true,
                                       bool a_bVisible = true)
            {
                MovingObject oCreated;

                if (0 == m_oRecycled.Count)
                {
                    // If there are no objects available for reuse, create a new one
                    oCreated = new MovingObject(this, a_oOwner, a_dRadius,
                                                a_v2Position, a_v2Velocity,
                                                a_pDrawFunction, a_pCollisionFunction,
                                                a_pUpdateFunction);
                }
                else
                {
                    // Otherwise, reuse an old object
                    oCreated = m_oRecycled.Pop();
                    oCreated.Owner = a_oOwner;
                    oCreated.Radius = a_dRadius;
                    oCreated.Position = a_v2Position;
                    oCreated.Velocity = a_v2Velocity;
                    oCreated.OnDraw = a_pDrawFunction;
                    oCreated.OnCollision = a_pCollisionFunction;
                    oCreated.OnUpdate = a_pUpdateFunction;
                }

                // Add to internal sets
                m_oInUse.Add(oCreated);
                SetMovement(oCreated, a_bMoving);
                SetVisibility(oCreated, a_bVisible);
                return oCreated;
            }
            public MovingObject Create(object a_oOwner,
                                       double a_dRadius,
                                       Vector2 a_v2Position)
            {
                return Create(a_oOwner, a_dRadius, a_v2Position, Vector2.Zero);
            }
            public MovingObject Create(object a_oOwner = null,
                                       double a_dRadius = 0)
            {
                return Create(a_oOwner, a_dRadius, Vector2.Zero, Vector2.Zero);
            }

            // draw all visible objects
            public void DrawVisible(GameTime gameTime)
            {
                foreach (MovingObject oObject in m_oVisible)
                {
                    if (null != oObject)
                    {
                        oObject.HandleDraw(gameTime);
                    }
                }
            }

            // Is the given object one of the objects in use that this factory
            // manages?
            public bool InUse(MovingObject oObject)
            {
                return (null != oObject && this == oObject.m_oFactory &&
                        m_oInUse.Contains(oObject));
            }

            // Is the given object one of the moving objects that this factory
            // manages?
            public bool IsMoving(MovingObject oObject)
            {
                return (null != oObject && this == oObject.m_oFactory &&
                        m_oMoving.Contains(oObject));
            }

            // Is the given object one of the visible objects that this factory
            // manages?
            public bool IsVisible(MovingObject oObject)
            {
                return (null != oObject && this == oObject.m_oFactory &&
                        m_oVisible.Contains(oObject));
            }

            // recycle all objects
            public void Recall()
            {
                HashSet<MovingObject> oToRecycle = new HashSet<MovingObject>(m_oInUse);
                foreach (MovingObject oObject in oToRecycle)
                {
                    Recycle(oObject);
                }
            }

            // Recycle the given object, if managed by this factory
            public void Recycle(MovingObject oObject)
            {
                if (InUse(oObject))
                {
                    SetVisibility(oObject, false);
                    SetMovement(oObject, false);
                    m_oInUse.Remove(oObject);
                    m_oRecycled.Push(oObject);
                }
            }

            // Set whether or not the given object, if managed by this factory,
            // moves according to its velocity
            public void SetMovement(MovingObject oObject, bool a_bMoving)
            {
                if (null != oObject && this == oObject.m_oFactory &&
                    m_oMoving.Contains(oObject) != a_bMoving)
                {
                    if (!a_bMoving)
                    {
                        m_oMoving.Remove(oObject);
                    }
                    else if (InUse(oObject))
                    {
                        m_oMoving.Add(oObject);
                    }
                }
            }

            // Set whether or not the given object, if managed by this factory,
            // is visible
            public void SetVisibility(MovingObject oObject, bool a_bVisible)
            {
                if (null != oObject && this == oObject.m_oFactory &&
                    m_oVisible.Contains(oObject) != a_bVisible)
                {
                    if (!a_bVisible)
                    {
                        m_oVisible.Remove(oObject);
                    }
                    else if (InUse(oObject))
                    {
                        m_oVisible.Add(oObject);
                    }
                }
            }

            // Update all active moving objects
            public void UpdateAll(GameTime gameTime)
            {
                // Call update functions
                HashSet<MovingObject> oAll = new HashSet<MovingObject>(m_oInUse);
                foreach (MovingObject oObject in oAll)
                {
                    oObject.HandleUpdate(gameTime);
                }

                // Update positions
                foreach (MovingObject oObject in m_oMoving)
                {
                    oObject.Position +=
                        new Vector2((float)(oObject.Velocity.X *
                                            gameTime.ElapsedGameTime.TotalSeconds),
                                    (float)(oObject.Velocity.Y *
                                            gameTime.ElapsedGameTime.TotalSeconds));
                    oObject.Position =
                        new Vector2((float)Bound(oObject.Position.X,
                                                 Boundary.X + Boundary.Width,
                                                 Boundary.X),
                                    (float)Bound(oObject.Position.Y,
                                                 Boundary.Y + Boundary.Height,
                                                 Boundary.Y));
                }

                // Handle collisions
                List<MovingObject> oCollideable = new List<MovingObject>(m_oMoving);
                for (int i = 0; i < oCollideable.Count; ++i)
                {
                    for (int j = i + 1; j < oCollideable.Count; ++j)
                    {
                        if (Colliding(oCollideable[i], oCollideable[j]))
                        {
                            oCollideable[i].HandleCollision(oCollideable[j], gameTime);
                            oCollideable[j].HandleCollision(oCollideable[i], gameTime);
                        }
                    }
                }
            }

            protected double Bound(double a_dValue, double a_dMax, double a_dMin = 0)
            {
                a_dValue -= a_dMin;
                a_dValue -= (a_dMax - a_dMin) * (int)(a_dValue / (a_dMax - a_dMin));
                return a_dValue + ((a_dValue < 0) ? a_dMax : a_dMin);
            }
        }
    }
}
