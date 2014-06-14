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
        TimeSpan compt;
        TimeSpan last;
        int compteur;

        Ressources ressource;

        Game game;

        Unit localUnit;
        ContentManager content;

        Random rand;
        Chat text;

        Texture2D Icone_chasseur;
        Texture2D Icone_chasseur_lourds;
        Texture2D Icone_corvette;
        Texture2D Icone_corvette_grisee;
        Texture2D Icone_croiseur;
        Texture2D Icone_croiseur_grisee;
        Texture2D Icone_cuirasse;
        Texture2D Icone_cuirasse_grisee;
        Texture2D Icone_destroyer;
        Texture2D Icone_destroyer_grisee;
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

        bool lancer_recrutement_chasseur;
        bool recrutement_chasseur_OK;
        bool lancer_recrutement_chasseur_lourd;
        bool recrutement_chasseur_lourd_OK;
        bool lancer_recrutement_corvette;
        bool recrutement_corvette_OK;
        bool lancer_recrutement_croiseur;
        bool recrutement_croiseur_OK;
        bool lancer_recrutement_destroyer;
        bool recrutement_destroyer_OK;
        bool lancer_recrutement_cuirasse;
        bool recrutement_cuirasse_OK;

        bool diminution_ressource_chasseur;
        bool diminution_ressource_chasseur_lourd;
        bool diminution_ressources_corvette;
        bool diminution_ressource_croiseur;
        bool diminution_resource_destroyer;
        bool diminution_ressource_cuirasse;

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
            Texture2D Icone_corvette_grisee,
            Texture2D Icone_croiseur_grisee,
            Texture2D Icone_destroyer_grisee,
            Texture2D Icone_cuirasse_grisee,
            Texture2D fleche,
            SpriteFont spriteFont,
            Ressources ressource,
            Chat text)
        {
            this.game = game;
            this.content = content;

            compt = new TimeSpan(0, 0, 1);
            compteur = 0;

            this.Icone_chasseur = Icone_chasseur;
            this.Icone_chasseur_lourds = Icone_chasseur_lourds;
            this.Icone_corvette = Icone_corvette;
            this.Icone_croiseur = Icone_croiseur;
            this.Icone_cuirasse = Icone_cuirasse;
            this.Icone_destroyer = Icone_destroyer;
            this.fleche = fleche;
            this.ressource = ressource;

            this.text = text;

            this.Icone_corvette_grisee = Icone_corvette_grisee;
            this.Icone_croiseur_grisee = Icone_croiseur_grisee;
            this.Icone_cuirasse_grisee = Icone_cuirasse_grisee;
            this.Icone_destroyer_grisee = Icone_destroyer_grisee;

            this.spriteFont = spriteFont;

            selected_chasseur = false;
            selected_chasseur_lourd = false;
            selected_corvette = false;
            selected_croiseur = false;
            selected_cuirasse = false;
            selected_destroyer = false;
            recrut = false;

            lancer_recrutement_chasseur = false;
            recrutement_chasseur_OK = false;
            lancer_recrutement_chasseur_lourd = false;
            recrutement_chasseur_lourd_OK = false;
            lancer_recrutement_corvette = false;
            recrutement_corvette_OK = false;
            lancer_recrutement_croiseur = false;
            recrutement_croiseur_OK = false;
            lancer_recrutement_cuirasse = false;
            recrutement_cuirasse_OK = false;
            lancer_recrutement_destroyer = false;
            recrutement_destroyer_OK = false;

            diminution_resource_destroyer = false;
            diminution_ressource_chasseur = false;
            diminution_ressource_chasseur_lourd = false;
            diminution_ressource_croiseur = false;
            diminution_ressource_cuirasse = false;
            diminution_ressources_corvette = false;


            rand = new Random();
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime, MouseState mouseState, MouseState oldmouse)
        {
            last = last.Add(gameTime.ElapsedGameTime);

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
            }
            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_chasseur = Math.Abs(mouseState.X - (position_icone_chasseur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_chasseur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_chasseur && ressource.curRessources() >= RecquiredRessourceChasseur())
                {
                    lancer_recrutement_chasseur = true;
                    diminution_ressource_chasseur = true;
                }
            }

            if (recrutement_chasseur_OK)
            {
                genHum(1);
                text.addString("Un Chasseur vient d'être créé !");
                recrutement_chasseur_OK = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_chasseur_lourd = Math.Abs(mouseState.X - (position_icone_chasseur_lourd.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_chasseur_lourd.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_chasseur_lourd && ressource.curRessources() >= RecquiredRessourceChasseurLourd())
                {
                    lancer_recrutement_chasseur_lourd = true;
                    diminution_ressource_chasseur_lourd = true;

                }
            }

            if (recrutement_chasseur_lourd_OK)
            {
                genHum(2);
                text.addString("Un Chasseur loud vient d'être créé !");
                recrutement_chasseur_lourd_OK = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_corvette = Math.Abs(mouseState.X - (position_icone_corvette.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_corvette.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_corvette && Planete.Niveau_caserne > 1 && ressource.curRessources() >= RecquiredRessourceCorvette())
                {
                    lancer_recrutement_corvette = true;
                    diminution_ressources_corvette = true;
                }
            }

            if (recrutement_corvette_OK)
            {
                genHum(3);
                text.addString("Une Corvette vient d'être créée !");
                recrutement_corvette_OK = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_croiseur = Math.Abs(mouseState.X - (position_icone_croiseur.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_croiseur.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_croiseur && Planete.Niveau_caserne > 2 && ressource.curRessources() >= RecquiredRessourceCroiseur())
                {
                    lancer_recrutement_croiseur = true;
                    diminution_ressource_croiseur = true;
                }
            }

            if (recrutement_croiseur_OK)
            {
                genHum(4);
                text.addString("Un Croiseur vient d'être créé !");
                recrutement_croiseur_OK = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_cuirasse = Math.Abs(mouseState.X - (position_icone_cuirasse.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_cuirasse.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_cuirasse && Planete.Niveau_caserne > 3 && ressource.curRessources() >= RecquiredRessourceCuirasse())
                {
                    lancer_recrutement_cuirasse = true;
                    diminution_ressource_cuirasse = true;
                }
            }

            if (recrutement_cuirasse_OK)
            {
                genHum(6);
                text.addString("Un Cuirasse vient d'être créé !");
                recrutement_cuirasse_OK = false;
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                selected_destroyer = Math.Abs(mouseState.X - (position_icone_destroyer.X + 50 / 2) + Camera2d.Origine.X) <= 50 / 2 & Math.Abs(mouseState.Y - (position_icone_destroyer.Y + 50 / 2) + Camera2d.Origine.Y) <= 50 / 2;
                if (selected_destroyer && Planete.Niveau_caserne > 4 && ressource.curRessources() >= RecquiredRessourceDestroyer())
                {
                    lancer_recrutement_destroyer = true;
                    diminution_resource_destroyer = true;
                }
            }

            if (recrutement_destroyer_OK)
            {
                genHum(5);
                text.addString("Un Destroyer vient d'être créé !");
                recrutement_destroyer_OK = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouseState;
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

            if (Planete.Niveau_caserne < 2)
                spriteBatch.Draw(Icone_corvette_grisee,
                 new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X),
                (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                Color.White);
            else
                spriteBatch.Draw(Icone_corvette,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X), 
                    (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y), 50, 50),
                Color.White);

            if (Planete.Niveau_caserne < 3)
                spriteBatch.Draw(Icone_croiseur_grisee,
                     new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                Color.White);
            else
                spriteBatch.Draw(Icone_croiseur,
                new Rectangle((int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                Color.White);

            if(Planete.Niveau_caserne < 4)
                spriteBatch.Draw(Icone_destroyer_grisee,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                    Color.White);
            else
                spriteBatch.Draw(Icone_destroyer,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X),
                        (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                    Color.White);

            if(Planete.Niveau_caserne < 5)
                spriteBatch.Draw(Icone_cuirasse_grisee,
                    new Rectangle((int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X),
                    (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y), 50, 50),
                Color.White);
            else
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
                    Color.Green);
            }

            if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y) &&
                mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 190 + Camera2d.Origine.Y + 50))
            {
                if(mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2  + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2  + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Chasseur" + "\nRequis : 50, 60",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Green);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X + 50))
                {
                    spriteBatch.DrawString(spriteFont,
                        "Chasseur Lourd" + "\nRequis : 70, 90",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Green);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X + 50))
                {
                    if (Planete.Niveau_caserne < 2)
                        spriteBatch.DrawString(spriteFont,
                            "Corvette" + "\nRecquiert caserne \nniveau 2 ou plus",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                            Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont,
                        "Corvette" + "\nRequis : 100, 140",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Green);
                }
            }

            if (mouseState.Y + Camera2d.Origine.Y > (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y) &&
                mouseState.Y + Camera2d.Origine.Y < (int)(game.Window.ClientBounds.Height - 90 + Camera2d.Origine.Y + 50))
            {
                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + Camera2d.Origine.X + 50))
                {
                    if (Planete.Niveau_caserne < 3)
                        spriteBatch.DrawString(spriteFont,
                            "Croiseur" + "\nRecquiert caserne \nniveau 3 ou plus",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont,
                        "Croiseur" + "\nRequis : 220, 190",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Green);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 250 + Camera2d.Origine.X + 50))
                {
                    if (Planete.Niveau_caserne < 4)
                        spriteBatch.DrawString(spriteFont,
                            "Destroyer" + "\nRecquiert caserne \nniveau 4 ou plus",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont,
                        "Destroyer" + "\nRequis : 250, 250",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Green);
                }

                if (mouseState.X + Camera2d.Origine.X > (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X) &&
                    mouseState.X + Camera2d.Origine.X < (int)(game.Window.ClientBounds.Width / 2 + 500 + Camera2d.Origine.X + 50))
                {
                    if (Planete.Niveau_caserne < 5)
                        spriteBatch.DrawString(spriteFont,
                            "Cuirasse" + "\nRecquiert caserne \nniveau 5",
                            new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Red);
                    else
                        spriteBatch.DrawString(spriteFont,
                        "Cuirasse" + "\nRequis : 450, 500",
                        new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y),
                        Color.Green);
                }
            }

            if ((lancer_recrutement_chasseur ||
                lancer_recrutement_chasseur_lourd ||
                lancer_recrutement_corvette ||
                lancer_recrutement_croiseur ||
                lancer_recrutement_cuirasse ||
                lancer_recrutement_destroyer) &&
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
                    if (lancer_recrutement_chasseur)
                    {
                        recrutement_chasseur_OK = true;
                        lancer_recrutement_chasseur = false;
                    }
                    if (lancer_recrutement_chasseur_lourd)
                    {
                        recrutement_chasseur_lourd_OK = true;
                        lancer_recrutement_chasseur_lourd = false;
                    }
                    if (lancer_recrutement_corvette)
                    {
                        recrutement_corvette_OK = true;
                        lancer_recrutement_corvette = false;
                    }
                    if (lancer_recrutement_croiseur)
                    {
                        recrutement_croiseur_OK = true;
                        lancer_recrutement_croiseur = false;
                    }
                    if (lancer_recrutement_cuirasse)
                    {
                        recrutement_cuirasse_OK = true;
                        lancer_recrutement_cuirasse = false;
                    }
                    if (lancer_recrutement_destroyer)
                    {
                        recrutement_destroyer_OK = true;
                        lancer_recrutement_destroyer = false;
                    }

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

                    string[] names = new string[] { "Roger", "Gerard", "Patrick", "Mouloud", "Dede", "Jean-Claude", "Herve", "Gertrude", "Germaine", "Gisele", "Frenegonde", "JacquesArt", "JacquesOuille", "Riton", "Korben", "Jonathan", "Sebastien", "Paul", "Ilan", "Prescillia", "Niro", "Bastien", "Kelly", "Axel", "Baptiste" };
                    int a = rand.Next(0, names.Length);
                    string name = names[a];

                    localUnit = new Unit(name , new Vector2(150, 150) ,unit);

                    localUnit.Init();
                    PositronNova.UnitList.Add(localUnit);
                    selected_chasseur = false;
                }
            }

            public Ressources setRessource()
            {
                if (diminution_ressource_chasseur)
                {
                    //ressource.Energie -= 50;
                    //ressource.Metal -= 60;
                    ressource -= RecquiredRessourceChasseur();
                    diminution_ressource_chasseur = false;
                }
                if (diminution_ressource_chasseur_lourd)
                {
                    //ressource.Energie -= 70;
                    //ressource.Metal -= 90;
                    ressource -= RecquiredRessourceChasseurLourd();
                    diminution_ressource_chasseur_lourd = false;
                }
                if (diminution_ressources_corvette)
                {
                    //ressource.Energie -= 100;
                    //ressource.Metal -= 140;
                    ressource -= RecquiredRessourceCorvette();
                    diminution_ressources_corvette = false;
                }
                if (diminution_ressource_croiseur)
                {
                    //ressource.Energie -= 220;
                    //ressource.Metal -= 190;
                    ressource -= RecquiredRessourceCroiseur();
                    diminution_ressource_croiseur = false;
                }
                if (diminution_resource_destroyer)
                {
                    //ressource.Energie -= 250;
                    //ressource.Metal -= 250;
                    ressource -= RecquiredRessourceDestroyer();
                    diminution_resource_destroyer = false;
                }
                if (diminution_ressource_cuirasse)
                {
                    //ressource.Energie -= 450;
                    //ressource.Metal -= 500;
                    ressource -= RecquiredRessourceCuirasse();
                    diminution_ressource_cuirasse = false;
                }
                if (Universite.Diminution_ressource_precision)
                {
                    ressource.Energie -= 500;
                    ressource.Metal -= 500;
                    Universite.Diminution_ressource_precision = false;
                }
                if (Universite.Diminution_ressource_moteur)
                {
                    ressource.Energie -= 400;
                    ressource.Metal -= 400;
                    Universite.Diminution_ressource_moteur = false;
                }

                return ressource;
            }

            private Ressources RecquiredRessourceChasseur()
            {
                return new Ressources(50, 60);
            }

            private Ressources RecquiredRessourceChasseurLourd()
            {
                return new Ressources(70, 90);
            }

            private Ressources RecquiredRessourceCorvette()
            {
                return new Ressources(100, 140);
            }

            private Ressources RecquiredRessourceCroiseur()
            {
                return new Ressources(220, 190);
            }
            private Ressources RecquiredRessourceDestroyer()
            {
                return new Ressources(250, 250);
            }
            private Ressources RecquiredRessourceCuirasse()
            {
                return new Ressources(450, 500);
            }

            public void Start()
            {
                ressource = Ressources.getStartRessources();
            }
    }
}
