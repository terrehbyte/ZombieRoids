using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Utility;

namespace ZombieRoids
{
    public class PlayState : GameState
    {
        #region Props & Vars
        
        // Score varible
        private int m_iScore = 0;
        private int m_iNextLifeScore;        

        // Background images
        private ParallaxingBackground m_pbgBGLayer1;
        private ParallaxingBackground m_pbgBGLayer2;

        // Enemies
        private TimeSpan m_tsPrevEnemySpawnTime;
        private HashSet<Enemy> m_oEnemies;

        // Enemy Wave Count
        private int m_iEnemyWaveCurrent = 0;
        private int m_iEnemyWaveQueue = 0;
        private TimeSpan m_tsEnemyWaveNext;

        // Player
        private Player m_oPlayer;

        // Background music loop
        private SoundEffectInstance m_oBGM;

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
            m_tsPrevEnemySpawnTime = TimeSpan.Zero;
        }

        public override void End()
        {
            base.End();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load game constants and assets
            GameConsts.Reload("Constants.xml");
            GameAssets.Reload(m_oContentManager);

            // Load player graphics
            Texture2D tPlayerTex = GameAssets.PlayerTexture;
            Texture2D tBulletTex = GameAssets.BulletTexture;
            Vector2 v2PlayerPos = new Vector2(m_rctViewport.Center.X, m_rctViewport.Center.Y);

            // Create player object
            m_oPlayer = new Player();
            m_oPlayer.BulletTexture = tBulletTex;
            m_oPlayer.Initialize(GameAssets.PlayerTexture, v2PlayerPos);
            m_oPlayer.Lives = GameConsts.PlayerLives;
            m_iNextLifeScore = m_iScore + GameConsts.LifeGainPoints;

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
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime a_oGameTime)
        {
            // Make a game context object for passing game state information to
            // entity Update functions
            Context oContext;
            oContext.time = a_oGameTime;
            oContext.viewport = m_rctViewport;
            oContext.random = m_rngRandom;
            oContext.enemies = m_oEnemies;
            oContext.score =
                new Ref<int>((() => m_iScore),
                             ((int a_iScore) => m_iScore = a_iScore));


            // Update player and enemies
            m_oPlayer.Update(oContext);
            UpdateEnemies(oContext);

            // If the new life point threshold has been passed,
            if (m_iScore == m_iNextLifeScore)
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
            m_oSpriteBatch.DrawString(GameAssets.ScoreFont, "Score: " + m_iScore,
                                      GameConsts.ScorePosition, Color.Black);

            // Draw lives
            m_oSpriteBatch.DrawString(GameAssets.ScoreFont, "Lives: " + m_oPlayer.Lives,
                                      GameConsts.LivesPosition, Color.Black);

            // Draw enemy count
            m_oSpriteBatch.DrawString(GameAssets.ScoreFont,
                                      "Enemies Remaining: " + m_oEnemies.Count,
                                      GameConsts.EnemyCountPosition, Color.Black);


            // Finish drawing
            m_oSpriteBatch.End();
        }

        #endregion

        #region Logic Methods
        private void UpdateEnemies(GameState.Context a_oContext)
        {
            // Calculate time since last spawn
            TimeSpan tsEnemySpawnTimeElapsed = a_oContext.time.TotalGameTime - m_tsPrevEnemySpawnTime;

            // Has enough time passed to spawn another enemy?
            if (tsEnemySpawnTimeElapsed > GameConsts.SpawnDelay)
            {
                // Are there any enemies left to spawn?
                if (m_iEnemyWaveQueue > 0 &&
                    a_oContext.time.TotalGameTime > m_tsEnemyWaveNext)
                {
                    m_tsPrevEnemySpawnTime = a_oContext.time.TotalGameTime;
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
            //  - no more enemies
            //  - no more queued enemies
            // If so, assign the next wave time
            if (m_oEnemies.Count == 0 && m_iEnemyWaveQueue == 0)
            {
                // Assign values for next wave
                if (a_oContext.time.TotalGameTime > m_tsEnemyWaveNext)
                {
                    // Calculate new queue
                    // queue = base + (waves * incPerWave)
                    m_iEnemyWaveQueue = GameConsts.InitialWaveSize +
                        m_iEnemyWaveCurrent * GameConsts.WaveSizeIncrement;

                    // Increment current wave
                    m_iEnemyWaveCurrent++;

                    // Assign time until next wave to start
                    m_tsEnemyWaveNext = a_oContext.time.TotalGameTime + GameConsts.WaveDelay;

#if DEBUG
                    Console.WriteLine("ROUND " + m_iEnemyWaveCurrent);
                    Console.WriteLine("QUEUED: " + m_iEnemyWaveQueue);
#endif
                }
            }
        }
        #endregion

    }
}
