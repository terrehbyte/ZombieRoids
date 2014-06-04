/// <list type="table">
/// <listheader><term>Entity.cs</term><description>
///     Class representing a moving object with hit points
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 2, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 4, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Refactoring Game1 class
/// </description></item>
/// </list>

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using RotatedRectangleCollisions;

namespace ZombieRoids
{
    /// <remarks>
    /// Represents a moving object with hitpoints
    /// </remarks>
    public class Entity : Sprite
    {
        public RotatedBoxCollider Collider
        {
            get { return new RotatedBoxCollider(Boundary, Rotation); }
        }

        /// <summary>
        /// Change in position
        /// </summary>
        public virtual Vector2 Velocity { get; set; }

        /// <summary>
        /// Speed
        /// </summary>
        public float Speed
        {
            get { return Velocity.Length(); }
            set
            {
                Velocity = Forward * value;
            }
        }

        /// <summary>
        /// Points of damage required to destroy this entity
        /// </summary>
        public int HitPoints { get; set; }

        /// <summary>
        /// Changing between active and inactive triggers the OnActive and
        /// OnInactive methods, respectively
        /// </summary>
        public bool Active
        {
            get { return m_bActive; }
            set
            {
                if (value != m_bActive)
                {
                    m_bActive = value;
                    if (value)
                    {
                        OnActive();
                    }
                    else
                    {
                        OnInactive();
                    }
                }
            }
        }
        private bool m_bActive;

        /// <summary>
        /// Does this entity have at least 1HP left?
        /// </summary>
        public bool Alive
        {
            get { return (HitPoints > 0); }
            set
            {
                if (value != (HitPoints > 0))
                {
                    HitPoints = (value ? 1 : 0);
                }
            }
        }

#if DEBUG
        // In debug mode, has this entity been initialized?
        private bool m_bInitialized;
#endif

        /// <summary>
        /// Assigns data critical to entity updates
        /// </summary>
        /// <param name="a_tTexture">Texture used for drawing</param>
        /// <param name="a_v2Position">Initial position of sprite</param>
        public virtual void Initialize(Texture2D a_tTexture,
                                       Vector2 a_v2Position)
        {
            // Assign Texture
            Texture = a_tTexture;

            // Assign Position
            Position = a_v2Position;

#if DEBUG
            // In debug mode, note that the entity has been initialized
            m_bInitialized = true;
#endif
        }

        /// <summary>
        /// Update logic for this entity, if it needs updating each frame
        /// </summary>
        /// <param name="a_gtGameTime"></param>
        public virtual void Update(Game1.Context a_oContext)
        {
#if DEBUG
            // In debug mode, throw an exception if this is called before the
            // entity has been initialized.
            if (!m_bInitialized)
            {
                throw new Exception("Not Initialized!");
            }
#endif
            if (Active)
            {
                Position += Velocity *
                            (float)a_oContext.time.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Is this entity no longer within the screen?
        /// </summary>
        /// <param name="a_ptScreenSize">Screen size</param>
        /// <returns>True if entity is completely outside screen area</returns>
        public bool CheckOffscreen(Rectangle a_oDisplayArea)
        {
            if (!a_oDisplayArea.Intersects(Boundary))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Draws the entity if it is active
        /// </summary>
        /// <param name="a_sbSpriteBatch"></param>
        public override void Draw(SpriteBatch a_sbSpriteBatch)
        {
            if (Active)
            {
                base.Draw(a_sbSpriteBatch);
            }
        }

        /// <summary>
        /// Called when the entity changes from inactive to active
        /// </summary>
        public virtual void OnActive() { }

        /// <summary>
        /// Called when the entity changes from active to inactive
        /// </summary>
        public virtual void OnInactive() { }
    }
}
