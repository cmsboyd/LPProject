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
    class SplashImage
    {
        private float time;
        private float currentTime;
        private Texture2D image;
        private bool display;
        public bool Display { get { return display; } }

        public SplashImage(ContentManager CM, String assetName, float Time){

            image = CM.Load<Texture2D>(assetName);
            currentTime = 0;
            time = Time;
            display = true;

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, new Rectangle (0,0, 1024, 768), new Rectangle(0,0,1024,768), Color.White,0f, new Vector2(0, 0), SpriteEffects.None, 1f);
        }

        public virtual void Update(GameTime t)
        {
            currentTime += t.ElapsedGameTime.Milliseconds;
            display = (currentTime < time);
        }

    }
}
