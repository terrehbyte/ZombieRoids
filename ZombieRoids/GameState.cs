using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieRoids
{
    public abstract class GameState
    {
        /// <summary>
        /// Loads the requested pieces of content to the game state
        /// </summary>
        protected virtual void LoadContent()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Unloads all content loaded by this game state
        /// </summary>
        protected virtual void UnloadContent()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called to begin the game state
        /// </summary>
        public virtual void Start()
        {
            LoadContent();
        }

        /// <summary>
        /// Called every game update frame
        /// </summary>
        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Called to end the game state
        /// </summary>
        public virtual void End()
        {
            UnloadContent();
        }
    }
}