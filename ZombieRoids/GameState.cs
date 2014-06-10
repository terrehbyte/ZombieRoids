/// <list type="table">
/// <listheader><term>GameConsts.cs</term><description>
///     Class containing base abstract class for game states
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 9, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Merging 'dev' into 'feature-terry'
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
            public HashSet<Enemy> enemies;
            public Ref<int> score;
        };

        protected GameState()
        {
        }

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
        /// Loads the requested pieces of content to the game state
        /// </summary>
        protected virtual void LoadContent()
        {
            m_oSpriteBatch = new SpriteBatch(m_oGraphicsDevice);
            m_rctViewport = m_oGraphicsDevice.Viewport.TitleSafeArea;
        }

        /// <summary>
        /// Unloads all content loaded by this game state
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
        /// Called every game update frame
        /// </summary>
        public abstract void Update(GameTime a_oGameTime);

        public abstract void Draw(GameTime a_oGameTime);

        /// <summary>
        /// Called to end the game state
        /// </summary>
        public virtual void End()
        {
            UnloadContent();
        }
    }
}