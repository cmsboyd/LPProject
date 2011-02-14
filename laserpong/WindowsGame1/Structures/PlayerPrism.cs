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
    class PlayerPrism : Prism
    {
        private Color player_color;

        public PlayerPrism(Level Parent, Vector2 Position, Player player)
        {
            parent = Parent;

            position = Position;
            bounds = new BoundingBox(new Vector3(position.X - 20, position.Y - 20, -1), new Vector3(position.X + 20, position.Y + 20, 1));
            player_color = player.Color;


        }


        public Vector2 getPosition()
        {
            return position;
        }


        public void loadImage(ContentManager theCm)
        {

            image = theCm.Load<Texture2D>("surface_corner");


        }


        public override void Draw(SpriteBatch batch)
        {

            if (lit_up) batch.Draw(image, new Rectangle((int)position.X - 20, (int)position.Y - 20, 40, 40), color);
            else batch.Draw(image, new Rectangle((int)position.X - 20, (int)position.Y - 20, 40, 40), player_color);

        }

        public override void resetColor()
        {
            color = new Color(0, 0, 0);
            lit_up = false;
        }

        protected override void typeEffect(Laser laser)
        {


            if (laser.Color != Color.White && laser.Color != player_color)
            {
                parent.addScore(laser.Color);
                lit_up = true;
                color = new Color(color.ToVector3() + laser.Color.ToVector3());

            }

        }


      
        }


    }