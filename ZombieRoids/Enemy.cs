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
///     June 5, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Enemy fragments now decrease in size
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

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

        // Velocity Caps

        // Off-screen Spawning
        private const int mc_iOffset = 100;  // Offset in Screen Space
        private const int mc_iMinVel = 100;  // Minimum Velocity for any axis
        private const int mc_iMaxVel = 200;  // Maximum Velocity for any axis

        // Fragment Spawning Deltas
        private const int mc_iXVelDelta = 100;
        private const int mc_iYVelDelta = 100;

        private const int mc_iXPosDelta = 50;
        private const int mc_iYPosDelta = 50;

        private const float mc_fFragmentMultiplier = 0.65f; // Scale of enemy fragments

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
        /// <param name="a_oContext"></param>
        public override void Update(Game1.Context a_oContext)
        {
            base.Update(a_oContext);

            if (Active)
            {
                // If dead
                if (!Alive)
                {
                    // Birth fragments if any
                    Spawn(a_oContext);

                    a_oContext.enemies.Remove(this);
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
            // Random position offscreen
            Vector2 v2Position =
                new Vector2((float)(a_oContext.random.NextDouble() * 2 - 1),
                            (float)(a_oContext.random.NextDouble() * 2 - 1));
            if (0f == v2Position.X)
            {
                v2Position.Y = (v2Position.Y < 0 ? -1 : 1);
            }
            else if (0f == v2Position.Y)
            {
                v2Position.X = (v2Position.X < 0 ? -1 : 1);
            }
            else
            {
                v2Position /= Math.Min(Math.Abs(v2Position.X),
                                       Math.Abs(v2Position.Y));
            }
            v2Position.X *= (a_oContext.viewport.Width + a_tTexture.Width) / 2;
            v2Position.Y *= (a_oContext.viewport.Height + a_tTexture.Height) / 2;
            v2Position.X += a_oContext.viewport.Left;
            v2Position.Y += a_oContext.viewport.Top;

            // Create enemy
            Enemy oEnemy = new Enemy();

            // - Determine Velocity -

            // Horizontal Speed
            // If Left of Screen
            if (v2Position.X < a_oContext.viewport.Left)
            {
                oEnemy.Velocity =
                    new Vector2(a_oContext.random.Next(mc_iMinVel, mc_iMaxVel),
                                oEnemy.Velocity.Y);
            }
            // Right of Screen
            else
            {
                oEnemy.Velocity =
                    new Vector2(a_oContext.random.Next(-mc_iMaxVel, -mc_iMinVel),
                                oEnemy.Velocity.Y);
            }

            // Vertical Speed
            // If Above
            if (v2Position.Y < a_oContext.viewport.Top)
            {
                oEnemy.Velocity =
                    new Vector2(oEnemy.Velocity.X,
                                a_oContext.random.Next(mc_iMinVel, mc_iMaxVel));
            }
            // If Below
            else
            {
                oEnemy.Velocity =
                    new Vector2(oEnemy.Velocity.X,
                                a_oContext.random.Next(-mc_iMaxVel, -mc_iMinVel));
            }

            // Assure that it isn't moving perfectly straight horizontally
            Debug.Assert(oEnemy.Velocity.Y != 0, "Invalid Enemy Velocity = " + oEnemy.Velocity.Y);

            // Initialize the Enemy
            oEnemy.Initialize(a_tTexture, v2Position);

            // Add the enemy to the list
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
                Enemy eneNewFoe = new Enemy();

                // New position randomly offset from this one
                Vector2 v2Position =
                    Position + new Vector2(a_oContext.random.Next(-mc_iXPosDelta, mc_iXPosDelta),
                                           a_oContext.random.Next(-mc_iYPosDelta, mc_iYPosDelta));
                
                // New velocity randomly offset from this one
                Vector2 v2Velocity =
                    Velocity + new Vector2(a_oContext.random.Next(-mc_iXVelDelta, mc_iXVelDelta),
                                a_oContext.random.Next(-mc_iYVelDelta, mc_iYVelDelta));


                // Cap Velocity
                v2Velocity.X = MathHelper.Clamp(v2Velocity.X, -mc_fSpeed, mc_fSpeed);
                v2Velocity.Y = MathHelper.Clamp(v2Velocity.Y, -mc_fSpeed, mc_fSpeed);

                Debug.Assert(v2Velocity.Y != 0, "Invalid Enemy Velocity = " + eneNewFoe.Velocity.Y);

                // Initialize and assign other values
                eneNewFoe.Initialize(Texture, v2Position);
                eneNewFoe.Velocity = v2Velocity;
                eneNewFoe.Scale = new Vector2(Scale.X * mc_fFragmentMultiplier, Scale.Y * mc_fFragmentMultiplier);

                // New enemy breaks into fewer fragments than this one
                eneNewFoe.FragmentCount = FragmentCount - 1;
                a_oContext.enemies.Add(eneNewFoe);
            }
        }
    }
}
