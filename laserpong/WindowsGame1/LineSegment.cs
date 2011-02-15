using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    struct LineSegment
    {
        private Vector2 start;
        private Vector2 end;

        public Vector2 Start { get { return start; } set { start = value; } }
        public Vector2 End { get { return end; } set { end = value; } }

        public LineSegment(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        /* Instance */
        public float Length()
        {
            return Vector2.Distance(start, end);
        }

        public float RotationAboveXAxis()
        {
            return (float)Math.Atan(start.X - end.X / start.Y / end.Y);
        }

        public Vector2 NormalizedDirection()
        {
            Vector2 direction = end - start;
            direction.Normalize();

            return direction;
        }

        /* PUBLIC STATIC */
        static bool Intersects(LineSegment first, LineSegment second)
        {
            return false;
        }

        static Vector2? IntersectionPoint(LineSegment first, LineSegment second)
        {
            return null;
        }
    }
}
