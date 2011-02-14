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
    class Surface : LineElement
    {
        public enum SurfaceType
        {
            Absorbant,
            Reflective,
            Refractive
        };

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
        

        public Surface(SurfaceType type, Vector2 start, Vector2 end)
        {
            m_type = type;
            LStart = start;
            LEnd = end;
            m_segments = (int)Length / 8;
            //m_orientation = LineElement.computeOrientation(m_direction);

            m_collisions = new Dictionary<Laser, Laser>();


            if (m_type == SurfaceType.Reflective) m_color = Color.Silver;
            else if (m_type == SurfaceType.Refractive) m_color = Color.Pink;
            else m_color = Color.Black;
        }


        public Surface(Tower a, Tower b, SurfaceType type, Level Parent)
        {
            tower_A = a;
            tower_A.AddSurface(this);
            tower_B = b;
            tower_B.AddSurface(this);
            m_type = type;
            LStart = a.getPosition();
            LEnd = b.getPosition();
            orientation = (float)Math.Atan(LStart.X - LEnd.X / LStart.Y / LEnd.Y);
            m_segments = (int)Length / 8;
            parent = Parent;

            if (m_type == SurfaceType.Reflective) m_color = Color.Silver;
            else if (m_type == SurfaceType.Refractive) m_color = Color.White;
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

            LStart = tower_A.getPosition();
            LEnd = tower_B.getPosition();
            orientation = (float)Math.Atan((LEnd.Y - LStart.Y) / (LEnd.X - LStart.X));
            m_segments = (int)Length / 8;
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
            Color temp = m_color;
            if (!built) temp = new Color(m_color, .5f);
             
            for (int i = 0; i < m_segments; i++)
            {
               // batch.Draw(m_surface_side, LStart + 8 * i * Direction, temp);
                batch.Draw(m_surface_side, LStart + 8 * i * Direction + new Vector2(5,5), new Rectangle(0, 0, 10, 10), temp, orientation, new Vector2(5, 5),1f, SpriteEffects.None, .5f);
             
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
            Vector2 v = this.End - this.Start;
            Vector2 w = l.Start - this.Start;

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
            Laser generating;
            bool flipped = false;

            Vector2 u = l.End - l.Start;
            Vector2 v = this.End - this.Start;
            Vector2 w = l.Start - this.Start;

            // Do we need to flip?
            Vector2 vPerp = new Vector2(-v.Y, v.X);
            vPerp.Normalize();

            if ((Vector2.Dot(l.Start - this.Start, vPerp)) > 0) {
                // Need to flip!!
                v = this.Start - this.End;
                flipped = true;
            }

            vPerp = new Vector2(v.Y, -v.X);

            float tIntersect = (v.Y * w.X - v.X * w.Y) / (v.X * u.Y - v.Y * u.X);

            Vector2 pIntersect = l.Start + tIntersect * u;
            
            // Have we already started a laser for this collision?
            if (!m_collisions.ContainsKey(l))
            {
                // Compute reflection
                u.Normalize();
                v.Normalize();
                float theta = (float)Math.Acos(Vector2.Dot(u, v));
                if (theta > (float)Math.PI * .5f)
                {
                   theta -= (float)Math.PI;
                }
               
                float ninety = (float)Math.PI / 2f;
                if (flipped) ninety = -ninety;

                if (m_type == SurfaceType.Refractive)
                {
                    float perpO = LineElement.computeOrientation(vPerp);
                    float phi = perpO - l.Orientation;

                    generating = new Laser(pIntersect, l.Orientation + phi / 2f, l.Color);
                }
                else
                {
                    generating = new Laser(pIntersect, l.Orientation - 2f*theta, l.Color);
                }

                if (m_type == SurfaceType.Reflective || m_type == SurfaceType.Refractive)
                {
                    parent.AddLaser(generating);
                }
                m_collisions.Add(l, generating);
            } else {
                generating = m_collisions[l];
            }
            
            // Compute length to chomp / add
            l.Chomp(u.Length() * tIntersect);

            if (m_type == SurfaceType.Reflective || m_type == SurfaceType.Refractive)
            {
                generating.AdjustLength(u.Length() * tIntersect);
            }

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
