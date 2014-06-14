/// <list type="table">
/// <listheader><term>GameConsts.cs</term><description>
///     Class providing access to consts pertaining to gameplay
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 6, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 14, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Particle System
/// </description></item>
/// </list>

using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ZombieRoids
{
    /// <summary>
    /// Loads, saves, and provides access to consts loaded from XML document
    /// </summary>
    public static class GameAssets
    {
        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_oXMLDoc">Source XML Document</param>
        public static void Reload(ContentManager a_oContent)
        {
            // Load textures
            PlayerTexture = a_oContent.Load<Texture2D>(GameConsts.PlayerTextureName);
            PlayerTeleportTexture = a_oContent.Load<Texture2D>(GameConsts.PlayerTeleportTextureName);
            BulletTexture = a_oContent.Load<Texture2D>(GameConsts.BulletTextureName);
            ZombieTexture = a_oContent.Load<Texture2D>(GameConsts.ZombieTextureName);
            BackgroundTexture = a_oContent.Load<Texture2D>(GameConsts.BackgroundTextureName);
            ParallaxTextureOne = a_oContent.Load<Texture2D>(GameConsts.Overlay1TextureName);
            ParallaxTextureTwo = a_oContent.Load<Texture2D>(GameConsts.Overlay2TextureName);
            PauseOverlayTexture = a_oContent.Load<Texture2D>(GameConsts.PauseOverlayTextureName);
            GameOverOverlayTexture = a_oContent.Load<Texture2D>(GameConsts.GameOverOverlayTextureName);
            TitleScreenTexture = a_oContent.Load<Texture2D>(GameConsts.TitleScreenTextureName);
            NewGameButtonTexture = a_oContent.Load<Texture2D>(GameConsts.NewGameButtonTextureName);
            ExitButtonTexture = a_oContent.Load<Texture2D>(GameConsts.ExitButtonTextureName);
            ScoreFont = a_oContent.Load<SpriteFont>(GameConsts.FontName);

            // Load sounds
            BackgroundMusic = a_oContent.Load<SoundEffect>(GameConsts.BackgroundMusicName);
            PauseScreenMusic = a_oContent.Load<SoundEffect>(GameConsts.PauseScreenMusicName);
            PlayerShootSound = a_oContent.Load<SoundEffect>(GameConsts.PlayerShootSoundName);
            PlayerDeathSound = a_oContent.Load<SoundEffect>(GameConsts.PlayerDeathSoundName);
            PlayerSpawnSound = a_oContent.Load<SoundEffect>(GameConsts.PlayerSpawnSoundName);
            PlayerTeleportSound = a_oContent.Load<SoundEffect>(GameConsts.PlayerTeleportSoundName);
            ZombieDeathSound = a_oContent.Load<SoundEffect>(GameConsts.ZombieDeathSoundName);
            SelectSound = a_oContent.Load<SoundEffect>(GameConsts.SelectSoundName);
            ConfirmSound = a_oContent.Load<SoundEffect>(GameConsts.ConfirmSoundName);
            LifeGainSound = a_oContent.Load<SoundEffect>(GameConsts.LifeGainSoundName);
        }

        #region Variables
        // Entities
        public static Texture2D PlayerTexture { get; private set; }
        public static Texture2D PlayerTeleportTexture { get; private set; }
        public static Texture2D BulletTexture { get; private set; }
        public static Texture2D ZombieTexture { get; private set; }

        // Background
        public static Texture2D BackgroundTexture { get; private set; }
        public static Texture2D ParallaxTextureOne { get; private set; }
        public static Texture2D ParallaxTextureTwo { get; private set; }

        // UI
        public static Texture2D PauseOverlayTexture { get; private set; }
        public static Texture2D GameOverOverlayTexture { get; private set; }
        public static SpriteFont ScoreFont { get; private set; }
        public static Texture2D TitleScreenTexture { get; private set; }
        public static Texture2D NewGameButtonTexture { get; private set; }
        public static Texture2D ExitButtonTexture { get; private set; }

        // Music
        public static SoundEffect BackgroundMusic { get; private set; } // BGM Path
        public static SoundEffect PauseScreenMusic { get; private set; } // PSM Path

        // Sounds
        public static SoundEffect PlayerShootSound { get; private set; }    // Player Throw Sound Path
        public static SoundEffect PlayerDeathSound { get; private set; }    // Player Death Sound Path
        public static SoundEffect PlayerSpawnSound { get; private set; }    // Player Spawn Sound Path
        public static SoundEffect PlayerTeleportSound { get; private set; } // Player Teleport Sound Path

        public static SoundEffect ZombieDeathSound { get; private set; }    // Zombie Death Sound Path

        public static SoundEffect SelectSound { get; private set; }     // UI Select Sound Path
        public static SoundEffect ConfirmSound { get; private set; }    // UI Confirm Sound Path
        public static SoundEffect LifeGainSound { get; private set; }   // UI Life Gain Sound Path
        #endregion
    }
}
