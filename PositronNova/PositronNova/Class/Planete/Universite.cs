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

        SpriteFont spriteFont;

        TimeSpan compt;
        TimeSpan last;
        int compteur;
        Random rand;

        Texture2D icone_precision;
        Texture2D icone_moteur;
        Texture2D icone_fleche;

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
            SpriteFont spriteFont)
        {
            this.game = game;

            this.icone_moteur = icone_moteur;
            this.icone_precision = icone_precision;
            this.icone_fleche = icone_fleche;

            this.spriteFont = spriteFont;

            selected_fleche = false;
            univ = false;
            selected_precision = false;
            selected_moteur = false;

            changement_moteur = false;
            changement_precision = false;
            lancer_recherche_precision = false;
            lancer_rechercher_moteur = false;

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
                if (selected_precision)
                {
                    if (!changement_precision)
                        lancer_recherche_precision = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_moteur = Math.Abs(mouseState.X - (position_icone_moteur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_moteur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_moteur)
                {
                    if (!changement_moteur)
                        lancer_rechercher_moteur = true;
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

            spriteBatch.Draw(icone_precision,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                Color.White);

            spriteBatch.Draw(icone_moteur,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                Color.White);

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
    }
}
