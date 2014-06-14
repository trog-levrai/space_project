using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PositronNova.Class
{
    class Cell
    {
        bool isVisited = false;
        Texture2D texture;
        public Rectangle rectangle;
        public Rectangle rectPickUp;
        Vector2 position;
        int largeur, hauteur;

        public Cell(Vector2 position, int largeur, int hauteur)
        {
            this.position = position;
            this.largeur = largeur;
            this.hauteur = hauteur;
            rectangle = new Rectangle((int)position.X, (int)position.Y, largeur, hauteur);
            rectPickUp = new Rectangle(50, 50, 50, 50);
        }

        public void ContentLoad(ContentManager content)
        {
            texture = content.Load<Texture2D>("img\\misc\\Brouillard");
        }

        public void Update(Cell[,] tab, int i, int j, int nbCellX, int nbCellY)
        {
            for (int z = 0; z < PositronNova.UnitList.Count; z++)
            {
                if (!tab[i, j].isVisited)
                    if (PositronNova.UnitList[z].Side == Unit.UnitSide.Humain && PositronNova.UnitList[z].champDeVision.Intersects(tab[i, j].rectangle))
                        tab[i, j].isVisited = true;

                if (!tab[i, j].isVisited)
                {
                    if (i > 0 && tab[i - 1, j].isVisited && j > 0 && tab[i, j - 1].isVisited)
                        rectPickUp = new Rectangle(0, 0, 50, 50);
                    else if (i > 0 && tab[i - 1, j].isVisited && j < nbCellY - 1 && tab[i, j + 1].isVisited)
                        rectPickUp = new Rectangle(0, 100, 50, 50);
                    else if (i < nbCellX - 1 && tab[i + 1, j].isVisited && j > 0 && tab[i, j - 1].isVisited)
                        rectPickUp = new Rectangle(100, 0, 50, 50);
                    else if (i < nbCellX - 1 && tab[i + 1, j].isVisited && j < nbCellY - 1 && tab[i, j + 1].isVisited)
                        rectPickUp = new Rectangle(100, 100, 50, 50);
                    if (i > 0 && tab[i - 1, j].isVisited)
                        rectPickUp = new Rectangle(0, 50, 50, 50);
                    else if (i < nbCellX - 1 && tab[i + 1, j].isVisited)
                        rectPickUp = new Rectangle(100, 50, 50, 50);
                    else if (j > 0 && tab[i, j - 1].isVisited)
                        rectPickUp = new Rectangle(50, 0, 50, 50);
                    else if (j < nbCellY - 1 && tab[i, j + 1].isVisited)
                        rectPickUp = new Rectangle(50, 100, 50, 50);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (!isVisited)
                sb.Draw(texture, rectangle, rectPickUp, Color.White);
        }
    }

    public class BrouillardDeGuerre
    {
        Cell[,] cellTab;
        int largeur, hauteur;
        int tailleCellX, tailleCellY;

        public BrouillardDeGuerre(int tailleCellX, int tailleCellY, int largeur, int hauteur)
        {
            this.largeur = largeur;
            this.hauteur = hauteur;
            this.tailleCellX = tailleCellX;
            this.tailleCellY = tailleCellY;
            cellTab = new Cell[largeur / tailleCellX, hauteur / tailleCellY];
            for (int i = 0; i < largeur / tailleCellX; i++)
                for (int j = 0; j < hauteur / tailleCellY; j++)
                {
                    cellTab[i, j] = new Cell(new Vector2(i * tailleCellX, j * tailleCellY), tailleCellX, tailleCellY);
                }
        }

        public void ContentLoad(ContentManager content)
        {
            for (int i = 0; i < largeur / tailleCellX; i++)
                for (int j = 0; j < hauteur / tailleCellY; j++)
                {
                    cellTab[i, j].ContentLoad(content);
                }
        }

        public void Update()
        {
            for (int i = 0; i < largeur / tailleCellX; i++)
                for (int j = 0; j < hauteur / tailleCellY; j++)
                {
                    cellTab[i, j].Update(cellTab, i, j, largeur / tailleCellX, hauteur / tailleCellY);
                }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < largeur / tailleCellX; i++)
                for (int j = 0; j < hauteur / tailleCellY; j++)
                {
                    if (cellTab[i, j].rectangle.Bottom > Camera2d.Origine.Y && // On dessine que ce qu'il y a dans le scroll (performance)
                        cellTab[i, j].rectangle.Top < Camera2d.Origine.Y + PositronNova.winHeight &&
                        cellTab[i, j].rectangle.Right > Camera2d.Origine.X &&
                        cellTab[i, j].rectangle.Left < Camera2d.Origine.X + PositronNova.winWidth )
                    {
                        cellTab[i, j].Draw(sb);
                    }
                }
        }
    }
}
