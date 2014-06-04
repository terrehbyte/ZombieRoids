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
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Game1 class
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
    public class Enemy : Entity
    {
        // initial values
        private const float mc_fSpeed = 120f;
        private const int mc_iHP = 10;
        private const int mc_iDamage = 10;
        private const int mc_iValue = 100;

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
            HitPoints = mc_iHP;
            Damage = mc_iDamage;
            Value = mc_iValue;
        }

        /// <summary>
        /// Enemy becomes inactive if it leaves the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Game1.Context a_oContext)
        {
            base.Update(a_oContext);
            if (Active)
            {
                // TODO: Add recycling
                if (!Alive || CheckOffscreen(a_oContext.viewport))
                {
                    a_oContext.enemies.Remove(this);
                    Spawn(a_oContext);
                }
            }
        }

        /// <summary>
        /// Create a new enemy at a random starting position using the given texture
        /// </summary>
        /// <param name="a_oContext"></param>
        /// <param name="a_tTexture"></param>
        public static void Spawn(Game1.Context a_oContext, Texture2D a_tTexture)
        {
            // Random start position on right edge of screen
            Vector2 v2Position =
                new Vector2(a_oContext.viewport.Right + a_tTexture.Width / 2,
                            a_oContext.random.Next(100, a_oContext.viewport.Bottom - 100));

            // Create enemy
            Enemy oEnemy = new Enemy();
            oEnemy.Velocity = new Vector2(-mc_fSpeed, 0);
            oEnemy.Initialize(a_tTexture, v2Position);
            a_oContext.enemies.Add(oEnemy);
        }

        /// <summary>
        /// Create enemies from fragments of this enemy
        /// </summary>
        /// <param name="a_oContext"></param>
        public void Spawn(Game1.Context a_oContext)
        {
            for (int i = 0; i < FragmentCount; ++i)
            {
                // New position randomly offset from this one
                Vector2 v2Position =
                    Position + new Vector2(a_oContext.random.Next(-50, 45),
                                           a_oContext.random.Next(-50, 55));
                Enemy eneNewFoe = new Enemy();

                // New velocity randomly offset from this one
                eneNewFoe.Velocity =
                    new Vector2(a_oContext.random.Next((int)Velocity.X, -1),
                                a_oContext.random.Next(-2, 2));
                eneNewFoe.Initialize(Texture, v2Position);

                // New enemy breaks into fewer fragments than this one
                eneNewFoe.FragmentCount = FragmentCount - 1;
                a_oContext.enemies.Add(eneNewFoe);
            }
        }
    }
}
