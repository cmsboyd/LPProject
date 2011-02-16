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

        public void Draw(SpriteBatch batch, Texture2D color, Color bobbleColor)
        {
            Rectangle destination = new Rectangle((int)position.X - 2, (int)position.Y - 2, 4, 4);

            batch.Draw(color, destination, bobbleColor);
        }
    }
}
