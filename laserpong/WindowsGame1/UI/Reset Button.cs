using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class Level_Button: MenuItem
    {

        LevelManager parent;
        Level target;


        public Level_Button(LevelManager Parent )
        {   
            parent = Parent;
        }


        public Level_Button(LevelManager Parent, String Id)
        {
            parent = Parent;
            id = Id;
        }


        public Level_Button(LevelManager Parent, Level Target)
        {
            parent = Parent;
            target = Target;
            id = target.id;
        }


        public override void execute()
        {
            parent.setLevel(target);
          //  parent.clearMenus();

        }


    }
}
