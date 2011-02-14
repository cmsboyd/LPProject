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
    class Spawnable
    {
        protected Texture2D image;
        protected Vector2 position;

        public Vector2 Position {get{return position;}}

        protected float speed = .5f;
        protected Vector2 direction;
        protected float s_orientation = 0f;
        protected float update_timer;
        protected float countdown_timer;

        
        protected Level region;
        protected Color s_color = Color.White;
        protected SpawnPoint parent;
        protected Random randomizer;
        protected int health;
        public int Health { get { return health; } }

        protected Laser_Turret target;

        public Spawnable() { }

        public Spawnable(Level Region, SpawnPoint Parent)
        {
            parent = Parent;
            region = Region;
            randomizer = new Random();
            double temp = randomizer.Next(0, 2);
            target = region.getPlayer((int)Math.Floor(temp));
            position = parent.getPosition();
            direction = target.getPosition()- position;
            direction.Normalize();
            health = 10;
            update_timer = 5000f;
           
        }



        public Spawnable(Level Region, Vector2 Position)
        {
            //parent = Parent;
            region = Region;
            randomizer = new Random();
            double temp = randomizer.Next(0, 1);
            target = region.getPlayer((int)Math.Floor(temp));
            position = Position;
            direction = target.getPosition() - position;
            direction.Normalize();
            health = 10;
            update_timer = 5000f;

        }



        public void Update(GameTime t)
        {
            countdown_timer += t.ElapsedGameTime.Milliseconds;

            if (countdown_timer > update_timer)
            {
                direction = target.getPosition() - position;
                direction.Normalize();
                Double temp = randomizer.Next(0, 360);
                Vector2 random = new Vector2((float)Math.Cos(temp), (float)Math.Sin(temp));
                direction += random;
                direction.Normalize();
                countdown_timer = 0;
            }
            position += speed * direction;
            if (getBounds().Intersects(target.bounds))
            {
                target.loseHealth();
                position -= speed * direction;

            }

            if (health < 0) onDeath();
        }

        public void loadImage(ContentManager theCM)
        {
            image = theCM.Load<Texture2D>("laser");
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, new Rectangle(0, 0, 80, 80), s_color, s_orientation, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public bool isColliding(Laser laser)
        {
            BoundingBox bounds = getBounds();
            Ray temp_las_up = new Ray(new Vector3(laser.End.X, laser.End.Y, 0), new Vector3(laser.Direction.X, laser.Direction.Y, 0));
            float? intersection = bounds.Intersects(temp_las_up);
            return (intersection > 0 && intersection < laser.Length);
        } 

        private BoundingBox getBounds()
        {
            return new BoundingBox(new Vector3(position.X - 5, position.Y - 5, 0), new Vector3(position.X + 5, position.Y + 5, 0)); 
        }

        public void changeColor(Color newCol)
        {

            s_color = newCol;
        }

        public virtual void damage(Laser l)
        {

            health--;
            changeColor(l.Color);
            


        }

        public virtual void damage(int dam)
        {
            health -= dam;
            

        }

        public virtual void onDeath()
        {
            region.addScore(1);
            region.killed.Add(this);

        }


    }
}
