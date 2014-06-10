using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace PositronNova.Class
{
    public class Bouton
    {
        Texture2D bouton;
        MouseState mouseState;
        MouseState previousMouseState;
        Vector2 positionBouton = new Vector2(0, 0);

        public virtual void LoadContent(ContentManager content, string assetName)
        {
            bouton = content.Load<Texture2D>("img\\`bouton_vaisseau1");
        }

        public virtual void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bouton, positionBouton, Color.White);
            spriteBatch.End();
        }

        void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 20, 20);

            Rectangle RectBouton = new Rectangle((int)positionBouton.X, (int)positionBouton.Y + 20, 170, 100);

            if (mouseClickRect.Intersects(RectBouton))
            {
                //faire apparaitre un vaisseau aléatoire (parmi toute la flotte disponible) n'importe ou sur la map (sans dépasser de la fenêtre)
            }
        }
    }

    
}
