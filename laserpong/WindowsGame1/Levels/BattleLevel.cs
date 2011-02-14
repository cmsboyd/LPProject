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
    class BattleLevel: Level
    {

        
        public BattleLevel(ContentManager contentMangager, LevelManager Manager, String ID)
        {
            width = 1024;
            height = 768;
            reflect_limit = 5;
            refract_limit = 5;
            absorb_limit = 10;
            id = ID;
            m_content = contentMangager;
            font = contentMangager.Load<SpriteFont>("Text");
            score_bar = contentMangager.Load<Texture2D>("surface_corner");

            start = new SplashImage(m_content, "FightSplash", 1000f);
            manager = Manager;
            wallSetting = WallType.FullReflect;
           
        }


     

        protected override void typeUpdate(GameTime time)
        {
            foreach (Laser_Turret t in turrets) if (t.getHealth() <= 0) manager.addMenu(manager.VictoryMenu);
            
        }



    }
}
