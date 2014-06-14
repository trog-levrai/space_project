using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PositronNova.Class.Unit;

namespace PositronNova
{
    class Universite
    {
        Game game;
        Color color;

        Ressources ressource;


        SpriteFont spriteFont;

        TimeSpan compt;
        TimeSpan last;
        int compteur;
        Random rand;

        Texture2D icone_precision;
        Texture2D icone_moteur;
        Texture2D icone_fleche;
        Texture2D icone_moteur_ok;
        Texture2D icone_precision_ok;

        Vector2 position_icone_fleche;
        Vector2 position_icone_precision;
        Vector2 position_icone_moteur;

        bool univ;
        public bool Univ
        {
            get { return univ; }
        }

        bool selected_fleche;
        bool selected_precision;
        bool selected_moteur;

        bool diminution_ressource_precision;
        bool diminution_ressource_moteur;

        bool lancer_recherche_precision;
        static private bool changement_precision;
        static public bool Changement_precision
        {
            get { return changement_precision; }
            set { changement_precision = value; }
        }
        bool lancer_rechercher_moteur;
        static private bool changement_moteur;
        static public bool Changement_moteur
        {
            get { return changement_moteur; }
            set { changement_moteur = value; }
        }

        public Universite(Game game,
            Texture2D icone_precision,
            Texture2D icone_moteur,
            Texture2D icone_fleche,
            Texture2D icone_precision_ok,
            Texture2D icone_moteur_ok,
            SpriteFont spriteFont,
            Ressources ressource)
        {
            this.game = game;
            this.ressource = ressource;

            this.icone_moteur = icone_moteur;
            this.icone_precision = icone_precision;
            this.icone_fleche = icone_fleche;
            this.icone_moteur_ok = icone_moteur_ok;
            this.icone_precision_ok = icone_precision_ok;

            this.spriteFont = spriteFont;

            selected_fleche = false;
            univ = false;
            selected_precision = false;
            selected_moteur = false;

            changement_moteur = false;
            changement_precision = false;
            lancer_recherche_precision = false;
            lancer_rechercher_moteur = false;
            diminution_ressource_moteur = false;
            diminution_ressource_precision = false;

            compt = new TimeSpan(0, 0, 1);
            compteur = 0;
            rand = new Random();
        }

        public void Update(GameTime gameTime, MouseState mouseState, MouseState oldmouse)
        {
            last = last.Add(gameTime.ElapsedGameTime);

            position_icone_fleche = new Vector2((int)(game.Window.ClientBounds.Width / 2 - 100 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y));
            position_icone_precision = new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));
            position_icone_moteur = new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y));
            
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                selected_fleche = Math.Abs(mouseState.X - (position_icone_fleche.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_fleche.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_fleche)
                    univ = true;
                else
                    univ = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_precision = Math.Abs(mouseState.X - (position_icone_precision.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_precision.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_precision && ressource.curRessources() >= RecquiredRessourcePrecision())
                {
                    if (!changement_precision)
                    {
                        lancer_recherche_precision = true;
                        diminution_ressource_precision = true;
                    }
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_moteur = Math.Abs(mouseState.X - (position_icone_moteur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_moteur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_moteur && ressource.curRessources() >= RecquiredRessourceMoteur())
                {
                    if (!changement_moteur)
                    {
                        lancer_rechercher_moteur = true;
                        diminution_ressource_moteur = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouseState;
            mouseState = Mouse.GetState();

            spriteBatch.Draw(icone_fleche,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 - 100 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y), 25, 25),
                Color.White);

            if(changement_precision)
                spriteBatch.Draw(icone_precision_ok,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                    Color.White);
            else
                spriteBatch.Draw(icone_precision,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                    Color.White);

            if(changement_moteur)
                spriteBatch.Draw(icone_moteur_ok,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                    Color.White);
            else
                spriteBatch.Draw(icone_moteur,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                    Color.White);


            if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 - 100 + Camera2d.Origine.X) &&
                mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 - 100 + Camera2d.Origine.X + 25) &&
                mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y) &&
                mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y + 25))
            {
                spriteBatch.DrawString(spriteFont, "Retour",
                    new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                    Color.Green);
            }

            if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X) &&
                mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X + 50))
            {
                if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y) &&
                    mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y + 50))
                {
                    if (changement_precision)
                        spriteBatch.DrawString(spriteFont,
                            "Degats" + "\n Technologie acquise",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                            Color.Green);
                    else
                        spriteBatch.DrawString(spriteFont,
                            "Degats     Requis : 500, 500" + 
                            "\nAugmente les degats de tous les vaisseaux !",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                            Color.Green);
                }

                if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y) &&
                    mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y + 50))
                {
                    if(changement_moteur)
                        spriteBatch.DrawString(spriteFont,
                            "Moteur" + "\n Technologie acquise",
                             new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                             Color.Green);
                    else
                        spriteBatch.DrawString(spriteFont,
                            "Moteur     Requis : 400, 400" +
                            "\nAugmente la vitesse de tous les vaisseaux en contruction" +
                            "\nN'augmente pas la vitesse des vaisseaux deja construits",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                            Color.Green);
                }
            }


            if ((lancer_recherche_precision || lancer_rechercher_moteur) &&
                last > compt)
            {
                spriteBatch.DrawString(spriteFont,
                             "Progression : " + compteur + "%",
                            new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y)),
                            Color.Red);
                compteur += rand.Next(1, 30);
                if (compteur > 100)
                {
                    compteur = 0;
                    if (lancer_recherche_precision)
                        changement_precision = true;
                    if (lancer_rechercher_moteur)
                        changement_moteur = true;

                    lancer_recherche_precision = false;
                    lancer_rechercher_moteur = false;
                }
                last = new TimeSpan(0);
            }
            else
            {
                spriteBatch.DrawString(spriteFont,
                            "Progression : " + compteur + "%",
                            new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y)),
                            Color.Red);
            }
        }

        public Ressources setRessource()
        {
            if (diminution_ressource_precision)
            {
                ressource.Energie -= 500;
                ressource.Metal -= 500;
                diminution_ressource_precision = false;
            }
            if (diminution_ressource_moteur)
            {
                ressource.Energie -= 400;
                ressource.Metal -= 400;
                diminution_ressource_moteur = false;
            }

            return ressource;
        }


        private Ressources RecquiredRessourcePrecision()
        {
            return new Ressources(500, 500);
        }

        private Ressources RecquiredRessourceMoteur()
        {
            return new Ressources(400, 400);
        }

        public void Start()
        {
            ressource = Ressources.getStartRessources();
        }
    }
}
