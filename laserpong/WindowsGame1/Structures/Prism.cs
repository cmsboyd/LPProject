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
    class Prism
    {
        
        protected Texture2D image;
        public Vector2 position;
        protected Color color;
        protected Level parent;
        //public string id;
        public bool lit_up = false;
        protected BoundingBox bounds;
        //protected Color temp_color;

        public BoundingBox BoundingBox { get { return bounds; } }


        public Prism()
        { 
        }

        public Prism(Level Parent, Vector2 Position)
        {
            parent = Parent;

            position = Position;
            bounds = new BoundingBox(new Vector3(position.X - 20, position.Y - 20, -1), new Vector3(position.X + 20, position.Y + 20, 1));
           // color = Color.White;
            

        }


        public Vector2 getPosition()
        {
            return position;
        }


        public void loadImage(ContentManager theCm)
        {

            image = theCm.Load<Texture2D>("surface_corner");


        }


        public virtual void Draw(SpriteBatch batch)
        {

            if(lit_up)batch.Draw(image, new Rectangle((int)position.X - 20, (int)position.Y - 20, 40, 40), color);
            else batch.Draw(image, new Rectangle((int)position.X - 20, (int)position.Y - 20, 40, 40), Color.White);
          
        }

        public virtual void resetColor()
        {
            color = new Color(0, 0, 0);
            lit_up = false;
        }

        protected virtual void typeEffect(Laser laser)
        {

            lit_up = true;
            color = new Color(color.ToVector3() + laser.Color.ToVector3());

            if (laser.Color != Color.White) parent.addScore(laser.Color);

        }

        public void HandleCollision(Laser laser)
        {
            typeEffect(laser);
        }


    }
}
