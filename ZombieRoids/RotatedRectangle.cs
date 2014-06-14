/// <list type="table">
/// <listheader><term>RotatedRectangle.cs</term><description>
///     Rotated bounding box for checking collisions of rotated rectangles
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
///     Adding comments
/// </description></item>
/// </list>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using ZombieRoids;

//@terrehbyte: THIS CODE FROM: http://bit.ly/1l0YMzB - PLEASE REVIEW

namespace RotatedRectangleCollisions
{
    /// <remarks>
    /// Rotated bounding box
    /// </remarks>
    public class RotatedBoxCollider
    {
        // bounding box before rotation
        public Rectangle CollisionRectangle
        {
            get { return m_oCollisionRectangle; }
            set { m_oCollisionRectangle = value; }
        }
        private Rectangle m_oCollisionRectangle;

        /// <summary>
        /// Angle of rotation in radians
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Origin about which the rotation occurs
        /// </summary>
        public Vector2 Origin { get; set; }

        #region Rectangle properties

        /// <summary>
        /// X coordinate of the rectangle's center, about which it rotates
        /// </summary>
        public int X
        {
            get { return CollisionRectangle.Center.X; }
        }

        /// <summary>
        /// Y coordinate of the rectangle's center, about which it rotates
        /// </summary>
        public int Y
        {
            get { return CollisionRectangle.Center.Y; }
        }

        /// <summary>
        /// Width of the rectangle before rotation
        /// </summary>
        public int Width
        {
            get { return CollisionRectangle.Width; }
        }

        /// <summary>
        /// Height of the rectangle before rotation
        /// </summary>
        public int Height
        {
            get { return CollisionRectangle.Height; }
        }

        #endregion

        #region Rotated corner positions

        /// <summary>
        /// Top-left corner coordinates rotated about origin
        /// </summary>
        public Vector2 TopLeft
        {
            get
            {
                Vector2 aUpperLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Top);
                aUpperLeft = RotatePoint(aUpperLeft, aUpperLeft + Origin, Rotation);
                return aUpperLeft;
            }
        }

