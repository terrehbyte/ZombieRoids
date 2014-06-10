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
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Merging 'dev' into 'feature-terry'
/// </description></item>
/// </list>

using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        public static bool Loaded
        {
            get { return m_bLoaded; }
        }
        private static bool m_bLoaded = false;
        private static T GetIfLoaded<T>(T a_value)
        {
            if (!m_bLoaded)
            {
                throw new System.Exception("Game constants not loaded");
            }
            return a_value;
        }

        #region Menu constants

        public static string SelectSoundName
        {
            get { return GetIfLoaded(m_sUISelectSnd); }
        }
        private static string m_sUISelectSnd;       // UI Select Sound Path
        public static string ConfirmSoundName
        {
            get { return GetIfLoaded(m_sUIConfirmSnd); }
        }
        private static string m_sUIConfirmSnd;      // UI Confirm Sound Path
        public static string PauseOverlayTextureName
        {
            get { return GetIfLoaded(m_sPauseOverlayTextureName); }
        }
        private static string m_sPauseOverlayTextureName;
        public static Color PauseOverlayTint
        {
            get { return GetIfLoaded(m_oPauseOverlayTint); }
        }
        private static Color m_oPauseOverlayTint;
        public static TimeSpan GameOverDuration
        {
            get { return GetIfLoaded(m_tsGameOverDuration); }
        }
        private static TimeSpan m_tsGameOverDuration;
        public static string GameOverOverlayTextureName
        {
            get { return GetIfLoaded(m_sGameOverOverlayTextureName); }
        }
        private static string m_sGameOverOverlayTextureName;
        public static Color GameOverOverlayTint
        {
            get { return GetIfLoaded(m_oGameOverOverlayTint); }
        }
        private static Color m_oGameOverOverlayTint;
        public static string TitleScreenTextureName
        {
            get { return GetIfLoaded(m_sTitleScreenTextureName); }
        }
        private static string m_sTitleScreenTextureName;
        public static Color ButtonClickTint
        {
            get { return GetIfLoaded(m_oButtonClickTint); }
        }
        private static Color m_oButtonClickTint;
        public static Color ButtonHoverTint
        {
            get { return GetIfLoaded(m_oButtonHoverTint); }
        }
        private static Color m_oButtonHoverTint;
        public static Color ButtonNormalTint
        {
            get { return GetIfLoaded(m_oButtonNormalTint); }
        }
        private static Color m_oButtonNormalTint;
        public static Color ButtonSelectedTint
        {
            get { return GetIfLoaded(m_oButtonSelectedTint); }
        }
        private static Color m_oButtonSelectedTint;
        public static string NewGameButtonTextureName
        {
            get { return GetIfLoaded(m_sNewGameButtonTextureName); }
        }
        private static string m_sNewGameButtonTextureName;
        public static Vector2 NewGameButtonPosition
        {
            get { return GetIfLoaded(m_v2NewGameButtonPosition); }
        }
        private static Vector2 m_v2NewGameButtonPosition;
        public static string ExitButtonTextureName
        {
            get { return GetIfLoaded(m_sExitButtonTextureName); }
        }
        private static string m_sExitButtonTextureName;
        public static Vector2 ExitButtonPosition
        {
            get { return GetIfLoaded(m_v2ExitButtonPosition); }
        }
        private static Vector2 m_v2ExitButtonPosition;

        #endregion

        #region World constants

        // World
        public static string BackgroundTextureName
        {
            get { return GetIfLoaded(m_sBackgroundTextureName); }
        }
        private static string m_sBackgroundTextureName;
        public static string FontName
        {
            get { return GetIfLoaded(m_sFontName); }
        }
        private static string m_sFontName;
        public static int LifeGainPoints
        {
            get { return GetIfLoaded(m_iLifeGainPoints); }
        }
        private static int m_iLifeGainPoints;
        public static Vector2 ScorePosition
        {
            get { return GetIfLoaded(m_v2ScorePosition); }
        }
        private static Vector2 m_v2ScorePosition;
        public static Vector2 LivesPosition
        {
            get { return GetIfLoaded(m_v2LivesPosition); }
        }
        private static Vector2 m_v2LivesPosition;
        public static Vector2 EnemyCountPosition
        {
            get { return GetIfLoaded(m_v2EnemyCountPosition); }
        }
        private static Vector2 m_v2EnemyCountPosition;
        public static string Overlay1TextureName
        {
            get { return GetIfLoaded(m_sOverlay1TextureName); }
        }
        private static string m_sOverlay1TextureName;
        public static int Overlay1Speed
        {
            get { return GetIfLoaded(m_iOverlay1Speed); }
        }
        private static int m_iOverlay1Speed;
        public static string Overlay2TextureName
        {
            get { return GetIfLoaded(m_sOverlay2TextureName); }
        }
        private static string m_sOverlay2TextureName;
        public static int Overlay2Speed
        {
            get { return GetIfLoaded(m_iOverlay2Speed); }
        }
        private static int m_iOverlay2Speed;
        public static string BackgroundMusicName
        {
            get { return GetIfLoaded(m_sUIBGM); }
        }
        private static string m_sUIBGM;             // BGM Path
        public static string LifeGainSoundName
        {
            get { return GetIfLoaded(m_sUILifeGainSnd); }
        }
        private static string m_sUILifeGainSnd;     // UI Life Gain Sound Path

        #endregion

        #region Player constants

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
        private static Color m_oPlayerInvulnerableTint; // Invulnerable player tint
        public static Color PlayerNormalTint
        {
            get { return GetIfLoaded(m_oPlayerNormalTint); }
        }
        private static Color m_oPlayerNormalTint;   // Normal player tint
        public static string PlayerTextureName
        {
            get { return GetIfLoaded(m_sPlayerTextureName); }
        }
        private static string m_sPlayerTextureName; // Player texture name
        public static string PlayerShootSoundName
        {
            get { return GetIfLoaded(m_sPlayerThrowSnd); }
        }
        private static string m_sPlayerThrowSnd;    // Player Throw Sound Path
        public static string PlayerDeathSoundName
        {
            get { return GetIfLoaded(m_sPlayerDeathSnd); }
        }
        private static string m_sPlayerDeathSnd;    // Player Death Sound Path
        public static string PlayerSpawnSoundName
        {
            get { return GetIfLoaded(m_sPlayerSpawnSnd); }
        }
        private static string m_sPlayerSpawnSnd;    // Player Spawn Sound Path
        public static string PlayerTeleportSoundName
        {
            get { return GetIfLoaded(m_sPlayerTeleportSnd); }
        }
        private static string m_sPlayerTeleportSnd; // Player Teleport Sound Path

        #endregion

        #region Bullet constants

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
        public static string BulletTextureName
        {
            get { return GetIfLoaded(m_sBulletTextureName); }
        }
        private static string m_sBulletTextureName; // Player texture name

        #endregion

        #region Zombie constants

        // Zombie
        public static int ZombieHP
        {
            get { return GetIfLoaded(m_iZombieHP); }
        }
        private static int m_iZombieHP;
        public static int ZombieDamage
        {
            get { return GetIfLoaded(m_iZombieDamage); }
        }
        private static int m_iZombieDamage;
        public static int ZombieValue
        {
            get { return GetIfLoaded(m_iZombieValue); }
        }
        private static int m_iZombieValue;
        public static int ZombieSpeed
        {
            get { return GetIfLoaded(m_iZombieSpeed); }
        }
        private static int m_iZombieSpeed;
        public static string ZombieTextureName
        {
            get { return GetIfLoaded(m_sZombieTextureName); }
        }
        private static string m_sZombieTextureName;
        public static string ZombieDeathSoundName
        {
            get { return GetIfLoaded(m_sZombieDeathSnd); }
        }
        private static string m_sZombieDeathSnd;    // Zombie Death Sound Path

        #endregion

        #region Fragment constants

        // Zombie Fragments
        public static int InitialFragments
        {
            get { return GetIfLoaded(m_iInitialFragments); }
        }
        private static int m_iInitialFragments;
        public static int FragmentMaxSpeed
        {
            get { return GetIfLoaded(m_iFragmentMaxSpeed); }
        }
        private static int m_iFragmentMaxSpeed;
        public static int FragmentMinSpeed
        {
            get { return GetIfLoaded(m_iFragmentMinSpeed); }
        }
        private static int m_iFragmentMinSpeed;
        public static int FragmentDeltaSpeed
        {
            get { return GetIfLoaded(m_iFragmentDeltaSpeed); }
        }
        private static int m_iFragmentDeltaSpeed;
        public static int FragmentDeltaPosition
        {
            get { return GetIfLoaded(m_iFragmentDeltaPosition); }
        }
        private static int m_iFragmentDeltaPosition;
        public static float FragmentScale
        {
            get { return GetIfLoaded(m_fFragmentScale); }
        }
        private static float m_fFragmentScale;

        #endregion

        #region Wave constants

        // Wave System
        public static int InitialWaveSize
        {
            get { return GetIfLoaded(m_iWaveBaseEnemyCount); }
        }
        private static int m_iWaveBaseEnemyCount;       // Enemy Count in Round 1
        public static int WaveSizeIncrement
        {
            get { return GetIfLoaded(m_iWaveIncrement); }
        }
        private static int m_iWaveIncrement;            // increase in enemies per wave
        public static TimeSpan WaveDelay
        {
            get { return GetIfLoaded(m_tsWaveDelay); }
        }
        private static TimeSpan m_tsWaveDelay;          // time between waves
        public static TimeSpan SpawnDelay
        {
            get { return GetIfLoaded(m_tsEnemySpawnDelay); }
        }
        private static TimeSpan m_tsEnemySpawnDelay;    // time between individual enemies

        #endregion

        #endregion

        #region Color Parsing

        /// <summary>
        /// Creates an ARGB hex string representation of the <see cref="Color"/> value.
        /// </summary>
        /// <remarks>Source: http://thedeadpixelsociety.com/2012/01/hex-colors-in-xna/</remarks>
        /// <param name="color">The <see cref="Color"/> value to parse.</param>
        /// <param name="includeHash">Determines whether to include the hash mark (#) character in the string.</param>
        /// <returns>A hex string representation of the specified <see cref="Color"/> value.</returns>
        private static string ToHex(this Color color, bool includeHash)
        {
            string[] argb = {
                color.A.ToString("X2"),
                color.R.ToString("X2"),
                color.G.ToString("X2"),
                color.B.ToString("X2"),
            };
            return (includeHash ? "#" : string.Empty) + string.Join(string.Empty, argb);
        }

        /// <summary>Creates a <see cref="Color"/> value from an ARGB or RGB hex string.  The string may
        /// begin with or without the hash mark (#) character.
        /// </summary>
        /// <remarks>Source: http://thedeadpixelsociety.com/2012/01/hex-colors-in-xna/</remarks>
        /// <param name="hexString">The ARGB hex string to parse.</param>
        /// <returns>
        /// A <see cref="Color"/> value as defined by the ARGB or RGB hex string.
        /// </returns>
        /// <exception cref="InvalidOperationException">Thrown if the string is not a valid ARGB or RGB hex value.</exception>
        private static Color ToColor(this string hexString)
        {
            if (hexString.StartsWith("#"))
                hexString = hexString.Substring(1);
            uint hex = uint.Parse(hexString, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            Color color = Color.White;
            if (hexString.Length == 8)
            {
                color.A = (byte)(hex >> 24);
                color.R = (byte)(hex >> 16);
                color.G = (byte)(hex >> 8);
                color.B = (byte)(hex);
            }
            else if (hexString.Length == 6)
            {
                color.R = (byte)(hex >> 16);
                color.G = (byte)(hex >> 8);
                color.B = (byte)(hex);
            }
            else
            {
                throw new InvalidOperationException("Invald hex representation of an ARGB or RGB color value.");
            }
            return color;
        }

        #endregion

        /// <summary>
        /// Reloads the variables with data from the XML document
        /// </summary>
        /// <param name="a_oXMLDoc">Source XML Document</param>
        public static void Reload(XmlDocument a_oXMLDoc)
        {
            ReloadMenuConstants(a_oXMLDoc.SelectSingleNode(".//MenuConstants"));
            ReloadWorldConstants(a_oXMLDoc.SelectSingleNode(".//WorldConstants"));
            ReloadPlayerConstants(a_oXMLDoc.SelectSingleNode(".//PlayerConstants"));
            ReloadBulletConstants(a_oXMLDoc.SelectSingleNode(".//BulletConstants"));
            ReloadZombieConstants(a_oXMLDoc.SelectSingleNode(".//ZombieConstants"));
            ReloadWaveConstants(a_oXMLDoc.SelectSingleNode(".//WaveConstants"));
            m_bLoaded = true;
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

        #region Load specific value types

        /// <summary>
        /// Reloads a string value from an attribute of the given XML node
        /// </summary>
        /// <param name="a_oNode">Node to load value from</param>
        /// <param name="a_sAttribute">Attribute name</param>
        /// <param name="a_rsValue">Location to store the value</param>
        /// <returns>True if successful</returns>
        private static bool Reload(XmlNode a_oNode, string a_sAttribute, ref string a_rsValue)
        {
            if (null != a_oNode.Attributes[a_sAttribute])
            {
                a_rsValue = a_oNode.Attributes[a_sAttribute].Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reloads an int value from an attribute of the given XML node
        /// </summary>
        /// <param name="a_oNode">Node to load value from</param>
        /// <param name="a_sAttribute">Attribute name</param>
        /// <param name="a_riValue">Location to store the value</param>
        /// <returns>True if successful</returns>
        private static bool Reload(XmlNode a_oNode, string a_sAttribute, ref int a_riValue)
        {
            if (null != a_oNode.Attributes[a_sAttribute])
            {
                return int.TryParse(a_oNode.Attributes[a_sAttribute].Value, out a_riValue);
            }
            return false;
        }

        /// <summary>
        /// Reloads a float value from an attribute of the given XML node
        /// </summary>
        /// <param name="a_oNode">Node to load value from</param>
        /// <param name="a_sAttribute">Attribute name</param>
        /// <param name="a_rfValue">Location to store the value</param>
        /// <returns>True if successful</returns>
        private static bool Reload(XmlNode a_oNode, string a_sAttribute, ref float a_rfValue)
        {
            if (null != a_oNode.Attributes[a_sAttribute])
            {
                return float.TryParse(a_oNode.Attributes[a_sAttribute].Value, out a_rfValue);
            }
            return false;
        }

        /// <summary>
        /// Reloads a timespan value from an attribute of the given XML node
        /// </summary>
        /// <param name="a_oNode">Node to load value from</param>
        /// <param name="a_sAttribute">Attribute name</param>
        /// <param name="a_rtsValue">Location to store the value</param>
        /// <returns>True if successful</returns>
        private static bool Reload(XmlNode a_oNode, string a_sAttribute, ref TimeSpan a_rtsValue)
        {
            float fSeconds = 0;
            if (Reload(a_oNode, a_sAttribute, ref fSeconds))
            {
                a_rtsValue = TimeSpan.FromSeconds(fSeconds);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reloads a color value from an attribute of the given XML node
        /// </summary>
        /// <param name="a_oNode">Node to load value from</param>
        /// <param name="a_sAttribute">Attribute name</param>
        /// <param name="a_roValue">Location to store the value</param>
        /// <returns>True if successful</returns>
        private static bool Reload(XmlNode a_oNode, string a_sAttribute, ref Color a_roValue)
        {
            if (null != a_oNode.Attributes[a_sAttribute])
            {
                try
                {
                    a_roValue = a_oNode.Attributes[a_sAttribute].Value.ToColor();
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Load groups of values

        /// <summary>
        /// Reloads menu system constants from attributes of the given XML node
        /// </summary>
        /// <param name="a_oMenuNode">Node to load values from</param>
        private static void ReloadMenuConstants(XmlNode a_oMenuNode)
        {
            if (null != a_oMenuNode)
            {
                Reload(a_oMenuNode, "SelectSound", ref m_sUISelectSnd);
                Reload(a_oMenuNode, "ConfirmSound", ref m_sUIConfirmSnd);
                Reload(a_oMenuNode, "PauseOverlay", ref m_sPauseOverlayTextureName);
                Reload(a_oMenuNode, "PauseOverlayTint", ref m_oPauseOverlayTint);
                Reload(a_oMenuNode, "GameOverDuration", ref m_tsGameOverDuration);
                Reload(a_oMenuNode, "GameOverOverlay", ref m_sGameOverOverlayTextureName);
                Reload(a_oMenuNode, "GameOverOverlayTint", ref m_oGameOverOverlayTint);
                Reload(a_oMenuNode, "TitleScreen", ref m_sTitleScreenTextureName);
                Reload(a_oMenuNode, "ButtonClickTint", ref m_oButtonClickTint);
                Reload(a_oMenuNode, "ButtonHoverTint", ref m_oButtonHoverTint);
                Reload(a_oMenuNode, "ButtonNormalTint", ref m_oButtonNormalTint);
                Reload(a_oMenuNode, "ButtonSelectedTint", ref m_oButtonSelectedTint);
                Reload(a_oMenuNode, "NewGameButton", ref m_sNewGameButtonTextureName);
                Reload(a_oMenuNode, "NewGameButtonX", ref m_v2NewGameButtonPosition.X);
                Reload(a_oMenuNode, "NewGameButtonY", ref m_v2NewGameButtonPosition.Y);
                Reload(a_oMenuNode, "ExitButton", ref m_sExitButtonTextureName);
                Reload(a_oMenuNode, "ExitButtonX", ref m_v2ExitButtonPosition.X);
                Reload(a_oMenuNode, "ExitButtonY", ref m_v2ExitButtonPosition.Y);
            }
        }

        /// <summary>
        /// Reloads world constants from attributes of the given XML node
        /// </summary>
        /// <param name="a_oWorldNode">Node to load values from</param>
        private static void ReloadWorldConstants(XmlNode a_oWorldNode)
        {
            if (null != a_oWorldNode)
            {
                Reload(a_oWorldNode, "Background", ref m_sBackgroundTextureName);
                Reload(a_oWorldNode, "ScoreFont", ref m_sFontName);
                Reload(a_oWorldNode, "LifeGainPoints", ref m_iLifeGainPoints);
                Reload(a_oWorldNode, "ScoreX", ref m_v2ScorePosition.X);
                Reload(a_oWorldNode, "ScoreY", ref m_v2ScorePosition.Y);
                Reload(a_oWorldNode, "LivesX", ref m_v2LivesPosition.X);
                Reload(a_oWorldNode, "LivesY", ref m_v2LivesPosition.Y);
                Reload(a_oWorldNode, "EnemyCountX", ref m_v2EnemyCountPosition.X);
                Reload(a_oWorldNode, "EnemyCountY", ref m_v2EnemyCountPosition.Y);
                Reload(a_oWorldNode, "Overlay1Texture", ref m_sOverlay1TextureName);
                Reload(a_oWorldNode, "Overlay1Speed", ref m_iOverlay1Speed);
                Reload(a_oWorldNode, "Overlay2Texture", ref m_sOverlay2TextureName);
                Reload(a_oWorldNode, "Overlay2Speed", ref m_iOverlay2Speed);
                Reload(a_oWorldNode, "BackgroundMusic", ref m_sUIBGM);
                Reload(a_oWorldNode, "LifeGainSound", ref m_sUILifeGainSnd);
            }
        }

        /// <summary>
        /// Reloads player constants from attributes of the given XML node
        /// </summary>
        /// <param name="a_oPlayerNode">Node to load values from</param>
        private static void ReloadPlayerConstants(XmlNode a_oPlayerNode)
        {
            if (null != a_oPlayerNode)
            {
                Reload(a_oPlayerNode, "BaseHP", ref m_iPlayerBaseHP);
                Reload(a_oPlayerNode, "Speed", ref m_iPlayerMoveSpeed);
                Reload(a_oPlayerNode, "InitialLives", ref m_iPlayerLifeCount);
                Reload(a_oPlayerNode, "ShotDelay", ref m_tsPlayerFireDelay);
                Reload(a_oPlayerNode, "InvulnerabilityPeriod", ref m_tsPlayerInvulnDuration);
                Reload(a_oPlayerNode, "NormalTint", ref m_oPlayerNormalTint);
                Reload(a_oPlayerNode, "InvulnerableTint", ref m_oPlayerInvulnerableTint);
                Reload(a_oPlayerNode, "Texture", ref m_sPlayerTextureName);
                Reload(a_oPlayerNode, "ShootSound", ref m_sPlayerThrowSnd);
                Reload(a_oPlayerNode, "DeathSound", ref m_sPlayerDeathSnd);
                Reload(a_oPlayerNode, "SpawnSound", ref m_sPlayerSpawnSnd);
                Reload(a_oPlayerNode, "TeleportSound", ref m_sPlayerTeleportSnd);
            }
        }

        /// <summary>
        /// Reloads bullet constants from attributes of the given XML node
        /// </summary>
        /// <param name="a_oBulletNode">Node to load values from</param>
        private static void ReloadBulletConstants(XmlNode a_oBulletNode)
        {
            if (null != a_oBulletNode)
            {
                Reload(a_oBulletNode, "Damage", ref m_iBulletDamage);
                Reload(a_oBulletNode, "Speed", ref m_iBulletMoveSpeed);
                Reload(a_oBulletNode, "Spin", ref m_fBulletRotationSpeed);
                Reload(a_oBulletNode, "LifeTime", ref m_tsBulletLifetime);
                Reload(a_oBulletNode, "Texture", ref m_sBulletTextureName);
            }
        }

        /// <summary>
        /// Reloads zombie constants from attributes of the given XML node
        /// </summary>
        /// <param name="a_oZombieNode">Node to load values from</param>
        private static void ReloadZombieConstants(XmlNode a_oZombieNode)
        {
            if (null != a_oZombieNode)
            {
                Reload(a_oZombieNode, "BaseHP", ref m_iZombieHP);
                Reload(a_oZombieNode, "Damage", ref m_iZombieDamage);
                Reload(a_oZombieNode, "Value", ref m_iZombieValue);
                Reload(a_oZombieNode, "Speed", ref m_iZombieSpeed);
                Reload(a_oZombieNode, "InitialFragmentCount", ref m_iInitialFragments);
                Reload(a_oZombieNode, "FragmentMinSpeed", ref m_iFragmentMinSpeed);
                Reload(a_oZombieNode, "FragmentMaxSpeed", ref m_iFragmentMaxSpeed);
                Reload(a_oZombieNode, "FragmentDeltaSpeed", ref m_iFragmentDeltaSpeed);
                Reload(a_oZombieNode, "FragmentDeltaPosition", ref m_iFragmentDeltaPosition);
                Reload(a_oZombieNode, "FragmentScale", ref m_fFragmentScale);
                Reload(a_oZombieNode, "Texture", ref m_sZombieTextureName);
                Reload(a_oZombieNode, "DeathSound", ref m_sZombieDeathSnd);
            }
        }

        /// <summary>
        /// Reloads wave constants from attributes of the given XML node
        /// </summary>
        /// <param name="a_oWaveNode">Node to load values from</param>
        private static void ReloadWaveConstants(XmlNode a_oWaveNode)
        {
            if (null != a_oWaveNode)
            {
                Reload(a_oWaveNode, "InitialWaveSize", ref m_iWaveBaseEnemyCount);
                Reload(a_oWaveNode, "WaveIncrement", ref m_iWaveIncrement);
                Reload(a_oWaveNode, "WaveInterval", ref m_tsWaveDelay);
                Reload(a_oWaveNode, "EnemySpawnTime", ref m_tsEnemySpawnDelay);
            }
        }

        #endregion
    }
}
