using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    /// <summary>
    /// Loads, saves, and provides access to consts loaded from XML document
    /// </summary>
    public static class AssetConsts
    {
        public static XmlDocument m_oXMLDoc;        // XmlDocument loaded for reading
        private static AssetConsts m_oInstance;     // Instance to be distributed

        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_oXMLDoc">Source XML Document</param>
        public static void Reload(XmlDocument a_oXMLDoc)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_sXMLDoc">File path of the XML Document</param>
        public static void Reload(string a_sXMLDoc)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Writes the current variables back into the XML document
        /// </summary>
        public static void Save()
        {
            throw new System.NotImplementedException();
        }

        #region Variables
        // Entities
        public static Texture2D PlayerTexture { get; private set; }
        private static string m_sPlayerTex;         // Player Texture Path
        public static Texture2D BulletTexture { get; private set; }
        private static string m_sBulletTex;         // Bullet Texture Path
        public static Texture2D ZombieTexture { get; private set; }
        private static string m_sZombieTex;         // Zombie Texture Path

        // Background
        public static Texture2D BackgroundTexture { get; private set; }
        private static string m_sMainBackgroundTex; // Background Tex Path
        public static Texture2D ParallaxTextureOne { get; private set; }
        private static string m_sParallaxOneTex;    // Parallax One Tex Path
        public static Texture2D ParallaxTextureTwo { get; private set; }
        private static string m_sParallaxTwoTex;    // Parallax Two Tex Path

        // UI
        public static SpriteFont ScoreFont { get; private set; }
        private static string m_sScoreFont;         // Font Path

        // Music
        private static string m_sUIBGM;             // BGM Path

        // Sounds
        private static string m_sPlayerThrowSnd;    // Player Throw Sound Path
        private static string m_sPlayerDeathSnd;    // Player Death Sound Path
        private static string m_sPlayerSpawnSnd;    // Player Spawn Sound Path

        private static string m_sZombieDeathSnd;    // Zombie Death Sound Path

        private static string m_sUISelectSnd;       // UI Select Sound Path
        private static string m_sUIConfirmSnd;      // UI Confirm Sound Path
        private static string m_sUILifeGainSnd;     // UI Life Gain Sound Path
#endregion
    }
}
