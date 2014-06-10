using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    class Ressources
    {
        System.TimeSpan ressou_rate;
        System.TimeSpan last;

        private int r_energie = 0;
        private int r_metal = 0;

        public int Energie
        {
            get { return r_energie; }
            set { r_energie = value; }
        }

        public int Metal
        {
            get { return r_metal; }
            set { r_metal = value; }
        }

        public static bool operator >=(Ressources r1, Ressources r2)
        {
            if (r1.Energie >= r2.Energie)
            {
                if (r1.Metal >= r2.Metal)
                    return true;
            }

            return false;
        }

        public static bool operator <=(Ressources r1, Ressources r2)
        {
            if (r1.Energie <= r2.Energie)
            {
                if (r1.Metal <= r2.Metal)
                    return true;
            }
            return false;
        }

        public static Ressources operator -(Ressources r1, Ressources r2)
        {
            return new Ressources(r1.Energie - r2.Energie, r1.Metal - r2.Metal);
        }

        public Ressources(int Energie, int Metal)
        {
            r_energie = Energie;
            r_metal = Metal;

            ressou_rate = new TimeSpan(0, 0, 3);
            last = new TimeSpan(0);
        }

        public override string ToString()
        {
            return "Energie : " + r_energie + "\nMetal : " + r_metal;
        }

        public static Ressources getStartRessources()
        {
            return new Ressources(100, 100);
        }

        public void Update(GameTime gameTime)
        {
            last = last.Add(gameTime.ElapsedGameTime);

            if (last >= ressou_rate)
            {
                r_energie += 50;
                r_metal += 50;

                last = new TimeSpan(0);
            }
        }
    }
}
