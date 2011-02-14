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
    class FullMenu: Menu
    {
        private Texture2D image;
        private Texture2D cursor_image;
        Dictionary<String, Player> players;
        FlashText player1Start;
        FlashText player2Start;

        public FullMenu(LevelManager Parent, InputController Input,ContentManager CM,  String assetName)
        {
            parent = Parent;
            //input = Input;
            font = parent.font;
            current_index = 0;
            x_position = 600;
            y_position = 200;
            y_spacing = 20;
            x_spacing = 0;
            image = CM.Load<Texture2D>(assetName);
            cursor_image = CM.Load<Texture2D>("laser");
        }


        public FullMenu(LevelManager Parent, Dictionary<String, Player> Players, ContentManager CM, String assetName, int x, int y, int xprime, int yprime)
        {
            parent = Parent;
            font = parent.font;
            current_index = 0;
            x_position = x;
            y_position = y;
            y_spacing = yprime;
            x_spacing = xprime;
            image = CM.Load<Texture2D>(assetName);
            player1Start = new FlashText(font, "Player 1 Press Start", 500f, 100, 660, Color.Red);

            player2Start = new FlashText(font, "Player 2 Press Start", 500f, 750, 660, Color.Blue);


            cursor_image = CM.Load<Texture2D>("laser");
            players = Players;

        }

        public override void Update(GameTime gametime)
        {
            if (!players.ContainsKey("Player1")) player1Start.Update(gametime);
            if (!players.ContainsKey("Player2")) player2Start.Update(gametime);

            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)&& (!players.ContainsKey("Player1"))) parent.addPlayer(InputController.InputMode.Player1);

            if (GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.Start)&& (!players.ContainsKey("Player2"))) parent.addPlayer(InputController.InputMode.Player2);   
            
            if ( players.Count>0)  foreach (Player p in players.Values)
            {
                if (current == null) current = menuitems.ElementAt(0);


                p.input.Update();
                if (p.input.IsMoveUpNewlyPressed()) current_index--;
                if (p.input.IsMoveDownNewlyPressed()) current_index++;
                if (p.input.IsIncrementTowerNewlyPressed()) p.cycleColorRightt();
                if (p.input.IsDecrementTowerNewlyPressed()) p.cycleColorLeft();
                if (p.input.IsToggleBuildNewlyPressed()) parent.cancelMenu();
                if (current_index >= menuitems.Count) current_index = 0;
                if (current_index < 0) current_index = menuitems.Count - 1;
                current = menuitems.ElementAt(current_index);
                if (p.input.IsCreateSurfaceNewlyPressed())
                {
                    p.input.Update();
                    current.execute();
                }

              
            }

        if (current == null) current = menuitems.ElementAt(0);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(image, new Rectangle(0, 0, 1024, 768), Color.White);
            if (!players.ContainsKey("Player1")) player1Start.Draw(batch);
            if (!players.ContainsKey("Player2")) player2Start.Draw(batch);

           
            foreach( Player p in players.Values) batch.Draw(cursor_image, new Rectangle(x_position + x_spacing * current_index - 20, y_position + y_spacing*current_index, 20, 20), p.Color);
            base.Draw(batch);
            //batch.Draw(image, new Rectangle(0, 0, 1024, 768), Color.White);

        }


    }
}
