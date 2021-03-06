﻿/// <list type="table">
/// <listheader><term>MainMenuState.cs</term><description>
///     Class containing logic for mainmenu
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 11, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Fixing button tint bug
/// </description></item>
/// </list>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ZombieRoids
{
    class MainMenuState : GameState
    {
        private Button m_oStartButton = new Button();
        private Button m_oExitButton = new Button();

        private MainMenuState()
        {
        }

        public MainMenuState(Game1 a_oMainGame) : base(a_oMainGame)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load game constants and assets
            GameConsts.Reload("Constants.xml");
            GameAssets.Reload(m_oContentManager);

            // Reset button tints, now that they've been loaded
            m_oStartButton.ResetTint();
            m_oExitButton.ResetTint();

            // Start Button
            m_oStartButton.Position = GameConsts.NewGameButtonPosition;
            m_oStartButton.Texture = GameAssets.NewGameButtonTexture;

            // End Button
            m_oExitButton.Position = GameConsts.ExitButtonPosition;
            m_oExitButton.Texture = GameAssets.ExitButtonTexture;
        }
        public override void Start()
        {
            base.Start();

            m_oGame.IsMouseVisible = true;

            // Subscribe to button events
            m_oStartButton.OnClickEnd += StartClick;
            m_oExitButton.OnClickEnd += ExitClick;
        }
        public override void Update(GameTime a_oGameTime)
        {
            // Make a game context object for passing game state information to
            // entity Update functions
            Context oContext;
            oContext.time = a_oGameTime;
            oContext.viewport = m_rctViewport;
            oContext.random = m_rngRandom;
            oContext.state = this;

            m_oStartButton.Update(oContext);
            m_oExitButton.Update(oContext);
        }
        public override void Draw(GameTime a_oGameTime)
        {
            m_oSpriteBatch.Begin();
            
            // Draw background
            m_oSpriteBatch.Draw(GameAssets.TitleScreenTexture, m_rctViewport, Color.White);
            
            // Draw new game button
            m_oStartButton.Draw(m_oSpriteBatch);
            m_oExitButton.Draw(m_oSpriteBatch);

            m_oSpriteBatch.End();
        }
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void End()
        {
            base.End();
        }

        #region Logic Members
        void StartClick(Button a_oControl, Context a_oContext)
        {
            StateStack.PopState();
            StateStack.AddState(StateStack.State.GAMEPLAY);
        }
        void ExitClick(Button a_oControl, Context a_oContext)
        {
            StateStack.PopState();
        }
        #endregion
    }
}
