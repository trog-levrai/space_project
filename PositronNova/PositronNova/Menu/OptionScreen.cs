using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace PositronNova
{
    class OptionScreen : GameScreen
    {
        OptionComponent optionComponent;
        OptionComponentSelection optionComponentSelection;
        Texture2D image;
        Rectangle imageRectangle;
        GraphicsDeviceManager graphics;

        public int SelectedIndex
        {
            get { return optionComponent.SelectedIndex; }
            set { optionComponent.SelectedIndex = value; }
        }

        public int SselectedIndex
        {
            get { return optionComponentSelection.SSelectedIndex; }
            set { optionComponentSelection.SSelectedIndex = value; }
        }


        
        public OptionScreen(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, Texture2D image, GraphicsDeviceManager graphics)
            : base(game, spriteBatch)
        {
            this.graphics = graphics;
            string[] OptionItems = { "Full Screen", "The volume of music", "Difficulty", "Fog of war" ,"Back"};
            optionComponent = new OptionComponent(game, spriteBatch, spriteFont, OptionItems);
            Components.Add(optionComponent);
            string[] OptionItemsMusique = { "On", "Off", "-", "+", "Facile", "Medium", "Hard", "On", "Off"};
            optionComponentSelection = new OptionComponentSelection(game, spriteBatch, spriteFont, OptionItemsMusique, optionComponent);
            Componentsoption.Add(optionComponentSelection);


            //string[] OptionItemsPleinEcran = { "Oui", "Non" };
            //optionComponentSelection = new OptionComponentSelection(game, spriteBatch, spriteFont, OptionItemsPleinEcran);

            //Components.Add(optionComponentSelection);


            this.image = image;
            imageRectangle = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Draw(image, imageRectangle, Color.White);
            base.Draw(gameTime);
        }
    }
}
