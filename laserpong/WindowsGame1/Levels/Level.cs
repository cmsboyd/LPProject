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
    class Level
    {

        public String id;
        public int width;
        public int height;
        protected Texture2D background;
        public List<Laser> lasers = new List<Laser>();
        protected List<Laser_Turret> turrets = new List<Laser_Turret>();
        protected List<Surface> surfaces = new List<Surface>();
        protected List<SpawnPoint> s_points = new List<SpawnPoint>();
        protected List<Spawnable> spawns = new List<Spawnable>();
        protected Dictionary<String, Tower> towers = new Dictionary<String, Tower>();
        protected List<Laser> collisions = new List<Laser>();
        protected List<Prism> prisms = new List<Prism>();
        protected int score;
        protected Texture2D score_bar;
        protected Dictionary<String, Player> players = new Dictionary<String, Player>();

        public List<Spawnable> killed = new List<Spawnable>();

        public List<Laser_Turret> Turrets { get { return turrets; } }
        public List<Spawnable> Spawns { get { return spawns; } }
      
        protected Stack<Menu> activeMenus = new Stack<Menu>();
        public SpriteFont font;
        protected SplashImage start;
        protected SplashImage victory;
        protected bool endingSequence;
        


        protected int reflect_limit;
        protected int absorb_limit;
        protected int refract_limit;
        protected WallType wallSetting;

        public LevelManager manager;
        protected ContentManager m_content;


        public enum WallType
    {
            FullAbsorb,
            FullReflect,
            SideReflect,
            UpDownReflect
            
    }

        public Level()
        {
        }

        public Level(ContentManager contentMangager, LevelManager Manager)
        {
            width = 1024;
            height = 768;

            id = "Default ID";
            m_content = contentMangager;
            font = contentMangager.Load<SpriteFont>("Text");
            score_bar = contentMangager.Load<Texture2D>("surface_corner");

            manager = Manager;
            
            start = new SplashImage(m_content, "FightSplash", 1000f);
        }

        public Level(ContentManager contentMangager, LevelManager Manager, String ID)
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
            
        }




        /*
        public void addMenu(Menu menu)
        {
            activeMenus.Push(menu);
            currentMenu = menu;
        }

        

        public void cancelMenu()
        {
            activeMenus.Pop();
            if (activeMenus.Count != 0) currentMenu = activeMenus.Peek();
            else currentMenu = null;
        }
        */

        public void clearAll()
        {
            towers.Clear();
            lasers.Clear();
            spawns.Clear();
            s_points.Clear();
            turrets.Clear();
            surfaces.Clear();
            prisms.Clear();
            baseConstruction();
        }

        public void AddTurret(InputController.InputMode playerMode){
            Laser_Turret add = new Laser_Turret(this, playerMode, manager, absorb_limit, reflect_limit, refract_limit);
            turrets.Add(add);
            add.loadImage(m_content);

 
        }

        public void addPlayers(Dictionary<String, Player> Players)
        {
            players = Players;

        }

        public void AddTurrets()
        {
            foreach (Player player in players.Values)
            {
                Laser_Turret add = new Laser_Turret(this, player, manager, absorb_limit, reflect_limit, refract_limit);
                turrets.Add(add);
                add.loadImage(m_content);
            }

        }

        public void AddPrism()
        {   Prism temp = new Prism(this, new Vector2(600, 300));
        prisms.Add(temp);
        temp.loadImage(m_content);
        }



        public void AddPrism(Vector2 Position)
        {
            Prism temp = new Prism(this,Position);
            prisms.Add(temp);
            temp.loadImage(m_content);
        }

        public void AddPrism(Prism add)
        {
            prisms.Add(add);
                add.loadImage(m_content);
        }
        
        public void checkScore()
        {
            if (score <= 0 || score >= 1024) manager.addMenu(manager.mainmenu());

        }



        public void addScore(Color player_color)
        {
            if (turrets[0].color == player_color) score += 3;
            else score -= 3;
        }

        public void addScore(int amount)
        {
            score += amount;

        }

        


        public virtual void RemoveSurface(Surface remove)
        {
            remove.tower_A.adj_surfaces.Remove(remove);
            remove.tower_B.adj_surfaces.Remove(remove);
            surfaces.Remove(remove);
           

        }

        public void RemoveTower(Tower remove)
        {
            List<Surface> destroyed = new List<Surface>();
            foreach (Surface s in remove.adj_surfaces)
            {
                destroyed.Add(s);
                remove.owner.decrementSurface(s.M_type);
            }
            foreach (Surface s in destroyed) RemoveSurface(s);
            towers.Remove(remove.id);
        }

        public Laser_Turret getPlayer(int index)
        {
            return turrets[index];
        }

        public void AddTower(String key,Tower add)
        {
            towers.Add(key,add);
            add.loadImage(m_content);


        }

        public void AddLaser(Laser add)
        {
            lasers.Add(add);
            add.LaserEliminated += new Laser.LaserHandler(RemoveLaser);
            add.loadImage(m_content);
        }

        public void RemoveLaser(Laser remove)
        {
            remove.LaserEliminated -= new Laser.LaserHandler(RemoveLaser);
            lasers.Remove(remove);
        }

        public void AddSpawn(Spawnable add)
        {
            spawns.Add(add);
            add.loadImage(m_content);
        }

        public void RemoveSpawn(Spawnable remove)
        {
            spawns.Remove(remove);
        }

        public void AddSpawnPoint(SpawnPoint add)
        {
            s_points.Add(add);
            add.loadImage(m_content);
        }

        public virtual void AddSurface(Surface.SurfaceType type, Vector2 start, Vector2 end)
        {
            surfaces.Add(new Surface(type, start, end));
        }

        public virtual void AddSurface(Surface add)
        {
            surfaces.Add(add);
            add.loadImage(m_content);
        }

        public void loadImages(ContentManager theCM)
        {
            foreach (Laser_Turret x in turrets) x.loadImage(theCM);
      
        
        }


        public void Update(GameTime t)
        {
            if (start.Display) start.Update(t);
            else if (victory.Display) victory.Update(t);
            else
            {

                foreach (Laser_Turret x in turrets) x.Update(t);


                foreach (Laser x in lasers) x.Update(t);


                foreach (SpawnPoint x in s_points) x.Update(t);

                foreach (Spawnable x in spawns) x.Update(t);
                List<Tower> destroyed = new List<Tower>();
                foreach (Tower x in towers.Values)
                {
                    if (x.Update(t)) destroyed.Add(x);

                }
                foreach (Tower x in destroyed)
                {

                    RemoveTower(x);
                }

                foreach (Surface x in surfaces) x.Update(t);
                HandleCollisions(t);
                typeUpdate(t);
            }
        }

        protected virtual void typeUpdate(GameTime t)
        {


        }

    public void HandleCollisions(GameTime t)
    {
        /* Surfaces */
        foreach (Surface s in surfaces)
        {
            collisions.Clear();
            foreach (Laser l in lasers.ToArray())
            {
                if (s.built && l.IsColliding(s.LineSegment))
                {
                    collisions.Add(l);
                }
            }
            foreach (Laser l in collisions)
            {
                s.HandleCollision(l, this);
            }

            s.deleteLasers(this);
        }


        foreach (Spawnable s in spawns)
        {
            foreach (Laser l in lasers.ToArray())
            {
                if (s.isColliding(l))
                {
                    s.damage(l);

                }
            }

        }

        foreach (Spawnable s in killed)
        {
            spawns.Remove(s);

        }
        killed.Clear();

        List<Tower> destroyed = new List<Tower>();
        foreach (Tower s in towers.Values)
        {
            
            foreach (Laser l in lasers.ToArray())
            {
                s.isColliding(l);
            }

        }
        foreach (Tower s in destroyed)
        {
            foreach (Surface p in s.adj_surfaces)
            {
                RemoveSurface(p);
            }
            Laser_Turret temp = s.getOwner();
            temp.removeTower(s);
            RemoveTower(s);
        }
        destroyed.Clear();
        foreach (Laser_Turret lt in turrets)
        {
            foreach (Laser l in lasers.ToArray())
            {
                if (l.IsColliding(lt.bounds)) {
                    lt.HandleCollision(l);
                }
            }
        }

        foreach (Prism p in prisms)
        {
            p.resetColor();
            foreach (Laser l in lasers.ToArray())
            {
                if (l.IsColliding(p.BoundingBox)) {
                    p.HandleCollision(l);
                }
            }
        }
    }


    public virtual void typeDraw(SpriteBatch batch)
    {


    }

        public virtual void baseConstruction()
        {
            AddTurrets();

            AddTower("Up_Left", new Tower("Up_Left", this, new Vector2(20, 20)));
            AddTower("Up_Right", new Tower("Up_Right", this, new Vector2(width - 31, 20)));
            AddTower("Down_Left", new Tower("Down_Left", this, new Vector2(20, height - 32)));
            AddTower("Down_Right", new Tower("Down_Right", this, new Vector2(width - 31, height - 32)));


            switch (wallSetting)
            {

                case WallType.FullAbsorb:

                    AddSurface(new Surface(towers["Up_Left"], towers["Up_Right"], Surface.SurfaceType.Absorbant, this));
                    AddSurface(new Surface(towers["Up_Left"], towers["Down_Left"], Surface.SurfaceType.Absorbant, this));
                    AddSurface(new Surface(towers["Up_Right"], towers["Down_Right"], Surface.SurfaceType.Absorbant, this));
                    AddSurface(new Surface(towers["Down_Left"], towers["Down_Right"], Surface.SurfaceType.Absorbant, this));

                    break;

                case WallType.FullReflect:

                    AddSurface(new Surface(towers["Up_Left"], towers["Up_Right"], Surface.SurfaceType.Reflective, this));
                    AddSurface(new Surface(towers["Up_Left"], towers["Down_Left"], Surface.SurfaceType.Reflective, this));
                    AddSurface(new Surface(towers["Up_Right"], towers["Down_Right"], Surface.SurfaceType.Reflective, this));
                    AddSurface(new Surface(towers["Down_Left"], towers["Down_Right"], Surface.SurfaceType.Reflective, this));

                    break;

                case WallType.SideReflect:

                    AddSurface(new Surface(towers["Up_Left"], towers["Up_Right"], Surface.SurfaceType.Absorbant, this));
                    AddSurface(new Surface(towers["Up_Left"], towers["Down_Left"], Surface.SurfaceType.Reflective, this));
                    AddSurface(new Surface(towers["Up_Right"], towers["Down_Right"], Surface.SurfaceType.Reflective, this));
                    AddSurface(new Surface(towers["Down_Left"], towers["Down_Right"], Surface.SurfaceType.Absorbant, this));

                    break;

                case WallType.UpDownReflect:

                    AddSurface(new Surface(towers["Up_Left"], towers["Up_Right"], Surface.SurfaceType.Reflective, this));
                    AddSurface(new Surface(towers["Up_Left"], towers["Down_Left"], Surface.SurfaceType.Absorbant, this));
                    AddSurface(new Surface(towers["Up_Right"], towers["Down_Right"], Surface.SurfaceType.Absorbant, this));
                    AddSurface(new Surface(towers["Down_Left"], towers["Down_Right"], Surface.SurfaceType.Reflective, this));

                    break;

            }
        
        
        }



        public void Draw(SpriteBatch batch)
        {   
            foreach( Menu x in activeMenus) x.Draw(batch);

            foreach (Laser_Turret x in turrets)x.Draw(batch);
            
            foreach (Laser x in lasers)x.Draw(batch);
            
            foreach (Surface x in surfaces) x.Draw(batch);

            foreach (SpawnPoint x in s_points) x.Draw(batch);

            foreach (Spawnable x in spawns) x.Draw(batch);

            foreach (Prism p in prisms) p.Draw(batch);

            foreach (Tower x in towers.Values) x.Draw(batch);

            typeDraw(batch);

            if (start.Display) start.Draw(batch);
            if (victory.Display) victory.Draw(batch);
        }


    }
}
