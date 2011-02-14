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
    class BombSpawnable: Spawnable
    {
        

        public BombSpawnable(Level Region, Vector2 Position)
        {
            //parent = Parent;
            region = Region;
            randomizer = new Random();
            double temp = randomizer.Next(0, 1);
            target = region.getPlayer((int)Math.Floor(temp));
            position = Position;
            direction = target.getPosition() - position;
            direction.Normalize();
            health = 1;
            update_timer = 5000f;
            s_color = Color.Black;

        }


        public override void onDeath()
        {
            foreach (Spawnable s in region.Spawns)
            { Vector2 distance = s.Position - position;
            if (distance.LengthSquared() < 14400)
            {
                s.damage(25);
                
            }


            }
            region.addScore(5);
            region.killed.Add(this);
        
        }

    }
}
