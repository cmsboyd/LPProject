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
    class SplashText
    {
    
        protected float time;
        protected float currentTime;
        protected SpriteFont font;
        protected String text;
        protected Color color;
        protected int x;
        protected int y;
        protected float rotation;
        protected float scale;
        protected SpriteEffects effects;

        protected bool display;
        public bool Display { get { return display; } }

        public SplashText() {
            effects = SpriteEffects.None;
        }

        public SplashText(SpriteFont Font, String assetName, float Time){

            font = Font;
            currentTime = 0;
            time = Time;
            display = true;
            x = 100;
            y = 660;
            rotation = 0f;
            color = Color.Red;
            scale = 1f;
            text = assetName;
            effects = SpriteEffects.None;

        }

        public void Draw(SpriteBatch batch)
        {
            if(display) batch.DrawString(font, text, new Vector2(x, y), color, rotation, new Vector2(0, 0), scale, effects, 1f);
        }
        public virtual void Update(GameTime t)
        {
            currentTime += t.ElapsedGameTime.Milliseconds;
            display = (currentTime < time);
        }

    }
}


