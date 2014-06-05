/// <list type="table">
/// <listheader><term>Game.cs</term><description>
///     The main class for the game as a whole.
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     May 27, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Game1 class
/// </description></item>
/// </list>

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;
#endregion

namespace ZombieRoids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region Props & Vars

        /// <summary>
        /// For passing information about the current game state to other
        /// objects' Update methods
        /// </summary>
        public struct Context
        {
            public GameTime time;
            public Rectangle viewport;
            public Random random;
            public HashSet<Enemy> enemies;
        };

        // For generating random numbers
        private Random m_rngRandom;

        // Screen display area
        private Rectangle m_rctViewport;

        // Used for handling graphics
        private GraphicsDeviceManager m_oGraphics;
        private SpriteBatch m_oSpriteBatch;

        // Background images
        private Texture2D m_tMainBackground;
        private ParallaxingBackground m_pbgBGLayer1;
        private ParallaxingBackground m_pbgBGLayer2;

        // Enemies
        private Texture2D m_tEnemyTex;
        private float mc_fEnemySpawnTimeSeconds = 1.0f;
        private TimeSpan m_tsEnemySpawnTime;
        private TimeSpan m_tsPrevEnemySpawnTime;
        private HashSet<Enemy> m_oEnemies;

        // Player
        private Player m_oPlayer;
        int iPlayerStartLives = 3;

        #endregion

        #region FrameworkMethods

        /// <summary>
        /// Constructor
        /// </summary>
        public Game1()
            : base()
        {
            m_oGraphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Initialize Engine Singleton
            Engine.Instance.m_Game = this;

            // Initialize basic game properties
            m_rctViewport = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            IsMouseVisible = true;
            m_rngRandom = new Random();

            // Track enemies
            m_oEnemies = new HashSet<Enemy>();
            m_tsPrevEnemySpawnTime = TimeSpan.Zero;
            m_tsEnemySpawnTime = TimeSpan.FromSeconds(mc_fEnemySpawnTimeSeconds);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_oSpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load player graphics
            Texture2D tPlayerTex = Content.Load<Texture2D>("Graphics\\player");
            Texture2D tBulletTex = Content.Load<Texture2D>("Graphics\\laser");
            Vector2 v2PlayerPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                              GraphicsDevice.Viewport.TitleSafeArea.Y +
                                              GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            // Create player object
            m_oPlayer = new Player();
            m_oPlayer.BulletTexture = tBulletTex;
            m_oPlayer.Initialize(tPlayerTex, v2PlayerPos);
            m_oPlayer.m_iLives = iPlayerStartLives;

            // Load background images
            m_tMainBackground = Content.Load<Texture2D>("Graphics/mainbackground");
            m_pbgBGLayer1 = new ParallaxingBackground();
            m_pbgBGLayer1.Initialize(Content, "Graphics/bgLayer1", GraphicsDevice.Viewport.Width,
                                     GraphicsDevice.Viewport.Height, -30);
            m_pbgBGLayer2 = new ParallaxingBackground();
            m_pbgBGLayer2.Initialize(Content, "Graphics/bgLayer2", GraphicsDevice.Viewport.Width,
                                   GraphicsDevice.Viewport.Height, -60);

            // Load enemy texture
            m_tEnemyTex = Content.Load<Texture2D>("Graphics\\mine");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Make a game context object for passing game state information to
            // entity Update functions
            Context oContext;
            oContext.time = gameTime;
            oContext.viewport = m_rctViewport;
            oContext.random = m_rngRandom;
            oContext.enemies = m_oEnemies;

            // Update player and enemies
            m_oPlayer.Update(oContext);
            UpdateEnemies(oContext);

            // Update parallaxing background
            m_pbgBGLayer1.Update(gameTime);
            m_pbgBGLayer2.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Get ready to draw
            GraphicsDevice.Clear(Color.CornflowerBlue);
            m_oSpriteBatch.Begin();
            
            // Draw background
            m_oSpriteBatch.Draw(m_tMainBackground, m_rctViewport, Color.White);
            m_pbgBGLayer1.Draw(m_oSpriteBatch);
            m_pbgBGLayer2.Draw(m_oSpriteBatch);

            // Draw player (which in turn draws bullets)
            m_oPlayer.Draw(m_oSpriteBatch);

            // Draw enemies
            foreach (Enemy oEnemy in m_oEnemies)
            {
                if (null != oEnemy)
                {
                    oEnemy.Draw(m_oSpriteBatch);
                }
            }

            // Finish drawing
            m_oSpriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

        #region Logic Methods
        /// <summary>
        /// Add enemy if neccessary and update all enemies
        /// </summary>
        /// <param name="a_oContext">Current state of game</param>
        private void UpdateEnemies(Game1.Context a_oContext)
        {
            //Max Number of Enemies
            int iMaxOnScreen = 5;

            // Add enemy if enough time has passed
            if (a_oContext.time.TotalGameTime - m_tsPrevEnemySpawnTime > m_tsEnemySpawnTime &&
                m_oEnemies.Count < iMaxOnScreen)
            {
                m_tsPrevEnemySpawnTime = a_oContext.time.TotalGameTime;
                Enemy.Spawn(a_oContext, m_tEnemyTex);
            }

            // Update current set of enemies (each enemy update could change set of enemies, so
            // iterate over copy of set)
            HashSet<Enemy> oCurrentEnemies = new HashSet<Enemy>(m_oEnemies);
            foreach (Enemy oEnemy in oCurrentEnemies)
            {
                oEnemy.Update(a_oContext);
            }
        }
        #endregion
    }
}