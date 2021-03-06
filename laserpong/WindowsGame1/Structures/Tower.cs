﻿using System;
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
    class Tower
    {

        private int health;
        private int max_health;
        private Texture2D image;
        public List<Surface> adj_surfaces = new List<Surface>();
        public Vector2 position;
        private Color color;
        public Laser_Turret owner;
        public string id;
        public bool built= true;
        public bool lit_up;
        private BoundingBox bounds;

        public BoundingBox Bounds { get { return bounds; } }

        Level parent;

        public Tower(Level Parent, Vector2 Position)
        {
            parent = Parent;
            position = Position;
            lit_up = false;

        }

        
        public Tower(string Id, Level Parent, Vector2 Position)
        {
            id = Id;
            parent = Parent;
            position = Position;
            color = Color.White;
            max_health = 1000000000;
            health = max_health;
            lit_up = false;
        }

        public Tower(string Id, Level Parent, Vector2 Position, Laser_Turret Owner)
        {
            max_health = 25;
            health = max_health;
            id = Id;
            parent = Parent;
            position = Position;
            owner = Owner;
            bounds = new BoundingBox(new Vector3(position.X-30, position.Y-30, -1), new Vector3(position.X + 30, position.Y + 30, 1));

            color = new Color(owner.color.ToVector3() + new Vector3(-.5f, -.5f, -.5f));
            lit_up = false;
            
        }


        public Vector2 getPosition()
        {
            return position;
        }

        public void AddSurface(Surface add)
        {

            adj_surfaces.Add(add);

        }

        public void ClearSurfaces()
        {
            adj_surfaces.Clear();

        }

        public bool Update(GameTime t)
        {
            return(health < 0 || adj_surfaces.Count==0);
        }

        public void Anchor()
        {
            bounds = new BoundingBox(new Vector3(position.X - 5, position.Y - 5, 0), new Vector3(position.X + 5, position.Y + 5, 0));
            built = true;
        }

        public Laser_Turret getOwner()
        {
            return owner;
        }

        public void loadImage(ContentManager theCm)
        {

            image = theCm.Load<Texture2D>("surface_corner");


        }

        public void Draw(SpriteBatch batch)
        {
            if (!built) batch.Draw(image, position, new Color(color, .5f));
            else batch.Draw(image, position, color);
          
        }


        public void RemoveSurface(Surface remove)
        {
            adj_surfaces.Remove(remove);
        }

        public void Highlight()
        {
            color = owner.color;

        }

        public void UnHighlight()
        {
            color = new Color(owner.color.ToVector3() + new Vector3(-.5f,-.5f, -.5f)) ;

        }


        public void HandleCollision(Laser laser)
        {
            if (laser.Color == Color.White) {
                health++;
            } else {
                health--;
            }

            if (health > max_health) {
                health = max_health;
            }

            laser.Chomp(bounds);
        }

    }
}