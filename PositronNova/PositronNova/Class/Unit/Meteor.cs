using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PositronNova
{
    class Meteor
    {
        private Texture2D tex;
        private Random rand = new Random();
                private int frameSquare;
        private double tempsActFrameSwitch;
        private double tempsPreFrameSwitch;
        private bool frameReaderSwitch = true;

        private int direction;
        private Vector2 position;
        private Vector2 speed;
        private int moveTime;

        private double tempsActuel = 0;
        private double tempsPrecedent = 0;

        private int tempsMouvMin = 2;
        private int tempsMouvMax = 6;

        ////METHODS
        //public void zomScreenBounds()
        //{
        //    if (position.X <= 0)
        //    {
        //        speed = Vector2.Zero;
        //        direction = 4;
        //    }
        //    else if (position.X >= Ressources.winWidth - 40)
        //    {
        //        speed = Vector2.Zero;
        //        direction = 3;
        //    }
        //    else if (position.Y <= 0)
        //    {
        //        speed = Vector2.Zero;
        //        direction = 1;
        //    }
        //    else if (position.Y >= Ressources.winHeight - 40)
        //    {
        //        speed = Vector2.Zero;
        //        direction = 2;
        //    }
        //    else
        //    {
        //        direction = rand.Next(5);
        //    }
        //}
        public Meteor(ContentManager Content)
        {
            //tex = Content.Load<Texture2D>("img\\cercle-vert2");
        }
        
    }
}
