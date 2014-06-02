using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Sprite
    {
        /// <summary>
        /// Position of sprite in world coordinates
        /// </summary>
        public Vector2 m_v2Pos;

        /// <summary>
        /// Sprite texture
        /// </summary>
        public Texture2D m_tTex;

        public Vector2 m_v2Dims
        {
            get
            {
                return new Vector2((int)m_tTex.Width, (int)m_tTex.Height);
            }
            private set
            {
                return;
            }
        }

        /// <summary>
        /// Queues the sprite for drawing in a sprite batch
        /// </summary>
        /// <param name="a_sbSpriteBatch">SpriteBatch to queue into</param>
        public virtual void Draw(SpriteBatch a_sbSpriteBatch)
        {
            a_sbSpriteBatch.Draw(m_tTex, m_v2Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
