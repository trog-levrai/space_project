using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    class Chat
    {
        //Capte le texte
        //Le code sera à compléter pour l'implémentation de l'autocomplete pour shell
        private bool tab;
        private int rank;
        private string input;
        private KBInput kb;
        public Chat()
        {
            tab = false;
            rank = 0;
            input = "";
            kb = new KBInput();
        }
        public void KBInput(KeyboardState keyboardState)
        {
            if (tab)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    int i = 0;
                    while (i<10 && texts[i] != "")
                    {
                        i += 1;
                    }
                    if (i == 10)
                    {
                        //On décale tout et on met input à la dernière place car tout est remplis
                        for (int j = 1; j < 9; j++)
                        {
                            texts[j - 1] = texts[j];
                        }
                        texts[9] = input;
                    }
                    else
                    {
                        texts[i] = input;
                    }
                    rank = i + 1;
                    input = "";
                    kb.current = "";
                    tab = false;
                }
                else
                {
                    input += kb.GetString(keyboardState);
                }
            }
            else
            {
                tab = keyboardState.IsKeyDown(Keys.Tab);
            }
        }
        //Position de l'affichage
        private Vector2 position;
        public Vector2 GetPosition()
        {
            position = new Vector2(0, GraphicsDeviceManager.DefaultBackBufferHeight - 14*rank);
            return position;
        }
        //Génère le texte à afficher
        private string[] texts = new string[10] { "", "", "", "", "", "", "", "", "", ""};
        public string ReturnString(KeyboardState kb)
        {
            string ans = "";
            foreach (string text in texts)
            {
                if (text != "")
                {
                    ans = ans + "<" + text + ">\n";
                }
            }
            return ans;
        }

    }
}
