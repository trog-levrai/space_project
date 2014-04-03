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
    public abstract class Unit
    {
        private Unit enn;
        public Unit Ennemy
        {
            get { return enn; }
            set { enn = value; }
        }
        protected System.TimeSpan fireRate;
        protected System.TimeSpan last;
        protected SoundEffect laserSound;
        private bool friendly;
        private Color color;
        private SpriteFont _font;
        private Texture2D cercle;
        public sprite sprite;
        private string name;
        private byte type;
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
        public void Update(GameTime gt, Vector2 Pos)
        {
            last = last.Add(gt.ElapsedGameTime);
            //On ne prend les touches que si c'est un allie
            if (friendly)
            {
                sprite.HandleInput(Keyboard.GetState(), Mouse.GetState(), Pos);
                //Le but du test est de savoir si il s'est bien ecoule au moins une seconde
                if (enn != null && last >= fireRate)
                {
                    KeyboardState keyboardState = Keyboard.GetState();
                    if (keyboardState.IsKeyDown(Keys.X) && enn.Pv >= 0)
                    {
                        attack(enn);
                        last = new TimeSpan(0);
                    }
                }
            }
            sprite.Update(gt);
        }
        //Ce qui suit est le constructeur
        //On pourra rajouter facilement plein d'options quand quelqu'un sera motive
        public Unit(string name, ContentManager Content, bool friendly)
        {
            this.friendly = friendly;
            if (friendly)
            {
                color = Color.Aqua;
            }
            else
            {
                color = Color.IndianRed;
            }
            this.name = name;
            _font = Content.Load<SpriteFont>("Affichage_mouse");
            laserSound = Content.Load<SoundEffect>("sounds\\laser");
            cercle = Content.Load<Texture2D>("img\\cercle-vert2");
            enn = null;
        }
        //Le but est de rendre le sprite de la classe private.
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, gameTime);
            spriteBatch.DrawString(_font, name, new Vector2(sprite.Position.X - 3, sprite.Position.Y - 15), color);
            //Mettre "|| !friendly" pour tester l'effet de la methode attack
            if (sprite.Selected || !friendly)
            {
                spriteBatch.DrawString(_font, pv + "/" + pv_max, new Vector2(sprite.Position.X - 3, sprite.Position.Y - 25), color);
            }

            if (sprite.Selected)
            {
                spriteBatch.Draw(cercle, new Vector2(sprite.Position.X, sprite.Position.Y + 9), Color.White);
            }
        }
        //Methode ultra basique pour l'attaque
        //Il va falloir rajouter plein de trucs du genre une portee de tir
        //Un systeme pour selectionner les ennemis a abatre, etc
        public void attack(Unit enn)
        {
            enn.Pv -= damage;
            laserSound.Play();
        }
    }
    //Les dignes heritieres de la class Unit :-)
    //On initialise une variable qui nous servira de timer pour les attques avec "last"
    class Fighter : Unit
    {
        public Fighter(string name, ContentManager Content,Vector2 Pos, bool friendly) : base(name, Content, friendly)
        {
            last = new TimeSpan(0);
            fireRate = new TimeSpan(0,0,0,1);
            pv_max = 10;
            pv = 10;
            damage = 1;
            sprite = new sprite(Pos,Content,"img\\Chasseur_1B");
            sprite.Speed = (float)0.15;
        }
    }
    class Destroyer : Unit
    {
        public Destroyer(string name, ContentManager Content, Vector2 Pos, bool friendly) : base(name, Content, friendly)
        {
            last = new TimeSpan(0);
            fireRate = new TimeSpan(0, 0, 0, 1);
            pv_max = 20;
            pv = 20;
            damage = 2;
            sprite = new sprite(Pos, Content, "img\\Destroyer_1B");
            sprite.Speed = (float)0.1;
        }
    }
    class Heavy : Unit
    {
        public Heavy(string name, ContentManager Content, Vector2 Pos, bool friendly) : base(name, Content, friendly)
        {
            last = new TimeSpan(0);
            fireRate = new TimeSpan(0, 0, 0, 2);
            pv_max = 30;
            pv = 30;
            damage = 4;
            sprite = new sprite(Pos, Content, "img\\Vaisseau_1_LourdB");
            sprite.Speed = (float)0.05;
        }
    }
}
