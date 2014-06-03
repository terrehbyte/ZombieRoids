/// <list type="table">
/// <listheader><term>Collision.cs</term><description>
///     Functions for checking for collisions between objects
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
using Microsoft.Xna.Framework;
using RotatedRectangleCollisions;

namespace ZombieRoids
{
    class Collision
    {
        public static bool CheckCollision(Entity a_entFirst, Entity a_entSecond)
        {
            return CheckCollision(a_entFirst.Boundary, a_entSecond.Boundary);
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
