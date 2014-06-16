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
    public class Ressources
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

            ressou_rate = new TimeSpan(0, 0, 1);
            last = new TimeSpan(0);
        }

        public override string ToString()
        {
            return "Energie : " + r_energie + "\nMetal : " + r_metal;
        }

        public static Ressources getStartRessources()
        {
            if (PositronNova.Difficulte_hard)
                return new Ressources(100, 100);
            else if (PositronNova.Difficulte_medium)
                return new Ressources(200, 200);
            else
                return new Ressources(300, 300);
        }

        public void Update(GameTime gameTime, int niveau_centrale, int niveau_extracteur)
        {
            last = last.Add(gameTime.ElapsedGameTime);

            if (last >= ressou_rate)
            {
                if (PositronNova.Difficulte_easy)
                {
                    switch (niveau_centrale)
                    {
                        case 1:
                            r_energie += 6;
                            break;
                        case 2:
                            r_energie += 15;
                            break;
                        case 3:
                            r_energie += 30;
                            break;
                        case 4:
                            r_energie += 70;
                            break;
                        case 5:
                            r_energie += 120;
                            break;
                        default:
                            r_energie += 6;
                            break;
                    }

                    switch (niveau_extracteur)
                    {
                        case 1:
                            r_metal += 6;
                            break;
                        case 2:
                            r_metal += 15;
                            break;
                        case 3:
                            r_metal += 30;
                            break;
                        case 4:
                            r_metal += 70;
                            break;
                        case 5:
                            r_metal += 120;
                            break;
                        default:
                            r_metal += 6;
                            break;
                    }
                }

                if (PositronNova.Difficulte_medium)
                {
                    switch (niveau_centrale)
                    {
                        case 1:
                            r_energie += 4;
                            break;
                        case 2:
                            r_energie += 10;
                            break;
                        case 3:
                            r_energie += 20;
                            break;
                        case 4:
                            r_energie += 50;
                            break;
                        case 5:
                            r_energie += 100;
                            break;
                        default:
                            r_energie += 4;
                            break;
                    }

                    switch (niveau_extracteur)
                    {
                        case 1:
                            r_metal += 4;
                            break;
                        case 2:
                            r_metal += 10;
                            break;
                        case 3:
                            r_metal += 20;
                            break;
                        case 4:
                            r_metal += 50;
                            break;
                        case 5:
                            r_metal += 100;
                            break;
                        default:
                            r_metal += 4;
                            break;
                    }
                }

                if (PositronNova.Difficulte_hard)
                {
                    switch (niveau_centrale)
                    {
                        case 1:
                            r_energie += 2;
                            break;
                        case 2:
                            r_energie += 8;
                            break;
                        case 3:
                            r_energie += 15;
                            break;
                        case 4:
                            r_energie += 40;
                            break;
                        case 5:
                            r_energie += 80;
                            break;
                        default:
                            r_energie += 2;
                            break;
                    }

                    switch (niveau_extracteur)
                    {
                        case 1:
                            r_metal += 2;
                            break;
                        case 2:
                            r_metal += 8;
                            break;
                        case 3:
                            r_metal += 15;
                            break;
                        case 4:
                            r_metal += 40;
                            break;
                        case 5:
                            r_metal += 80;
                            break;
                        default:
                            r_metal += 2;
                            break;
                    }
                }

                last = new TimeSpan(0);
            }
        }

        public Ressources curRessources()
        {
            return new Ressources(r_energie, r_metal);
        }

    }
}
