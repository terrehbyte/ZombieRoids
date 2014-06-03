/// <list type="table">
/// <listheader><term>Entity.cs</term><description>
///     Class representing a moving object
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 2, 2014
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
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    class Entity : Sprite
    {
        /// <summary>
        /// Change in position
        /// </summary>
        public Vector2 m_v2Vel;
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
            Texture = a_tTex;

            // Assign Position
            Position = a_v2Pos;


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

            if (!rctScreen.Intersects(Boundary))
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
            if (Position.Y < 0)
            {
                Position = new Vector2(Position.X,Game1.v2ScreenDims.Y);
            }
            // Below
            else if (Position.Y > Game1.v2ScreenDims.Y)
            {
                Position = new Vector2(Position.X,0);
            }
            
            // Horizontal Wrap
            // Left
            if (Position.X < 0)
            {
                Position = new Vector2(Game1.v2ScreenDims.X, Position.Y);
            }

            // Right
            else if (Position.X > Game1.v2ScreenDims.X)
            {
                Position = new Vector2(0, Position.Y);
            }

        }
        private void ExitPrint()
        {
            Console.WriteLine("Exited");
        }
    }
}
