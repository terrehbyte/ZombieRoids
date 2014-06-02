using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    class Entity : Sprite
    {
        /// <summary>
        /// Change in position
        /// </summary>
        public Vector2 m_v2Vel;

        /// <summary>
        /// Assigns data critical to entity updates
        /// </summary>
        /// <param name="a_tTex">Texture used for drawing</param>
        /// <param name="a_v2Pos">Initial position of sprite</param>
        public virtual void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            // Assign Texture
            m_tTex = a_tTex;

            // Assign Position
            m_v2Pos = a_v2Pos;
        }

        public virtual void Update(GameTime a_gtGameTime)
        {

        }
    }
}