        /// <summary>
        /// Top-right corner coordinates rotated about origin
        /// </summary>
        public Vector2 TopRight
        {
            get
            {
                Vector2 aUpperRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Top);
                aUpperRight = RotatePoint(aUpperRight, aUpperRight + new Vector2(-Origin.X, Origin.Y), Rotation);
                return aUpperRight;
            }
        }

        /// <summary>
        /// Bottom-left corner coordinates rotated about origin
        /// </summary>
        public Vector2 BottomLeft
        {
            get
            {
                Vector2 aLowerLeft = new Vector2(CollisionRectangle.Left, CollisionRectangle.Bottom);
                aLowerLeft = RotatePoint(aLowerLeft, aLowerLeft + new Vector2(Origin.X, -Origin.Y), Rotation);
                return aLowerLeft;
            }
        }

        /// <summary>
        /// Bottom-right corner coordinates rotated about origin
        /// </summary>
        public Vector2 BottomRight
        {
            get
            {
                Vector2 aLowerRight = new Vector2(CollisionRectangle.Right, CollisionRectangle.Bottom);
                aLowerRight = RotatePoint(aLowerRight, aLowerRight + new Vector2(-Origin.X, -Origin.Y), Rotation);
                return aLowerRight;
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="theRectangle">Rectangle to rotate about its center</param>
        /// <param name="theInitialRotation">Angle of rotation in radians</param>
        public RotatedBoxCollider(Rectangle theRectangle, float theInitialRotation)
        {
            CollisionRectangle = theRectangle;
            Rotation = theInitialRotation;

            //Calculate the Rectangles origin. We assume the center of the Rectangle will
            //be the point that we will be rotating around and we use that for the origin
            Origin = new Vector2((int)theRectangle.Width / 2, (int)theRectangle.Height / 2);
        }

        /// <summary>
        /// Used for changing the X and Y position of the RotatedRectangle
        /// </summary>
        /// <param name="theXPositionAdjustment"></param>
        /// <param name="theYPositionAdjustment"></param>
        public void ChangePosition(int theXPositionAdjustment, int theYPositionAdjustment)
        {
            m_oCollisionRectangle.X += theXPositionAdjustment;
            m_oCollisionRectangle.Y += theYPositionAdjustment;
        }

        /// <summary>
        /// This intersects method can be used to check a standard XNA framework Rectangle
        /// object and see if it collides with a Rotated Rectangle object
        /// </summary>
        /// <param name="theRectangle"></param>
        /// <returns></returns>
        public bool Intersects(Rectangle theRectangle)
        {
            return Intersects(new RotatedBoxCollider(theRectangle, 0.0f));
        }

        /// <summary>
        /// Check to see if two Rotated Rectangls have collided
        /// </summary>
        /// <param name="theRectangle"></param>
        /// <returns></returns>
        public bool Intersects(RotatedBoxCollider theRectangle)
        {
            //Calculate the Axis we will use to determine if a collision has occurred
            //Since the objects are rectangles, we only have to generate 4 Axis (2 for
            //each rectangle) since we know the other 2 on a rectangle are parallel.
            List<Vector2> aRectangleAxis = new List<Vector2>();
            aRectangleAxis.Add(TopRight - TopLeft);
            aRectangleAxis.Add(TopRight - BottomRight);
            aRectangleAxis.Add(theRectangle.TopLeft - theRectangle.BottomLeft);
            aRectangleAxis.Add(theRectangle.TopLeft - theRectangle.TopRight);

            //Cycle through all of the Axis we need to check. If a collision does not occur
            //on ALL of the Axis, then a collision is NOT occurring. We can then exit out 
            //immediately and notify the calling function that no collision was detected. If
            //a collision DOES occur on ALL of the Axis, then there is a collision occurring
            //between the rotated rectangles. We know this to be true by the Seperating Axis Theorem
            foreach (Vector2 aAxis in aRectangleAxis)
            {
                if (!IsAxisCollision(theRectangle, aAxis))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if a collision has occurred on an Axis of one of the
        /// planes parallel to the Rectangle
        /// </summary>
        /// <param name="theRectangle"></param>
        /// <param name="aAxis"></param>
        /// <returns></returns>
        private bool IsAxisCollision(RotatedBoxCollider theRectangle, Vector2 aAxis)
        {
            //Project the corners of the Rectangle we are checking on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleAScalars = new List<int>();
            aRectangleAScalars.Add(GenerateScalar(theRectangle.TopLeft, aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.TopRight, aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.BottomLeft, aAxis));
            aRectangleAScalars.Add(GenerateScalar(theRectangle.BottomRight, aAxis));

            //Project the corners of the current Rectangle on to the Axis and
            //get a scalar value of that project we can then use for comparison
            List<int> aRectangleBScalars = new List<int>();
            aRectangleBScalars.Add(GenerateScalar(TopLeft, aAxis));
            aRectangleBScalars.Add(GenerateScalar(TopRight, aAxis));
            aRectangleBScalars.Add(GenerateScalar(BottomLeft, aAxis));
            aRectangleBScalars.Add(GenerateScalar(BottomRight, aAxis));

            //Get the Maximum and Minium Scalar values for each of the Rectangles
            int aRectangleAMinimum = aRectangleAScalars.Min();
            int aRectangleAMaximum = aRectangleAScalars.Max();
            int aRectangleBMinimum = aRectangleBScalars.Min();
            int aRectangleBMaximum = aRectangleBScalars.Max();

            //If we have overlaps between the Rectangles (i.e. Min of B is less than Max of A)
            //then we are detecting a collision between the rectangles on this Axis
            if (aRectangleBMinimum <= aRectangleAMaximum && aRectangleBMaximum >= aRectangleAMaximum)
            {
                return true;
            }
            else if (aRectangleAMinimum <= aRectangleBMaximum && aRectangleAMaximum >= aRectangleBMaximum)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Generates a scalar value that can be used to compare where corners of 
        /// a rectangle have been projected onto a particular axis. 
        /// </summary>
        /// <param name="theRectangleCorner"></param>
        /// <param name="theAxis"></param>
        /// <returns></returns>
        private int GenerateScalar(Vector2 theRectangleCorner, Vector2 theAxis)
        {
            //Using the formula for Vector projection. Take the corner being passed in
            //and project it onto the given Axis
            float aNumerator = (theRectangleCorner.X * theAxis.X) + (theRectangleCorner.Y * theAxis.Y);
            float aDenominator = (theAxis.X * theAxis.X) + (theAxis.Y * theAxis.Y);
            float aDivisionResult = aNumerator / aDenominator;
            Vector2 aCornerProjected = new Vector2(aDivisionResult * theAxis.X, aDivisionResult * theAxis.Y);

            //Now that we have our projected Vector, calculate a scalar of that projection
            //that can be used to more easily do comparisons
            float aScalar = (theAxis.X * aCornerProjected.X) + (theAxis.Y * aCornerProjected.Y);
            return (int)aScalar;
        }

        /// <summary>
        /// Rotate a point from a given location and adjust using the Origin we
        /// are rotating around
        /// </summary>
        /// <param name="thePoint"></param>
        /// <param name="theOrigin"></param>
        /// <param name="theRotation"></param>
        /// <returns></returns>
        private Vector2 RotatePoint(Vector2 thePoint, Vector2 theOrigin, float theRotation)
        {
            Vector2 aTranslatedPoint = new Vector2();
            aTranslatedPoint.X = (float)(theOrigin.X + (thePoint.X - theOrigin.X) * Math.Cos(theRotation)
                - (thePoint.Y - theOrigin.Y) * Math.Sin(theRotation));
            aTranslatedPoint.Y = (float)(theOrigin.Y + (thePoint.Y - theOrigin.Y) * Math.Cos(theRotation)
                + (thePoint.X - theOrigin.X) * Math.Sin(theRotation));
            return aTranslatedPoint;
        }

    }
}
