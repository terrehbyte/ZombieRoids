using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
            BulletTexture = a_oContent.Load<Texture2D>(GameConsts.BulletTextureName);
            ZombieTexture = a_oContent.Load<Texture2D>(GameConsts.ZombieTextureName);
            BackgroundTexture = a_oContent.Load<Texture2D>(GameConsts.BackgroundTextureName);
            ParallaxTextureOne = a_oContent.Load<Texture2D>(GameConsts.Overlay1TextureName);
            ParallaxTextureTwo = a_oContent.Load<Texture2D>(GameConsts.Overlay2TextureName);
            ScoreFont = a_oContent.Load<SpriteFont>(GameConsts.FontName);

            // Load sounds
            // TODO
        }

        #region Variables
        // Entities
        public static Texture2D PlayerTexture { get; private set; }
        public static Texture2D BulletTexture { get; private set; }
        public static Texture2D ZombieTexture { get; private set; }

        // Background
        public static Texture2D BackgroundTexture { get; private set; }
        public static Texture2D ParallaxTextureOne { get; private set; }
        public static Texture2D ParallaxTextureTwo { get; private set; }

        // UI
        public static SpriteFont ScoreFont { get; private set; }

        // Music
        //public static ? BackgroundMusic { get; private set; }   // BGM Path

        // Sounds
        //public static ? PlayerShootSound { get; private set; }    // Player Throw Sound Path
        //public static ? PlayerDeathSound { get; private set; }    // Player Death Sound Path
        //public static ? PlayerSpawnSound { get; private set; }    // Player Spawn Sound Path

        //public static ? ZombieDeathSound { get; private set; }    // Zombie Death Sound Path

        //public static ? SelectSound { get; private set; }     // UI Select Sound Path
        //public static ? ConfirmSound { get; private set; }    // UI Confirm Sound Path
        //public static ? LifeGainSound { get; private set; }   // UI Life Gain Sound Path
        #endregion
    }
}
