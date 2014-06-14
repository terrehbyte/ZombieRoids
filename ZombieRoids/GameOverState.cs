/// <list type="table">
/// <listheader><term>GameOverState.cs</term><description>
///     Class containing logic for game over menu
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
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

namespace ZombieRoids
{
    class GameOverState : GameState
    {
        private Button m_oExitButton = new Button();

        private GameOverState()
        {
        }

        public GameOverState(Game1 a_oMainGame)
            : base(a_oMainGame)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load game constants and assets
            GameConsts.Reload("Constants.xml");
            GameAssets.Reload(m_oContentManager);

            // End Button
            m_oExitButton.Position = GameConsts.ExitButtonPosition;
            m_oExitButton.Texture = GameAssets.ExitButtonTexture;
        }
        public override void Start()
        {
            base.Start();

            m_oGame.IsMouseVisible = true;

            // Subscribe to button events
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

            m_oExitButton.Update(oContext);
        }
        public override void Draw(GameTime a_oGameTime)
        {
            m_oSpriteBatch.Begin();

            // Draw background
            m_oSpriteBatch.Draw(GameAssets.GameOverOverlayTexture, m_rctViewport,
                                GameConsts.GameOverOverlayEndTint);

            // Draw new game button
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
        void ExitClick(Button a_oControl, Context a_oContext)
        {
            StateStack.PopState();
            StateStack.AddState(StateStack.State.MAINMENU);
        }
        #endregion
    }
}
