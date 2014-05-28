using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Player
    {
        public Texture2D tPlayerTex;
        public Vector2 v2Pos;
        public bool bActive;
        public int iHealth;
        public int iWidth
        {
            get
            {
                return tPlayerTex.Width;
            }
        }
        public int iHeight
        {
            get
            {
                return tPlayerTex.Height;
            }
        }

        public void Initialize(Texture2D a_tTex, Vector2 a_v2Pos)
        {
            tPlayerTex = a_tTex;
            v2Pos = a_v2Pos;
            bActive = true;
            iHealth = 100;
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tPlayerTex, v2Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
