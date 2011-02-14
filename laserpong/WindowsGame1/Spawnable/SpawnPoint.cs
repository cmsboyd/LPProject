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
    class SpawnPoint
    {
        private float spawn_timer;
        private float current_time;

        private Vector2 position;
        private float orientation;


        private Level parent;
        private Texture2D image;
        private Color color = Color.White;




        public SpawnPoint(Vector2 start, float time, Level Parent)
        {
            parent = Parent;
            current_time = 0;
            position = start;
            spawn_timer = time;
            orientation = 0f;
        }


        public void Spawn()
        {
            parent.AddSpawn(new Spawnable(parent, this));

        }

        public Vector2 getPosition()
        {
            return position;
          }


        public void loadImage(ContentManager theCM)
        {
            image = theCM.Load<Texture2D>("laser");
        }

        public void Update(GameTime t){
            current_time += t.ElapsedGameTime.Milliseconds;
            if (current_time > spawn_timer)
            {
                current_time = 0;
                Spawn();
            }
        }

              public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, new Rectangle(0, 0, 80, 80), color, orientation, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        



    }
}
