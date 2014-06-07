using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    public static class GameConsts
    {
        #region Variables

        // constants loaded
        private static bool m_bLoaded = false;
        private static T GetIfLoaded<T>(T a_value)
        {
            if (!m_bLoaded)
            {
                throw new System.Exception("Game constants not loaded");
            }
            return a_value;
        }

        // Player
        public static int PlayerHP
        {
            get { return GetIfLoaded(m_iPlayerBaseHP); }
        }
        private static int m_iPlayerBaseHP;     // Starting Health
        public static int PlayerSpeed
        {
            get { return GetIfLoaded(m_iPlayerMoveSpeed); }
        }
        private static int m_iPlayerMoveSpeed;  // Movement Speed
        public static int PlayerLives
        {
            get { return GetIfLoaded(m_iPlayerLifeCount); }
        }
        private static int m_iPlayerLifeCount;  // Starting Lives
        public static TimeSpan PlayerFireInterval
        {
            get { return GetIfLoaded(m_tsPlayerFireDelay); }
        }
        private static TimeSpan m_tsPlayerFireDelay;        // Delay between shots
        public static TimeSpan PlayerInvulnerabilityDuration
        {
            get { return GetIfLoaded(m_tsPlayerInvulnDuration); }
        }
        private static TimeSpan m_tsPlayerInvulnDuration;   // Duration of invulnerbility
        public static Color PlayerInvulnerableTint
        {
            get { return GetIfLoaded(m_oPlayerInvulnerableTint); }
        }
        private static Color m_oPlayerInvulnerableTint;     // Invulnerable player tint
        public static Color PlayerNormalTint
        {
            get { return GetIfLoaded(m_oPlayerNormalTint); }
        }
        private static Color m_oPlayerNormalTint;     // Normal player tint

        // Bullet
        public static int BulletDamage
        {
            get { return GetIfLoaded(m_iBulletDamage); }
        }
        private static int m_iBulletDamage;      // Damage
        public static int BulletSpeed
        {
            get { return GetIfLoaded(m_iBulletMoveSpeed); }
        }
        private static int m_iBulletMoveSpeed;      // Movement Speed
        public static float BulletSpin
        {
            get { return GetIfLoaded(m_fBulletRotationSpeed); }
        }
        private static float m_fBulletRotationSpeed;// Rotation Speed
        public static TimeSpan BulletLifetime
        {
            get { return GetIfLoaded(m_tsBulletLifetime); }
        }
        private static TimeSpan m_tsBulletLifetime; // Duration of Bullet

        // Wave System
        public static int InitialWaveSize
        {
            get { return GetIfLoaded(m_iWaveBaseEnemyCount); }
        }
        private static int m_iWaveBaseEnemyCount;   // Enemy Count in Round 1
        public static int WaveSizeIncrement
        {
            get { return GetIfLoaded(m_iWaveIncrement); }
        }
        private static int m_iWaveIncrement;        // increase in enemies per wave
        public static TimeSpan WaveDelay
        {
            get { return GetIfLoaded(m_tsWaveDelay); }
        }
        private static TimeSpan m_tsWaveDelay;       // time between waves

        // Enemy Spawning
        public static float SpawnDelay
        {
            get { return GetIfLoaded(m_fEnemySpawnDelay); }
        }
        private static float m_fEnemySpawnDelay;    // time between individual enemies

        #endregion
        
        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_oXMLDoc">Source XML Document</param>
        public static void Reload(XmlDocument a_oXMLDoc)
        {
            XmlNode oPlayer = a_oXMLDoc.SelectSingleNode(".//PlayerConstants");
            if (null != oPlayer)
            {
                Reload(oPlayer, "BaseHP", ref m_iPlayerBaseHP);
                Reload(oPlayer, "Speed", ref m_iPlayerMoveSpeed);
                // TODO
            }
        }
        private static bool Reload(XmlNode a_oNode, string a_sAttribute, ref int value)
        {
            if (null != a_oNode.Attributes[a_sAttribute])
            {
                return int.TryParse(a_oNode.Attributes[a_sAttribute].Value, out value);
            }
            return false;
        }
        private static bool Reload(XmlNode a_oNode, string a_sAttribute, ref float value)
        {
            if (null != a_oNode.Attributes[a_sAttribute])
            {
                return float.TryParse(a_oNode.Attributes[a_sAttribute].Value, out value);
            }
            return false;
        }
        
        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_sXMLDoc">File path of the XML Document</param>
        public static void Reload(string a_sXMLDoc)
        {
            XmlDocument oDocument = new XmlDocument();
            oDocument.Load(a_sXMLDoc);
            Reload(oDocument);
        }
    }
}
