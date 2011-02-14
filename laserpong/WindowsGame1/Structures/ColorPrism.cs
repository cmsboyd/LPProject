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
    class ColorPrism:Prism
    {
        Color keyColor;

        public ColorPrism(Level Parent, Vector2 Position, Color KeyColor)
        {
            parent = Parent;
            keyColor = KeyColor;
            position = Position;
            bounds = new BoundingBox(new Vector3(position.X - 20, position.Y - 20, -1), new Vector3(position.X + 20, position.Y + 20, 1));
           // color = Color.White;
            

        }





        protected override void typeEffect(Laser laser)
        {
            color = new Color(color.ToVector3() + laser.Color.ToVector3());


            if (color == keyColor) lit_up = true;
            

        }

        public override void Draw(SpriteBatch batch)
        {

        batch.Draw(image, new Rectangle((int)position.X - 20, (int)position.Y - 20, 40, 40), color);
        
        }

        


    }
}
