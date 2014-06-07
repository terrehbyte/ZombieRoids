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
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 6, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Added header description
/// </description></item>
/// </list>

using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieRoids
{
    /// <summary>
    /// Loads, saves, and provides access to consts loaded from XML document
    /// </summary>
    class AssetConsts
    {
        public static XmlDocument m_oXMLDoc;        // XmlDocument loaded for reading
        private static AssetConsts m_oInstance;     // Instance to be distributed

        // Made private due to access only via .Instance
        private AssetConsts() { }

        /// <summary>
        /// Obtain the global instance of the AssetConsts
        /// </summary>
        public static AssetConsts Instance
        {
            get
            {
                if (m_oInstance == null)
                {
                    m_oInstance = new AssetConsts();
                }
                return m_oInstance;
            }
        }

        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_oXMLDoc">Source XML Document</param>
        public void Reload(XmlDocument a_oXMLDoc)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_sXMLDoc">File path of the XML Document</param>
        public void Reload(string a_sXMLDoc)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// Writes the current variables back into the XML document
        /// </summary>
        public void Save()
        {
            throw new System.NotImplementedException();
        }

#region Variables
        // Entities
        public string m_sPlayerTex;         // Player Texture Path
        public string m_sBulletTex;         // Bullet Texture Path
        public string m_sZombieTex;         // Zombie Texture Path
        
        // Background
        public string m_sMainBackgroundTex; // Background Tex Path
        public string m_sParallaxOneTex;    // Parallax One Tex Path
        public string m_sParallaxTwoTex;    // Parallax Two Tex Path

        // Music
        public string m_sUIBGM;             // BGM Path

        // UI
        public string m_sScoreFont;         // Font Path

        // Sounds
        public string m_sPlayerThrowSnd;    // Player Throw Sound Path
        public string m_sPlayerDeathSnd;    // Player Death Sound Path
        public string m_sPlayerSpawnSnd;    // Player Spawn Sound Path

        public string m_sZombieDeathSnd;    // Zombie Death Sound Path

        public string m_sUISelectSnd;       // UI Select Sound Path
        public string m_sUIConfirmSnd;      // UI Confirm Sound Path
        public string m_sUILifeGainSnd;     // UI Life Gain Sound Path
#endregion
    }
}
