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
            player = new Player();

            fPlayerMoveSpeed = 8.0f;
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            Animation aniPlayerAni = new Animation();
            Texture2D tPlayerTex = Content.Load<Texture2D>("Graphics\\shipanimation");

            //player.Initialize(Content.Load<Texture2D>("Graphics\\player"), v2playerPos);
            aniPlayerAni.Initialize(tPlayerTex, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

            Vector2 v2PlayerPos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                              GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            
            player.Initialize(aniPlayerAni, v2PlayerPos);
            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            prevGamepadState = curGamepadState;
            prevKeyboardState = curKeyboardState;
            prevMouseState = curMouseState;

            curKeyboardState = Keyboard.GetState();
            curGamepadState = GamePad.GetState(PlayerIndex.One);
            curMouseState = Mouse.GetState();



            UpdatePlayer(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);
            // Gamepad Input
            player.m_v2Pos.X += curGamepadState.ThumbSticks.Left.X * fPlayerMoveSpeed;
            player.m_v2Pos.Y += curGamepadState.ThumbSticks.Left.Y * fPlayerMoveSpeed;

            // KB || DPAD
            if (curKeyboardState.IsKeyDown(Keys.Left) ||
                curGamepadState.DPad.Left == ButtonState.Pressed)
            {
                player.m_v2Pos.X -= fPlayerMoveSpeed;
            }
            if (curKeyboardState.IsKeyDown(Keys.Right) ||
                curGamepadState.DPad.Right == ButtonState.Pressed)
            {
                player.m_v2Pos.X += fPlayerMoveSpeed;
            }
            if (curKeyboardState.IsKeyDown(Keys.Up) ||
                curGamepadState.DPad.Up == ButtonState.Pressed)
            {
                player.m_v2Pos.Y -= fPlayerMoveSpeed;
            }
            if (curKeyboardState.IsKeyDown(Keys.Down) ||
                curGamepadState.DPad.Down == ButtonState.Pressed)
            {
                player.m_v2Pos.Y += fPlayerMoveSpeed;
            }

            player.m_v2Pos.X = MathHelper.Clamp(player.m_v2Pos.X, 0, GraphicsDevice.Viewport.Width - player.m_iWidth);
            player.m_v2Pos.Y = MathHelper.Clamp(player.m_v2Pos.Y, 0, GraphicsDevice.Viewport.Height - player.iHeight);

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
            
            // Player
            player.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
