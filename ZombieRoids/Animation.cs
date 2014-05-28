using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Animation
    {
        // Spritesheet for Animation
        Texture2D tspriteSheet;

        // Strip Scale
        float fscale;

        // Time since last animation frame
        int ielapsedtime;

        // Time until next animation frame
        int iframetime;

        // Frame Count
        int iframeCount;

        // Frame Index
        int icurFrame;

        // Frame Color
        Color ccolor;

        // Source Rect to display
        Rectangle rctsourceRect = new Rectangle();
        Rectangle rctdestRect = new Rectangle();

        // Frame Dims
        public int iFrameWidth;
        public int iFrameHeight;

        public bool bActive;
        public bool bLooping;

        public Vector2 v2Pos;

        public void Initialize(Texture2D a_tTexture, Vector2 a_v2Pos, int a_iFrameWidth, int a_iFrameHeight,
                               int a_iFrameCount, int a_iFrameTime,
                               Color a_cColor, float a_fScale, bool a_bLooping)
        {
            this.ccolor = a_cColor;
            this.iFrameWidth = a_iFrameWidth;
            this.iFrameHeight = a_iFrameHeight;
            this.iframeCount = a_iFrameCount;
            this.iframetime = a_iFrameTime;
            this.fscale = a_fScale;

            bLooping = a_bLooping;
            v2Pos = a_v2Pos;
            tspriteSheet = a_tTexture;

            bActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!bActive)
            {
                return;
            }

            ielapsedtime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (ielapsedtime > iframetime)
            {
                // Next Frame
                icurFrame++;

                if (icurFrame == iframeCount)
                {
                    icurFrame = 0;

                    if (!bLooping)
                    {
                        bActive = false;
                    }
                }


                // Reset elapsed time
                ielapsedtime = 0;

            }


            rctsourceRect = new Rectangle(icurFrame * iFrameWidth, 0, iFrameWidth, iFrameHeight);

            rctdestRect = new Rectangle((int)v2Pos.X - (int)(iFrameWidth * fscale) / 2,
                                        (int)v2Pos.Y - (int)(iFrameHeight * fscale) / 2,
                                        (int)(iFrameWidth * fscale),
                                        (int)(iFrameHeight * fscale));

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (bActive)
            {
                spriteBatch.Draw(tspriteSheet, rctdestRect, rctsourceRect, ccolor);
            }
        }
    }
}
