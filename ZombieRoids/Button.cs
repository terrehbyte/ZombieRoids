/// <list type="table">
/// <listheader><term>Button.cs</term><description>
///     Class representing a clickable button displayed on the screen
/// </description></listheader>
/// <item><term>Author</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 10, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 11, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Fixing button tint bug
/// </description></item>
/// </list>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ZombieRoids
{
    /// <remarks>
    /// Class representing a clickable button displayed on the screen that can
    /// call event handlers when clicked or hovered over.
    /// </remarks>
    public class Button : Sprite
    {
        /// <summary>
        /// Delegate type for handling button events
        /// </summary>
        /// <param name="a_oOrigin">The button calling the event handler</param>
        /// <param name="a_oContext">Game context when called</param>
        public delegate void EventHandler(Button a_oOrigin,
                                          GameState.Context a_oContext);

        /// <summary>
        /// Called on an update where the mouse is hovering over this button,
        /// the left mouse button is pressed, and the previous update did not
        /// have this button being clicked.
        /// </summary>
        public EventHandler OnClickStart { get; set; }

        /// <summary>
        /// Called on every update where the mouse is hovering over this button
        /// and the the left mouse button is pressed.
        /// </summary>
        public EventHandler OnClick { get; set; }

        /// <summary>
        /// Called on an update where the mouse is not hovering over this button
        /// or the left mouse button is not pressed but the previous update had
        /// this button being clicked.
        /// </summary>
        public EventHandler OnClickEnd { get; set; }

        /// <summary>
        /// Called on an update where the mouse is hovering over this button and
        /// the previous update did not have this button being hovered over.
        /// </summary>
        public EventHandler OnHoverStart { get; set; }

        /// <summary>
        /// Called on every update where the mouse is hovering over this button.
        /// </summary>
        public EventHandler OnHover { get; set; }

        /// <summary>
        /// Called on an update where the mouse is not hovering over this button
        /// but the previous update had this button being hovered over.
        /// </summary>
        public EventHandler OnHoverEnd { get; set; }

        /// <summary>
        /// Is the mouse hovering over this button?
        /// </summary>
        public bool Hover
        {
            get { return m_bHover; }
            private set
            {
                if (value != m_bHover)
                {
                    m_bHover = value;
                    ResetTint();
                }
            }
        }
        private bool m_bHover = false;

        /// <summary>
        /// Is the mouse hovering over this button with the left button pressed?
        /// </summary>
        public bool Click
        {
            get { return m_bClick; }
            private set
            {
                if (value != m_bClick)
                {
                    m_bClick = value;
                    ResetTint();
                }
            }
        }
        private bool m_bClick = false;

        /// <summary>
        /// Current Selection state
        /// </summary>
        public bool Selected
        {
            get { return m_bSelected; }
            set
            {
                if (value != m_bSelected)
                {
                    m_bSelected = value;
                    ResetTint();
                }
            }
        }
        private bool m_bSelected = false;

        /// <summary>
        /// Set tint according to click, hover, and selected state
        /// </summary>
        public void ResetTint()
        {
            Tint = (Click ? GameConsts.ButtonClickTint :
                    Hover ? GameConsts.ButtonHoverTint :
                    Selected ? GameConsts.ButtonSelectedTint :
                    GameConsts.ButtonNormalTint);
        }

        /// <summary>
        /// Updates hover/click state and calls appropriate event handlers
        /// </summary>
        /// <param name="a_oContext"></param>
        public void Update(GameState.Context a_oContext)
        {
            // Get mouse location and left button state
            MouseState mCurState = Mouse.GetState();
            bool bHover = Boundary.Contains(mCurState.Position);
            bool bClick = ((ButtonState.Pressed == mCurState.LeftButton) && bHover);
            EventHandler ehToCall = null;

            // Update hover state
            if (bHover)
            {
                if (!Hover)
                {
                    ehToCall += OnHoverStart;
                }
                ehToCall += OnHover;
            }
            else if (Hover)
            {
                ehToCall += OnHoverEnd;
            }
            Hover = bHover;

            // Update click state
            if (bClick)
            {
                if (!Click)
                {
                    ehToCall += OnClickStart;
                }
                ehToCall += OnClick;
            }
            else if (Click)
            {
                ehToCall += OnClickEnd;
            }
            Click = bClick;

            // Call triggered event handlers
            if (null != ehToCall)
            {
                ehToCall(this, a_oContext);
            }
        }
    }
}
