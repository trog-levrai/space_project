using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PositronNova.Class.Unit
{
    public abstract class Unit
    {
        private bool friendly;
        private Color color;
        private SpriteFont _font;
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
        public void Update(GameTime gt, Unit enn)
        {
            //On ne prend les touches que si c'est un allie
            if (friendly)
            {
                sprite.HandleInput(Keyboard.GetState(), Mouse.GetState());
                attack(enn);
            }
            sprite.Update(gt);
        }
        //Ce qui suit est le constructeur
        public Unit(string name, ContentManager Content, bool friendly)
        {
            this.friendly = friendly;
            if (friendly)
            {
                color = Color.Blue;
            }
            else
            {
                color = Color.IndianRed;
            }
            this.name = name;
            _font = Content.Load<SpriteFont>("Affichage_mouse");
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
        }
        //Methode ultra basique pour le moment
        public void attack(Unit enn)
        {
            enn.Pv -= damage;
        }
    }
    class Fighter : Unit
    {
        public Fighter(string name, ContentManager Content,Vector2 Pos, bool friendly) : base(name, Content, friendly)
        {
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
            pv_max = 30;
            pv = 30;
            damage = 4;
            sprite = new sprite(Pos, Content, "img\\Vaisseau_1_LourdB");
            sprite.Speed = (float)0.05;
        }
    }
}
