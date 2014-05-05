using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PositronNova.Class.Unit
{
    public enum UnitType
    {
        Chasseur, Destroyer, Corvette, Croiseur, Cuirasse
    };

    public class Unit : sprite
    {
        // Sound et tir
        private int range;
        public int Range
        { get { return range;  } }
        System.TimeSpan fireRate;
        System.TimeSpan last;
        SoundEffect laserSound;
        Bullet localBullet;
        BulletType weaponType;

        // Caractéristiques unit
        private Color color;
        private SpriteFont _font;
        private string name;
        public string Name
        {
            get { return name; }
        }
        UnitType unitType;

        public bool moving = false;
        bool selected;
        private bool hasTarget;
        public bool HasTarget
        {
            set { hasTarget = value; }
        }
        private Texture2D selection;
        private Texture2D cible;

        protected int damage;
        protected int pv_max;
        public int Pv_max
        {
            get { return pv_max; }
        }
        protected int pv;
        public int Pv
        {
            get { return pv; }
            set { pv = value; }
        }

        // Cible
        private Unit enn;
        public Unit Ennemy
        {
            get { return enn; }
            set { enn = value; }
        }

        //oui moi aussi :o)
        private bool friendly;
        public bool Friendly
        {
            get { return friendly; }
        }

        //////////////////////////////////// CONSTRUCTEURS /////////////////////////////////////

        public Unit(String name, Vector2 position, UnitType unitType, bool friendly)
            : base(position)
        {
            this.name = name;
            this.unitType = unitType;
            this.friendly = friendly;
        }

        public override void Init()
        {
            if (friendly)
                color = Color.Aqua;
            else
                color = Color.IndianRed;
            speed = 0;
            direction = Vector2.Zero;
            destination = position;

            base.Init();
        }
        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Affichage_mouse");
            laserSound = content.Load<SoundEffect>("sounds\\laser");
            selection = content.Load<Texture2D>("img\\SelectionCarre");
            cible = content.Load<Texture2D>("img\\TargetCarre");

            switch (unitType)
            {
                case UnitType.Chasseur:
                    texture = content.Load<Texture2D>("img\\Chasseur");
                    last = new TimeSpan(0);
                    fireRate = new TimeSpan(0, 0, 0, 0, 100);
                    weaponType = BulletType.LittleCinetique;
                    pv_max = 10;
                    pv = pv_max;
                    speed = 2.2f;
                    range = 200;
                    break;
                case UnitType.Destroyer:
                    texture = content.Load<Texture2D>("img\\Destroyer");
                    last = new TimeSpan(0);
                    fireRate = new TimeSpan(0, 0, 0, 0, 200);
                    weaponType = BulletType.Cinetique;
                    pv_max = 40;
                    pv = pv_max;
                    speed = 1.9f;
                    range = 300;
                    break;
                case UnitType.Corvette:
                    texture = content.Load<Texture2D>("img\\Corvette");
                    last = new TimeSpan(0);
                    fireRate = new TimeSpan(0, 0, 0, 0, 400);
                    weaponType = BulletType.Laser;
                    pv_max = 60;
                    pv = pv_max;
                    speed = 1.6f;
                    range = 400;
                    break;
                case UnitType.Croiseur:
                    texture = content.Load<Texture2D>("img\\Croiseur");
                    last = new TimeSpan(0);
                    fireRate = new TimeSpan(0, 0, 0, 0, 500);
                    weaponType = BulletType.Plasma;
                    pv_max = 110;
                    pv = pv_max;
                    speed = 1.3f;
                    range = 500;
                    break;
                case UnitType.Cuirasse:
                    texture = content.Load<Texture2D>("img\\Cuirasse");
                    last = new TimeSpan(0);
                    fireRate = new TimeSpan(0, 0, 0, 2);
                    weaponType = BulletType.Missile;
                    pv_max = 250;
                    pv = pv_max;
                    speed = 1f;
                    range = 600;
                    break;
            }

            centre = position + new Vector2(texture.Width / 2, texture.Height / 2);

            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
        }
        public Vector2 getPosition()
        {
            return position;
        }
        public virtual void HandleInput(KeyboardState keyboardState, MouseState mouseState/*, Vector2 Pos*/)
        {
            //Ce code est magique, ne pas trop toucher SVP :-) effectivement trop bien ce code :o) GG !
            if (mouseState.LeftButton == ButtonState.Pressed && friendly)
            {
                selected = Math.Abs(mouseState.X - (position.X + texture.Width / 2) + Camera2d.Origine.X) <= texture.Width / 2 & Math.Abs(mouseState.Y - (position.Y + texture.Height / 2) + Camera2d.Origine.Y) <= texture.Height / 2; // Le Camera2d.Origine c'est la décalage hein ;) distance entre l'orgine du background et l'origine de la cam
            }
            if (mouseState.RightButton == ButtonState.Pressed && selected)
            {
                enn = PositronNova.GetEnnemy(friendly);
                if (enn != null)
                    hasTarget = Math.Pow(position.X - enn.getPosition().X, 2) + Math.Pow(position.Y - enn.getPosition().Y, 2) <= Math.Pow(range, 2);
                else
                {
                    hasTarget = false;
                }
                if (!hasTarget)
                {
                    destination = new Vector2(mouseState.X - texture.Width / 2 + Camera2d.Origine.X, mouseState.Y - texture.Height / 2 + Camera2d.Origine.Y); //position de la mouse par rapport à l'origine de l'écran + décalage par rapport à l'origine de l'écran par rapport à l'origine du background
                    direction = new Vector2(destination.X - centre.X, destination.Y - centre.Y);
                    direction.Normalize();
                    moving = true;
                }
            }
        }

        public void Update(GameTime gt)
        {
            last = last.Add(gt.ElapsedGameTime);
            if (hasTarget && last >= fireRate && enn != null && Math.Pow(position.X - enn.getPosition().X,2) + Math.Pow(position.Y - enn.getPosition().Y, 2) <= Math.Pow(range, 2))
            {
                shoot();
                last = new TimeSpan(0);
            }

            deplacement(gt);

            // Bords du background
            if (position.X <= 5 || position.X + texture.Width >= PositronNova.BackgroundTexture.Width - 5 ||
                position.Y <= 5 || position.Y + texture.Height >= PositronNova.BackgroundTexture.Height - 5)
                moving = false;

            base.Update(gt);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, name, new Vector2(position.X - 3, position.Y - 15), color);
            //Mettre "|| !friendly" pour tester l'effet de la methode attack
            if (direction.X > 0)
                spriteBatch.Draw(texture, centre, null, Color.White, (float)Math.Atan(direction.Y / direction.X), Vector2.Zero + new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.FlipHorizontally, 0);
            else if (direction.X < 0)
                spriteBatch.Draw(texture, centre, null, Color.White, (float)Math.Atan(direction.Y / direction.X), Vector2.Zero + new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(texture, position, Color.White);
            isSelected(spriteBatch);
            if (selected)
                spriteBatch.DrawString(_font, pv + "/" + pv_max, new Vector2(position.X - 3, position.Y - 25), color);
            //aUneCible(spriteBatch);

            base.Draw(spriteBatch);
        }

        /////////////////////////////////// METHODES //////////////////////////

        void isSelected(SpriteBatch sb)
        {
            if (selected)
            {
                sb.Draw(selection, new Vector2(centre.X - selection.Width / 2, centre.Y - selection.Height / 2), Color.White);
                aUneCible(sb);
            }
        }

        void aUneCible(SpriteBatch sb)
        {
            if (hasTarget)
                if (enn != null && enn.pv > 0)
                    sb.Draw(cible, enn.centre - new Vector2(cible.Width / 2, cible.Height / 2), Color.White);
        }

        void deplacement(GameTime gameTime)
        {
            int stopPrecision = 2;

            if (moving)
            {
                destination = new Vector2((int)destination.X, (int)destination.Y);
                direction = new Vector2(destination.X - position.X, destination.Y - position.Y);
                direction.Normalize();
                position += direction * speed; // Silence ça pousse... ahem... bouge ! :o)
                centre += direction * speed;

                if (Math.Abs(position.X - destination.X) <= stopPrecision && Math.Abs(position.Y - destination.Y) <= stopPrecision) // empêche le ship de tourner (vibrer?) autour de la destination avec stopPrecision
                    position = destination;
            }
            else
            {
                destination = position;
            }
            moving = position != destination;
        }

        void shoot()
        {
            if (enn != null && enn.pv > 0)
            {
                localBullet = new Bullet(centre, enn, weaponType);
                PositronNova.AddBullet(localBullet);
            }
            else
                hasTarget = false;
        }

        public bool Destruction()
        {
            return pv <= 0;
        }

        public bool CollisionInterVaisseau(Unit unit)
        {
            return (Physique.IntersectPixel(texture, position, textureData, unit.texture, unit.position, unit.textureData));
        }
    }

    //Les dignes heritieres de la class Unit :-)
    //On initialise une variable qui nous servira de timer pour les attques avec "last"
    //class Fighter : Unit
    //{
    //    public Fighter(string name, Vector2 Pos, bool friendly) 
    //        : base(name, Pos)
    //    {
    //        last = new TimeSpan(0);
    //        fireRate = new TimeSpan(0,0,0,1);
    //        pv_max = 10;
    //        pv = 10;
    //        damage = 1;
    //        //sprite = new sprite(Pos,Content,"img\\Chasseur_1B");
    //        speed = 0.15f;
    //    }
    //}
    //class Destroyer : Unit
    //{
    //    public Destroyer(string name, Vector2 Pos, bool friendly)
    //        : base(name, Pos)
    //    {
    //        last = new TimeSpan(0);
    //        fireRate = new TimeSpan(0, 0, 0, 1);
    //        pv_max = 20;
    //        pv = 20;
    //        damage = 2;
    //        //sprite = new sprite(Pos, Content, "img\\Destroyer_1B");
    //        speed = 0.1f;
    //    }
    //}
    //class Heavy : Unit
    //{
    //    public Heavy(string name, Vector2 Pos, bool friendly)
    //        : base(name, Pos)
    //    {
    //        last = new TimeSpan(0);
    //        fireRate = new TimeSpan(0, 0, 0, 2);
    //        pv_max = 30;
    //        pv = 30;
    //        damage = 4;
    //        //sprite = new sprite(Pos, Content, "img\\Vaisseau_1_LourdB");
    //        speed = 0.05f;
    //    }
    //}
}
