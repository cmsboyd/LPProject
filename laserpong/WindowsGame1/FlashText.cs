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
    class FlashText: SplashText
    {
        public FlashText(SpriteFont Font, String assetName, float Time){

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

        }

        public FlashText(SpriteFont Font, String assetName, float Time, int X, int Y, Color Color)
        {

            font = Font;
            currentTime = 0;
            time = Time;
            display = true;
            x = X;
            y = Y;
            rotation = 0f;
            color = Color;
            scale = 1f;
            text = assetName;

        }

        public override void Update(GameTime t)
        {
           
        
            currentTime += t.ElapsedGameTime.Milliseconds;
            if (currentTime > time) { display = !display; currentTime = 0; }

        }

    }
}
