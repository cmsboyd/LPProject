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
    class Player
    {
        protected Color color;
        protected int colorIndex;
        public Color Color { get { return color; } }
        public InputController input;
        private String id;
        public string ID { get { return id; } }
        public int current_menu_index;

        public Player(InputController.InputMode PlayerNumber)
        {
            input = new InputController(PlayerNumber);
            switch (PlayerNumber)
            {
                case InputController.InputMode.Player1:
                    colorIndex = 0;
                    setColor();
                    id = "Player1";
                    break;
                case InputController.InputMode.Player2:
                    colorIndex = 5;
                    setColor();
                    id = "Player2";
                    break;
            }
            current_menu_index = 0;

        }


        public void cycleColorLeft()
        {
            if (colorIndex == 0) colorIndex = 8; 
            else colorIndex --;
            setColor();
        }

        public void cycleColorRightt()
        {
            if (colorIndex == 8) colorIndex = 0;
            else colorIndex++;
            setColor();
        }
        public void setColor()
        {
            switch (colorIndex)
            {

                case 0:
                    color = Color.Red;
                    break;

                case 1:
                    color = Color.Orange;
                    break;

                case 2:
                    color = Color.Yellow;
                    break;

                case 3:
                    color = Color.GreenYellow;
                    break;

                case 5:
                    color = Color.Cyan;
                    break;

                case 6:
                    color = Color.Blue;
                    break;

                case 7:
                    color = Color.Indigo;
                    break;
                case 8:
                    color = Color.HotPink;
                    break;
                default:
                    break;

            }


        }



    }
}
