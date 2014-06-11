/// <list type="table">
/// <listheader><term>PlayState.cs</term><description>
///     Class containing logic for gameplay for fighting zombies
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
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Added logic for moving to gameplay from mainmenu
/// </description></item>
/// </list>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Utility;
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    public class PlayState : GameState
    {
        #region Props & Vars
        
        // Score varible
        public int Score { get; set; }
        private int m_iNextLifeScore;        

        // Background images
        private ParallaxingBackground m_pbgBGLayer1;
        private ParallaxingBackground m_pbgBGLayer2;

        // Enemies
        private TimeSpan m_tsTimeSinceEnemySpawn;
        public HashSet<Enemy> Enemies { get { return m_oEnemies; } }
        private HashSet<Enemy> m_oEnemies = new HashSet<Enemy>();

        // Enemy Wave Count
        private int m_iEnemyWaveCurrent = 0;
        private int m_iEnemyWaveQueue = 0;
        private TimeSpan m_tsTimeUntilNextWave;

        // Player
        private Player m_oPlayer;

        // Background music loop
        private SoundEffectInstance m_oBGM;

        // Game is paused
        public bool Paused { get; set; }

        // Game over
        public bool GameOver
        {
            get { return !m_oPlayer.Alive && 0 == m_oPlayer.Lives; }
        }
        private TimeSpan m_tsTimeUntilOnGameOver;

        // Pause key is pressed
        private bool m_bPauseKeyDown = false;

        #endregion

        private PlayState()
        {
        }

        public PlayState(Game1 a_oMainGame) : base(a_oMainGame)
        {
        }

        #region GameState Members

        public override void Start()
        {
            base.Start();

            // Initialize basic game properties
            m_oGame.IsMouseVisible = true;
            m_rngRandom = new Random();

            // Track enemies
            m_oEnemies = new HashSet<Enemy>();
        }

        public override void End()
        {
            base.End();

            m_oBGM.Stop();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load game constants and assets
            GameConsts.Reload("Constants.xml");
            GameAssets.Reload(m_oContentManager);

            Vector2 v2PlayerPos = new Vector2(m_rctViewport.Center.X, m_rctViewport.Center.Y);

            // Create player object
            m_oPlayer = new Player();
            m_oPlayer.BulletTexture = GameAssets.BulletTexture;
            m_oPlayer.Initialize(GameAssets.PlayerTexture, v2PlayerPos);

            // Create parallaxing overlays
            m_pbgBGLayer1 = new ParallaxingBackground();
            m_pbgBGLayer1.Initialize(GameAssets.ParallaxTextureOne, m_rctViewport.Width,
                                     m_rctViewport.Height, GameConsts.Overlay1Speed);
            m_pbgBGLayer2 = new ParallaxingBackground();
            m_pbgBGLayer2.Initialize(GameAssets.ParallaxTextureTwo, m_rctViewport.Width,
                                   m_rctViewport.Height, GameConsts.Overlay2Speed);

            //Load background sound
            m_oBGM = GameAssets.BackgroundMusic.CreateInstance();
            m_oBGM.IsLooped = true;
            m_oBGM.Play();

            // Now that everything is loaded, start playing
            NewGame();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime a_oGameTime)
        {
            CheckPauseKey();

            if (GameOver)
            {
                m_tsTimeUntilOnGameOver -= a_oGameTime.ElapsedGameTime;
                if (TimeSpan.Zero >= m_tsTimeUntilOnGameOver)
                {
                    OnGameOver(a_oGameTime);
                }
            }

            // If not paused, update game state
            if (!Paused)
            {
                // Make a game context object for passing game state information to
                // entity Update functions
                Context oContext;
                oContext.time = a_oGameTime;
                oContext.viewport = m_rctViewport;
                oContext.random = m_rngRandom;
                oContext.state = this;

                // Update player and enemies
                m_oPlayer.Update(oContext);
                UpdateEnemies(oContext);

                // If the new life point threshold has been passed,
                if (Score == m_iNextLifeScore)
                {
                    // add a life,
                    ++m_oPlayer.Lives;

                    /// set a new threshold,
                    m_iNextLifeScore += GameConsts.LifeGainPoints;

                    // and play the new life sound
                    GameAssets.LifeGainSound.Play();
                }

                // Update parallaxing background
                m_pbgBGLayer1.Update(a_oGameTime);
                m_pbgBGLayer2.Update(a_oGameTime);
            }
        }

        public override void Draw(GameTime a_oGameTime)
        {
            m_oSpriteBatch.Begin();

            // Draw background
            m_oSpriteBatch.Draw(GameAssets.BackgroundTexture, m_rctViewport, Color.White);


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

            m_pbgBGLayer1.Draw(m_oSpriteBatch);
            m_pbgBGLayer2.Draw(m_oSpriteBatch);

            // - DRAW UI -

            // Draw score
            m_oSpriteBatch.DrawString(GameAssets.ScoreFont, "Score: " + Score,
                                      GameConsts.ScorePosition, Color.Black);

            // Draw lives
            m_oSpriteBatch.DrawString(GameAssets.ScoreFont, "Lives: " + m_oPlayer.Lives,
                                      GameConsts.LivesPosition, Color.Black);

            // Draw enemy count
            m_oSpriteBatch.DrawString(GameAssets.ScoreFont,
                                      "Enemies Remaining: " + m_oEnemies.Count,
                                      GameConsts.EnemyCountPosition, Color.Black);

            // If game over, draw game over overlay
            if (GameOver)
            {
                m_oSpriteBatch.Draw(GameAssets.GameOverOverlayTexture,
                                    m_rctViewport, GameConsts.GameOverOverlayTint);
            }
            // If paused, draw pause overlay
            else if (Paused)
            {
                m_oSpriteBatch.Draw(GameAssets.PauseOverlayTexture,
                                    m_rctViewport, GameConsts.PauseOverlayTint);
            }

            // Finish drawing
            m_oSpriteBatch.End();
        }



        #endregion

        #region Logic Methods

        /// <summary>
        /// Checks to see if the pause key has been pressed and reacts appropriately
        /// </summary>
        protected void CheckPauseKey()
        {
            // Check for pause/unpause
            KeyboardState kbCurKeys = Keyboard.GetState();
            if (kbCurKeys.IsKeyDown(Keys.P) ||
                kbCurKeys.IsKeyDown(Keys.Space) ||
                kbCurKeys.IsKeyDown(Keys.NumLock))
            {
                m_bPauseKeyDown = true;
            }
            else if (m_bPauseKeyDown)
            {
                m_bPauseKeyDown = false;
                if (GameOver)
                {
                    // On GameOver screen, exits the GameOver screen
                    m_tsTimeUntilOnGameOver = TimeSpan.Zero;
                }
                else
                {
                    // Otherwise, toggle pause state
                    Paused = !Paused;
                }
            }
        }

        private void UpdateEnemies(GameState.Context a_oContext)
        {
            // Calculate time since last spawn
            m_tsTimeSinceEnemySpawn += a_oContext.time.ElapsedGameTime;
            m_tsTimeUntilNextWave -= a_oContext.time.ElapsedGameTime;

            // Has enough time passed to spawn another enemy?
            if (m_tsTimeSinceEnemySpawn > GameConsts.SpawnDelay)
            {
                // Are there any enemies left to spawn?
                if (m_iEnemyWaveQueue > 0 &&
                    TimeSpan.Zero > m_tsTimeUntilNextWave)
                {
                    m_tsTimeSinceEnemySpawn = TimeSpan.Zero;
                    Enemy.Spawn(a_oContext, GameAssets.ZombieTexture);

                    // Decrement queue
                    m_iEnemyWaveQueue--;
#if DEBUG
                    Console.WriteLine(m_iEnemyWaveQueue + " enemies left in queue.");
#endif
                }
            }

            // Update current set of enemies (each enemy update could change set of enemies, so
            // iterate over copy of set)
            HashSet<Enemy> oCurrentEnemies = new HashSet<Enemy>(m_oEnemies);
            foreach (Enemy oEnemy in oCurrentEnemies)
            {
                oEnemy.Update(a_oContext);

            }

            // Check wave end conditions
            // - no more enemies
            // - no more queued enemies
            // If so, assign the next wave time
            if (m_oEnemies.Count == 0 && m_iEnemyWaveQueue == 0)
            {
                // Assign values for next wave
                if (TimeSpan.Zero > m_tsTimeUntilNextWave)
                {
                    // Calculate new queue
                    // queue = base + (waves * incPerWave)
                    m_iEnemyWaveQueue = GameConsts.InitialWaveSize +
                        m_iEnemyWaveCurrent * GameConsts.WaveSizeIncrement;

                    // Increment current wave
                    m_iEnemyWaveCurrent++;

                    // Assign time until next wave to start
                    m_tsTimeUntilNextWave = GameConsts.WaveDelay;

#if DEBUG
                    Console.WriteLine("ROUND " + m_iEnemyWaveCurrent);
                    Console.WriteLine("QUEUED: " + m_iEnemyWaveQueue);
#endif
                }
            }
        }

        /// <summary>
        /// Reset variables to start a new game
        /// </summary>
        public void NewGame()
        {
            Score = 0;
            m_iNextLifeScore = GameConsts.LifeGainPoints;
            m_tsTimeUntilOnGameOver = GameConsts.GameOverDuration;
            m_tsTimeSinceEnemySpawn = TimeSpan.Zero;
            m_tsTimeUntilNextWave = GameConsts.WaveDelay;
            m_iEnemyWaveCurrent = 0;
            m_iEnemyWaveQueue = 0;
            m_oEnemies.Clear();
            m_oPlayer.Reset(new Vector2(m_rctViewport.Center.X,
                                        m_rctViewport.Center.Y));
            Paused = false;
        }

        /// <summary>
        /// After showing the GameOver screen, call this
        /// </summary>
        /// <param name="gameTime"></param>
        protected void OnGameOver(GameTime gameTime)
        {
            StateStack.PopState();
            StateStack.AddState(StateStack.State.GAMEOVER);
        }
        #endregion

    }
}
