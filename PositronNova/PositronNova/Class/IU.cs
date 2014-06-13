using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PositronNova.Class;
using PositronNova.Class.Unit;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PositronNova
{
    class IU
    {
        List<Unit> UnitSelected = new List<Unit>();

        Texture2D selection;
        Texture2D cible;

        public IU()
        {
        }

        public void ContentLoad(ContentManager content)
        {
            selection = content.Load<Texture2D>("img\\SelectionCarre");
            cible = content.Load<Texture2D>("img\\TargetCarre");
        }

        public void HandleInput(KeyboardState keyboard, MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (keyboard.IsKeyDown(Keys.LeftShift))
                {
                    if (UnitSelected[0].Side == UnitSide.Alien)
                        UnitSelected.RemoveAt(0);
                    if (PositronNova.GetHumain() != null)
                        UnitSelected.Add(PositronNova.GetHumain());
                    else if (PositronNova.GetEnnemy() != null)
                        UnitSelected.Add(PositronNova.GetEnnemy());
                }
                else
                {
                    for (int j = 0; j < UnitSelected.Count; j++)
                        UnitSelected.RemoveAt(j);
                    if (PositronNova.GetHumain() != null)
                        UnitSelected.Add(PositronNova.GetHumain());
                    else if (PositronNova.GetEnnemy() != null)
                        UnitSelected.Add(PositronNova.GetEnnemy());
                }
            }
            if (mouse.RightButton == ButtonState.Pressed)
            {
                for (int i = 0; i < UnitSelected.Count; i++)
                {
                    if (UnitSelected[i].Side == UnitSide.Humain)
                    {
                        UnitSelected[i].Ennemy = PositronNova.GetEnnemy();
                        if (UnitSelected[i].Ennemy != null)
                        {
                            UnitSelected[i].HasTarget = Math.Pow(UnitSelected[i].position.X - UnitSelected[i].Ennemy.position.X, 2) + Math.Pow(UnitSelected[i].position.Y - UnitSelected[i].Ennemy.position.Y, 2) <= Math.Pow(UnitSelected[i].Range, 2);
                        }
                        else
                        {
                            UnitSelected[i].HasTarget = false;
                        }
                        if (!UnitSelected[i].HasTarget)
                        {
                            UnitSelected[i].Destination = new Vector2(mouse.X - UnitSelected[i].texture.Width / 2 + Camera2d.Origine.X, mouse.Y - UnitSelected[i].texture.Height / 2 + Camera2d.Origine.Y); //position de la mouse par rapport à l'origine de l'écran + décalage par rapport à l'origine de l'écran par rapport à l'origine du background
                            UnitSelected[i].Direction = new Vector2(UnitSelected[i].Destination.X - UnitSelected[i].position.X, UnitSelected[i].Destination.Y - UnitSelected[i].position.Y);
                            UnitSelected[i].Direction.Normalize();
                            UnitSelected[i].moving = true;
                        }
                    }
                }
            }
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            isSelected(sb);
        }

        void isSelected(SpriteBatch sb)
        {
            if (UnitSelected.Count > 0)
            {
                for (int i = 0; i < UnitSelected.Count; i++)
                {
                    if (!UnitSelected[i].Destruction())
                    {
                        if (UnitSelected[i].Side == UnitSide.Alien)
                        {
                            sb.Draw(cible, UnitSelected[i].position + new Vector2(-4, -4), new Rectangle(0, 0, 7, 7), Color.White);
                            sb.Draw(cible, UnitSelected[i].position + new Vector2(UnitSelected[i].texture.Width - 4, -4), new Rectangle(6, 0, 7, 7), Color.White);
                            sb.Draw(cible, UnitSelected[i].position + new Vector2(-4, UnitSelected[i].texture.Height - 4), new Rectangle(0, 6, 7, 7), Color.White);
                            sb.Draw(cible, UnitSelected[i].position + new Vector2(UnitSelected[i].texture.Width - 4, UnitSelected[i].texture.Height - 4), new Rectangle(6, 6, 7, 7), Color.White);
                        }
                        else
                        {
                            sb.Draw(selection, UnitSelected[i].position + new Vector2(-4, -4), new Rectangle(0, 0, 7, 7), Color.White);
                            sb.Draw(selection, UnitSelected[i].position + new Vector2(UnitSelected[i].texture.Width - 4, -4), new Rectangle(6, 0, 7, 7), Color.White);
                            sb.Draw(selection, UnitSelected[i].position + new Vector2(-4, UnitSelected[i].texture.Height - 4), new Rectangle(0, 6, 7, 7), Color.White);
                            sb.Draw(selection, UnitSelected[i].position + new Vector2(UnitSelected[i].texture.Width - 4, UnitSelected[i].texture.Height - 4), new Rectangle(6, 6, 7, 7), Color.White);
                        }
                        aUneCible(UnitSelected[i], sb);
                        UnitSelected[i].LifeBar.Draw(sb, (int)UnitSelected[i].position.X - 4, (int)UnitSelected[i].position.Y - 10);
                    }
                }
            }
        }

        void aUneCible(Unit unit, SpriteBatch sb)
        {
            if (unit.HasTarget)
                if (unit.Ennemy != null && unit.Ennemy.Pv > 0)
                {
                    sb.Draw(cible, unit.Ennemy.position + new Vector2(-4, -4), new Rectangle(0, 0, 7, 7), Color.White);
                    sb.Draw(cible, unit.Ennemy.position + new Vector2(unit.Ennemy.texture.Width - 4, -4), new Rectangle(6, 0, 7, 7), Color.White);
                    sb.Draw(cible, unit.Ennemy.position + new Vector2(-4, unit.Ennemy.texture.Height - 4), new Rectangle(0, 6, 7, 7), Color.White);
                    sb.Draw(cible, unit.Ennemy.position + new Vector2(unit.Ennemy.texture.Width - 4, unit.Ennemy.texture.Height - 4), new Rectangle(6, 6, 7, 7), Color.White);
                }
        }
    }
}
