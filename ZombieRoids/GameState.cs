/// <list type="table">
/// <listheader><term>GameState.cs</term><description>
///     Class containing base abstract class for game states
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 9, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 11, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Implementing Pause state
/// </description></item>
/// </list>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Utility;

namespace ZombieRoids
{
    public abstract class GameState
    {
        // Initialized by Game
        protected ContentManager m_oContentManager;     // requires Game.Services
        private GraphicsDevice m_oGraphicsDevice;       // don't know where this is initialized

        // Used for handling graphics
        protected SpriteBatch m_oSpriteBatch;           // per State

        // Game Logic
        protected Random m_rngRandom;
        protected Rectangle m_rctViewport;

        // Legacy Access to Game
        protected Game m_oGame;

        public struct Context
        {
            public GameTime time;
            public Rectangle viewport;
            public Random random;
            public GameState state;
        };

        protected GameState()
        {
        }

        /// <summary>
        /// Initializes GameState with properties needed
        /// </summary>
        /// <param name="a_oMainGame"></param>
        protected GameState(Game1 a_oMainGame)
        {
            // Get variables to access Game attributes
            m_oGame = a_oMainGame;
            m_oGraphicsDevice = a_oMainGame.GraphicsDevice;

            // Initialize ContentManager for this state
            m_oContentManager = new ContentManager(a_oMainGame.Services);
            m_oContentManager.RootDirectory = "Content";
        }

        /// <summary>
        /// Loads the requested content to the content manager
        /// </summary>
        protected virtual void LoadContent()
        {
            m_oSpriteBatch = new SpriteBatch(m_oGraphicsDevice);
            m_rctViewport = m_oGraphicsDevice.Viewport.TitleSafeArea;
        }

        /// <summary>
        /// Unloads all content in the content manager
        /// </summary>
        protected virtual void UnloadContent()
        {
            m_oContentManager.Unload();
        }
        
        /// <summary>
        /// Called to begin the game state
        /// </summary>
        public virtual void Start()
        {
            LoadContent();

            m_rngRandom = new Random();
        }

        /// <summary>
        /// Runs a frame of the game logic
        /// </summary>
        public abstract void Update(GameTime a_oGameTime);

        /// <summary>
        /// Draws textures to the sprite batch
        /// </summary>
        /// <param name="a_oGameTime"></param>
        public abstract void Draw(GameTime a_oGameTime);

        /// <summary>
        /// Called when gamestate is popped from the gamestate stack
        /// </summary>
        public virtual void End()
        {
            UnloadContent();
        }

        /// <summary>
        /// Called when another gamestate is pused onto the stack on top of this
        /// one.
        /// </summary>
        public virtual void Suspend() { }

        /// <summary>
        /// Called when another gamestate directly above this one in the stack
        /// is popped, making this state the current state once again.
        /// </summary>
        public virtual void Resume() { }
    }
}