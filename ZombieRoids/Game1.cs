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
///     Merged with dev for @emlowry complete refactor
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Point m_ptScreenSize;

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

        int iStartLives = 3;

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

            m_ptScreenSize = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
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
            Texture2D tBulletTex = Content.Load<Texture2D>("Graphics\\laser");

            Vector2 v2PlayerPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                              GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);

            player.BulletTexture = tBulletTex;
            player.Initialize(tPlayerTex, v2PlayerPos);
            player.m_iLives = iStartLives;

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
        }

        Enemy AddEnemy()
        {
            Console.WriteLine("Spawning new enemy.");
            Texture2D tEnemyTex = Content.Load<Texture2D>("Graphics\\mine");

            Enemy eneTemp = new Enemy();
            Random rngGennie = new Random();

            int iOffset = 100;  // Offset in Screen Space
            int iMinVel = 100;    // Minimum Velocity for any axis
            int iMaxVel = 200;    // Maximum Velocity for any axis

            // RNG Enemy Pos
            Vector2 v2EnePos = new Vector2(rngGennie.Next(-iOffset, (int)m_ptScreenSize.X + iOffset),
                                           rngGennie.Next(-iOffset, (int)m_ptScreenSize.Y + iOffset));

            // Correct Y Offset if it will spawn in the middle
            if (v2EnePos.X > 0 &&
                v2EnePos.X < m_ptScreenSize.X)
            {
                if (v2EnePos.Y > m_ptScreenSize.Y / 2)
                {
                    v2EnePos.Y = -iOffset;
                }
                else
                {
                    v2EnePos.Y = m_ptScreenSize.Y + iOffset;
                }
            }

            // H-Speed
            // If Left
            if (v2EnePos.X < 0)
            {
                eneTemp.Velocity = new Vector2(rngGennie.Next(iMinVel, iMaxVel), eneTemp.Velocity.Y);
            }
            // Right
            else
            {
                eneTemp.Velocity = new Vector2(rngGennie.Next(-iMaxVel, -iMinVel), eneTemp.Velocity.Y);
            }
            

            // V-Speed
            // If Top
            if (v2EnePos.Y < 0)
            {
                eneTemp.Velocity = new Vector2(eneTemp.Velocity.X, rngGennie.Next(iMinVel, iMaxVel));
            }
            // RIght
            else
            {
                eneTemp.Velocity = new Vector2(eneTemp.Velocity.X, rngGennie.Next(-iMaxVel, -iMinVel));
            }

            Debug.Assert(eneTemp.Velocity.Y != 0);

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

                // If 
                if (lenEnemyList[i].Alive == false)
                {
                    // Record number of children to spawn in place of dead parent
                    int iChildren = lenEnemyList[i].FragmentCount;

                    // If there are children to spawn
                    if (iChildren != 0)
                    {
                        // Instance RNGgennies
                        Random rngXOffset = new Random();
                        Random rngYOffset = new Random();

                        Vector2 v2OrigPos = lenEnemyList[i].Position;
                        Vector2 v2OrigVel = lenEnemyList[i].Velocity;

                        for (int j = 0; j < iChildren; j++)
                        {
                            Enemy eneNewFoe = AddEnemy();
                            
                            // Influence Child Position
                            eneNewFoe.Position = v2OrigPos + new Vector2(rngXOffset.Next(-50, 45),
                                                                        rngYOffset.Next(-50, 55));

                            // Influence Child Velocity
                            eneNewFoe.Velocity = v2OrigVel + new Vector2(rngXOffset.Next(-1, 1),
                                                             rngYOffset.Next(-1, 1));

                            // Decrement the number of children that will spawn from this child
                            eneNewFoe.FragmentCount = iChildren - 1;
                        }
                    }

                    /// Remove the dead enemy
                    lenEnemyList.RemoveAt(i);
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
                    if (player.m_lbulBullets[j].Active)
                    {
                        if (Collision.CheckCollision(player.m_lbulBullets[j].Collider,
                                                     lenEnemyList[i].Collider))
                        {
                            lenEnemyList[i].HitPoints = 0;
                            player.m_lbulBullets[j].Active = false;
                        }
                    }
                }

                // If Player is still in play
                if (player.Active)
                {
                    // If Player-Enemy Collision
                    if (Collision.CheckCollision(player.Collider,
                                                 lenEnemyList[i].Collider))
                    {
                        // If Not Invulnerable
                        if (!player.m_bInvuln)
                        {
                            // Inflict damage on the player
                            player.HitPoints -= lenEnemyList[i].Damage;
                        }

                        // Kill Enemy
                        lenEnemyList[i].HitPoints = 0;
                    }
                }
            }
        }
        #endregion
    }
}
