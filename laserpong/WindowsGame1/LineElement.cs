using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class LineElement
    {
        private Vector2 m_start;
        private Vector2 m_end;

        private Vector2 m_front;
        private float m_length;
        private float m_orientation;
        private Vector2 m_direction;

        public Vector2 Start { get { return m_start; } }
        public Vector2 End { get { return m_end; } }

        public Vector2 Front { get { return m_front; } }
        public float Length { get { return m_length; } }
        public float Orientation { get { return m_orientation; } }
        public Vector2 Direction { get { return m_direction; } }


        protected Vector2 LStart { get { return m_start; } set { m_start = value; updateFrontLengthOrientation(m_start, m_end); } }
        protected Vector2 LEnd { get { return m_end; } set { m_end = value; updateFrontLengthOrientation(m_start, m_end); } }

        protected Vector2 LFront { get { return m_front; } set { m_front = value; updateStartEnd(m_front, m_length, m_orientation); } }
        protected float LLength { get { return m_length; } set { m_length = value; updateStartEnd(m_front, m_length, m_orientation); } }
        protected float LOrientation {
            get { return m_orientation; }
            set {
                m_orientation = value;
                m_direction = computeDirection(m_orientation);
                updateStartEnd(m_front, m_length, m_orientation);
            } 
        }
        protected Vector2 LDirection
        {
            get { return m_direction; }
            set
            {
                m_direction = value;
                m_orientation = computeOrientation(m_direction);
                updateStartEnd(m_front, m_length, m_orientation);
            }
        }

        /***
         * Compute the orientation (in radians) of a given direction vector (need not be normalized).
         * @param direction Direction vector to compute orientation from.
         * @return orientation of the given direction vector (in radians)
         */
        public static float computeOrientation(Vector2 direction)
        {
            float theta = (float)Math.Atan((double)direction.Y / (double)direction.X);
            if (direction.X < 0) theta += (float)Math.PI;

            return theta;
        }

        public static Vector2 computeDirection(float orientation)
        {
            return new Vector2((float)Math.Cos((double)orientation), (float)Math.Sin((double)orientation));
        }

        private void updateStartEnd(Vector2 front, float length, float orientation)
        {
            Vector2 direction = computeDirection(orientation);

            m_start = front;
            m_end = m_start + (length * -direction);
        }
        private void updateFrontLengthOrientation(Vector2 start, Vector2 end)
        {
            m_front = start;
            m_length = (end - start).Length();
            m_direction = (end - start);
            m_direction.Normalize();

            m_orientation = computeOrientation(m_direction);
        }


        
    }
}
