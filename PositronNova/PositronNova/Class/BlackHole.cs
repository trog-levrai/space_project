using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PositronNova.Class
{
    class BlackHole
    {
        private Texture2D trounoir;
        private float elapsed;
        private float delay = 100f;
        private int frames = 0;
        private Rectangle gravite;
        private Rectangle degats;
        private Vector2 position;

        public BlackHole(Vector2 position)
        {
            this.position = position;
        }

        public void ContentLoad(ContentManager content)
        {
            trounoir = content.Load<Texture2D>("img\\misc\\sprites");
            gravite = new Rectangle((int)(position.X - 400), (int)(position.Y - 400), 800, 800);
            degats = new Rectangle((int)(position.X - 50), (int)(position.Y - 50), 100, 100);}

        public void Update(GameTime gametime)
        {
            elapsed += (float)gametime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                if (frames >= 8)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }

            ModifPosition();
        }

        public void Draw(SpriteBatch sb)
        {
            //sb.Draw(Manager.lifeBrick_t, degats, Color.White);
            sb.Draw(trounoir, new Rectangle((int)(position.X - 300), (int)(position.Y - 300), 600, 600), new Rectangle(150* frames,0,150,150), Color.White);
        }

        public void ModifPosition()
        {
            for (int i = 0; i < PositronNova.UnitList.Count; i++)
            {
                for (int j = 0; j < PositronNova.UnitList[i].hitBoxes.Length; j++)
                {
                    if (gravite.Intersects(PositronNova.UnitList[i].hitBoxes[j]))
                    {
                        Vector2 temp = (PositronNova.UnitList[i].position + PositronNova.UnitList[i].centre) - position;
                        temp.Normalize();
                        if (PositronNova.UnitList[i].position == PositronNova.UnitList[i].Destination)
                        {
                            PositronNova.UnitList[i].Destination -= temp/4;
                        }
                        PositronNova.UnitList[i].position -= temp / 4;
                    }

                    if (degats.Intersects((PositronNova.UnitList[i].hitBoxes[j])))
                    {
                        PositronNova.UnitList[i].Pv = 0;
                    }
                }
            }
        }

    }
}
