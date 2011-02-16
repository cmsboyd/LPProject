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
    class FullLevelMenu:FullMenu
    {
        //protected Level_Button level;
        protected ToggleWallsButton wallType;
        protected ToggleLimitsButton reflectiveAllowance;
        protected ToggleLimitsButton refractiveAllowance;
        protected ToggleLimitsButton absorbantAllowance;


        public FullLevelMenu(LevelManager Parent, Level Target, InputController Input, ContentManager CM, Dictionary<String, Player> Players, int x, int y, int xprime, int yprime)
        {

            parent = Parent;
            font = parent.font;
            current_index = 0;
            x_position = x;
            y_position = y;
            y_spacing = yprime;
            x_spacing = xprime;
            image = CM.Load<Texture2D>("Main Menu");
            cursor_image = CM.Load<Texture2D>("laser");
            addMenuItem(new Level_Button(parent, Target));
            wallType = new ToggleWallsButton(Target);
            wallType.addOption("Full Reflective", Level.WallType.FullReflect);
            wallType.addOption("Full Absorb", Level.WallType.FullAbsorb);
            wallType.addOption("H", Level.WallType.SideReflect);
            wallType.addOption("I", Level.WallType.UpDownReflect);
            wallType.addOption("X", Level.WallType.DiagonalReflectingOctagon);
            wallType.execute();
            addMenuItem(wallType);
            reflectiveAllowance = new ToggleLimitsButton(Target, Surface.SurfaceType.Reflective);
            addMenuItem(reflectiveAllowance);

            absorbantAllowance = new ToggleLimitsButton(Target, Surface.SurfaceType.Absorbant);
            addMenuItem(absorbantAllowance);

            refractiveAllowance = new ToggleLimitsButton(Target, Surface.SurfaceType.Refractive);
            addMenuItem(refractiveAllowance);

            player1Start = new FlashText(font, "Player 1 Press Start", 500f, 100, 660, Color.Red);

            player2Start = new FlashText(font, "Player 2 Press Start", 500f, 750, 660, Color.Blue);


            players = Players;

        }


    }
}
