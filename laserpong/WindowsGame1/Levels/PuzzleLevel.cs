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
    class PuzzleLevel: Level
    {
        private bool success;


        public PuzzleLevel(ContentManager contentMangager, LevelManager Manager, String ID)
        {

            
            width = 1024;
            height = 768;
            id = ID;

            m_content = contentMangager;
            font = contentMangager.Load<SpriteFont>("Text");
            score_bar = contentMangager.Load<Texture2D>("surface_corner");
            manager = Manager;

            start = new SplashImage(m_content, "FightSplash", 1000f);
            victory = new SplashImage(m_content, "VictorySplash", 500f);

            baseConstruction();
        }

        public override void baseConstruction()
        {
            base.baseConstruction();


            start.Start();
            
            reflect_limit = 2;
            refract_limit = 2;
            absorb_limit = 1000;

            AddPrism(new Vector2 (600, 300));


        }

        protected override void typeUpdate(GameTime t)
        {
            if (success && (victory.Display == false)) {manager.closeLevel(); }
            success = true;
            foreach (Prism p in prisms)
            {
                if (!p.lit_up) success = false;

            }

            if (success && (victory.Display == false)) { victory.Start(); }




        }



       




    }
}
