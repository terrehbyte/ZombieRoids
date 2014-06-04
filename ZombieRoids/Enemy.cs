﻿/// <list type="table">
/// <listheader><term>Enemy.cs</term><description>
///     Class representing an enemy for the player to shoot at
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
///     June 3, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Player and Bullet classes
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Enemy : Entity
    {
        public Rectangle m_rctCollider;

        public int m_iDamage;   // damage dealt to other thigns
        public int m_iValue;    // score value
        public int m_iDivisions = 2; // How many will this break into

        int m_iSpeed = 90;

        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            base.Initialize(a_tTex, a_v2Pos);

            Active = true;

            HitPoints = 10;
            m_iDamage = 10;
            m_iValue = 100;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Active)
            {
                // TODO: Add recycling
                if (Left < -Size.X || HitPoints <= 0)
                {
                    Active = false;
                }
            }
        }
    }
}
