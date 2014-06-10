/// <list type="table">
/// <listheader><term>GameConsts.cs</term><description>
///     Class containing logic for mainmenu
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
///     Creation
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
        }
        public override void Start()
        {
            base.Start();


        }
        public override void Update(GameTime a_oGameTime)
        {
            //throw new NotImplementedException();
        }
        public override void Draw(GameTime a_oGameTime)
        {
            m_oSpriteBatch.Begin();
            
            // Draw background
            m_oSpriteBatch.Draw(GameAssets.TitleScreenTexture, m_rctViewport, Color.White);
            
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
    }
}
