using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class LaserParticle
    {
        private const float SPEED = 700.0f;
        private const int SPLIT_THREASHOLD = 10;
        private const int SPLIT_THREASHOLD_SQUARED = SPLIT_THREASHOLD * SPLIT_THREASHOLD;

        public Vector2 Position { get { return position; } set { position = value;} }
        public Vector2 Velocity { 
            get { return velocity; } 
            set { 
                value.Normalize();
                velocity = value * SPEED;
            }
        }

        public LaserParticle Next { get { return next; } set { next = value; } }
        public LaserParticle Prev { get { return prev; } set { prev = value; } }

        private Vector2 position;
        private Vector2 velocity;
        private float power;

        private LaserParticle next;
        private LaserParticle prev;

        public LaserParticle(Vector2 position, Vector2 velocity)
        {
            this.position = position;
            velocity.Normalize();

            this.velocity = velocity * SPEED;

            next = null;
            prev = null;
        }

        public void Update(GameTime t)
        {
            position += velocity * (float)t.ElapsedGameTime.TotalSeconds;
        }

        public void Split()
        {
            if (next != null) {
                LaserParticle after = next;
                SplitIfNecessary(this, after);
            }
        }

        public void Draw(SpriteBatch batch, Texture2D color, Color bobbleColor)
        {
            Rectangle destination = new Rectangle((int)position.X - 2, (int)position.Y - 2, 4, 4);

            batch.Draw(color, destination, bobbleColor);
        }

        private static void SplitIfNecessary(LaserParticle before, LaserParticle after)
        {
            if (ShouldParticlesSplit(before, after)) {

                LaserParticle insertion = AverageParticles(before, after);
                InsertParticle(before, after, insertion);

                /* Recurse! */
                SplitIfNecessary(before, insertion);
                SplitIfNecessary(insertion, after);
            }
        }

        private static bool ShouldParticlesSplit(LaserParticle a, LaserParticle b)
        {
            return (Vector2.DistanceSquared(a.Position, b.Position) > SPLIT_THREASHOLD_SQUARED);
        }

        private static LaserParticle AverageParticles(LaserParticle a, LaserParticle b)
        {
            return new LaserParticle((a.Position + b.Position) / 2, (a.Velocity + b.Velocity) / 2);
        }

        private static void InsertParticle(LaserParticle before, LaserParticle after, LaserParticle insertion)
        {
            // before.Prev already set
            before.Next = insertion;

            insertion.Prev = before;
            insertion.Next = after;

            after.Prev = insertion;
            // after.Next already set
        }
    }
}
