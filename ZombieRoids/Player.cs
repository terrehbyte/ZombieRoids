using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Player
    {
        public Animation aniPlayerAnimation;
        public Texture2D tPlayerTex;
        public Vector2 v2Pos;
        public bool bActive;
        public int iHealth;
        public int iWidth
        {
            get
            {
                return aniPlayerAnimation.iFrameWidth;
            }
        }
        public int iHeight
        {
            get
            {
                return aniPlayerAnimation.iFrameHeight;
            }
        }

        public void Initialize(Animation a_animation, Vector2 a_v2Pos)
        {
            aniPlayerAnimation = a_animation;
            v2Pos = a_v2Pos;
            bActive = true;
            iHealth = 100;
        }

        public void Update(GameTime gameTime)
        {
            aniPlayerAnimation.v2Pos = v2Pos;
            aniPlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(tPlayerTex, v2Pos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            aniPlayerAnimation.Draw(spriteBatch);
        }
    }
}
