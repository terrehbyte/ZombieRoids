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
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring GameState.Context
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace ZombieRoids
{
    /// <remarks>
    /// Represents an enemy for the player to shoot
    /// </remarks>
    public class Enemy : Entity
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
        public int FragmentCount { get; set; }

        /// <summary>
        /// Sets up the enemy
        /// </summary>
        /// <param name="a_tTex">Enemy image</param>
        /// <param name="a_v2Pos">Enemy position</param>
        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            base.Initialize(a_tTex, a_v2Pos);
            Active = true;
            HitPoints = GameConsts.ZombieHP;
            Damage = GameConsts.ZombieDamage;
            Value = GameConsts.ZombieValue;
        }

        /// <summary>
        /// Enemy becomes inactive if it leaves the screen
        /// </summary>
        /// <param name="a_oContext"></param>
        public override void Update(GameState.Context a_oContext)
        {
            base.Update(a_oContext);

            if (Active)
            {
                // If dead
                if (!Alive)
                {
                    // Birth fragments if any
                    if (a_oContext.state is PlayState)
                    {
                        Spawn(a_oContext);
                        (a_oContext.state as PlayState).Enemies.Remove(this);
                    }

                    // Play death sound
                    GameAssets.ZombieDeathSound.Play();
                }
            }
        }

        /// <summary>
        /// Create a new enemy at a random starting position using the given texture
        /// </summary>
        /// <param name="a_oContext"></param>
        /// <param name="a_tTexture"></param>
        public static void Spawn(GameState.Context a_oContext, Texture2D a_tTexture)
        {
            if (!(a_oContext.state is PlayState))
            {
                return;
            }

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
                v2Position /= Math.Max(Math.Abs(v2Position.X),
                                       Math.Abs(v2Position.Y));
            }
            v2Position.X *= (a_oContext.viewport.Width + a_tTexture.Width) / 2;
            v2Position.Y *= (a_oContext.viewport.Height + a_tTexture.Height) / 2;
            v2Position.X += a_oContext.viewport.Left + a_oContext.viewport.Width / 2;
            v2Position.Y += a_oContext.viewport.Top + a_oContext.viewport.Height / 2;

            // Create enemy
            Enemy oEnemy = new Enemy();
            oEnemy.FragmentCount = GameConsts.InitialFragments;

            // - Determine Velocity -

            // Horizontal Speed
            // If Left of Screen
            if (v2Position.X < a_oContext.viewport.Center.X)
            {
                if (v2Position.Y < a_oContext.viewport.Center.Y)
                {
                    oEnemy.Rotation =
                        (float)(a_oContext.random.NextDouble() * Math.PI / 2);
                }
                else
                {
                    oEnemy.Rotation =
                        (float)((2 - a_oContext.random.NextDouble() / 2) * Math.PI);
                }
            }
            // Right of Screen
            else
            {
                if (v2Position.Y < a_oContext.viewport.Center.Y)
                {
                    oEnemy.Rotation =
                        (float)((1 + a_oContext.random.NextDouble()) * Math.PI / 2);
                }
                else
                {
                    oEnemy.Rotation =
                        (float)((1 + a_oContext.random.NextDouble() / 2) * Math.PI);
                }
            }
            oEnemy.Velocity = oEnemy.Forward * GameConsts.ZombieSpeed;

            Debug.Assert(Math.Abs(oEnemy.Position.X) < a_oContext.viewport.Width &&
                         Math.Abs(oEnemy.Position.Y) < a_oContext.viewport.Height, "Invalid Enemy Position = " + oEnemy.Position);

            // Initialize the Enemy
            oEnemy.Initialize(a_tTexture, v2Position);

            // Add the enemy to the list
            (a_oContext.state as PlayState).Enemies.Add(oEnemy);
        }

        /// <summary>
        /// Create enemies from fragments of this enemy
        /// </summary>
        /// <param name="a_oContext"></param>
        public void Spawn(GameState.Context a_oContext)
        {
            if (!(a_oContext.state is PlayState))
            {
                return;
            }

            for (int i = 0; i < FragmentCount; ++i)
            {
                Enemy eneNewFoe = new Enemy();

                // New position randomly offset from this one
                Vector2 v2Position =
                    Position + new Vector2(
                        a_oContext.random.Next(-GameConsts.FragmentDeltaPosition,
                                               GameConsts.FragmentDeltaPosition),
                        a_oContext.random.Next(-GameConsts.FragmentDeltaPosition,
                                               GameConsts.FragmentDeltaPosition));
                
                // New velocity randomly offset from this one
                Vector2 v2Velocity =
                    Velocity + new Vector2(
                        a_oContext.random.Next(-GameConsts.FragmentDeltaSpeed,
                                               GameConsts.FragmentDeltaSpeed),
                        a_oContext.random.Next(-GameConsts.FragmentDeltaSpeed,
                                               GameConsts.FragmentDeltaSpeed));


                // Cap Velocity
                if (v2Velocity.LengthSquared() == 0)
                {
                    v2Velocity = Velocity * GameConsts.FragmentMinSpeed / Velocity.Length();
                }
                else if (v2Velocity.LengthSquared() >
                    GameConsts.FragmentMaxSpeed * GameConsts.FragmentMaxSpeed)
                {
                    v2Velocity *= GameConsts.FragmentMaxSpeed / v2Velocity.Length();
                }
                else if (v2Velocity.LengthSquared() <
                    GameConsts.FragmentMinSpeed * GameConsts.FragmentMinSpeed)
                {
                    v2Velocity *= GameConsts.FragmentMinSpeed / v2Velocity.Length();
                }

                // Initialize and assign other values
                eneNewFoe.Initialize(Texture, v2Position);
                eneNewFoe.Velocity = v2Velocity;
                eneNewFoe.Forward = v2Velocity;
                eneNewFoe.Scale = Scale * GameConsts.FragmentScale;

                // New enemy breaks into fewer fragments than this one
                eneNewFoe.FragmentCount = FragmentCount - 1;
                (a_oContext.state as PlayState).Enemies.Add(eneNewFoe);
            }
        }
    }
}
