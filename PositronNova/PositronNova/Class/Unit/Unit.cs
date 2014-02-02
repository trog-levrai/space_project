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
    class Unit
    {
        private SpriteFont _font;
        public sprite sprite;
        private string name;
        private byte type;
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
        public void Update(GameTime gt)
        {
            sprite.HandleInput(Keyboard.GetState(), Mouse.GetState());
            sprite.Update(gt);
        }
        //Ce qui suit est le constructeur
        public Unit(string name, byte type, ContentManager Content)
        {
            //Le switch sert a donner les caracteristiques selon la classe du vaisseau
            switch (type)
            {
                case 1:
                    pv_max = 10;
                    pv = 10;
                    sprite = new sprite();
                    sprite.Direction = Vector2.Zero;
                    sprite.Mouse = Vector2.Zero;
                    sprite.Initialize();
                    sprite.LoadContent(Content, "img\\Chasseur_1B");
                    sprite.Speed = (float)0.15;
                    break;
                case 2:
                    pv_max = 15;
                    pv = 15;
                    sprite = new sprite();
                    sprite.Direction = Vector2.Zero;
                    sprite.Mouse = Vector2.Zero;
                    sprite.Initialize();
                    sprite.LoadContent(Content, "img\\Vaisseau_1_LourdB");
                    sprite.Speed = (float)0.125;
                    break;
                case 3:
                    pv_max = 20;
                    pv = 20;
                    sprite = new sprite();
                    sprite.Direction = Vector2.Zero;
                    sprite.Mouse = Vector2.Zero;
                    sprite.Initialize();
                    sprite.LoadContent(Content, "img\\Destroyer_1B");
                    sprite.Speed = (float)0.1;
                    break;
            }
            this.name = name;
            _font = Content.Load<SpriteFont>("Affichage_mouse");
        }
        //Le but est de rendre le sprite de la classe private.
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            sprite.Draw(spriteBatch, gameTime);
            if (sprite.Selected)
            {
                spriteBatch.DrawString(_font, name, new Vector2(sprite.Position.X - 3, sprite.Position.Y - 15), Color.Thistle);
                spriteBatch.DrawString(_font, pv + "/" + pv_max, new Vector2(sprite.Position.X - 3, sprite.Position.Y - 25), Color.Thistle);
            }
            else
            {
                spriteBatch.DrawString(_font, name, new Vector2(sprite.Position.X - 3, sprite.Position.Y - 15), Color.Thistle);
            }
        }
    }
}
