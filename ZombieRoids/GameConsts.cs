using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieRoids
{
    class GameConsts
    {
        public static XmlDocument m_oXMLDoc;        // XmlDocument loaded for reading
        private static GameConsts m_oInstance;     // Instance to be distributed

        // Made private due to access only via .Instance
        private GameConsts() { }

        /// <summary>
        /// Obtain the global instance of the AssetConsts
        /// </summary>
        public static GameConsts Instance
        {
            get
            {
                if (m_oInstance == null)
                {
                    m_oInstance = new GameConsts();
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
        // Player
        public int m_iPlayerBaseHP;     // Starting Health
        public int m_iPlayerMoveSpeed;  // Movement Speed
        public int m_iPlayerLifeCount;  // Starting Lives
        
        public TimeSpan m_tsPlayerFireDelay;        // Delay between shots
        public TimeSpan m_tsPlayerInvulnDuration;   // Duration of invulnerbility

        // Bullet
        public int m_iBulletMoveSpeed;      // Movement Speed
        public float m_fBulletRotationSpeed;// Rotation Speed
        public TimeSpan m_tsBulletLifetime; // Duration of Bullet

        // Wave System
        public int mc_iWaveBaseEnemyCount;   // Enemy Count in Round 1
        public int mc_iWaveIncrement;        // increase in enemies per wave
        public TimeSpan m_tsWaveDelay;       // time between waves

        // Enemy Spawning
        public float mc_fEnemySpawnDelay;    // time between individual enemies
#endregion
    }
}
