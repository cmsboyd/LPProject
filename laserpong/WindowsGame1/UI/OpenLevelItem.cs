using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class OpenLevelItem: MenuItem
    {
        Level target;
        LevelManager parent;


        public override void execute()
        {
            parent.currentLevel = target;

        }


    }
}
