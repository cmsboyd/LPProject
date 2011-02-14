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
    class HordeLevel: Level
    {

        private float timer;
        private float current_time;
        private int num_spawn;

               public HordeLevel(ContentManager contentMangager, LevelManager Manager, String ID)
        {
            width = 1024;
            height = 768;
            reflect_limit = 5;
            refract_limit = 5;
            absorb_limit = 50;
            id = ID;
            m_content = contentMangager;
            font = contentMangager.Load<SpriteFont>("Text");
            score_bar = contentMangager.Load<Texture2D>("surface_corner");
            score = 0;
            manager = Manager;
            start = new SplashImage(m_content, "FightSplash", 1000f);
            wallSetting = WallType.UpDownReflect;
            
        }

        

        public override void baseConstruction()
        {
            
            timer = 2000f;
            current_time = 0f;
            num_spawn = 1;

            base.baseConstruction();
            //AddPrism(new Vector2(600, 300));


        }
        

        public override void typeDraw(SpriteBatch batch)
        {
            batch.Draw(score_bar, new Rectangle(12, 5, 1000, 5), Color.Blue);
            batch.Draw(score_bar, new Rectangle(12, 5, score, 5), Color.Red);
            batch.DrawString(font, "" + score, new Vector2(500, 5), Color.Black);
        }

        protected override void typeUpdate(GameTime t)
        {
            current_time += t.ElapsedGameTime.Milliseconds;
            if(current_time>timer){
                Random rand = new Random();
                for (int i = 0; i < num_spawn; i++)
                {
                    int x = (rand.Next() % 800) + 200;
                    int y = (rand.Next() % 600) + 20;
                    if (rand.Next() % 100 < 5) AddSpawn(new BombSpawnable(this, new Vector2(x, y))); 
                    else AddSpawn(new Spawnable(this, new Vector2(x, y)));
                }
                current_time = 0;
                num_spawn++;
                }


            foreach (Laser_Turret T in turrets) if (T.getHealth() <= 0) manager.addMenu(manager.VictoryMenu);
            
        }

     

    }
}
