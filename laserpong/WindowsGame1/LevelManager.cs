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
    class LevelManager
    {
        List<Level> levels = new List<Level>();
        Menu currentMenu;
        Stack<Menu> activeMenus = new Stack<Menu>();
        public Level currentLevel;
        ContentManager CM;
        InputController input;
        Menu LevelMenu;
        public Menu PuzzleMenu;
        public SpriteFont font;
        Level blankLevel;
        Menu pauseMenu;
        public Menu VictoryMenu;
      //  private SplashImage logo;
        Dictionary<String, Player> players = new Dictionary<String, Player>();



        public LevelManager(ContentManager theCM)
        {   
            input = new InputController();
            CM = theCM;
            font = theCM.Load<SpriteFont>("Text");
            LevelMenu = new FullMenu(this, players, CM, "Main menu", 400, 400, 0, 20);
            PuzzleMenu = new FullMenu(this, players, CM, "Main menu", 400, 400, 0 , 20);
            blankLevel = new BattleLevel(CM, this, "Blank Battle Level");
            addLevel(blankLevel);
          //  setLevel(blankLevel);
            LevelMenu.addMenuItem(new AddMenuItem(this, PuzzleMenu, "Puzzle Levels"));
           // pauseMenu = new Menu(this, input);
           // pauseMenu.addMenuItem(new MenuItem("Pause"));
           // VictoryMenu = new Menu(this, input);
           // VictoryMenu.addMenuItem(new ReplaceMenuItem(this, LevelMenu, "Victory!"));
        //    logo = new SplashImage(theCM, "LogoSplash", 2000f);
        }

        public void addLevel(Level add)
        {
            levels.Add(add);
            LevelMenu.addMenuItem(new Level_Button(this,add));

        }

        public void addPuzzleLevel(Level add)
        {
            levels.Add(add);
            PuzzleMenu.addMenuItem(new Level_Button(this, add));

        }

        public void setLevel(Level target)
        {
            target.addPlayers(players);
            target.clearAll();
            currentLevel = target;
            currentLevel.loadImages(CM);
        }

        public Menu mainmenu()
        {
            return LevelMenu;

        }

        public void closeLevel()
        {
            currentLevel = null;
        }

        public void pauseGame()
        {
            addMenu(pauseMenu);
        }


        public void Update(GameTime t)
        {
            if (currentMenu != null && currentMenu == pauseMenu) currentMenu.Update(t);
            else if (currentLevel == null) currentMenu.Update(t);
                else currentLevel.Update(t);
        }

        public void addPlayer (InputController.InputMode playerNUM)
        {
            players.Add("" + playerNUM, new Player(playerNUM));
        }

        public void removePlayer(InputController.InputMode player)
        {   
            players.Remove("" + player);
        }

        public void Draw(SpriteBatch batch)
        {
            //if (logo.Display) logo.Draw(batch);
            //else
            {
                if(currentLevel == null) currentMenu.Draw(batch);
                else currentLevel.Draw(batch);

            }
        }

        public bool hasPlayer(String playerNUM)
        {
            return players.ContainsKey(playerNUM);

        }


        public void addMenu(Menu menu)
        {
            activeMenus.Push(menu);
            currentMenu = menu;
        }


        public void cancelMenu()
        {
            activeMenus.Pop();
            if (activeMenus.Count != 0) currentMenu = activeMenus.Peek();
            else currentMenu = null;
        }

        public void clearMenus()
        {
            activeMenus.Clear();
            currentMenu = null;

        }
        



    }
}
