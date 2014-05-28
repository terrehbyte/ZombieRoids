#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace ZombieRoids
{
    class PhysicsObject2D
    {
        private double m_dMass;
        private Vector2 m_v2Velocity;
        private Vector2 m_v2Position;
        private static HashSet<PhysicsObject2D> m_oActive;
        private static Stack<PhysicsObject2D> m_oRecycled;

        private PhysicsObject2D(double a_dMass )
        {

        }
    }
}
