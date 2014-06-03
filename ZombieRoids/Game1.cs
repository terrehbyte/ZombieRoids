#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        MovingObject.Factory m_oObjectFactory;
        MovingObject m_oPlayer;
        const double mc_dShotsPerSecond = 4.0;
        const double mc_dMaxPlayerSpeed = 200.0;
        const double mc_dPlayerTurnSpeed = 4.0;
        const double mc_dShotSpeed = 400.0;
        const double mc_dShotSpinSpeed = 8.0;
        const double mc_dShotLifetime = 1.5;
        const double mc_dPlayerAcceleration = 200.0;
        const double mc_dMinTargetSpeed = 50.0;
        const double mc_dMaxTargetSpeed = 100.0;
        Random m_oRandom;
        static readonly Rectangle mc_oBoundary = new Rectangle(0, 0, 800, 480);
        bool m_bShooting;
        double m_dLastShotTime;
        bool m_bWarpKeyPressed;
        Texture2D m_oPlayerImage, m_oShotImage, m_oTargetImage, m_oFrag1Image, m_oFrag2Image, m_oFrag3Image;
        double m_dShotRadius, m_dTargetRadius, m_dFrag1Radius, m_dFrag2Radius, m_dFrag3Radius;
        HashSet<MovingObject> m_oTargets;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_oObjectFactory = new MovingObject.Factory();
            m_oObjectFactory.Boundary = mc_oBoundary;
            m_oPlayer = m_oObjectFactory.Create(null);
            m_oPlayer.Position = new Vector2(mc_oBoundary.Width / 2,
                                             mc_oBoundary.Height / 2);
            m_oPlayer.OnDraw =
                (oObject, oTime) => DrawMovingObject(oObject, oTime, m_oPlayerImage);
            m_oPlayer.OnCollision = TouchEnemy;
            m_bShooting = false;
            m_dLastShotTime = 0;
            m_bWarpKeyPressed = false;
            m_oPlayerImage = m_oShotImage = m_oTargetImage = m_oFrag1Image = m_oFrag2Image = m_oFrag3Image = null;
            m_oRandom = new Random();
            m_oTargets = new HashSet<MovingObject>();
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            m_oPlayerImage = Content.Load<Texture2D>("images/player");
            m_oPlayer.Radius = (double)(m_oPlayerImage.Width + m_oPlayerImage.Height) / 4;
            m_oShotImage = Content.Load<Texture2D>("images/shot");
            m_dShotRadius = (double)(m_oShotImage.Width + m_oShotImage.Height) / 4;
            m_oTargetImage = Content.Load<Texture2D>("images/target");
            m_dTargetRadius = (double)(m_oTargetImage.Width + m_oTargetImage.Height) / 4;
            m_oFrag1Image = Content.Load<Texture2D>("images/fragment1");
            m_dFrag1Radius = (double)(m_oFrag1Image.Width + m_oFrag1Image.Height) / 4;
            m_oFrag2Image = Content.Load<Texture2D>("images/fragment2");
            m_dFrag2Radius = (double)(m_oFrag2Image.Width + m_oFrag2Image.Height) / 4;
            m_oFrag3Image = Content.Load<Texture2D>("images/fragment3");
            m_dFrag3Radius = (double)(m_oFrag3Image.Width + m_oFrag3Image.Height) / 4;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            m_oPlayerImage = null;
            m_oShotImage = null;
            m_oTargetImage = null;
            m_oFrag1Image = null;
            m_oFrag2Image = null;
            m_oFrag3Image = null;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            ControlMotion(gameTime);
            ControlShooting(gameTime);
            ControlWarp();
            m_oObjectFactory.UpdateAll(gameTime);
            if (m_oTargets.Count == 0)
            {
                SpawnTargets(4, m_oPlayer.Radius);
            }
            base.Update(gameTime);
        }

        // player acceleration
        protected void ControlMotion(GameTime gameTime)
        {
            double dSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                m_oPlayer.Rotation -= dSeconds * mc_dPlayerTurnSpeed;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                m_oPlayer.Rotation += dSeconds * mc_dPlayerTurnSpeed;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                double dBoostPower =
                    Math.Min(mc_dMaxPlayerSpeed - m_oPlayer.Velocity.Length(),
                             dSeconds * mc_dPlayerAcceleration);
                Vector2 v2Boost = m_oPlayer.Direction;
                v2Boost.X *= (float)dBoostPower;
                v2Boost.Y *= (float)dBoostPower;
                m_oPlayer.Velocity += v2Boost;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                double dBreakPower =
                    Math.Min(m_oPlayer.Velocity.Length(),
                             dSeconds * mc_dPlayerAcceleration);
                Vector2 v2Break = m_oPlayer.Direction;
                v2Break.X *= (float)dBreakPower;
                v2Break.Y *= (float)dBreakPower;
                m_oPlayer.Velocity -= v2Break;
            }
        }

        // player shoots
        protected void ControlShooting(GameTime gameTime)
        {
            bool nowShooting =
                (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
                 Keyboard.GetState().IsKeyDown(Keys.Space));
            if (nowShooting)
            {
                if (!m_bShooting)
                {
                    m_dLastShotTime = gameTime.TotalGameTime.TotalSeconds;
                }
                else
                {
                    double dTimeSinceLastShot = gameTime.TotalGameTime.TotalSeconds -
                                                m_dLastShotTime;
                    if (dTimeSinceLastShot > 1.0 / mc_dShotsPerSecond)
                    {
                        m_dLastShotTime = gameTime.TotalGameTime.TotalSeconds;
                    }
                }
                if (m_dLastShotTime == gameTime.TotalGameTime.TotalSeconds)
                {
                    double dExpirationDate =
                        gameTime.TotalGameTime.TotalSeconds + mc_dShotLifetime;
                    m_oObjectFactory.Create(m_oPlayer, m_dShotRadius,
                        new Vector2(m_oPlayer.Direction.X * (float)m_oPlayer.Radius,
                                    m_oPlayer.Direction.Y * (float)m_oPlayer.Radius)
                        + m_oPlayer.Position,
                        new Vector2(m_oPlayer.Direction.X * (float)mc_dShotSpeed,
                                    m_oPlayer.Direction.Y * (float)mc_dShotSpeed),
                        DrawShot, Shoot,
                        (oSelf, oTime) => Expire(oSelf, oTime, dExpirationDate));
                }
            }
            m_bShooting = nowShooting;
        }

        // If the game time is at or past the expiration date, recycle the object
        protected void Expire(MovingObject a_oSelf, GameTime gameTime, double a_dExpirationDate)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= a_dExpirationDate)
            {
                a_oSelf.Recycle();
            }
        }

        // If a shot hits something
        protected void Shoot(MovingObject a_oSelf, MovingObject a_oTarget, GameTime a_oGameTime)
        {
            if (null != a_oSelf && null != a_oTarget &&
                !Object.ReferenceEquals(m_oPlayer, a_oTarget) &&
                !Object.ReferenceEquals(m_oPlayer, a_oTarget.Owner))
            {
                a_oSelf.Recycle();
                // TODO increment score
            }
        }

        // If an enemy is hit by something
        protected void BeShot(MovingObject a_oSelf, MovingObject a_oProjectile, GameTime a_oGameTime)
        {
            if (null != a_oSelf && null != a_oProjectile &&
                (Object.ReferenceEquals(m_oPlayer, a_oProjectile) ||
                 Object.ReferenceEquals(m_oPlayer, a_oProjectile.Owner)))
            {
                a_oSelf.Recycle();
                m_oTargets.Remove(a_oSelf);
                // TODO fragment
            }
        }

        // If the player bumps into an enemy
        protected void TouchEnemy(MovingObject a_oSelf, MovingObject a_oEnemy, GameTime a_oGameTime)
        {
            if (null != a_oSelf && null != a_oEnemy &&
                !Object.ReferenceEquals(m_oPlayer, a_oEnemy.Owner))
            {
                // TODO lose life
            }
        }

        // player warps to random location
        protected void ControlWarp()
        {
            if ((GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed ||
                 Keyboard.GetState().IsKeyDown(Keys.Tab)))
            {
                if (!m_bWarpKeyPressed)
                {
                    m_bWarpKeyPressed = true;
                    m_oPlayer.Position =
                        new Vector2(mc_oBoundary.X + (float)m_oRandom.NextDouble() * mc_oBoundary.Width,
                                    mc_oBoundary.Y + (float)m_oRandom.NextDouble() * mc_oBoundary.Height);
                }
            }
            else
            {
                m_bWarpKeyPressed = false;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            m_oObjectFactory.DrawVisible(gameTime);
            base.Draw(gameTime);
        }

        protected void DrawMovingObject(MovingObject a_oObject, GameTime a_oGameTime, Texture2D a_oImage)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(a_oImage, a_oObject.Position,
                             a_oImage.Bounds, Color.White,
                             (float)a_oObject.Rotation,
                             new Vector2(a_oImage.Width / 2,
                                         a_oImage.Height / 2),
                             1, SpriteEffects.None, 1);
            spriteBatch.End();
        }

        protected void DrawShot(MovingObject a_oObject, GameTime a_oGameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(m_oShotImage, a_oObject.Position,
                             m_oShotImage.Bounds, Color.White,
                             (float)(a_oGameTime.TotalGameTime.TotalSeconds * mc_dShotSpinSpeed),
                             new Vector2(m_oShotImage.Width / 2,
                                         m_oShotImage.Height / 2),
                             1, SpriteEffects.None, 1);
            spriteBatch.End();
        }

        protected void SpawnTargets(int a_iCount, double a_dBufferRadius)
        {
            double dTooClose = m_oPlayer.Radius + m_dTargetRadius + a_dBufferRadius;
            dTooClose *= dTooClose;
            while (m_oTargets.Count < a_iCount)
            {
                Vector2 v2Position = new Vector2();
                do
                {
                    v2Position.X = (float)(mc_oBoundary.X + m_oRandom.NextDouble() * mc_oBoundary.Width);
                    v2Position.Y = (float)(mc_oBoundary.Y + m_oRandom.NextDouble() * mc_oBoundary.Height);
                } while ((m_oPlayer.Position - v2Position).LengthSquared() < dTooClose);
                double dAngle = m_oRandom.NextDouble() * Math.PI * 2;
                double dSpeed = mc_dMinTargetSpeed + (mc_dMaxTargetSpeed - mc_dMinTargetSpeed) * m_oRandom.NextDouble();
                m_oTargets.Add(
                    m_oObjectFactory.Create(null, m_dTargetRadius, v2Position,
                                            new Vector2((float)(dSpeed * Math.Cos(dAngle)),
                                                        (float)(dSpeed * Math.Sin(dAngle))),
                                            (oObject, oTime) => DrawMovingObject(oObject, oTime, m_oTargetImage),
                                            BeShot));
            }
        }
    }
}
