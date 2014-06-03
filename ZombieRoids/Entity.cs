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

        public int m_iHealth;
        public bool m_bActive;
        public bool m_bAlive;

        private bool m_bOnScreen;

        public delegate void OnScreenExit();
        public OnScreenExit ScreenExitEvent;


#if DEBUG
        private bool m_bInitialized;
#endif

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


            ScreenExitEvent = ScreenWrap;

#if DEBUG
            m_bInitialized = true;
            ScreenExitEvent += ExitPrint;
#endif
        }

        public virtual void Update(GameTime a_gtGameTime)
        {
#if DEBUG
            if (!m_bInitialized)
            {
                throw new Exception("Not Initialized!");
            }

            if (m_bOnScreen)
            {
                if (!CheckOffscreen(Game1.v2ScreenDims))
                {
                    m_bOnScreen = true;
                }
            }
            else
            {
                if (CheckOffscreen(Game1.v2ScreenDims))
                {
                    m_bOnScreen = false;
                    ScreenExitEvent();
                }
            }
#endif
        }

        public bool CheckOffscreen(Vector2 a_v2ScreenDims)
        {
            Rectangle rctScreen = new Rectangle(0, 0, (int)a_v2ScreenDims.X, (int)a_v2ScreenDims.Y);
            Rectangle entBox = new Rectangle((int)m_v2Pos.X, (int)m_v2Pos.Y, (int)m_v2Dims.X, (int)m_v2Dims.Y);

            if (!rctScreen.Intersects(entBox))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Draw(SpriteBatch a_sbSpriteBatch)
        {
            if (m_bActive)
            {
                base.Draw(a_sbSpriteBatch);
            }
        }

        private void ScreenWrap()
        {
            // Vertical Wrap
            // Top
            if (m_v2Pos.Y < 0)
            {
                m_v2Pos.Y = Game1.v2ScreenDims.Y;
            }
            // Below
            else if (m_v2Pos.Y > Game1.v2ScreenDims.Y)
            {
                m_v2Pos.Y = 0;
            }
            
            // Horizontal Wrap
            // Left
            if (m_v2Pos.X < 0)
            {
                m_v2Pos.X = Game1.v2ScreenDims.X;
            }

            // Right
            else if (m_v2Pos.X > Game1.v2ScreenDims.X)
            {
                m_v2Pos.X = 0;
            }

        }
        private void ExitPrint()
        {
            Console.WriteLine("Exited");
        }
    }
}
