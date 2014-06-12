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
    class Planete
    {
        MouseState mouseState;
        MouseState oldMouseState;

        Texture2D image_planete;
        Texture2D image_plus;
        Texture2D image_centrale;
        Texture2D image_extracteur;
        Texture2D image_tick;

        Rectangle imageRectangle;
        bool selected;
        bool selected_centrale;
        bool selected_extracteur;
        bool plus;
        int verif;

        Vector2 position;
        SpriteFont spriteFont;
        Game game;

        Vector2 position_icone_centrale;
        Vector2 position_icone_extracteur;
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

        public Planete(Game game, Texture2D image_planete, Texture2D image_plus, Texture2D image_centrale, Texture2D image_extracteur, Texture2D image_tick, SpriteFont spriteFont)
        {
            this.image_planete = image_planete;
            this.image_plus = image_plus;
            this.image_centrale = image_centrale;
            this.image_extracteur = image_extracteur;
            this.image_tick = image_tick;
            this.spriteFont = spriteFont;
            this.game = game;

            niveau_centrale = 1;
            niveau_extracteur = 1;

            position = new Vector2(50, 250);
            imageRectangle = new Rectangle(50, 250, image_planete.Width * 2, image_planete.Height * 2);

            selected = false;
            selected_centrale = false;
            selected_extracteur = false;
            plus = false;
            verif = -1;
        }

        public void Update(GameTime gameTime, MouseState mouse, MouseState oldmouse)
        {
            position_icone_centrale = new Vector2((int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X), 
                (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));
            position_icone_extracteur = new Vector2((int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X),
                (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y));
            position_icone_plus = new Vector2((int)(game.Window.ClientBounds.Width - 100 + Camera2d.Origine.X),
                (int)(game.Window.ClientBounds.Height - 150 + Camera2d.Origine.Y));

            

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

                    }

                }

                if (selected && selected_centrale && plus)
                {
                    niveau_centrale += 1;
                    if (niveau_centrale > 5)
                        niveau_centrale = 5;
                    selected = true;
                }

                if (selected && selected_extracteur && plus)
                {
                    niveau_extracteur += 1;
                    if (niveau_extracteur > 5)
                        niveau_extracteur = 5;
                    selected = true;
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
            }

            if (selected)
            {
                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 - 50 + Camera2d.Origine.X + 50))
                {
                    if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y) &&
                        mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y + 50))
                    {
                        spriteBatch.DrawString(spriteFont, "Niveau Suivant :", new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y), Color.Red);
                    }

                    if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y) &&
                        mouseState.Y + Camera2d.Origine.Y < (int)(int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y + 50))
                    {
                        spriteBatch.DrawString(spriteFont, "Niveau Suivant :", new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y), Color.Red);
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
       
            }
            

        }
    }
}
