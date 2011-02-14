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
    class Laser_Turret
    {
        public Vector2 position;
        public float orientation;
        public string id;
        InputController input;
        Texture2D image_map;
        public Color color;
        SpriteEffects Player_effect = SpriteEffects.None;
        private int health;
        Texture2D health_bar;
        private int health_bar_pos;
        private int energy_bar_pos;
        private int energy;


        private Level m_parent;

        private Laser m_generating;
        private Vector2 m_movement;
        private float m_rotation;
        private Cursor cursor;

        public int num_towers=0;
        public List<Tower> m_towers = new List<Tower>();

       public BoundingBox bounds;
       public LevelManager manager;



       private int reflect_limit;
       private int absorb_limit;
       private int refract_limit;
       private int absorb_amount;
       private int reflect_amount;
       private int refract_amount;


        public bool isFiring()
        {
            return (m_generating != null);
        }


        
        public Laser_Turret(InputController.InputMode playerMode)
        {
            input = new InputController(playerMode);
            position = new Vector2(0, 0);
            orientation = 0f;
            id = "Player1";
            color = Color.Red;
            cursor = new Cursor(this, input, m_parent);


        }
        public Laser_Turret(Level parent, InputController.InputMode playerMode, LevelManager Manager, int a, int b, int c)
        {
            energy = 748;
            health = 748;
            m_parent = parent;
            input = new InputController(playerMode);
            id = input.getMode();
            manager = Manager;

            
           reflect_limit=b;
           absorb_limit=a;
           refract_limit=c;
           absorb_amount=0;
           reflect_amount=0;
           refract_amount=0;

            switch (id)
            {
                case "Player1":
                    health_bar_pos = 0;
                    energy_bar_pos = 8;
                    position = new Vector2(30, m_parent.height / 2);
                    orientation = 0f;
                    id = "Player 1";
                    color = Color.Red;
                    cursor = new Cursor(this, input, m_parent);
                    break;

                case "Player2":

                    health_bar_pos = m_parent.width - 5;
                    energy_bar_pos = m_parent.width - 13;
                    position = new Vector2(m_parent.width - 33, m_parent.height / 2);
                    orientation = (float)Math.PI;
                    id = "Player 2";
                    color = Color.Blue;

                    cursor = new Cursor(this, input, m_parent);
                    break;

                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
            bounds = new BoundingBox(new Vector3(position.X - 20, position.Y - 20, -1), new Vector3(position.X + 20, position.Y + 20, 1));

        }



        public Laser_Turret(Level parent, Player player, LevelManager Manager, int a, int b, int c)
        {
            energy = 748;
            health = 748;
            m_parent = parent;
            input = player.input;
            id = player.ID;
            color = player.Color;
            manager = Manager;


            reflect_limit = b;
            absorb_limit = a;
            refract_limit = c;
            absorb_amount = 0;
            reflect_amount = 0;
            refract_amount = 0;

            switch (input.getMode())
            {
                case "Player1":
                    health_bar_pos = 0;
                    energy_bar_pos = 8;
                    position = new Vector2(30, m_parent.height / 2);
                    orientation = 0f;
                    id = "Player 1";
                    cursor = new Cursor(this, input, m_parent);
                    break;

                case "Player2":

                    health_bar_pos = m_parent.width - 5;
                    energy_bar_pos = m_parent.width - 13;
                    position = new Vector2(m_parent.width - 33, m_parent.height / 2);
                    orientation = (float)Math.PI;
                    id = "Player 2";

                    cursor = new Cursor(this, input, m_parent);
                    break;

                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
            bounds = new BoundingBox(new Vector3(position.X - 20, position.Y - 20, -1), new Vector3(position.X + 20, position.Y + 20, 1));

        }

        public Laser_Turret(Level parent, InputController.InputMode playerMode, LevelManager Manager)
        {
            energy = 748;
            health = 748;
            m_parent = parent;
            input = new InputController(playerMode);
            id = input.getMode();
            manager = Manager;

            switch (id)
            {
                case "Player1":
                    health_bar_pos = 0;
                    energy_bar_pos = 8;
                    position = new Vector2(30, m_parent.height/2);
                    orientation = 0f;
                    id = "Player 1";
                    color = Color.Red;
                    cursor = new Cursor(this, input, m_parent);
                    break;

                case "Player2":

                    health_bar_pos = m_parent.width - 5;
                    energy_bar_pos = m_parent.width - 13;
                    position = new Vector2(m_parent.width - 33, m_parent.height/2);
                    orientation = (float) Math.PI;
                    id = "Player 2";
                    color = Color.White;

                    cursor = new Cursor(this, input, m_parent);
                    break;

                default:
                    throw new NotImplementedException("Received unexpected input mode");
            }
            bounds = new BoundingBox(new Vector3(position.X - 20, position.Y - 20, -1), new Vector3(position.X + 20, position.Y + 20, 1));

        }

        public void setHealth(int h)
        {
            health = h;
        }

        public int getHealth()
        {
            return health;
        }


        public void Update(GameTime gameTime)
        {
            Vector2 update = Vector2.Zero;

            
            KeyboardUpdate();
            CursorUpdate();
            MovementUpdate();
            LaserUpdate(gameTime);
        }

        public Tower getNextTower()
        {
            return m_towers[1];
        }

        public InputController getInput()
        {
            return input;
        }



        public Vector2 getPosition()
        {
            return position;
        }

        public void CursorUpdate()
        {
            cursor.Update();

        }

        public void KeyboardUpdate()
        {

            input.Update();
            if (input.IsPauseMenuNewlyPressed()) manager.pauseGame();
            else
            {
                if (m_generating != null)
                {
                    energy -=15;
                    if (input.IsMoveUpPressed() && !input.IsMoveDownPressed()) { m_movement.X = 0; m_movement.Y = -10f; }
                    else if (!input.IsMoveUpPressed() && input.IsMoveDownPressed()) { m_movement.X = 0; m_movement.Y = 10f; }
                    else { m_movement.X = 0; m_movement.Y = 0f; }

                    if (input.IsAimUpPressed() && !input.IsAimDownPressed()) m_rotation = .1f;
                    else if (!input.IsAimUpPressed() && input.IsAimDownPressed()) m_rotation = -.1f;
                    else m_rotation = 0;
                }
                else
                {   
                    energy +=5;
                    if (energy > 748) energy = 748;
                    m_movement.X = 0; m_movement.Y = 0; m_rotation = 0;
                }
                if (input.IsFireNewlyPressed()) { m_generating = new Laser(this); m_parent.AddLaser(m_generating); }
                else if (input.IsAltFireNewlyPressed()) { m_generating = new Laser(this, true); m_parent.AddLaser(m_generating); }
                if ((input.IsAltFireReleased() && input.IsFireReleased()) || energy <=0) { m_generating = null; }
            }
        }

        private void MovementUpdate()
        {
            Vector2 temp = cursor.position - position;

            switch (id)
            {
                case "Player 1":
                    orientation = (float)Math.Atan(temp.Y / temp.X);
                    break;
                case "Player 2":
                    orientation = (float)(Math.Atan(temp.Y / temp.X) + Math.PI);
                    break;
                default:
                    throw new NotImplementedException("Received unexpected value");

            }

        }

        public bool legalSurface(Surface.SurfaceType type)
        {
            switch (type)
            {
                case Surface.SurfaceType.Absorbant:
                    return (absorb_amount < absorb_limit);
                    
                case Surface.SurfaceType.Reflective:
                    return (reflect_amount<reflect_limit) ;
                case Surface.SurfaceType.Refractive:
                    return (refract_amount < refract_limit) ;
                default:
                    return false;
            }

        }

        public void incrementSurface(Surface.SurfaceType type)
        {

            switch (type)
            {
                case Surface.SurfaceType.Absorbant:
                    absorb_amount++;
                    break;

                case Surface.SurfaceType.Reflective:
                   reflect_amount++;
                   break;
                case Surface.SurfaceType.Refractive:
                    refract_amount++;
                    break;
                default:
                    break;
            }


        }

        public void decrementSurface(Surface.SurfaceType type)
        {

            switch (type)
            {
                case Surface.SurfaceType.Absorbant:
                    absorb_amount--;
                    if (absorb_amount < 0) absorb_amount = 0;
                    break;

                case Surface.SurfaceType.Reflective:
                    reflect_amount--;
                    if (reflect_amount < 0) reflect_amount = 0;
                    break;

                case Surface.SurfaceType.Refractive:
                    refract_amount++;
                    if (refract_amount < 0) refract_amount = 0;
                    break;
                default:
                    break;
            }
        }




        private void LaserUpdate(GameTime gameTime)
        {
            if (m_generating != null)
            {
                m_generating.IncreaseLength(gameTime.ElapsedGameTime);
            }
        }

        public void loadImage(ContentManager theCM)
        {
            image_map = theCM.Load<Texture2D>("turret");
            health_bar = theCM.Load<Texture2D>("surface_corner");
            cursor.loadImage(theCM);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(image_map, position, new Rectangle(0, 0, 80, 80), color, orientation, new Vector2 (40,40), 1f, Player_effect, 1f);
            batch.Draw(health_bar, new Rectangle(energy_bar_pos, 10, 10, energy), color);
            batch.Draw(health_bar, new Rectangle(health_bar_pos, 10, 10, health), color);
            
            cursor.Draw(batch);
        }

        public void removeTower(Tower remove)
        {
            m_towers.Remove(remove);

        }


        public void isColliding(Laser laser)
        {
            Ray temp_las_up = new Ray(new Vector3(laser.End.X, laser.End.Y, 0), new Vector3(laser.Direction.X, laser.Direction.Y, 0));
            float? intersection = bounds.Intersects(temp_las_up);
            if(intersection > 0 && intersection < laser.Length){
                if (laser.Color == Color.White) health+=2;
                    else health-=5;
                laser.Chomp(laser.Length - (float)intersection);
            }
        }

        public void loseHealth()
        {
            health--;
        }

        }

        



    }

