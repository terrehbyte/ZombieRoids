using System;
using Microsoft.Xna.Framework;
using RotatedRectangleCollisions;

namespace ZombieRoids
{
    class Collision
    {
        public static bool CheckCollision(Entity a_entFirst, Entity a_entSecond)
        {
            Rectangle rctFirst = new Rectangle((int)a_entFirst.m_v2Pos.X,
                                               (int)a_entFirst.m_v2Pos.Y,
                                               (int)a_entFirst.m_v2Dims.X,
                                               (int)a_entFirst.m_v2Dims.Y);

            Rectangle rctSecond = new Rectangle((int)a_entSecond.m_v2Pos.X,
                                               (int)a_entSecond.m_v2Pos.Y,
                                               (int)a_entSecond.m_v2Dims.X,
                                               (int)a_entSecond.m_v2Dims.Y);

            return rctFirst.Intersects(rctSecond);
        }

        public static bool CheckCollision(Rectangle a_rctFirst, Rectangle a_rctSecond)
        {
            return a_rctFirst.Intersects(a_rctSecond);
        }

        public static bool CheckCollision(Rectangle a_rctFirst, RotatedBoxCollider a_rotrctSecond)
        {
            return a_rotrctSecond.Intersects(a_rctFirst);
        }

        public static bool CheckCollision(RotatedBoxCollider a_rotrctFirst, Rectangle a_rctSecond)
        {
            return a_rotrctFirst.Intersects(a_rctSecond);
        }

        public static bool CheckCollision(RotatedBoxCollider a_rotrctFirst, RotatedBoxCollider a_rotrctSecond)
        {
            return a_rotrctFirst.Intersects(a_rotrctSecond);
        }
    }
}
