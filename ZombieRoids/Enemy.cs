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
///     Refactoring Sprite class
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

        public int m_iValue;    // score value
        public int m_iDivisions = 2; // How many will this break into

        public override void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            base.Initialize(a_tTex, a_v2Pos);

            m_bAlive = true;
            m_bActive = true;
            m_iValue = 100;
        }

        public void UpdateCollider()
        {
            m_rctCollider = Boundary;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_bActive)
            {
                Position += m_v2Vel;

                // Disable if dead
                if (!m_bAlive)
                {
                    m_bActive = false;
                }

                UpdateCollider();
            }
        }
    }
}
