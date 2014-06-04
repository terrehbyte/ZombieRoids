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
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Adding comments
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using RotatedRectangleCollisions;

namespace ZombieRoids
{
    /// <remarks>
    /// Static class providing functions for collision checking
    /// </remarks>
    public static class Collision
    {
        /// <summary>
        /// Check for collision between two entities
        /// </summary>
        /// <param name="a_entFirst"></param>
        /// <param name="a_entSecond"></param>
        /// <returns>True if entities collide</returns>
        public static bool CheckCollision(Entity a_entFirst, Entity a_entSecond)
        {
            return CheckCollision(a_entFirst.Collider, a_entSecond.Collider);
        }

        /// <summary>
        /// Check for collision between two rectangles
        /// </summary>
        /// <param name="a_entFirst"></param>
        /// <param name="a_entSecond"></param>
        /// <returns>True if rectangles collide</returns>
        public static bool CheckCollision(Rectangle a_rctFirst,
                                          Rectangle a_rctSecond)
        {
            return a_rctFirst.Intersects(a_rctSecond);
        }

        /// <summary>
        /// Check for collision between a rotated rectangles and a normal rectangle
        /// </summary>
        /// <param name="a_entFirst"></param>
        /// <param name="a_entSecond"></param>
        /// <returns>True if rectangles collide</returns>
        public static bool CheckCollision(Rectangle a_rctFirst,
                                          RotatedBoxCollider a_rotrctSecond)
        {
            return a_rotrctSecond.Intersects(a_rctFirst);
        }

        /// <summary>
        /// Check for collision between two rotated rectangles
        /// </summary>
        /// <param name="a_entFirst"></param>
        /// <param name="a_entSecond"></param>
        /// <returns>True if rectangles collide</returns>
        public static bool CheckCollision(RotatedBoxCollider a_rotrctFirst,
                                          Rectangle a_rctSecond)
        {
            return a_rotrctFirst.Intersects(a_rctSecond);
        }

        /// <summary>
        /// Check for collision between two rotated rectangles
        /// </summary>
        /// <param name="a_entFirst"></param>
        /// <param name="a_entSecond"></param>
        /// <returns>True if rectangles collide</returns>
        public static bool CheckCollision(RotatedBoxCollider a_rotrctFirst,
                                          RotatedBoxCollider a_rotrctSecond)
        {
            return a_rotrctFirst.Intersects(a_rotrctSecond);
        }
    }
}
