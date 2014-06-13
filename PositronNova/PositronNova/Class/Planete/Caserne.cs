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
    class Caserne
    {
        Game game;
        MouseState mouseState;
        Unit localUnit;
        ContentManager content;

        Random rand;

        int choix;

        Texture2D Icone_chasseur;
        Texture2D Icone_chasseur_lourds;
        Texture2D Icone_corvette;
        Texture2D Icone_croiseur;
        Texture2D Icone_cuirasse;
        Texture2D Icone_destroyer;
        Texture2D fleche;

        bool selected_fleche;
        bool recrut;
        public bool Recrut
        {
            get { return recrut; }
        }
        bool selected_chasseur;
        bool selected_chasseur_lourd;
        bool selected_corvette;
        bool selected_croiseur;
        bool selected_destroyer;
        bool selected_cuirasse;

        Vector2 position_icone_fleche;
        Vector2 position_icone_chasseur;
        Vector2 position_icone_chasseur_lourd;
        Vector2 position_icone_corvette;
        Vector2 position_icone_croiseur;
        Vector2 position_icone_destroyer;
        Vector2 position_icone_cuirasse;

        SpriteFont spriteFont;

        public Caserne(Game game,
            ContentManager content,
            Texture2D Icone_chasseur, 
            Texture2D Icone_chasseur_lourds,
            Texture2D Icone_corvette,
            Texture2D Icone_croiseur,
            Texture2D Icone_cuirasse,
            Texture2D Icone_destroyer,
            Texture2D fleche,
            SpriteFont spriteFont)
        {
            this.game = game;
            this.content = content;

            this.Icone_chasseur = Icone_chasseur;
            this.Icone_chasseur_lourds = Icone_chasseur_lourds;
            this.Icone_corvette = Icone_corvette;
            this.Icone_croiseur = Icone_croiseur;
            this.Icone_cuirasse = Icone_cuirasse;
            this.Icone_destroyer = Icone_destroyer;
            this.fleche = fleche;

            this.spriteFont = spriteFont;

            selected_chasseur = false;
            selected_chasseur_lourd = false;
            selected_corvette = false;
            selected_croiseur = false;
            selected_cuirasse = false;
            selected_destroyer = false;
            recrut = false;

            choix = -1;

            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();

            position_icone_fleche = new Vector2((int)(game.Window.ClientBounds.Width / 2 - 100 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y));
            position_icone_chasseur = new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X), 
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));
            position_icone_chasseur_lourd = new Vector2((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));
            position_icone_corvette = new Vector2((int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X), 
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y));
            position_icone_croiseur = new Vector2((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y));
            position_icone_destroyer = new Vector2((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y));
            position_icone_cuirasse = new Vector2((int)(game.Window.ClientBounds.Width / 2 +500 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y));

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                selected_fleche = Math.Abs(mouseState.X - (position_icone_fleche.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_fleche.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_fleche)
                    recrut = true;
                else
                    recrut = false;
                //selected_chasseur_lourd = Math.Abs(mouseState.X - (position_icone_chasseur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_chasseur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                selected_chasseur = Math.Abs(mouseState.X - (position_icone_chasseur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_chasseur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_chasseur)
                {
                    genHum(1);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mouseState = Mouse.GetState();

            spriteBatch.Draw(fleche,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2  - 100 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y), 25, 25), 
                Color.White);

            spriteBatch.Draw(Icone_chasseur, 
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X), 
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50), 
                Color.White);

            spriteBatch.Draw(Icone_chasseur_lourds,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                Color.White);

            spriteBatch.Draw(Icone_corvette,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X), 
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                Color.White);

            spriteBatch.Draw(Icone_croiseur,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                Color.White);

            spriteBatch.Draw(Icone_destroyer,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                Color.White);

            spriteBatch.Draw(Icone_cuirasse,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 +500 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                Color.White);


            if(mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2  - 100 + Camera2d.Origine.X) &&
                mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2  - 100 + Camera2d.Origine.X + 25) &&
                mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y) &&
                mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 130 + Camera2d.Origine.Y + 25))
            {
                spriteBatch.DrawString(spriteFont, "Retour",
                    new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                    Color.Red);
            }

            if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y) &&
                mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y + 50))
            {
                if(mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2  + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2  + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Chasseur",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Chasseur Lourd",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Corvette",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                }
            }

            if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y) &&
                mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y + 50))
            {
                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Croiseur",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Destroyer",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Cuirasse",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                }
            }

        }
            void genHum(int choix)
            {
                if(choix > 0 && choix <7)
                {
                    UnitType unit;
                    switch(choix)
                    {
                        case 1:
                            unit = UnitType.Chasseur;
                            break;
                        case 2:
                            unit = UnitType.Bombardier;
                            break;
                        case 3:
                            unit = UnitType.Corvette;
                            break;
                        case 4:
                            unit = UnitType.Croiseur;
                            break;
                        case 5:
                            unit = UnitType.Destroyer;
                            break;
                        case 6:
                            unit = UnitType.Cuirasse;
                            break;
                        default:
                            unit = UnitType.Chasseur;
                            break;
                    }

                    string[] names = new string[] {"Roger", "Gerard", "Patrick", "Mouloud", "Dede", "Jean-Claude", "Herve", "Gertrude", "Germaine", "Gisele", "Frenegonde", "JacquesArt", "JacquesOuille", "Riton", "Korben", "Jonathan", "Sebastien", "Paul", "Ilan", "Baptiste"};
                    int a = rand.Next(0, names.Length);
                    string name = names[a];
                    localUnit = new Unit(name , new Vector2(150, 150) ,unit);
                    localUnit.LoadContent(content);
                    localUnit.Init();
                    PositronNova.UnitList.Add(localUnit);
                    selected_chasseur = false;
                }
            }
    }
}
