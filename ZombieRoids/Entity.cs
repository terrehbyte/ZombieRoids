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
///     June 11, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Animating Zombie
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
    public class Entity : Animation
    {
        /// <summary>
        /// Bounding box
        /// </summary>
        public RotatedBoxCollider Collider
        {
            get { return new RotatedBoxCollider(Boundary, Rotation); }
        }

        #region Movement

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
        /// Rate of change in rotation in radians per second
        /// </summary>
        public float AngularVelocity { get; set; }

        /// <summary>
        /// Rate of change in rotation in degrees per second
        /// </summary>
        public float AngularVelocityInDegrees
        {
            get { return (float)(AngularVelocity * 360 / Math.PI); }
            set { AngularVelocity = (float)(value * Math.PI / 360); }
        }

        #endregion

        /// <summary>
        /// Points of damage required to destroy this entity
        /// </summary>
        public int HitPoints { get; set; }

        #region Wrap/Clamp settings

        /// <summary>
        /// Options for when, if ever, to wrap an entity's position to within
        /// the game display area when updating
        /// </summary>
        public enum WrapSetting
        {
            /// <summary>
            /// Don't wrap the entity's position to within the screen
            /// </summary>
            None,
            /// <summary>
            /// Wrap the entity's position to within the screen when it exits
            /// </summary>
            OnExit,
            /// <summary>
            /// Always wrap the entity's position to within the screen
            /// </summary>
            Always
        }

        /// <summary>
        /// When, if ever, should this entity's position be wrapped to within
        /// the game display area on updating?
        /// </summary>
        public WrapSetting Wrap
        {
            get { return m_eWrap; }
            set { m_eWrap = value; }
        }
        private WrapSetting m_eWrap = WrapSetting.OnExit;

        /// <summary>
        /// Options for when, if ever, to clamp an entity's position to within
        /// the game display area when updating
        /// </summary>
        public enum ClampSetting
        {
            /// <summary>
            /// Don't clamp the entity's position to within the screen
            /// </summary>
            None,
            /// <summary>
            /// Clamp the entity's position to within the screen when it exits,
            /// stopping outbound velocity
            /// </summary>
            HaltOnExit,
            /// <summary>
            /// Clamp the entity's position to within the screen when it exits,
            /// reversing outbound velocity
            /// </summary>
            BounceOnExit,
            /// <summary>
            /// Always clamp the entity's position to within the screen,
            /// stopping outbound velocity
            /// </summary>
            AlwaysHalt,
            /// <summary>
            /// Always clamp the entity's position to within the screen,
            /// reversing outbound velocity
            /// </summary>
            AlwaysBounce
        }

        /// <summary>
        /// When, if ever, should this entity's position be clamped to within
        /// the game display area on updating?
        /// </summary>
        public ClampSetting Clamp
        {
            get { return m_eClamp; }
            set { m_eClamp = value; }
        }
        private ClampSetting m_eClamp = ClampSetting.None;

        #endregion

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

        /// <summary>
        /// Is this entity within the screen display area (as of last update)?
        /// </summary>
        public bool OnScreen { get; private set; }

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
            Initialize(a_tTexture, a_v2Position, 1, 1, 1,
                       0.0f, Color.White, Vector2.One, true);
        }
        public override void Initialize(Texture2D a_tTexture, Vector2 a_v2Pos,
                                                int a_iColumns, int a_iRows,
                                                int a_iFrameCount, float a_fFPS,
                                                Color a_cColor, Vector2 a_v2Scale,
                                                bool a_bLooping)
        {
            base.Initialize(a_tTexture, a_v2Pos, a_iColumns, a_iRows,
                            a_iFrameCount, a_fFPS, a_cColor, a_v2Scale,
                            a_bLooping);

            // Assign OnScreen
            OnScreen = true;

#if DEBUG
            // In debug mode, note that the entity has been initialized
            m_bInitialized = true;
#endif
        }

        #region Update logic

        /// <summary>
        /// Update logic for this entity, if it needs updating each frame
        /// </summary>
        /// <param name="a_oContext"></param>
        public virtual void Update(GameState.Context a_oContext)
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
                // Animate
                Update(a_oContext.time);

                // Update Position
                Vector2 v2OldPosition = Position;
                Position += Velocity *
                            (float)a_oContext.time.ElapsedGameTime.TotalSeconds;

                // Update Rotation
                Rotation += AngularVelocity *
                            (float)a_oContext.time.ElapsedGameTime.TotalSeconds;

                // Update OnScreen
                UpdateOnScreen(a_oContext.viewport, v2OldPosition);
            }
        }

        /// <summary>
        /// Update OnScreen property and adjust position based on wrap/clamp settings
        /// </summary>
        /// <param name="a_oDisplayArea">Screen display area</param>
        /// <param name="a_v2OldPosition">Position before the last update</param>
        private void UpdateOnScreen(Rectangle a_oDisplayArea,
                                    Vector2 a_v2OldPosition)
        {
            // Check for clamping to the screen
            Vector2 v2IntermediatePosition = Position;
            if (ScreenClamp(a_oDisplayArea))
            {
                a_v2OldPosition = v2IntermediatePosition;
            }

            // Check for exiting the screen
            if (OnScreen && CheckOffscreen(a_oDisplayArea))
            {
                OnScreen = false;
                OnScreenExit(a_oDisplayArea, a_v2OldPosition);
            }

            // Check for screen wrapping
            v2IntermediatePosition = Position;
            if (ScreenWrap(a_oDisplayArea))
            {
                a_v2OldPosition = v2IntermediatePosition;
            }

            // Check for entering the screen
            if (!OnScreen && !CheckOffscreen(a_oDisplayArea))
            {
                OnScreen = true;
                OnScreenEnter(a_oDisplayArea, a_v2OldPosition);
            }
        }

        /// <summary>
        /// If appropriate, clamp this entity's position to within the given
        /// display area
        /// </summary>
        /// <param name="a_oDisplayArea">Display area to clamp within</param>
        /// <returns>True if clamping occured</returns>
        private bool ScreenClamp(Rectangle a_oDisplayArea)
        {
            if (ClampSetting.None != Clamp && CheckOffscreen(a_oDisplayArea))
            {
                // Trigger OnScreenClamp?
                bool bClamp = false;

                // bounce?
                bool bBounce = (ClampSetting.AlwaysHalt == Clamp ||
                                ClampSetting.HaltOnExit == Clamp);

                // clamp on both enter and exit?
                bool bAlwaysClamp = (ClampSetting.AlwaysHalt == Clamp ||
                                     ClampSetting.AlwaysBounce == Clamp);

                // Previous position and velocity for passing to OnScreenWrap
                Vector2 v2Position = Position;
                Vector2 v2Velocity = Velocity;

                // Vertical Clamp
                // Top
                if (Top < a_oDisplayArea.Top &&
                    (bAlwaysClamp || Velocity.Y <= 0))
                {
                    Top += (a_oDisplayArea.Top - Top) * (bBounce ? 2 : 1);
                    if (Velocity.Y <= 0)
                    {
                        Velocity = new Vector2(Velocity.X, bBounce ? 0 : -Velocity.Y);
                    }
                    bClamp = true;
                }
                // Below
                else if (Bottom > a_oDisplayArea.Bottom &&
                    (bAlwaysClamp || Velocity.Y >= 0))
                {
                    Bottom -= (Bottom - a_oDisplayArea.Bottom) * (bBounce ? 2 : 1);
                    if (Velocity.Y >= 0)
                    {
                        Velocity = new Vector2(Velocity.X, bBounce ? 0 : -Velocity.Y);
                    }
                    bClamp = true;
                }

                // Horizontal Clamp
                // Left
                if (Left < a_oDisplayArea.Left &&
                    (bAlwaysClamp || Velocity.X <= 0))
                {
                    Left += (a_oDisplayArea.Left - Left) * (bBounce ? 2 : 1);
                    if (Velocity.X <= 0)
                    {
                        Velocity = new Vector2(bBounce ? 0 : -Velocity.X, Velocity.Y);
                    }
                    bClamp = true;
                }
                // Right
                else if (Left > a_oDisplayArea.Right &&
                    (bAlwaysClamp || Velocity.X >= 0))
                {
                    Right += (Right - a_oDisplayArea.Right) * (bBounce ? 2 : 1);
                    if (Velocity.X >= 0)
                    {
                        Velocity = new Vector2(bBounce ? 0 : -Velocity.X, Velocity.Y);
                    }
                    bClamp = true;
                }

                // Call any functions triggered by clamping
                if (bClamp)
                {
                    OnScreenClamp(a_oDisplayArea, v2Position, v2Velocity);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// If appropriate, wrap this entity's position to within the given
        /// display area
        /// </summary>
        /// <param name="a_oDisplayArea">Display area to wrap within</param>
        /// <returns>True if wrapping occured</returns>
        private bool ScreenWrap(Rectangle a_oDisplayArea)
        {
            if (WrapSetting.None != Wrap && CheckOffscreen(a_oDisplayArea))
            {
                // Trigger OnScreenWrap?
                bool bWrap = false;

                // Previous position for passing to OnScreenWrap
                Vector2 v2Position = Position;

                // Vertical Wrap
                // Top
                if (Bottom < a_oDisplayArea.Top &&
                    (WrapSetting.Always == Wrap || Velocity.Y <= 0))
                {
                    Top = a_oDisplayArea.Bottom;
                    bWrap = true;
                }
                // Below
                else if (Top > a_oDisplayArea.Bottom &&
                    (WrapSetting.Always == Wrap || Velocity.Y >= 0))
                {
                    Bottom = a_oDisplayArea.Top;
                    bWrap = true;
                }
            
                // Horizontal Wrap
                // Left
                if (Right < a_oDisplayArea.Left &&
                    (WrapSetting.Always == Wrap || Velocity.X <= 0))
                {
                    Left = a_oDisplayArea.Right;
                    bWrap = true;
                }
                // Right
                else if (Left > a_oDisplayArea.Right &&
                    (WrapSetting.Always == Wrap || Velocity.X >= 0))
                {
                    Right = a_oDisplayArea.Left;
                    bWrap = true;
                }

                // Call any functions triggered by wrapping
                if (bWrap)
                {
                    OnScreenWrap(a_oDisplayArea, v2Position);
                    return true;
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Is this entity no longer within the screen?
        /// </summary>
        /// <param name="a_ptScreenSize">Screen size</param>
        /// <returns>True if entity is completely outside screen area</returns>
        public bool CheckOffscreen(Rectangle a_oDisplayArea)
        {
            return !a_oDisplayArea.Intersects(Boundary);
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

        #region Virtual functions for handling events

        /// <summary>
        /// Called when the entity changes from inactive to active
        /// </summary>
        public virtual void OnActive() { }

        /// <summary>
        /// Called when the entity changes from active to inactive
        /// </summary>
        public virtual void OnInactive() { }

        /// <summary>
        /// Called when the entity's position is clamped within the screen
        /// </summary>
        /// <param name="a_oDisplayArea">Display area clamped within</param>
        /// <param name="a_v2OldPosition">Position before clamping</param>
        /// <param name="a_v2OldVelocity">Velocity before clamping</param>
        public virtual void OnScreenClamp(Rectangle a_oDisplayArea,
                                          Vector2 a_v2OldPosition,
                                          Vector2 a_v2OldVelocity) { }

        /// <summary>
        /// Called when the entity is completely visible
        /// </summary>
        /// <param name="a_oDisplayArea">Display area entered</param>
        /// <param name="a_v2OldPosition">Position before entering</param>
        public virtual void OnScreenEnter(Rectangle a_oDisplayArea,
                                          Vector2 a_v2OldPosition) { }

        /// <summary>
        /// Called when the entity is completely not visible
        /// </summary>
        /// <param name="a_oDisplayArea">Display area exited</param>
        /// <param name="a_v2OldPosition">Position before exiting</param>
        public virtual void OnScreenExit(Rectangle a_oDisplayArea,
                                         Vector2 a_v2OldPosition) { }

        /// <summary>
        /// Called when the entity wraps from one edge of the screen to another
        /// </summary>
        /// <param name="a_oDisplayArea">Display area wrapped within</param>
        /// <param name="a_v2OldPosition">Position before wrapping</param>
        public virtual void OnScreenWrap(Rectangle a_oDisplayArea,
                                         Vector2 a_v2OldPosition) { }

        #endregion
    }
}
