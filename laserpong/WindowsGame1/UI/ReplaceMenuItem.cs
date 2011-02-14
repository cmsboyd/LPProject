using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class ReplaceMenuItem:MenuItem
    {
        Menu target;
        LevelManager manager;

        public ReplaceMenuItem(LevelManager Manager, Menu Target)
        {
            manager = Manager;
            target = Target;
            id = "Default menu ID";
        }


        public ReplaceMenuItem(LevelManager Manager, Menu Target, String ID)
        {
            manager = Manager;
            target = Target;
            id = ID;
        }

        public override void execute()
        {   
            manager.clearMenus();
            manager.addMenu(target);

        }




    }
}

   