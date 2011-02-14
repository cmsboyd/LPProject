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
    class Menu
    {
        
        protected List<MenuItem> menuitems = new List<MenuItem>();
        protected MenuItem current;
        protected Player player;
        protected LevelManager parent;
        protected SpriteFont font;
        protected int current_index;
        protected int x_position;
        protected int y_position;
        protected int x_spacing;
        protected int y_spacing;

        public Menu() { }

        public Menu(LevelManager Parent, Player Player)
        {
            parent = Parent;
            player = Player;
            font = parent.font;
            current_index = 0;
            x_position = 400;
            y_position = 400;
            y_spacing = 20;
            x_spacing = 0;

        }

        public Menu(LevelManager Parent)
        {
            parent = Parent;
            font = parent.font;
            current_index = 0;
            x_position = 400;
            y_position = 400;
            y_spacing = 20;
            x_spacing = 0;

        }


        public void addMenuItem(MenuItem menuitem)
        {
            menuitems.Add(menuitem);

        }
    
        public virtual void Update(GameTime gametime)
        {

            player.input.Update();
            if (player.input.IsMoveUpNewlyPressed()) player.current_menu_index--;
            if (player.input.IsMoveDownNewlyPressed()) player.current_menu_index++;
            if (player.input.IsToggleBuildNewlyPressed()) parent.cancelMenu();
            if (player.current_menu_index >= menuitems.Count) player.current_menu_index = 0;
            if (player.current_menu_index < 0) player.current_menu_index= menuitems.Count - 1;
            if (player.input.IsCreateSurfaceNewlyPressed()) menuitems[player.current_menu_index].execute();
                
            
            
        }

        public virtual void Draw(SpriteBatch batch)
        {
            
            foreach (MenuItem x in menuitems)
            {
                
                    int i = menuitems.IndexOf(x);
                    batch.DrawString(font, x.getId(), new Vector2(x_position + i* x_spacing, y_position + y_spacing * i), Color.Black);
                
            }

        }
    
    
    
    }

    
}
