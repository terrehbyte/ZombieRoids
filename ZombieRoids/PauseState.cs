/// <list type="table">
/// <listheader><term>PauseState.cs</term><description>
///     Class containing logic for pause menu
/// </description></listheader>
/// <item><term>Author</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 11, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 11, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Adding pause screen fade-in time
/// </description></item>
/// </list>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    public class PauseState : GameState
    {
        private GameState m_oPausedState;
        private bool m_bUnpauseKeyDown;
        private SoundEffectInstance m_oBGM;
        private TimeSpan m_tsTimeUntilFadedIn;

        public PauseState(Game1 a_oMainGame, GameState a_oPausedState)
            : base(a_oMainGame)
        {
            m_oPausedState = a_oPausedState;
            m_bUnpauseKeyDown = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load game constants and assets
            if (0 == StateStack.StackCount)
            {
                GameConsts.Reload("Constants.xml");
                GameAssets.Reload(m_oContentManager);
            }

            // TODO - change to pause menu music
            m_oBGM = GameAssets.PauseScreenMusic.CreateInstance();
            m_oBGM.IsLooped = true;
            m_oBGM.Play();
        }

        public override void End()
        {
            base.End();
            m_oBGM.Stop();
        }

        public override void Start()
        {
            base.Start();
            m_tsTimeUntilFadedIn = GameConsts.PauseFadeInTime;
            if (null != m_oBGM)
            {
                if (SoundState.Paused == m_oBGM.State)
                {
                    m_oBGM.Resume();
                }
                else if (SoundState.Stopped == m_oBGM.State)
                {
                    m_oBGM.Play();
                }
            }
        }

        public override void Update(GameTime a_oGameTime)
        {
            KeyboardState kbCurKeys = Keyboard.GetState();
            if (kbCurKeys.IsKeyDown(Keys.P) ||
                kbCurKeys.IsKeyDown(Keys.Space) ||
                kbCurKeys.IsKeyDown(Keys.NumLock))
            {
                m_bUnpauseKeyDown = true;
            }
            else if (m_bUnpauseKeyDown)
            {
                m_oBGM.Stop();
                StateStack.PopState();
                if (0 == StateStack.StackCount && null != m_oPausedState)
                {
                    StateStack.AddState(m_oPausedState);
                }
            }
            if (TimeSpan.Zero < m_tsTimeUntilFadedIn)
            {
                m_tsTimeUntilFadedIn -= a_oGameTime.ElapsedGameTime;
                if (TimeSpan.Zero > m_tsTimeUntilFadedIn)
                {
                    m_tsTimeUntilFadedIn = TimeSpan.Zero;
                }
            }
        }

        public override void Draw(GameTime a_oGameTime)
        {
            if (null != m_oPausedState)
            {
                m_oPausedState.Draw(a_oGameTime);
            }
            Color oTint =
                Color.Lerp(GameConsts.PauseOverlayEndTint,
                           GameConsts.PauseOverlayStartTint,
                           (float)(m_tsTimeUntilFadedIn.TotalSeconds /
                                   GameConsts.PauseFadeInTime.TotalSeconds));
            m_oSpriteBatch.Begin();
            m_oSpriteBatch.Draw(GameAssets.PauseOverlayTexture,
                                m_rctViewport, oTint);
            m_oSpriteBatch.End();
        }
    }
}
