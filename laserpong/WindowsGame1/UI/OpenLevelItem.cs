using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class OpenLevelItem: MenuItem
    {
        private Level target;
        private LevelManager parent;

        public Level Target { get { return target; } set { target = value; } }
        public LevelManager Parent { get { return parent; } set { parent = value; } }

        public OpenLevelItem()
        {
            target = null;
            parent = null;
        }


        public override void execute()
        {
            parent.currentLevel = target;

        }


    }
}
