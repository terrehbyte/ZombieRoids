/// <list type="table">
/// <listheader><term>StateStack.cs</term><description>
///     Class containing logic for game over menu
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Added logic for moving to gameplay from mainmenu
/// </description></item>
/// </list>using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System;

namespace ZombieRoids
{
    public static class StateStack
    {
        /// <summary>
        /// Remaining states in the stack
        /// </summary>
        public static int StackCount
        {
            get
            {
                return m_oStates.Count;
            }
        }

        /// <summary>
        /// Registers the Game1 state
        /// </summary>
        /// <param name="a_oGame"></param>
        public static void RegisterGame(Game1 a_oGame)
        {
            m_oGame = a_oGame;
        }

        // States that can be added
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
            // Add state to the stack
            m_oStates.Push(a_oState);

            // Call the state's start function for initialization
            m_oStates.Peek().Start();
        }

        /// <summary>
        /// Pushes and starts a state to the stack based on enum
        /// </summary>
        /// <param name="a_oNewState"></param>
        public static void AddState(State a_oNewState)
        {
            // Call AddState on the correct state requested
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
        /// Ends and Pops the topmost GameState
        /// </summary>
        public static void PopState()
        {
            m_oStates.Peek().End();
            m_oStates.Pop();
        }

        /// <summary>
        /// Updates the topmost GameState
        /// </summary>
        public static void Update(GameTime a_oGameTime)
        {
            m_oStates.Peek().Update(a_oGameTime);
        }

        /// <summary>
        /// Draws the topmost GameState
        /// </summary>
        /// <param name="a_oGameTime"></param>
        public static void Draw(GameTime a_oGameTime)
        {
            m_oStates.Peek().Draw(a_oGameTime);
        }
    }
}
