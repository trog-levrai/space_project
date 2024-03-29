﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PositronNova.Class.Unit;
using PositronNova.Class;

namespace PositronNova
{
    class Universite
    {
        Game game;

        Ressources ressource;

        Chat text;

        SpriteFont spriteFont;

        TimeSpan compt;
        TimeSpan last;
        int compteur;
        Random rand;

        Texture2D icone_precision;
        Texture2D icone_moteur;
        Texture2D icone_blindage;
        Texture2D icone_fleche;
        Texture2D icone_moteur_ok;
        Texture2D icone_precision_ok;
        Texture2D icone_blindage_ok;

        Vector2 position_icone_fleche;
        Vector2 position_icone_precision;
        Vector2 position_icone_moteur;
        Vector2 position_icone_blindage;

        bool univ;
        public bool Univ
        {
            get { return univ; }
        }

        bool selected_fleche;
        bool selected_precision;
        bool selected_moteur;
        bool selected_blindage;

        static bool diminution_ressource_precision;
        static public bool Diminution_ressource_precision
        {
            get { return diminution_ressource_precision; }
            set { diminution_ressource_precision = value; }
        }
        static bool diminution_ressource_moteur;
        static public bool Diminution_ressource_moteur
        {
            get { return diminution_ressource_moteur; }
            set { diminution_ressource_moteur = value; }
        }
        static bool diminution_ressource_blindage;
        static public bool Diminution_ressource_blindage
        {
            get { return diminution_ressource_blindage; }
            set { diminution_ressource_blindage = value; }
        }

        bool precision_ok;
        bool moteur_ok;
        bool blindage_ok;

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

        bool lancer_recherche_blindage;
        static private bool changement_blindage;
        static public bool Changement_blindage
        {
            get { return changement_blindage; }
            set { changement_blindage = value; }
        }

