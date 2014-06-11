using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZombieRoids
{
    public static class StateStack
    {
        public static int StackCount
        {
            get
            {
                return m_oStates.Count;
            }
        }

        public static void RegisterGame(Game1 a_oGame)
        {
            m_oGame = a_oGame;
        }

        public enum State
        {
            MAINMENU,
            GAMEPLAY,
            GAMEOVER,
            PAUSE
        }

        // Stack of GameStates
        private static Stack<GameState> m_oStates = new Stack<GameState>();

        // Game
        private static Game1 m_oGame;

        /// <summary>
        /// Pushes a new GameState to the top of the stack
        /// </summary>
        /// <param name="a_oState"></param>
        public static  void AddState(GameState a_oState)
        {
            m_oStates.Push(a_oState);
            m_oStates.Peek().Start();
        }

        public static void AddState(State a_oNewState)
        {
            switch (a_oNewState)
            {
                case (State.MAINMENU):
                    {
                        AddState(new MainMenuState(m_oGame));
                        break;
                    }
                case (State.GAMEPLAY):
                    {
                        AddState(new PlayState(m_oGame));
                        break;
                    }
                case (State.GAMEOVER):
                    {
                        AddState(new GameOverState(m_oGame));
                        break;
                    }
                case (State.PAUSE):
                    {
                        throw new System.NotImplementedException("Pause not implemented!");
                        break;
                    }
                default:
                    {
                        throw new Exception("Invalid State Added!");
                    }
            }
        }

        /// <summary>
        /// Pops the topmost GameState from the stack
        /// </summary>
        public static void PopState()
        {
            m_oStates.Peek().End();
            m_oStates.Pop();
        }

        /// <summary>
        /// Calls update on the topmost GameState
        /// </summary>
        public static void Update(GameTime a_oGameTime)
        {
            m_oStates.Peek().Update(a_oGameTime);
        }

        public static void Draw(GameTime a_oGameTime)
        {
            m_oStates.Peek().Draw(a_oGameTime);
        }
    }
}
