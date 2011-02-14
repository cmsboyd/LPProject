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
    class PrismBattleLevel : Level
    {

        
        public PrismBattleLevel(ContentManager contentMangager, LevelManager Manager, String ID)
        {
            width = 1024;
            height = 768;
            reflect_limit = 5;
            refract_limit = 5;
            absorb_limit = 0;
            id = ID;
            m_content = contentMangager;
            font = contentMangager.Load<SpriteFont>("Text");
            score_bar = contentMangager.Load<Texture2D>("surface_corner");
            score = 500;
            manager = Manager;
            start = new SplashImage(m_content, "FightSplash", 1000f);
            wallSetting = WallType.UpDownReflect;

        }


        public override void baseConstruction()
        {
            base.baseConstruction();

          AddPrism(new PlayerPrism(this, new Vector2(100, 100), players["Player1"]));
          AddPrism(new PlayerPrism(this, new Vector2(100, 668), players["Player1"]));

          //  AddPrism(new PlayerPrism(this, new Vector2(924, 100), turrets[1]));
          //  AddPrism(new PlayerPrism(this, new Vector2(924, 668), turrets[1]));

        
        }


     
        protected override void typeUpdate(GameTime t)
        {

            if (score <= 0 || score >= 1000) manager.addMenu(manager.mainmenu());

        }


        public override void typeDraw(SpriteBatch batch)
        {
            batch.Draw(score_bar, new Rectangle(12, 5, 1000, 5), Color.Blue);
            batch.Draw(score_bar, new Rectangle(12, 5, score, 5), Color.Red);
        }

    }
}
