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
    class Laser : LineElement
    {
        private static float SPEED = 750.0f;
        private Texture2D m_laser;
        private Color m_color;

        public Color Color { get { return m_color; } }

        public Laser(Vector2 position, float orientation, Color color)
        {
            LFront = position;
            LOrientation = orientation;
            LLength = 0;
            m_color = color;
        }

        public Laser(Laser_Turret turret)
        {
            LFront = turret.position ;
            LOrientation = turret.orientation;
            LLength = 0;
            m_color = turret.color;
        }


        public Laser(Laser_Turret turret, bool hurray)
        {
            LFront = turret.position;
            LOrientation = turret.orientation;
            LLength = 0;
            m_color = Color.White;
        }


        public void loadImage(ContentManager theCM)
        {
           m_laser = theCM.Load<Texture2D>("laser");
        }

        public void Update(GameTime t)
        {
            LFront += SPEED * (float)t.ElapsedGameTime.TotalSeconds * Direction;
        }

        public void IncreaseLength(TimeSpan timeDelta)
        {
            AdjustLength(SPEED * (float)timeDelta.TotalSeconds);
        }


        public void AdjustLength(float amount)
        {
            LLength += amount;
        }

        public void Chomp(float amount)
        {
            LFront -= amount * Direction;
            LLength -= amount;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(m_laser, new Rectangle((int)End.X + 7, (int)End.Y + 7, (int)Length, 12), new Rectangle(9,0,1,20), m_color, (float)Orientation, Vector2.Zero, SpriteEffects.None, 1f);
        }

    }
}
