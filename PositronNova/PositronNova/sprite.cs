using System;
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
            private bool moving;
            public Texture2D Texture
            {
                get { return _texture; }
                set { _texture = value; }
            }
            private Texture2D _texture;
            public Vector2 Position
            {
                get { return _position; }
                set { _position = value; }
            }
            private Vector2 _position;
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
            private Vector2 _direction;
            public Vector2 FPosition
            {
                get { return _direction; }
                set { _direction = Vector2.Normalize(value); }
            }
            private Vector2 _fposition;
            public float Speed
            {
                get { return _speed; }
                set { _speed = value; }
            }
            private float _speed;
            public virtual void Initialize()
            {
                _position = Vector2.Zero;
                _direction = Vector2.Zero;
                _speed = 0;
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
            public virtual void Update(GameTime gameTime)
            {
                if (Math.Abs(_position.X - _mouse.X) <= 2 && Math.Abs(_position.Y - _mouse.Y) <= 2)
                {
                    moving = false;
                }
                if (moving)
                {
                    _position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            /// <summary>
            /// Permet de gérer les entrées du joueur
            /// </summary>
            /// <param name="keyboardState">L'état du clavier à tester</param>
            /// <param name="mouseState">L'état de la souris à tester</param>
            /// <param name="joueurNum">Le numéro du joueur qui doit être surveillé</param>
            public virtual void HandleInput(KeyboardState keyboardState, MouseState mouseState)
            {
                //Ce code est magique, ne pas trop toucher SVP :-)
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    _position.Y--;
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    _position.Y++;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    _position.X++;
                }
                else if (keyboardState.IsKeyDown(Keys.Left))
                {
                    _position.X--;
                }
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    selected = Math.Abs(mouseState.X - _position.X) <= 40 && Math.Abs(mouseState.Y - _position.Y) <= 26;
                }
                if (mouseState.RightButton == ButtonState.Pressed && selected)
                {
                    moving = true;
                    _mouse.X = mouseState.X;
                    _mouse.Y = mouseState.Y;
                    _direction.X = mouseState.X - _position.X;
                    _direction.Y = mouseState.Y - _position.Y;
                    _direction.Normalize();
                }
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
        }
    }
