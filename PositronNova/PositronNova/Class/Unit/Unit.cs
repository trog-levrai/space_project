using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PositronNova.Class.Unit
{
    class Unit
    {
        public sprite sprite;
        private string name;
        public string Name
        {
            get { return name; }
        }
        private byte type;
        protected int pv_max;
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
        public Unit(string name, byte type, ContentManager Content)
        {
            switch (type)
            {
                case 1:
                    pv_max = 10;
                    pv = 10;
                    sprite = new sprite();
                    sprite.Direction = Vector2.Zero;
                    sprite.Mouse = Vector2.Zero;
                    sprite.Initialize();
                    sprite.LoadContent(Content, "img\\nyan");
                    sprite.Speed = (float)0.15;
                    break;
                case 2:
                    pv_max = 15;
                    pv = 15;
                    sprite = new sprite();
                    sprite.Direction = Vector2.Zero;
                    sprite.Mouse = Vector2.Zero;
                    sprite.Initialize();
                    sprite.LoadContent(Content, "img\\nyan");
                    sprite.Speed = (float)0.125;
                    break;
                case 3:
                    pv_max = 20;
                    pv = 20;
                    sprite = new sprite();
                    sprite.Direction = Vector2.Zero;
                    sprite.Mouse = Vector2.Zero;
                    sprite.Initialize();
                    sprite.LoadContent(Content, "img\\nyan");
                    sprite.Speed = (float)0.1;
                    break;
            }
            this.name = name;
        }
    }
}
