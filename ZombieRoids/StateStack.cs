using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static void Update()
        {
            m_oStates.Peek().Update();
        }   
    }
}