        public Universite(Game game,
            Texture2D icone_precision,
            Texture2D icone_moteur,
            Texture2D icone_fleche,
            Texture2D icone_precision_ok,
            Texture2D icone_moteur_ok,
            Texture2D icone_blindage,
            Texture2D icone_blindage_ok,
            SpriteFont spriteFont,
            Ressources ressource,
            Chat text)
        {
            this.game = game;
            this.ressource = ressource;

            this.icone_moteur = icone_moteur;
            this.icone_precision = icone_precision;
            this.icone_fleche = icone_fleche;
            this.icone_moteur_ok = icone_moteur_ok;
            this.icone_precision_ok = icone_precision_ok;
            this.icone_blindage = icone_blindage;
            this.icone_blindage_ok = icone_blindage_ok;

            this.spriteFont = spriteFont;
            this.text = text;

            selected_fleche = false;
            univ = false;
            selected_precision = false;
            selected_moteur = false;
            selected_blindage = false;

            changement_moteur = false;
            changement_precision = false;
            changement_blindage = false;
            lancer_recherche_precision = false;
            lancer_rechercher_moteur = false;
            lancer_recherche_blindage = false;
            diminution_ressource_moteur = false;
            diminution_ressource_precision = false;
            diminution_ressource_blindage = false;
            precision_ok = false;
            moteur_ok = false;
            blindage_ok = false;

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
            position_icone_blindage = new Vector2((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));
            
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
                if (selected_precision && PositronNova.ressources.curRessources() >= RecquiredRessourcePrecision()/*&& ressource.curRessources() >= RecquiredRessourcePrecision()*/)
                {
                    if (!changement_precision)
                    {
                        lancer_recherche_precision = true;
                        Universite.Diminution_ressource_precision = true;
                    }
                }
                else
                {
                    //Son : Pas possible
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_moteur = Math.Abs(mouseState.X - (position_icone_moteur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_moteur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_moteur && PositronNova.ressources.curRessources() >= RecquiredRessourceMoteur()/*&& ressource.curRessources() >= RecquiredRessourceMoteur()*/)
                {
                    if (!changement_moteur)
                    {
                        lancer_rechercher_moteur = true;
                        diminution_ressource_moteur = true;
                    }
                }
                else
                {
                    //Son : Pas possible
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_blindage = Math.Abs(mouseState.X - (position_icone_blindage.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_blindage.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_blindage && PositronNova.ressources.curRessources() >= RecquiredRessourceBlindage()/*&& ressource.curRessources() >= RecquiredRessourceMoteur()*/)
                {
                    if (!changement_blindage)
                    {
                        lancer_recherche_blindage = true;
                        diminution_ressource_blindage = true;
                    }
                }
                else
                {
                    //Son : Pas possible
                }
            }

            if (precision_ok)
            {
                text.addString("La technologie 'Dégats' est maintenant recherchée");
                Manager.rechercheTerminee_s.Play();
                precision_ok = false;
            }
            if (moteur_ok)
            {
                text.addString("La technologie 'Moteur' est maintenant recherchée");
                Manager.rechercheTerminee_s.Play();
                moteur_ok = false;
            }
            if (blindage_ok)
            {
                text.addString("La technologie 'Blindage' est maintenant recherchée");
                Manager.rechercheTerminee_s.Play();
                blindage_ok = false;
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

            if (changement_blindage)
                spriteBatch.Draw(icone_blindage_ok,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                    Color.White);
            else
                spriteBatch.Draw(icone_blindage,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
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
                    else if(PositronNova.ressources.curRessources() <= RecquiredRessourcePrecision())
                            spriteBatch.DrawString(spriteFont,
                            "Degats     Requis : 500, 500" + 
                            "\nAugmente les degats de tous les vaisseaux !",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                            Color.Red);
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
                    else if (PositronNova.ressources.curRessources() <= RecquiredRessourceMoteur())
                        spriteBatch.DrawString(spriteFont,
                            "Moteur     Requis : 400, 400" +
                            "\nAugmente la vitesse de tous les vaisseaux en contruction" +
                            "\nN'augmente pas la vitesse des vaisseaux deja construits",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                            Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont,
                            "Moteur     Requis : 400, 400" +
                            "\nAugmente la vitesse de tous les vaisseaux en contruction" +
                            "\nN'augmente pas la vitesse des vaisseaux deja construits",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                            Color.Green);
                }
            }

            if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X) &&
                mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X +50) &&
                mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y) &&
                mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y + 50))
            {
                if (changement_blindage)
                    spriteBatch.DrawString(spriteFont,
                        "Blindage" + "\n Technologie acquise",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y +Camera2d.Origine.Y),
                        Color.Green);
                else if (PositronNova.ressources.curRessources() <= RecquiredRessourceBlindage())
                    spriteBatch.DrawString(spriteFont,
                        "Blindage     Requis : 600, 600" +
                        "\n Augmente les points de vie de tous les vaisseaux en construction" +
                        "\n N'augmente pas les points de vie des vaisseaux deja construits",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                else
                    spriteBatch.DrawString(spriteFont,
                        "Blindage     Requis : 600, 600" +
                        "\n Augmente les points de vie de tous les vaisseaux en construction" +
                        "\n N'augmente pas les points de vie des vaisseaux deja construits",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Green);
            }


            if ((lancer_recherche_precision || lancer_rechercher_moteur || lancer_recherche_blindage) &&
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
                    {
                        changement_precision = true;
                        precision_ok = true;
                    }
                    if (lancer_rechercher_moteur)
                    {
                        changement_moteur = true;
                        moteur_ok = true;
                    }
                    if(lancer_recherche_blindage)
                    {
                        changement_blindage = true;
                        blindage_ok = true;
                    }

                    lancer_recherche_precision = false;
                    lancer_rechercher_moteur = false;
                    lancer_recherche_blindage = false;
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
                //ressource.Energie -= 500;
                //ressource.Metal -= 500;
                ressource -= RecquiredRessourcePrecision();
                diminution_ressource_precision = false;
            }
            if (diminution_ressource_moteur)
            {
                //ressource.Energie -= 400;
                //ressource.Metal -= 400;
                ressource -= RecquiredRessourceMoteur();
                diminution_ressource_moteur = false;
            }
            if (diminution_ressource_blindage)
            {
                ressource -= RecquiredRessourceBlindage();
                diminution_ressource_blindage = false;
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

        private Ressources RecquiredRessourceBlindage()
        {
            return new Ressources(600, 600);
        }

        public void Start()
        {
            ressource = Ressources.getStartRessources();
        }

    }
}
