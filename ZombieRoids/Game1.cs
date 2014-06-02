#region Using Statements
using System;
using System.Collections.Generic;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;

        // Keyboard States
        KeyboardState curKeyboardState;
        KeyboardState prevKeyboardState;

        // Gamepad States
        GamePadState curGamepadState;
        GamePadState prevGamepadState;

        // Mouse States
        MouseState curMouseState;
        MouseState prevMouseState;

        // Move Speed
        float fPlayerMoveSpeed;

        // Background

        Texture2D tMainBackground;
        Rectangle rctBackground;
        float fBGScale = 1.0f;

        ParallaxingBackground pbgBGLayer1;
        ParallaxingBackground pbgBGLayer2;

        Texture2D tEnemyTex;
        List<Enemy> lenEnemyList;

        TimeSpan tsEnemySpawnTime;
        TimeSpan tsPrevEnemySpawnTime;

        Random rngRandom;

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
            fPlayerMoveSpeed = 8.0f;
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            rctBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);


            lenEnemyList = new List<Enemy>();
            tsPrevEnemySpawnTime = TimeSpan.Zero;

            tsEnemySpawnTime = TimeSpan.FromSeconds(1.0f);
            rngRandom = new Random();

            
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

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            player.m_v2Pos.X = MathHelper.Clamp(player.m_v2Pos.X, 0, GraphicsDevice.Viewport.Width - player.m_v2Dims.X);
            player.m_v2Pos.Y = MathHelper.Clamp(player.m_v2Pos.Y, 0, GraphicsDevice.Viewport.Height - player.m_v2Dims.Y);
        }

        void AddEnemy()
        {
            Animation aniEnemyAnim = new Animation();

            aniEnemyAnim.Initialize(tEnemyTex, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

            // RNG Enemy Pos

            Vector2 v2EnePos = new Vector2(GraphicsDevice.Viewport.Width + tEnemyTex.Width / 2,
                                           rngRandom.Next(100, GraphicsDevice.Viewport.Height - 100));

            Enemy eneTemp = new Enemy();

            eneTemp.Initialize(aniEnemyAnim, v2EnePos);

            lenEnemyList.Add(eneTemp);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - tsPrevEnemySpawnTime > tsEnemySpawnTime)
            {
                tsPrevEnemySpawnTime = gameTime.TotalGameTime;

                // Add Enemy
                AddEnemy();
            }

            for (int i = lenEnemyList.Count - 1; i >= 0; i--)
            {
                lenEnemyList[i].Update(gameTime);
                if (lenEnemyList[i].m_bActive == false)
                {
                    lenEnemyList.RemoveAt(i);
                }
            }
            
        }

        void UpdateCollision()
        {
            Rectangle rctPlayer;
            Rectangle rctEnemy;

            rctPlayer = new Rectangle((int)player.m_v2Pos.X,
                                      (int)player.m_v2Pos.Y,
                                      (int)player.m_v2Dims.X,
                                      (int)player.m_v2Dims.Y);

            for (int i = 0; i < lenEnemyList.Count; i++)
            {
                rctEnemy = new Rectangle((int)lenEnemyList[i].m_v2Pos.X,
                                         (int)lenEnemyList[i].m_v2Pos.Y,
                                         lenEnemyList[i].m_iWidth,
                                         lenEnemyList[i].m_iHeight);

                if (rctPlayer.Intersects(rctEnemy))
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
    }
}
