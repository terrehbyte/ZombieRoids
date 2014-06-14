using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZombieRoids
{
    class BulletManager
    {
        #region Properties and Variables
        
        // Internal List of Bullets
        private List<Bullet> m_loBullets;

        #endregion

        #region Methods

        public Bullet GetInactiveBullet()
        {
            throw new System.NotImplementedException();
        }

        #region Public Methods

        /// <summary>
        /// Forcibly adds a bullet to the manager
        /// </summary>
        /// <param name="a_oSrcBullet">Bullet to add</param>
        /// <returns>Bullet added</returns>
        public Bullet AddBullet(Bullet a_oSrcBullet)
        {
            // Add bullet to the list
            m_loBullets.Add(a_oSrcBullet);

            // Return bullet
            return m_loBullets.Last();
        }

        /// <summary>
        /// Forcibly removes a bullet from
        /// </summary>
        /// <param name="a_oSrcBullet">Bullet to remove</param>
        public void RemoveBullet(Bullet a_oSrcBullet)
        {
            m_loBullets.Remove(a_oSrcBullet);
        }

        /// <summary>
        /// Overwrites existing and inactive bullet with new data
        /// </summary>
        /// <param name="a_oSrcBullet">Bullet to copy from</param>
        /// <returns>Bullet overwritten</returns>
        public Bullet RecycleBullet(Bullet a_oSrcBullet)
        {
            for (int i = 0; i < m_loBullets.Count; i++)
            {
                if (!m_loBullets[i].Active)
                {
                    // Assign data to inactive bullet
                    m_loBullets[i] = a_oSrcBullet;

                    // Return newly assigned bullet
                    return m_loBullets[i];
                }
            }

            // Failed to find and overwrite an existing bullet
            return null;
        }

        /// <summary>
        /// Removes inactive bullets
        /// </summary>
        public void CullBullets()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #endregion

    }
}
