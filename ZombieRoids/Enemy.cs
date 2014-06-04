/// <list type="table">
/// <listheader><term>Enemy.cs</term><description>
///     Class representing an enemy for the player to shoot
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     May 29, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Merged with dev for @emlowry Enemy refactor
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    /// <remarks>
    /// Represents an enemy for the player to shoot
    /// </remarks>
    class Enemy : Entity
    {
        /// <summary>
        /// Damage inflicted on things that collide with this enemy
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Points scored from the destruction of this enemy
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Number of fragments this enemy breaks into
        /// </summary>
        public int FragmentCount
        {
            get { return m_iFragmentCount; }
            set { m_iFragmentCount = value; }
        }
        private int m_iFragmentCount = 2;

        /// <summary>
        /// Sets up the enemy
        /// </summary>
        /// <param name="a_tTex">Enemy image</param>
        /// <param name="a_v2Pos">Enemy position</param>
        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            base.Initialize(a_tTex, a_v2Pos);
            Active = true;
            HitPoints = 10;
            Damage = 10;
            Value = 100;
        }

        /// <summary>
        /// Enemy becomes inactive if it leaves the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Disable if dead
            if (Active)
            {
                // TODO: Add recycling
                if (Right < 0 || HitPoints <= 0)
                {
                    Active = false;
                }
            }
        }
    }
}
