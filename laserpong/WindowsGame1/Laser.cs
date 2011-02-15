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
        private LaserParticle head;
        private LaserParticle tail;
        private Texture2D laser;
        private Color color;

        public float Length { get { return Vector2.Distance(head.Position, tail.Position); } }
        public float Orientation { get { return (float)Math.Atan2(tail.Velocity.X, tail.Velocity.Y); } }
        public Vector2 Direction { get { return tail.Velocity; } }
        public Vector2 End { get { return tail.Position; } }
        public Vector2 Start { get { return head.Position; } }

        public Color Color { get { return color; } }

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

            construct(turret.position, velocity, turret.color);
        }


        public Laser(Laser_Turret turret, bool hurray)
        {
            Vector2 velocity = Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(turret.orientation));

            construct(turret.position, velocity, Color.White);
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
            head.Update(t);
            tail.Update(t);
        }

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

        public void Chomp(float amount)
        {
            Vector2 headVelocity = head.Velocity;
            headVelocity.Normalize();

            head.Position -= headVelocity * amount;
        }

        public void Draw(SpriteBatch batch)
        {
            float radians = (float)Math.Atan2(tail.Velocity.Y, tail.Velocity.X);
            float length = Vector2.Distance(tail.Position, head.Position);

            Rectangle dest = new Rectangle((int)tail.Position.X, (int)tail.Position.Y - 1, (int)length, 2);
            /* texture is a 'CIRCLE', so pull a chunk out of it! */
            Rectangle source = new Rectangle(5, 5, 1, 1);

            batch.Draw(laser, dest, source, color, radians, Vector2.Zero, SpriteEffects.None, 1f);

            head.Draw(batch, laser, Color.White);
            tail.Draw(batch, laser, Color.White);
        }
    }
}
