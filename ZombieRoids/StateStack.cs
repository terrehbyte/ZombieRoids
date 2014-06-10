using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZombieRoids
{
    public static class StateStack
    {


        // Stack of GameStates
        private static Stack<GameState> m_oStates = new Stack<GameState>();

        /// <summary>
        /// Pushes a new GameState to the top of the stack
        /// </summary>
        /// <param name="a_oState"></param>
        public static  void AddState(GameState a_oState)
        {
            m_oStates.Push(a_oState);
            m_oStates.Peek().Start();
        }

        /// <summary>
        /// Pops the topmost GameState from the stack
        /// </summary>
        public static void PopState()
        {
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
