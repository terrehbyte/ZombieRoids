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
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 3, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Sprite class
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
#endregion

namespace ZombieRoids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region Props & Vars
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Vector2 v2ScreenDims;

        Texture2D tMainBackground;
        Rectangle rctBackground;

        ParallaxingBackground pbgBGLayer1;
        ParallaxingBackground pbgBGLayer2;

        Texture2D tEnemyTex;
        
        TimeSpan tsEnemySpawnTime;
        TimeSpan tsPrevEnemySpawnTime;

        Random rngRandom;

        Player player;
        List<Enemy> lenEnemyList;

        Vector2 v2BulletGraveyard = new Vector2(-100, -100);

        #endregion

        #region FrameworkMethods

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here

            base.Initialize();

            // Player
            rctBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            lenEnemyList = new List<Enemy>();
            tsPrevEnemySpawnTime = TimeSpan.Zero;

            tsEnemySpawnTime = TimeSpan.FromSeconds(1.0f);
            rngRandom = new Random();

            IsMouseVisible = true;

            // Initialize Engine Singleton
            Engine.Instance.m_Game = this;

            v2ScreenDims = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player = new Player();

            Texture2D tPlayerTex = Content.Load<Texture2D>("Graphics\\player");

            Vector2 v2PlayerPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                              GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            player.Initialize(tPlayerTex, v2PlayerPos);

            // Background
            pbgBGLayer1 = new ParallaxingBackground();
            pbgBGLayer2 = new ParallaxingBackground();

            // BACKGROUND
            pbgBGLayer1.Initialize(Content, "Graphics/bgLayer1", GraphicsDevice.Viewport.Width,
                                   GraphicsDevice.Viewport.Height, -1);

            pbgBGLayer2.Initialize(Content, "Graphics/bgLayer2", GraphicsDevice.Viewport.Width,
                                   GraphicsDevice.Viewport.Height, -2);

            tMainBackground = Content.Load<Texture2D>("Graphics/mainbackground");

            // Enemy
            tEnemyTex = Content.Load<Texture2D>("Graphics/mineAnimation");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UpdatePlayer(gameTime);
            UpdateEnemies(gameTime);
            UpdateCollision();

            pbgBGLayer1.Update(gameTime);
            pbgBGLayer2.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            // BG
            spriteBatch.Draw(tMainBackground, rctBackground, Color.White);

            pbgBGLayer1.Draw(spriteBatch);
            pbgBGLayer2.Draw(spriteBatch);

            // Player
            player.Draw(spriteBatch);

            for (int i = 0; i < lenEnemyList.Count; i++)
            {
                lenEnemyList[i].Draw(spriteBatch);
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

        #region Logic Methods
        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            // Keep player in window
            player.Left = MathHelper.Clamp(player.Left, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Top = MathHelper.Clamp(player.Top, 0, GraphicsDevice.Viewport.Height - player.Height);

            // Check bullets
            for (int i = 0; i < player.m_lbulBullets.Count; i++)
            {
                if (player.m_lbulBullets[i].CheckOffscreen(v2ScreenDims))
                {
                    player.m_lbulBullets[i].m_bActive = false;
                }
            }
        }

        Enemy AddEnemy()
        {
            Texture2D tEnemyTex = Content.Load<Texture2D>("Graphics\\mine");

            // RNG Enemy Pos
            Vector2 v2EnePos = new Vector2(GraphicsDevice.Viewport.Width + tEnemyTex.Width / 2,
                                           rngRandom.Next(100, GraphicsDevice.Viewport.Height - 100));

            Enemy eneTemp = new Enemy();
            eneTemp.m_v2Vel = new Vector2(-4f, 0);

            eneTemp.Initialize(tEnemyTex, v2EnePos);

            lenEnemyList.Add(eneTemp);

            return lenEnemyList.Last();
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - tsPrevEnemySpawnTime > tsEnemySpawnTime)
            {
                tsPrevEnemySpawnTime = gameTime.TotalGameTime;

                // Add Enemy
                AddEnemy();
            }

            // Prune dead enemies
            for (int i = lenEnemyList.Count - 1; i >= 0; i--)
            {
                lenEnemyList[i].Update(gameTime);
                if (lenEnemyList[i].m_bActive == false)
                {
                    int iChildren = lenEnemyList[i].m_iDivisions;
                    Vector2 v2OrigPos = lenEnemyList[i].Position;
                    Vector2 v2OrigVel = lenEnemyList[i].m_v2Vel;

                    lenEnemyList.RemoveAt(i);

                    


                    Random rngXOffset = new Random();
                    Random rngYOffset = new Random();

                    if (iChildren != 0)
                    {
                        for (int j = 0; j < iChildren; j++)
                        {
                            Enemy eneNewFoe = AddEnemy();
                            // Influence new Position
                            eneNewFoe.Position = v2OrigPos + new Vector2(rngXOffset.Next(-50, 45),
                                                                        rngYOffset.Next(-50, 55));

                            eneNewFoe.m_v2Vel = new Vector2(rngXOffset.Next((int)v2OrigVel.X, -1),
                                                             rngYOffset.Next(-2, 2));

                            eneNewFoe.m_iDivisions = iChildren - 1;
                        }
                    }
                }
            }

        }

        void UpdateCollision()
        {
            for (int i = 0; i < lenEnemyList.Count; i++)
            {
                // Check Against bullet
                for (int j = 0; j < player.m_lbulBullets.Count; j++)
                {
                    // Only check if the bullet is active
                    if (player.m_lbulBullets[j].m_bActive)
                    {
                        if (Collision.CheckCollision(player.m_lbulBullets[j].m_rotrctCollider,
                                                     lenEnemyList[i].m_rctCollider))
                        {
                            lenEnemyList[i].m_iHealth = 0;
                            player.m_lbulBullets[j].m_bActive = false;
                        }
                    }
                }

                if (player.m_bActive)
                {
                    if (Collision.CheckCollision(player.m_rotrctCollider,
                                                 lenEnemyList[i].m_rctCollider))
                    {
                        player.m_iHealth -= lenEnemyList[i].m_iDamage;

                        lenEnemyList[i].m_iHealth = 0;
                    }

                    if (player.m_iHealth <= 0)
                    {
                        player.m_bActive = false;
                    }
                }
            }
        }
        #endregion
    }
}
