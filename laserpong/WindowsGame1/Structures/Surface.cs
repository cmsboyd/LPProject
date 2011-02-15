using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame1
{
    class Surface
    {
        public enum SurfaceType
        {
            Absorbant,
            Reflective,
            Refractive
        };

        private const int SEGMENT_SPACING = 8;
        private static readonly Vector2 SEGMENT_SIZE = new Vector2(5, 5);

        private SurfaceType m_type;
        public SurfaceType M_type{get{return m_type;}}
        private Dictionary<Laser, Laser> m_collisions = new Dictionary<Laser,Laser>();
        private List<Laser> m_handledCollision = new List<Laser>();
        private Texture2D m_surface_corner;
        private Texture2D m_surface_side;
        private Color m_color;
        private int m_segments;
        public bool built= true;
        private float orientation;

        public Tower tower_A;
        public Tower tower_B;
        private Level parent;

        private LineSegment surfaceLineSegment;
        

        public Surface(SurfaceType type, Vector2 start, Vector2 end)
        {
            surfaceLineSegment = new LineSegment(start, end);
            m_type = type;

            construct();
        }


        public Surface(Tower a, Tower b, SurfaceType type, Level Parent)
        {
            tower_A = a; tower_A.AddSurface(this);
            tower_B = b; tower_B.AddSurface(this);
            m_type = type;

            surfaceLineSegment = new LineSegment(a.getPosition(), b.getPosition());

            parent = Parent;

            construct();
        }

        private void construct()
        {
            orientation = surfaceLineSegment.RotationAboveXAxis();
            m_segments = (int)surfaceLineSegment.Length() / SEGMENT_SPACING;

            m_collisions = new Dictionary<Laser, Laser>();

            if (m_type == SurfaceType.Reflective) m_color = Color.Silver;
            else if (m_type == SurfaceType.Refractive) m_color = Color.Pink;
            else m_color = Color.Black;
        }

        public void loadImage(ContentManager theCM)
        {
            m_surface_corner = theCM.Load<Texture2D>("surface_corner");
            m_surface_side = theCM.Load<Texture2D>("surface_side");

        }
        public void Update(GameTime t)
        {
            if (tower_A == null)
            {
                parent.RemoveSurface(this);
                tower_B.RemoveSurface(this);
            }

            if (tower_B == null)
            {
                parent.RemoveSurface(this);
                tower_A.RemoveSurface(this);
            }

            surfaceLineSegment.Start = tower_A.getPosition();
            surfaceLineSegment.End = tower_B.getPosition();

            orientation = surfaceLineSegment.RotationAboveXAxis();
            m_segments = (int)surfaceLineSegment.Length() / SEGMENT_SPACING;
        }

        public void ChangeType(Surface.SurfaceType type)
        {
            m_type = type;
                
            if (m_type == SurfaceType.Reflective) m_color = Color.Silver;
            else if (m_type == SurfaceType.Refractive) m_color = Color.White;
            else m_color = Color.Black;
        }

        public void Draw(SpriteBatch batch)
        {
            Color alphaBlendedColor = m_color;
            if (!built) alphaBlendedColor = new Color(m_color, .5f);
             
            for (int i = 0; i < m_segments; i++)
            {
                Vector2 position = surfaceLineSegment.Start + (SEGMENT_SPACING * i * surfaceLineSegment.NormalizedDirection());

                batch.Draw(m_surface_side, position + SEGMENT_SIZE, null, alphaBlendedColor, orientation, SEGMENT_SIZE, 
                    1f, SpriteEffects.None, .5f);
            }
        }

        public void changeTower(Tower new_tower)
        {
            tower_B = new_tower;
        }

        public bool isColliding(Laser l)
        {
            if (!built) return false;
            // First do a very fast box-collision detection
            /*Vector2 lTopLeft = new Vector2(Math.Min(l.Head.X, l.Tail.X), Math.Min(l.Head.Y, l.Tail.Y));
            Vector2 lBottomRight = new Vector2(Math.Max(l.Head.X, l.Tail.X), Math.Max(l.Head.Y, l.Tail.Y));

            Vector2 sTopLeft = new Vector2(Math.Min(Start.X, End.X), Math.Min(Start.Y, End.Y));
            Vector2 sBottomRight = new Vector2(Math.Max(Start.X, End.X), Math.Max(Start.Y, End.Y));

            if (!(lTopLeft.X < sBottomRight.X && lBottomRight.X < sTopLeft.X
                && lTopLeft.Y < sBottomRight.Y && lBottomRight.Y < sTopLeft.Y))
            {
                return false;
            }*/
            
            // More expensive line intersection.
            // time of intersection

            Vector2 u = l.End - l.Start;
            Vector2 v = surfaceLineSegment.End - surfaceLineSegment.Start;
            Vector2 w = l.Start - surfaceLineSegment.Start;

            float tIntersect = (v.Y * w.X - v.X * w.Y) / (v.X * u.Y - v.Y * u.X);
            if (tIntersect > 0f && tIntersect < 1f)
            {
                float sIntersect = (u.X * w.Y - u.Y * w.X) / (u.X * v.Y - u.Y * v.X);
                if (sIntersect > 0f && sIntersect < 1f)
                {
                    return true;
                }
            }
            return false;
        }

        public void handleCollision(Laser l, Level parent)
        {
            /* Is this a laser we're generating? Don't bother! */
            if (m_collisions.ContainsValue(l)) { return; }

            Laser generating;

            Vector2 u = l.End - l.Start;
            Vector2 v = surfaceLineSegment.End - surfaceLineSegment.Start;
            Vector2 w = l.Start - surfaceLineSegment.Start;

             // Do we need to flip?
            Vector2 vPerp = new Vector2(-v.Y, v.X);
            vPerp.Normalize();

            if ((Vector2.Dot(l.Start - surfaceLineSegment.Start, vPerp)) > 0) {
                // Need to flip!!
                v = surfaceLineSegment.Start - surfaceLineSegment.End;
            }

            float tIntersect = (v.Y * w.X - v.X * w.Y) / (v.X * u.Y - v.Y * u.X);

            Vector2 pIntersect = l.Start + tIntersect * u;
            
            // Have we already started a laser for this collision?
            if (!m_collisions.ContainsKey(l))
            {
                Vector2 direction = surfaceLineSegment.NormalizedDirection();
                Vector2 normal;
                normal.X = -direction.Y;
                normal.Y = direction.X;

                if (m_type == SurfaceType.Refractive)
                {
                    if (Vector2.Dot(normal, l.Direction) < 0) {
                        normal = -normal;
                    }

                    /* Need 3d vectors for rotations and cross products! */
                    Vector3 direction3d = new Vector3(l.Direction, 0f);
                    direction3d.Normalize();

                    Vector3 normal3d = new Vector3(normal, 0f);
                    /* Already normalized! */

                    Vector3 cross = Vector3.Cross(direction3d, normal3d);
                    bool incidentToNormalClockwiseRotation = (cross.Z > 0);

                    /* If direction -> normal is a CLOCKWISE rotation, then
                     * normal -> refractedDirection is a COUNTER-CLOCKWISE rotation. You will find this
                     * by the negating of theta2 below. */
                    

                    float incidentTheta = (float)Math.Acos(Vector3.Dot(direction3d, normal3d));
                    float refractedTheta = incidentTheta / 2;

                    if (incidentToNormalClockwiseRotation) {
                        refractedTheta = -refractedTheta; /* See above! */
                    }

                    Vector3 refractedDirection3d = Vector3.Transform(normal3d, Matrix.CreateRotationZ(refractedTheta));
                    Vector2 refractedDirection = new Vector2(refractedDirection3d.X, refractedDirection3d.Y);

                    generating = new Laser(pIntersect, refractedDirection, l.Color);
                }
                else
                {
                    Vector2 newDirection = Vector2.Reflect(l.Direction, normal);
                    generating = new Laser(pIntersect, newDirection, l.Color);
                }

                if (m_type == SurfaceType.Reflective || m_type == SurfaceType.Refractive)
                {
                    parent.AddLaser(generating);
                }
                m_collisions.Add(l, generating);
            } else {
                generating = m_collisions[l];
            }
            
            if (m_type == SurfaceType.Reflective || m_type == SurfaceType.Refractive)
            {
                generating.AdjustLength(u.Length() * tIntersect);
            }

            l.Chomp(u.Length() * tIntersect);

            m_handledCollision.Add(l);
        }

        public void deleteLasers(Level parent)
        {
            List<Laser> deleted = new List<Laser>();
            foreach (Laser l in m_collisions.Keys)
            {
                if (!m_handledCollision.Contains(l))
                {
                    // Finish chomping.
                    deleted.Add(l);
                }
            }

            foreach (Laser l in deleted)
            {
                if (m_type == SurfaceType.Reflective)
                {
                    m_collisions[l].AdjustLength(l.Length);
                }
                parent.RemoveLaser(l);
                m_collisions.Remove(l);
            }

            m_handledCollision.Clear();
        }
    }
}
