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
        public class sprite
        {
            //Booleen de selection
            private bool selected = false;
            public bool Selected
            {
                get { return selected; }
            }

            private Texture2D _texture;
            public Texture2D Texture
            {
                get { return _texture; }
                set { _texture = value; }
            }

            public Vector2 textureOrigine;
    
            public float textureRotation;

            private Vector2 _position;
            public Vector2 Position
            {
                get { return _position; }
                set { _position = value; }
            }

            private Vector2 _direction;
            public Vector2 Direction
            {
                get { return _direction; }
                set { _direction = Vector2.Normalize(value); }
            }

            private Vector2 _mouse;
            public Vector2 Mouse
            {
                get { return _mouse; }
                set { _mouse = value; }
            }

            private float _speed;
            public float Speed
            {
                get { return _speed; }
                set { _speed = value; }
            }

            private bool moving;

            Vector2 destination;

            public Color[] textureData; //Pour les collisions Pix/Pix 

            /*public Vector2 FPosition
            {
                get { return _direction; }
                set { _direction = Vector2.Normalize(value); }
            }
            private Vector2 _fposition;*/
            
            ////////////////////////// CONSTRUCTEURS

            public sprite(Vector2 pos, ContentManager content, string cont)
            {
                _position = pos;
                _direction = Vector2.Zero;
                _speed = 0;
                Mouse = Vector2.Zero;
                LoadContent(content, cont);
            }

            public sprite(Vector2 pos, float textureRotation, ContentManager content, string cont)
            {
                _position = pos;
                this.textureRotation = textureRotation;
                _direction = Vector2.Zero;
                _speed = 0;
                Mouse = Vector2.Zero;
                LoadContent(content, cont);
            }

            /////////////////////// METHODES

            public void Init()
            {
                Speed = 0;
                Direction = Vector2.Zero;
                destination = Position;
            }
            /// <summary>
            /// Charge l'image voulue grâce au ContentManager donné
            /// </summary>
            /// <param name="content">Le ContentManager qui chargera l'image</param>
            /// <param name="assetName">L'asset name de l'image à charger pour ce Sprite</param>
            public virtual void LoadContent(ContentManager content, string assetName)
            {
                _texture = content.Load<Texture2D>(assetName);
            }
            /// <summary>
            /// Met à jour les variables du sprite
            /// </summary>
            /// <param name="gameTime">Le GameTime associé à la frame</param>
            /// 

            /// <summary>
            /// Permet de gérer les entrées du joueur
            /// </summary>
            /// <param name="keyboardState">L'état du clavier à tester</param>
            /// <param name="mouseState">L'état de la souris à tester</param>
            /// <param name="joueurNum">Le numéro du joueur qui doit être surveillé</param>
            public virtual bool GetEnnemy(MouseState mouseState)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    return Math.Abs(mouseState.X - _position.X + Camera2d.Origine.X) <= 40 & Math.Abs(mouseState.Y - _position.Y + Camera2d.Origine.Y) <= 26;
                else
                    return false;
            }
            public virtual void HandleInput(KeyboardState keyboardState, MouseState mouseState/*, Vector2 Pos*/)
            {
                //Ce code est magique, ne pas trop toucher SVP :-)
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    selected = Math.Abs(mouseState.X - (_position.X + Texture.Width / 2)+ Camera2d.Origine.X) <= Texture.Width / 2 & Math.Abs(mouseState.Y - (_position.Y + Texture.Height / 2) + Camera2d.Origine.Y) <= Texture.Height / 2; // Le Camera2d.Origine c'est la décalage hein ;) distance entre l'orgine du background et l'origine de la cam
                }
                if (mouseState.RightButton == ButtonState.Pressed && selected)
                {
                    destination = new Vector2(mouseState.X + Camera2d.Origine.X, mouseState.Y + Camera2d.Origine.Y); //position de la mouse par rapport à l'origine de l'écran + décalage par rapport à l'origine de l'écran par rapport à l'origine du background
                    moving = true;
                }
            }
            
            void deplacement()
            {
                int stopPrecision = 2;

                if (moving)
                {
                    destination = new Vector2((int)destination.X, (int)destination.Y);
                    Direction = new Vector2(destination.X - Position.X, destination.Y - Position.Y);
                    Position += Direction * Speed; // Silence ça pousse... ahem... bouge ! :o)
                    
                    if (Math.Abs(Position.X - destination.X) <= stopPrecision && Math.Abs(Position.Y - destination.Y) <= stopPrecision) // empêche le ship de tourner (vibrer?) autour de la destination avec stopPrecision
                        Position = destination;
                }
                else
                    Init();

                if (Position != destination)
                {
                    moving = true;
                    Speed = 2f;
                }
                else
                    moving = false;
            }

            //////////////////////////// UPDATE & DRAW

            public virtual void Update(GameTime gameTime)
            {
                deplacement();

                if (Position.X <= 5 || Position.X + Texture.Width >= PositronNova.BackgroundTexture.Width - 5||
                    Position.Y <= 5 || Position.Y + Texture.Height >= PositronNova.BackgroundTexture.Height - 5)
                    moving = false;
            }
            
            /// <summary>
            /// Dessine le sprite en utilisant ses attributs et le spritebatch donné
            /// </summary>
            /// <param name="spriteBatch">Le spritebatch avec lequel dessiner</param>
            /// <param name="gameTime">Le GameTime de la frame</param>
            public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
            {
                spriteBatch.Draw(_texture, _position, Color.White);
            }

            public virtual void Draw(SpriteBatch sb)
            {
                sb.Draw(_texture, _position, null, Color.White, textureRotation, textureOrigine, 1f, SpriteEffects.None, 0);
            }
        }
    }
