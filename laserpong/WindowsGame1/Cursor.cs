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
    class Cursor
    {

        public Vector2 position;
        InputController input;
        public Color color;
        Laser_Turret parent;
        private Vector2 c_movement;
        private Texture2D image;
        private string player;
        Level region;
        Tower selected;
        Tower selected_2;
        Surface temp_surface;
        public int tower_index=0;

        private Vector2 c_temp;
        private bool c_build;
        Surface.SurfaceType c_build_type = Surface.SurfaceType.Absorbant;
        private bool tower_selected;




        public Cursor(Laser_Turret Parent, InputController Input, Level Region)
        {
            input = Input;
            parent = Parent;
            player = Parent.id;
            region = Region;
            position = parent.getPosition();
            switch (player){
            
                case "Player 1":

            position.X += 10f;

                    break;

                case "Player 2":
                    position.X -= 10f;
                break;

                default:
                throw new NotImplementedException("Received unexpected output");
            }
            color = parent.color;
            

        }

        public void FixPosition()
        {
            position.X = Math.Min(position.X, region.width-41);
            position.X = Math.Max(position.X, 20);
            position.Y = Math.Min(position.Y, region.height-42);
            position.Y = Math.Max(position.Y, 20);


        }

        public void searchTowers()
        {
            

        }

        public void Update()
        {
            if (!parent.isFiring())
            {
                position += 15* input.LStickPosition();
            }

            FixPosition();

            if (input.IsCreateTowerNewlyPressed()) region.AddSpawnPoint(new SpawnPoint(position, 1000f, region));

            if (input.IsCancelSurfaceNewlyPressed())
            {
                if (selected_2 != null && !selected_2.built) region.RemoveTower(selected_2);
                if(selected != null && !selected.built) region.RemoveTower(selected);
                selected = null;
                c_build = false;
            }
        
            if (input.IsIncrementTowerNewlyPressed())
            {
                incrementTowerIndex();
                if (c_build)
                {
                    if (!selected_2.built)
                    {
                        Tower temp = selected_2;
                        selected_2 = parent.m_towers[tower_index];
                        temp_surface.changeTower(selected_2);
                        temp.ClearSurfaces();
                        region.RemoveTower(temp);
                    }
                    else selected_2 = parent.m_towers[tower_index];


                    position = selected_2.position;
                    Vector2 distance = selected.position - position;
                    while (distance.LengthSquared() > 10000)
                    {
                        incrementTowerIndex();
                        selected_2 = parent.m_towers[tower_index];
                        position = selected_2.position;
                        distance = selected.position - position;

                    }
                    temp_surface.changeTower(selected_2);
                }
                else selectTower(parent.m_towers[tower_index]);        
            }



            if (input.IsDecrementTowerNewlyPressed())
            {
                decrementTowerIndex();

                if (c_build)
                {
                    if (!selected_2.built){

                        Tower temp = selected_2;
                        selected_2 = parent.m_towers[tower_index];
                        temp_surface.changeTower(selected_2);
                        temp.ClearSurfaces();
                        region.RemoveTower(temp);

                    }else selected_2 = parent.m_towers[tower_index];

                    position = selected_2.position;
                    Vector2 distance = selected.position - position;
                    while (distance.LengthSquared() > 14400)
                    {
                        incrementTowerIndex();
                        selected_2 = parent.m_towers[tower_index];
                        position = selected_2.position;
                        distance = selected.position - position;

                    }
                    temp_surface.changeTower(selected_2);
                }

                else selectTower(parent.m_towers[tower_index]);                
            }

            
           if (c_build)
           {   Vector2 distance = selected.position - position;

           if (distance.LengthSquared() > 14400){
               distance.Normalize();
               position = selected.position - distance * 120f;
           }
               if (!selected_2.built)
               {
                   selected_2.position = position;
               }
               temp_surface.ChangeType(c_build_type);

            }


            if (input.IsCreateSurfaceNewlyPressed() && parent.legalSurface(c_build_type))
            {

                if (c_build)
                {
                    selected.Anchor();
                    selected_2.Anchor();
                    c_temp = Vector2.Zero;
                    temp_surface.built = true;
                        c_build = false;
                        selected.UnHighlight();
                        selected = null;

                        selected_2 = null;
                        temp_surface = null;
                        parent.incrementSurface(c_build_type);
                    
                }else    {
                    if (selected == null) buildTower();
                    else buildTower(selected);
                       
            }


        }
            
            if (input.IsToggleBuildNewlyPressed())
            {
                region.AddPrism(new AntiPrism(region, position));
            }
            


            if (input.IsSetReflectiveNewlyPressed() && parent.legalSurface(Surface.SurfaceType.Reflective)) c_build_type = Surface.SurfaceType.Reflective;
            if (input.IsSetAbsorbantNewlyPressed() && parent.legalSurface(Surface.SurfaceType.Absorbant)) c_build_type = Surface.SurfaceType.Absorbant;
            if (input.IsSetRefractiveNewlyPressed() && parent.legalSurface(Surface.SurfaceType.Refractive)) c_build_type = Surface.SurfaceType.Refractive;



            if (input.createPrismNewlyPressed())
            {
                region.AddPrism(position);

            }
        }

        public void loadImage(ContentManager theCM)
        {
            image = theCM.Load<Texture2D>("laser");
        }

        public void incrementTowerIndex()
        {
            tower_index++;
            if (tower_index == parent.m_towers.Count) tower_index = 0;
        }

        public void decrementTowerIndex()
        {
            tower_index--;
            if (tower_index < 0) tower_index = parent.m_towers.Count-1;
        }


        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image, position, new Rectangle(0, 0, 80, 80), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public void selectTower(Tower select)
        {
            if(selected != null) selected.UnHighlight();
            selected = select;
            selected.Highlight();

        }

        public void buildTower()
        {

            selectTower(new Tower(parent.id + " " + parent.num_towers, region, position, parent));
            selected.built = false;

            region.AddTower(parent.id + " " + parent.num_towers, selected);
            parent.num_towers++;
            selected_2 = new Tower(parent.id + " " + parent.num_towers, region, position, parent);
            selected_2.built = false;

            region.AddTower(parent.id + " " + parent.num_towers, selected_2);
            parent.m_towers.Add(selected);
            parent.m_towers.Add(selected_2);

            parent.num_towers++;

            c_build = true;
            temp_surface = new Surface(selected, selected_2, c_build_type, region);
            temp_surface.built = false;
            region.AddSurface(temp_surface);
                       

        }

        public void buildTower(Tower built)
        {
            selected_2 = new Tower(parent.id + " " + parent.num_towers, region, position, parent);
            selected_2.built = false;

            region.AddTower(parent.id + " " + parent.num_towers, selected_2);
 
            parent.m_towers.Add(selected_2);

            parent.num_towers++;

            c_build = true;
            temp_surface = new Surface(selected, selected_2, c_build_type, region);
            temp_surface.built = false;
            region.AddSurface(temp_surface);


        }

    }
}
