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
    class Laser
    {
        public delegate void LaserHandler(Laser l);
        public event LaserHandler LaserEliminated;

        private LaserParticle head;
        private LaserParticle tail;
        private Texture2D laser;
        private Color color;

        public float Length { get { return Vector2.Distance(head.Position, tail.Position); } }
        public float RotationAboveXAxis { get { return (float)Math.Atan2(tail.Velocity.X, tail.Velocity.Y); } }
        public Vector2 Direction { get { return tail.Velocity; } }
        public Vector2 End { get { return tail.Position; } }
        public Vector2 Start { get { return head.Position; } }

        public Color Color { get { return color; } }

        private void signalLaserEliminated()
        {
            if (LaserEliminated != null) {
                LaserEliminated(this);
            }
        }

        public Laser(Vector2 position, float orientation, Color color)
        {
            Vector2 velocity = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(orientation));

            construct(position, velocity, color);
        }

        public Laser(Vector2 position, Vector2 direction, Color color)
        {
            direction.Normalize();

            construct(position, direction, color);
        }

        public Laser(Laser_Turret turret)
        {
            Vector2 velocity = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(turret.orientation));

            construct(turret.laserStart, velocity, turret.color);
        }


        public Laser(Laser_Turret turret, bool hurray)
        {
            Vector2 velocity = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(turret.orientation));

            construct(turret.laserStart, velocity, Color.White);
        }

        private void construct(Vector2 position, Vector2 velocity, Color color)
        {
            head = new LaserParticle(position, velocity);
            tail = new LaserParticle(position, velocity);

            head.Next = tail;
            tail.Prev = head;
            this.color = color;
        }


        public void loadImage(ContentManager theCM)
        {
           laser = theCM.Load<Texture2D>("laser");
        }

        public void Update(GameTime t)
        {
            LaserParticle current = head;
            while (current != null) {
                current.Update(t);
                current = current.Next;
            }
        }

        public bool IsColliding(LineSegment segment)
        {
            float tIntersect = findIntersection(segment);
            return (tIntersect >= 0f);
        }

        public bool IsColliding(BoundingBox boundingBox)
        {
            /* If the bounding box '''contains''' either of our end-points, we're definitely colliding */
            if ( boundingBox.Contains(new Vector3(Start, 0f)) == ContainmentType.Contains ||
                 boundingBox.Contains(new Vector3(End, 0f)) == ContainmentType.Contains ) {
                return true;
            }

            float intersection = findIntersection(boundingBox);
            if (intersection > 0 && intersection < Length) {
                return true;
            }

            return false;
        }

        public float Chomp(LineSegment segment)
        {
            if (!IsColliding(segment)) {
                return 0f;
            }

            float amountToChomp = findIntersection(segment);

            Chomp(amountToChomp);
            return amountToChomp;
        }

        public Vector2? FindIntersectionPoint(LineSegment segment) {
            float tIntersect = findIntersection(segment);

            if (tIntersect < 0) {
                return null;
            }

            Vector2 u = End - Start;
            u.Normalize();
            Vector2 pIntersect = Start + tIntersect * u;

            return pIntersect;
        }

        public float Chomp(BoundingBox boundingBox)
        {
            if (!IsColliding(boundingBox)) {
                return 0f;
            }

            float amountToChomp = findIntersection(boundingBox);

            /* Value is now actually the amountToChomp! */
            amountToChomp = Length - amountToChomp;

            Chomp(amountToChomp);
            return amountToChomp;
        }

        private float findIntersection(LineSegment segment)
        {
            Vector2 u = End - Start;
            Vector2 v = segment.End - segment.Start;
            Vector2 w = Start - segment.Start;

            Vector2 vPerp = new Vector2(-v.Y, v.X);
            vPerp.Normalize();

            if ((Vector2.Dot(Start - segment.Start, vPerp)) > 0) {
                // Need to flip!!
                //v = segment.Start - segment.End;
            }

            float tIntersect = (v.Y * w.X - v.X * w.Y) / (v.X * u.Y - v.Y * u.X);
            if (tIntersect >= 0f && tIntersect <= 1f) {
                float sIntersect = (u.X * w.Y - u.Y * w.X) / (u.X * v.Y - u.Y * v.X);
                if (sIntersect > 0f && sIntersect < 1f) {
                    return tIntersect * Length;
                }
            }
            return -1f;
        }

        private float findIntersection(BoundingBox boundingBox)
        {
            Vector2 normalizedLaserDirection = Direction;
            normalizedLaserDirection.Normalize();
            Ray laser_ray = new Ray(new Vector3(End, 0), new Vector3(normalizedLaserDirection, 0));

            float? intersection = boundingBox.Intersects(laser_ray);
            if (intersection == null) {
                intersection = -1f;
            }

            return (float)intersection;
        }

        public void AppendParticle(LaserParticle addition)
        {
            LaserParticle penultimate = tail;

            // penultimate.Prev already is correct
            penultimate.Next = addition;
            addition.Prev = penultimate;
            addition.Next = null;

            tail = addition;
        }

        /* SOON TO BE DEPRECATED LASER API */

        public void IncreaseLength(TimeSpan timeDelta)
        {
            Vector2 tailVelocity = tail.Velocity;
            tail.Position -= tailVelocity * (float)timeDelta.TotalSeconds;
        }

        
        public void AdjustLength(float amount)
        {
            Vector2 tailVelocity = tail.Velocity;
            tailVelocity.Normalize();

            tail.Position -= tailVelocity * amount;
        }

        private void Chomp(float amount)
        {
            System.Diagnostics.Debug.WriteLine("CHOMPING " + amount);
            if (amount >= Length) {
                /* Kill the laser! */
                head.Position = tail.Position;
                head.Velocity = Vector2.Zero;
                tail.Velocity = Vector2.Zero;
                signalLaserEliminated();
            }
            Vector2 headVelocity = head.Velocity;
            headVelocity.Normalize();

            head.Position -= headVelocity * amount;
        }

        public void Draw(SpriteBatch batch)
        {
            DrawLaserForParticlePair(head, tail, batch);

            LaserParticle current = head;
            while (current != null) {
                current.Draw(batch, laser, Color.White);
                current = current.Next;
            }
        }

        private void DrawLaserForParticlePair(LaserParticle a, LaserParticle b, SpriteBatch batch)
        {
            float radians = (float)Math.Atan2(b.Velocity.Y, b.Velocity.X);
            float length = Vector2.Distance(b.Position, a.Position);

            Rectangle dest = new Rectangle((int)b.Position.X, (int)b.Position.Y - 1, (int)length, 2);
            /* texture is a 'CIRCLE', so pull a chunk out of it! */
            Rectangle source = new Rectangle(5, 5, 1, 1);

            batch.Draw(laser, dest, source, color, radians, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}
