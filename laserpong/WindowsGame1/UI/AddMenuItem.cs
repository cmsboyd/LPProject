using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class AddMenuItem: MenuItem
    {
        Menu target;
        LevelManager manager;

        public AddMenuItem(LevelManager Manager, Menu Target)
        {
            manager = Manager;
            target = Target;
            id = "Default menu ID";
        }

        public AddMenuItem(LevelManager Manager, Menu Target, String ID)
        {
            manager = Manager;
            target = Target;
            id = ID;
        }

        public override void execute()
        {
            manager.addMenu(target);

        }




    }
}
