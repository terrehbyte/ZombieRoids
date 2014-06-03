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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Vector2 v2ScreenDims;

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
            //player.m_v2Pos.X = MathHelper.Clamp(player.m_v2Pos.X, 0 + player.m_v2Dims.X / 2, GraphicsDevice.Viewport.Width - player.m_v2Dims.X / 2);
            //player.m_v2Pos.Y = MathHelper.Clamp(player.m_v2Pos.Y, 0 + player.m_v2Dims.Y / 2, GraphicsDevice.Viewport.Height - player.m_v2Dims.Y / 2);
        }

        Enemy AddEnemy()
        {
            Texture2D tEnemyTex = Content.Load<Texture2D>("Graphics\\mine");

            Enemy eneTemp = new Enemy();
            Random rngGennie = new Random();

            int iOffset = 100;  // Offset in Screen Space
            int iMinVel = 1;    // Minimum Velocity for any axis
            int iMaxVel = 2;    // Maximum Velocity for any axis

            // RNG Enemy Pos
            Vector2 v2EnePos = new Vector2(rngGennie.Next(-iOffset, (int)v2ScreenDims.X + iOffset),
                                           rngGennie.Next(-iOffset, (int)v2ScreenDims.Y + iOffset));

            // Correct Y Offset if it will spawn in the middle
            if (v2EnePos.X > 0 &&
                v2EnePos.X < v2ScreenDims.X)
            {
                if (v2EnePos.Y > v2ScreenDims.Y / 2)
                {
                    v2EnePos.Y = -iOffset;
                }
                else
                {
                    v2EnePos.Y = v2ScreenDims.Y + iOffset;
                }
            }

            // H-Speed
            // If Left
            if (v2EnePos.X < 0)
            {
                eneTemp.m_v2Vel.X = rngGennie.Next(iMinVel, iMaxVel);
            }
            // Right
            else
            {
                eneTemp.m_v2Vel.X = rngGennie.Next(-iMaxVel, -iMinVel);
            }
            

            // V-Speed
            // If Top
            if (v2EnePos.Y < 0)
            {
                eneTemp.m_v2Vel.Y = rngGennie.Next(iMinVel, iMaxVel);
            }
            // RIght
            else
            {
                eneTemp.m_v2Vel.Y = rngGennie.Next(-iMaxVel, -iMinVel);
            }

            Debug.Assert(eneTemp.m_v2Vel.Y != 0);

            // Initialize Enemy
            eneTemp.Initialize(tEnemyTex, v2EnePos);

            // Add Enemy to List
            lenEnemyList.Add(eneTemp);

            // Return Added Enemy
            return lenEnemyList.Last();
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            int iMaxOnScreen = 5;

            if (gameTime.TotalGameTime - tsPrevEnemySpawnTime > tsEnemySpawnTime &&
                lenEnemyList.Count < iMaxOnScreen)
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
                    Vector2 v2OrigPos = lenEnemyList[i].m_v2Pos;
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
                            eneNewFoe.m_v2Pos = v2OrigPos + new Vector2(rngXOffset.Next(-50, 45),
                                                                        rngYOffset.Next(-50, 55));
                            eneNewFoe.m_v2Vel = v2OrigVel;
                            eneNewFoe.m_v2Vel += new Vector2(rngXOffset.Next(-1, 1),
                                                             rngYOffset.Next(-1, 1));

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
                            lenEnemyList[i].m_bAlive = false;
                            player.m_lbulBullets[j].m_bActive = false;
                        }
                    }
                }

                if (player.m_bActive)
                {
                    if (Collision.CheckCollision(player.m_rotrctCollider,
                                                 lenEnemyList[i].m_rctCollider))
                    {
                        lenEnemyList[i].m_bAlive = false;
                    }
                }
            }
        }
        #endregion
    }
}
