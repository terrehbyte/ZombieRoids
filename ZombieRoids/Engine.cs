using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    class Engine
    {
        public Game m_Game;
        private static Engine m_engInstance;

        private Engine()
        {
        }

        public static Engine Instance
        {
            get
            {
                if (m_engInstance == null)
                {
                    m_engInstance = new Engine();
                }
                return m_engInstance;
            }
        }

        public Entity Instantiate(string a_sTexture, Vector2 a_v2Pos)
        {
            Entity tempEntity = new Entity();

            Texture2D tEntityTex = m_Game.Content.Load<Texture2D>(a_sTexture);
            tempEntity.Initialize(tEntityTex, a_v2Pos);

            return tempEntity;
        }
    }
}
