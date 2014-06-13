﻿using System;
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
    class Planete
    {

        MouseState mouseState;
        MouseState oldMouseState;

        private Ressources ressource;
        public Ressources Ressource
        {
            get { return ressource; }
            set { ressource = value; }
        }

        Color color;

        Texture2D image_planete;
        Texture2D image_plus;
        Texture2D image_centrale;
        Texture2D image_extracteur;
        Texture2D image_tick;
        Texture2D image_caserne;

        Rectangle imageRectangle;
        bool selected;
        bool selected_centrale;
        bool selected_extracteur;
        bool selected_caserne;
        bool plus;
        bool diminution_centrale;
        bool diminution_extracteur;
        bool diminution_caserne;
        bool progress_centrale;
        bool progress_extracteur;
        bool progress_caserne;

        string batiments;

        int verif;
        int diminution_centrale_ressource;
        int diminution_extracteur_ressource;
        int diminution_caserne_ressource;
        int recquired_ressource_centrale;
        int recquired_ressource_extracteur;

        TimeSpan compt;
        TimeSpan last;
        Random rand;
        int compteur;

        Vector2 position;
        SpriteFont spriteFont;
        SpriteFont progressFont;
        Game game;

        Vector2 position_icone_centrale;
        Vector2 position_icone_extracteur;
        Vector2 position_icone_caserne;
        Vector2 position_icone_plus;

        private int niveau_centrale;
        public int Niveau_centrale
        {
            get { return niveau_centrale; } 
        }
        private int niveau_extracteur;
        public int Niveau_extracteur
        {
            get { return niveau_extracteur; }
        }
        private int niveau_caserne;
        public int Niveau_caserne
        {
            get { return niveau_caserne; }
        }

        public Planete(Game game, 
            Texture2D image_planete, 
            Texture2D image_plus, 
            Texture2D image_centrale, 
            Texture2D image_extracteur, 
            Texture2D image_tick,
            Texture2D image_caserne,
            Ressources ressource,
            SpriteFont spriteFont,
            SpriteFont progressFont)
        {
            this.image_planete = image_planete;
            this.image_plus = image_plus;
            this.image_centrale = image_centrale;
            this.image_extracteur = image_extracteur;
            this.progressFont = progressFont;
            this.image_tick = image_tick;
            this.image_caserne = image_caserne;
            this.spriteFont = spriteFont;
            this.game = game;
            this.ressource = ressource;

            niveau_centrale = 1;
            niveau_extracteur = 1;
            niveau_caserne = 0;
            diminution_centrale_ressource = 0;
            diminution_extracteur_ressource = 0;
            diminution_caserne_ressource = -1;
            recquired_ressource_centrale = 0;
            recquired_ressource_extracteur = 0;

            compt = new TimeSpan(0, 0, 1);
            compteur = 0;
            rand = new Random();

            position = new Vector2(50, 250);
            imageRectangle = new Rectangle(50, 250, image_planete.Width * 2, image_planete.Height * 2);

            selected = false;
            selected_centrale = false;
            selected_extracteur = false;
            selected_caserne = false;
            plus = false;
            diminution_centrale = false;
            diminution_extracteur = false;
            diminution_caserne = false;
            progress_centrale = false;
            progress_extracteur = false;
            progress_caserne = false;
            verif = -1;
        }

        public void Update(GameTime gameTime, MouseState mouse, MouseState oldmouse, KeyboardState keyboardState, KeyboardState oldKeyboardState)
        {
            last = last.Add(gameTime.ElapsedGameTime);

            position_icone_centrale = new Vector2((int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X), 
                (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));
            position_icone_extracteur = new Vector2((int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X),
                (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y));
            position_icone_plus = new Vector2((int)(game.Window.ClientBounds.Width - 100 + Camera2d.Origine.X),
                (int)(game.Window.ClientBounds.Height - 150 + Camera2d.Origine.Y));
            position_icone_caserne = new Vector2((int)(game.Window.ClientBounds.Width / 2 + 50 + Camera2d.Origine.X),
                (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));

            

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                selected = Math.Abs(mouseState.X - (position.X + image_planete.Width) + Camera2d.Origine.X) <= image_planete.Width & Math.Abs(mouseState.Y - (position.Y + image_planete.Height) + Camera2d.Origine.Y) <= image_planete.Height; // Le Camera2d.Origine c'est la décalage hein ;) distance entre l'orgine du background et l'origine de la cam
            }


            if (mouse.LeftButton == ButtonState.Pressed)
            {
                selected_centrale = Math.Abs(mouse.X - (position_icone_centrale.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouse.Y - (position_icone_centrale.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2; // Le Camera2d.Origine c'est la décalage hein ;) distance entre l'orgine du background et l'origine de la cam
                if (selected_centrale)
                {
                    selected = true;
                    verif = 0;
                }
            }

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                selected_extracteur = Math.Abs(mouse.X - (position_icone_extracteur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouse.Y - (position_icone_extracteur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2; // Le Camera2d.Origine c'est la décalage hein ;) distance entre l'orgine du background et l'origine de la cam
                if (selected_extracteur)
                {
                    selected = true;
                    verif = 1;
                }
            }

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                selected_caserne = Math.Abs(mouse.X - (position_icone_caserne.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouse.Y - (position_icone_caserne.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2; // Le Camera2d.Origine c'est la décalage hein ;) distance entre l'orgine du background et l'origine de la cam
                if (selected_caserne)
                {
                    selected = true;
                    verif = 2;
                }
            }

            if (mouse.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                plus = Math.Abs(mouse.X - (position_icone_plus.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouse.Y - (position_icone_plus.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2; // Le Camera2d.Origine c'est la décalage hein ;) distance entre l'orgine du background et l'origine de la cam
                if (plus)
                {
                    selected = true;
                    if (verif == 0)
                        selected_centrale = true;
                    if (verif == 1)
                        selected_extracteur = true;
                    if (verif == 2)
                        selected_caserne = true;

                }

            }

            if (selected && selected_centrale && plus)
            {
                if (ressource.curRessources() >= recquiredRessourceCentrale())
                {
                    progress_centrale = true;
                    switch (niveau_centrale)
                    {
                        case 1:
                            diminution_centrale_ressource = 1;
                            break;
                        case 2:
                            diminution_centrale_ressource = 2;
                            break;
                        case 3:
                            diminution_centrale_ressource = 3;
                            break;
                        case 4:
                            diminution_centrale_ressource = 4;
                            break;
                        default:
                            diminution_centrale_ressource = 0;
                            break;
                    }
                    niveau_centrale += 1;
                    if (niveau_centrale > 5)
                        niveau_centrale = 5;

                    diminution_centrale = true;
                    plus = false;

                }
            }

            if (selected && selected_extracteur && plus)
            {
                if (ressource.curRessources() >= recquiredRessourceExtracteur())
                {
                    progress_extracteur = true;
                    switch (niveau_extracteur)
                    {
                        case 1:
                            diminution_extracteur_ressource = 1;
                            break;
                        case 2:
                            diminution_extracteur_ressource = 2;
                            break;
                        case 3:
                            diminution_extracteur_ressource = 3;
                            break;
                        case 4:
                            diminution_extracteur_ressource = 4;
                            break;
                        default:
                            diminution_extracteur_ressource = 0;
                            break;
                    }

                    niveau_extracteur += 1;
                    if (niveau_extracteur > 5)
                        niveau_extracteur = 5;
                    diminution_extracteur = true;
                    plus = false;
                }
            }
            if (selected && selected_caserne && plus)
            {
                if (ressource.curRessources() >= recquiredRessourceCaserne())
                {
                    progress_caserne = true;
                    switch (niveau_caserne)
                    {
                        case 0:
                            diminution_caserne_ressource = 0;
                            break;
                        case 1:
                            diminution_caserne_ressource = 1;
                            break;
                        case 2:
                            diminution_caserne_ressource = 2;
                            break;
                        case 3:
                            diminution_caserne_ressource = 3;
                            break;
                        case 4:
                            diminution_caserne_ressource = 4;
                            break;
                        default:
                            diminution_caserne_ressource = -1;
                            break;
                    }
                    niveau_caserne += 1;
                    if (niveau_caserne > 5)
                        niveau_caserne = 5;
                    diminution_caserne = true;
                    plus = false;
                }
            }

            if (plus)
            {
                selected = true;
                verif = -1;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image_planete, imageRectangle, Color.White);

            mouseState = Mouse.GetState();

            if (selected)
            {
                spriteBatch.Draw(image_centrale, 
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 190 +Camera2d.Origine.Y), 50, 50),
                    Color.White);
                spriteBatch.Draw(image_extracteur,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 -50 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                    Color.White);
                spriteBatch.Draw(image_plus,
                    new Rectangle((int)(game.Window.ClientBounds.Width - 100 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 150 + Camera2d.Origine.Y), 50, 50), 
                    Color.White);
                spriteBatch.Draw(image_caserne,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 50 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                    Color.White);
            }

            if (selected)
            {
                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X + 50))
                {
                    if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y) &&
                        mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y + 50))
                    {
                        if (ressource.curRessources() >= recquiredRessourceCentrale())
                            color = Color.Green;
                        else
                            color = Color.Red;
                        spriteBatch.DrawString(spriteFont, "Niveau actuel :" + niveau_centrale + "\nNiveau suivant :" + recquiredRessourceCentrale().Energie + ", " + recquiredRessourceCentrale().Metal, new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y), color);
                    }

                    if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y) &&
                        mouseState.Y + Camera2d.Origine.Y < (int)(int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y + 50))
                    {
                        if (ressource.curRessources() >= recquiredRessourceExtracteur())
                            color = Color.Green;
                        else
                            color = Color.Red;
                        spriteBatch.DrawString(spriteFont, "Niveau actuel :" + niveau_extracteur + "\nNiveau suivant :" + recquiredRessourceExtracteur().Energie + ", " + recquiredRessourceExtracteur().Metal, new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y), color);
                    }

                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 50 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 50 + Camera2d.Origine.X + 50))
                {
                    if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y) &&
                        mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y + 50))
                    {
                        if (ressource.curRessources() >= recquiredRessourceCaserne())
                            color = Color.Green;
                        else
                            color = Color.Red;
                        if (niveau_caserne == 0)
                            spriteBatch.DrawString(spriteFont, "Pas construit" + "\nConstruction :" + recquiredRessourceCaserne().Energie + ", " + recquiredRessourceCaserne().Metal, new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y), color);
                        else
                            spriteBatch.DrawString(spriteFont, "Niveau actuel :" + niveau_caserne + "\nNiveau suivant :" + recquiredRessourceCaserne().Energie + ", " + recquiredRessourceCaserne().Metal, new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y), color);
                    }
                }

                if (selected_centrale)
                {
                    spriteBatch.Draw(image_tick,
                        new Rectangle((int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 10, 10),
                        Color.White);
                }

                if (selected_extracteur)
                {
                    spriteBatch.Draw(image_tick,
                        new Rectangle((int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 10, 10),
                        Color.White);
                }
                if (selected_caserne)
                {
                    spriteBatch.Draw(image_tick,
                        new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 50 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 10, 10),
                        Color.White);
                }

           }
            if (progress_centrale || progress_extracteur || progress_caserne)
            {
                if (progress_centrale)
                    batiments = "Centrale";
                else if (progress_extracteur)
                    batiments = "Extracteur";
                else if (progress_caserne)
                    batiments = "Caserne";
                else
                    batiments = "Nothing";

                spriteBatch.DrawString(progressFont,
                    batiments + " en construction",
                    new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 110 + Camera2d.Origine.Y)),
                        Color.Red);

                selected = false;
                if (last > compt)
                {
                    spriteBatch.DrawString(progressFont,
                         "Progression : " + compteur + "%",
                        new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 140 + Camera2d.Origine.Y)), 
                        Color.Red);
                    compteur += rand.Next(1, 10);
                    if (compteur > 100)
                    {
                        compteur = 0;
                        progress_centrale = false;
                        progress_extracteur = false;
                        progress_caserne = false;
                        selected = true;
                    }
                    last = new TimeSpan(0);
                }
                else
                    spriteBatch.DrawString(progressFont, 
                        "Progression : " + compteur + "%",
                        new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X), (int)(game.Window.ClientBounds.Height - 140 + Camera2d.Origine.Y)), 
                        Color.Red);
            }
            

        }
        public Ressources setRessource()
        {
            if (diminution_centrale)
            {
                switch (diminution_centrale_ressource)
                {
                    case 1:
                        ressource.Energie -= 60;
                        ressource.Metal -= 60;
                        break;
                    case 2:
                        ressource.Energie -= 150;
                        ressource.Metal -= 150;
                        break;
                    case 3:
                        ressource.Energie -= 300;
                        ressource.Metal -= 300;
                        break;
                    case 4:
                        ressource.Energie -= 500;
                        ressource.Metal -= 500;
                        break;
                    default:
                        ressource.Energie -= 0;
                        ressource.Metal -= 0;
                        break;
                }

            }
            if (diminution_extracteur)
            {
                switch (diminution_extracteur_ressource)
                {
                    case 1:
                        ressource.Energie -= 60;
                        ressource.Metal -= 60;
                        break;
                    case 2:
                        ressource.Energie -= 150;
                        ressource.Metal -= 150;
                        break;
                    case 3:
                        ressource.Energie -= 300;
                        ressource.Metal -= 300;
                        break;
                    case 4:
                        ressource.Energie -= 500;
                        ressource.Metal -= 500;
                        break;
                    default:
                        ressource.Energie -= 0;
                        ressource.Metal -= 0;
                        break;
                }
            }
            if (diminution_caserne)
            {
                switch (diminution_caserne_ressource)
                {
                    case 0:
                        ressource.Energie -= 120;
                        ressource.Metal -= 120;
                        break;
                    case 1:
                        ressource.Energie -= 150;
                        ressource.Metal -= 150;
                        break;
                    case 2:
                        ressource.Energie -= 170;
                        ressource.Metal -= 170;
                        break;
                    case 3:
                        ressource.Energie -= 190;
                        ressource.Metal -= 190;
                        break;
                    case 4:
                        ressource.Energie -= 210;
                        ressource.Metal -= 210;
                        break;
                    default:
                        ressource.Energie -= 0;
                        ressource.Metal -= 0;
                        break;
                }
            }

            diminution_centrale = false;
            diminution_extracteur = false;
            diminution_caserne = false;
            return ressource;
        }

        private Ressources recquiredRessourceCentrale()
        {
            switch (niveau_centrale)
            {
                case 1:
                    recquired_ressource_centrale = 60;
                    recquired_ressource_extracteur = 60;
                    break;
                case 2:
                    recquired_ressource_centrale = 150;
                    recquired_ressource_extracteur = 150;
                    break;
                case 3:
                    recquired_ressource_centrale = 300;
                    recquired_ressource_extracteur = 300;
                    break;
                case 4:
                    recquired_ressource_centrale = 500;
                    recquired_ressource_extracteur = 500;
                    break;
                default:
                    recquired_ressource_centrale = 0;
                    recquired_ressource_extracteur = 0;
                    break;
            }

            return new Ressources(recquired_ressource_centrale, recquired_ressource_extracteur);
        }

        private Ressources recquiredRessourceExtracteur()
        {
            switch (niveau_extracteur)
            {
                case 1:
                    recquired_ressource_centrale = 60;
                    recquired_ressource_extracteur = 60;
                    break;
                case 2:
                    recquired_ressource_centrale = 150;
                    recquired_ressource_extracteur = 150;
                    break;
                case 3:
                    recquired_ressource_centrale = 300;
                    recquired_ressource_extracteur = 300;
                    break;
                case 4:
                    recquired_ressource_centrale = 500;
                    recquired_ressource_extracteur = 500;
                    break;
                default:
                    recquired_ressource_centrale = 0;
                    recquired_ressource_extracteur = 0;
                    break;
            }

            return new Ressources(recquired_ressource_centrale, recquired_ressource_extracteur);
        }

        public Ressources recquiredRessourceCaserne()
        {
            switch (niveau_caserne)
            {
                case 0:
                    recquired_ressource_centrale = 120;
                    recquired_ressource_extracteur = 120;
                    break;
                case 1:
                    recquired_ressource_centrale = 150;
                    recquired_ressource_extracteur = 150;
                    break;
                case 2:
                    recquired_ressource_centrale = 170;
                    recquired_ressource_extracteur = 170;
                    break;
                case 3 :
                    recquired_ressource_centrale = 190;
                    recquired_ressource_extracteur = 190;
                    break;
                case 4 :
                    recquired_ressource_centrale = 210;
                    recquired_ressource_extracteur = 210;
                    break;
                default:
                    recquired_ressource_centrale = 0;
                    recquired_ressource_extracteur = 0;
                    break;
            }

            return new Ressources(recquired_ressource_centrale, recquired_ressource_extracteur);
        }

    }
}