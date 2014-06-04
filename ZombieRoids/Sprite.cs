/// <list type="table">
/// <listheader><term>Sprite.cs</term><description>
///     Sprite class - base for objects drawn to the screen
/// </description></listheader>
/// <item><term>Author</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 2, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Terry Nguyen
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 3, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Merged with dev for @emlowry Sprite refactor
/// </description></item>
/// </list>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRoids
{
    /// <remarks>
    /// Base class for objects drawn to the screen
    /// </remarks>
    class Sprite
    {

        #region Basic transformation properties

        #region Position

        /// <summary>
        /// Position of the center of the sprite in world coordinates
        /// </summary>
        public Vector2 Position
        {
            get { return m_v2Position; }
            set { m_v2Position = value; }
        }
        private Vector2 m_v2Position = Vector2.Zero;

        /// <summary>
        /// Horizontal position of the center of the sprite in world coordinates
        /// </summary>
        public float X
        {
            get { return m_v2Position.X; }
            set { m_v2Position.X = value; }
        }

        /// <summary>
        /// Vertical position of the center of the sprite in world coordinates
        /// </summary>
        public float Y
        {
            get { return m_v2Position.Y; }
            set { m_v2Position.Y = value; }
        }

        #endregion

        /// <summary>
        /// Rotation of the sprite about its center in radians
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Rotation of the sprite about its center in degrees
        /// </summary>
        public float RotationInDegrees
        {
            get { return (float)(Rotation * 360 / Math.PI); }
            set { Rotation = (float)(value * Math.PI / 360); }
        }

        #region Scale

        /// <summary>
        /// Scale of the sprite
        /// </summary>
        public Vector2 Scale
        {
            get { return m_v2Scale; }
            set { m_v2Scale = value; }
        }
        private Vector2 m_v2Scale = Vector2.One;

        /// <summary>
        /// Horizontal scale of the sprite
        /// </summary>
        public float HorizontalScale
        {
            get { return m_v2Scale.X; }
            set { m_v2Scale.X = value; }
        }

        /// <summary>
        /// Horizontal scale of the sprite
        /// </summary>
        public float VerticalScale
        {
            get { return m_v2Scale.Y; }
            set { m_v2Scale.Y = value; }
        }

        #endregion

        #region Slice

        /// <summary>
        /// The area of the texture to draw in texels (null means draw all)
        /// </summary>
        public Rectangle? Slice
        {
            get { return m_oSlice; }
            set { m_oSlice = value; }
        }
        private Rectangle? m_oSlice = null;

        /// <summary>
        /// Width in texels of the area of the texture to draw
        /// </summary>
        public int SliceWidth
        {
            get
            {
                return (null == m_oSlice ? Texture.Width
                                         : ((Rectangle)m_oSlice).Width);
            }
            set
            {
                Rectangle oSlice = (null == m_oSlice ? Texture.Bounds
                                                     : (Rectangle)m_oSlice);
                oSlice.Width = value;
                m_oSlice = oSlice;
            }
        }

        /// <summary>
        /// Height in texels of the area of the texture to draw
        /// </summary>
        public int SliceHeight
        {
            get
            {
                return (null == m_oSlice ? Texture.Height
                                         : ((Rectangle)m_oSlice).Height);
            }
            set
            {
                Rectangle oSlice = (null == m_oSlice ? Texture.Bounds
                                                     : (Rectangle)m_oSlice);
                oSlice.Height = value;
                m_oSlice = oSlice;
            }
        }

        /// <summary>
        /// Distance in texels from the left edge of the texture to the left
        /// edge of the area of the texture to draw
        /// </summary>
        public int SliceLeft
        {
            get { return (null == m_oSlice ? 0 : ((Rectangle)m_oSlice).X); }
            set
            {
                Rectangle oSlice = (null == m_oSlice ? Texture.Bounds
                                                     : (Rectangle)m_oSlice);
                oSlice.X = value;
                m_oSlice = oSlice;
            }
        }

        /// <summary>
        /// Distance in texels from the top of the texture to the top of the
        /// area of the texture to draw
        /// </summary>
        public int SliceTop
        {
            get { return (null == m_oSlice ? 0 : ((Rectangle)m_oSlice).Y); }
            set
            {
                Rectangle oSlice = (null == m_oSlice ? Texture.Bounds
                                                     : (Rectangle)m_oSlice);
                oSlice.Y = value;
                m_oSlice = oSlice;
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Sprite texture
        /// </summary>
        public Texture2D Texture { get; set; }

        #region Color

        /// <summary>
        /// Color to tint the sprite with when drawing
        /// </summary>
        public Color Tint
        {
            get { return m_oTint; }
            set { m_oTint = value; }
        }
        private Color m_oTint = Color.White;

        /// <summary>
        /// Tint color alpha channel
        /// </summary>
        public float TintAlpha
        {
            get { return (float)m_oTint.A / 255; }
            set
            {
                m_oTint.A = (byte)MathHelper.Clamp((int)(value * 255), 0, 255);
            }
        }

        /// <summary>
        /// Tint color red channel
        /// </summary>
        public float TintRed
        {
            get { return (float)m_oTint.R / 255; }
            set
            {
                m_oTint.R = (byte)MathHelper.Clamp((int)(value * 255), 0, 255);
            }
        }

        /// <summary>
        /// Tint color red channel
        /// </summary>
        public float TintGreen
        {
            get { return (float)m_oTint.G / 255; }
            set
            {
                m_oTint.G = (byte)MathHelper.Clamp((int)(value * 255), 0, 255);
            }
        }

        /// <summary>
        /// Tint color red channel
        /// </summary>
        public float TintBlue
        {
            get { return (float)m_oTint.B / 255; }
            set
            {
                m_oTint.B = (byte)MathHelper.Clamp((int)(value * 255), 0, 255);
            }
        }

        #endregion

        public bool Visible
        {
            get { return m_bVisible && null != Texture; }
            set { m_bVisible = value; }
        }
        private bool m_bVisible = true;

        #region Properties dependant on other properties

        /// <summary>
        /// Rectangle outlining the sprite (before rotation)
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return new Rectangle((int)Left, (int)Top,
                                     (int)Width, (int)Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
                Left = value.X;
                Top = value.Y;
            }
        }

        /// <summary>
        /// Unit vector in the direction of this sprite's rotation
        /// </summary>
        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(Rotation),
                                   (float)Math.Sin(Rotation));
            }
            set
            {
                if (Vector2.Zero != value)
                {
                    Rotation = (float)Math.Atan2(value.Y, value.X);
                }
            }
        }

        #region Position

        /// <summary>
        /// Distance from the world x-axis to the bottom of the sprite (before
        /// rotation)
        /// </summary>
        public float Bottom
        {
            get { return Y + (Height / 2); }
            set { Y = value - (Height / 2); }
        }

        /// <summary>
        /// Location of the bottom-left corner of the sprite in world
        /// coordinates (before rotation)
        /// </summary>
        public Vector2 BottomLeft
        {
            get { return new Vector2(Left, Bottom); }
            set { Left = value.X; Bottom = value.Y; }
        }

        /// <summary>
        /// Location of the bottom-right corner of the sprite in world
        /// coordinates (before rotation)
        /// </summary>
        public Vector2 BottomRight
        {
            get { return new Vector2(Right, Bottom); }
            set { Right = value.X; Bottom = value.Y; }
        }

        /// <summary>
        /// Distance from the world y-axis to the left edge of the sprite
        /// (before rotation)
        /// </summary>
        public float Left
        {
            get { return X - (Width / 2); }
            set { X = value + (Width / 2); }
        }

        /// <summary>
        /// Distance from the world y-axis to the right edge of the sprite
        /// (before rotation)
        /// </summary>
        public float Right
        {
            get { return X + (Width / 2); }
            set { X = value - (Width / 2); }
        }

        /// <summary>
        /// Distance from the world x-axis to the top of the sprite (before
        /// rotation)
        /// </summary>
        public float Top
        {
            get { return Y - (Height / 2); }
            set { Y = value + (Height / 2); }
        }

        /// <summary>
        /// Location of the top-left corner of the sprite in world coordinates
        /// (before rotation)
        /// </summary>
        public Vector2 TopLeft
        {
            get { return new Vector2(Left, Top); }
            set { Left = value.X; Top = value.Y; }
        }

        /// <summary>
        /// Location of the top-right corner of the sprite in world coordinates
        /// (before rotation)
        /// </summary>
        public Vector2 TopRight
        {
            get { return new Vector2(Right, Top); }
            set { Right = value.X; Top = value.Y; }
        }

        #endregion

        #region Size

        /// <summary>
        /// Height of the sprite
        /// </summary>
        public float Height
        {
            get { return SliceHeight * VerticalScale; }
            set { VerticalScale = value / SliceHeight; }
        }

        /// <summary>
        /// Length and Width of the sprite
        /// </summary>
        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
            set { Width = value.X; Height = value.Y; }
        }

        /// <summary>
        /// Width of the sprite
        /// </summary>
        public float Width
        {
            get { return SliceWidth * HorizontalScale; }
            set { HorizontalScale = value / SliceWidth; }
        }

        #endregion

        #region Slice

        /// <summary>
        /// Distance in texels from the top of the texture to the bottom of the
        /// area of the texture to draw
        /// </summary>
        public int SliceBottom
        {
            get { return SliceTop + SliceHeight; }
            set { SliceHeight = value - SliceTop; }
        }
        
        /// <summary>
        /// Location in texels of the bottom-left corner of the area of the
        /// texture to draw relative to the top-left corner of the texture
        /// </summary>
        public Point SliceBottomLeft
        {
            get { return new Point(SliceLeft, SliceBottom); }
            set { SliceLeft = value.X; SliceBottom = value.Y; }
        }

        /// <summary>
        /// Location in texels of the bottom-right corner of the area of the
        /// texture to draw relative to the top-left corner of the texture
        /// </summary>
        public Point SliceBottomRight
        {
            get { return new Point(SliceRight, SliceBottom); }
            set { SliceRight = value.X; SliceBottom = value.Y; }
        }

        public Point SliceSize
        {
            get { return new Point(SliceWidth, SliceHeight); }
            set { SliceWidth = value.X; SliceHeight = value.Y; }
        }

        /// <summary>
        /// Distance in texels from the right edge of the texture to the right
        /// edge of the area of the texture to draw
        /// </summary>
        public int SliceRight
        {
            get { return SliceLeft + SliceWidth; }
            set { SliceWidth = value - SliceLeft; }
        }

        /// <summary>
        /// Location in texels of the top-left corner of the area of the texture
        /// to draw relative to the top-left corner of the texture
        /// </summary>
        public Point SliceTopLeft
        {
            get { return new Point(SliceLeft, SliceTop); }
            set { SliceLeft = value.X; SliceTop = value.Y; }
        }

        /// <summary>
        /// Location in texels of the top-right corner of the area of the
        /// texture to draw relative to the top-left corner of the texture
        /// </summary>
        public Point SliceTopRight
        {
            get { return new Point(SliceRight, SliceTop); }
            set { SliceRight = value.X; SliceTop = value.Y; }
        }

        #endregion

        #endregion

        /// <summary>
        /// Queues the sprite for drawing in a sprite batch
        /// </summary>
        /// <param name="a_sbSpriteBatch">SpriteBatch to queue into</param>
        public virtual void Draw(SpriteBatch a_oSpriteBatch)
        {
            if (Visible)
            {
                a_oSpriteBatch.Draw(Texture, Boundary, Slice, Tint, Rotation,
                                    new Vector2(Width / 2, Height / 2),
                                    SpriteEffects.None, 0f);
            }
        }
    }
}
