﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PositronNova
{
    public abstract class sprite
    {
        public Texture2D texture;
        public Texture2D textureAnime;
        public Rectangle champDeVision;
        // HitBoxes
        public Rectangle[] hitBoxes;
        protected int nbHitBoxes, hitBoxesWidth, hitBoxesHeight, decalageHitBoxes;
        // ----
        // collisionInterVaisseau
        public Rectangle[] collisionInterVaisseau;
        protected int nbCIV;
        public int CIVWidth, CIVHeight;
        // ----
        public Vector2 position;
        public Vector2 centre; // Centre de l'unité, autrement dit centre de la texture :o)
        protected Vector2 direction;
        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        protected Vector2 destination;
        public Vector2 Destination
        {
            get { return destination; }
            set { destination = value; }
        }


        protected float speed;

        protected float textureRotation;

        // Animation
        protected System.TimeSpan timeToNextFrame;
        protected System.TimeSpan frameTimer;
        protected int frameSquare = 0;
        protected int nbFrame;
        protected int frameWidth, frameHeight;
        protected bool reverseFrame = false;

        public bool moving = false;

        //public Color[] textureData; //Pour les collisions Pix/Pix 

        ////////////////////////// CONSTRUCTEURS

        public sprite(Vector2 pos)
        {
            position = pos;
            speed = 0;
        }

        public sprite(Vector2 origine, Vector2 destination) // Pour les bullet
        {
            direction = new Vector2(destination.X - origine.X, destination.Y - origine.Y);
            direction.Normalize();

            if (destination.X - origine.X >= 0)
                textureRotation = (float)Math.Atan((destination.Y - origine.Y) / (destination.X - origine.X)); // Angle compris entre pi/2 et -pi/2
            else
                textureRotation = (float)Math.Atan((destination.Y - origine.Y) / (destination.X - origine.X)) + (float)Math.PI; // En rajoutant pi on passe de l'autre côté ^^

            position = origine;
        }

        /////////////////////// METHODES

        public virtual void Init()
        {

        }

        /// <summary>
        /// Charge l'image voulue grâce au ContentManager donné
        /// </summary>
        /// <param name="content">Le ContentManager qui chargera l'image</param>
        /// <param name="assetName">L'asset name de l'image à charger pour ce Sprite</param>
        /*public virtual void LoadContent(ContentManager content)
        {
            //texture = content.Load<Texture2D>(assetName);
        }*/
        /// <summary>
        /// Met à jour les variables dorigineu sprite
        /// </summary>
        /// <param name="gameTime">Le GameTime associé à la frame</param>
        /// 

        /// <summary>
        /// Permet de gérer les entrées du joueur
        /// </summary>
        /// <param name="keyboardState">L'état du clavier à tester</param>
        /// <param name="mouseState">L'état de la souris à tester</param>
        /// <param name="joueurNum">Le numéro du joueur qui doit être surveillé</param>
        //public virtual bool GetEnnemy(MouseState mouseState)
        //{
        //    for (int i = 0; i < 
        //    if (mouseState.LeftButton == ButtonState.Pressed)
        //        return Math.Abs(mouseState.X - _position.X + Camera2d.Origine.X) <= 40 & Math.Abs(mouseState.Y - _position.Y + Camera2d.Origine.Y) <= 26;
        //    else
        //        return false;
        //}

        //////////////////////////// UPDATE & DRAW

        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Dessine le sprite en utilisant ses attributs et le spritebatch donné
        /// </summary>
        /// <param name="spriteBatch">Le spritebatch avec lequel dessiner</param>
        /// <param name="gameTime">Le GameTime de la frame</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void DrawRot(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, textureRotation, centre, 1f, SpriteEffects.None, 0);
        }
    }
}
