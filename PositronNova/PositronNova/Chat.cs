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
        public string[] texts;
        public Chat()
        {
            tab = false;
            rank = 0;
            input = "";
            kb = new KBInput();
            texts = new string[10] {"", "", "", "", "", "", "", "", "", ""};
        }
        public void KBInput(KeyboardState keyboardState)
        {
            if (tab)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    int i = 0;
                    do
                    {
                        i += 1;
                    } while (i < texts.Length && texts[i] != "");
                    if (i == texts.Length && input != "")
                    {
                        //On décale tout et on met input à la dernière place car tout est remplis
                        for (int j = 1; j < texts.Length; j++)
                        {
                            texts[j - 1] = texts[j];
                        }
                        texts[texts.Length - 1] = input;
                        rank = 10;
                    }
                    if (input != "" && i < texts.Length)
                    {
                        texts[i] = input;
                        rank += 1;
                    }
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
        public string ReturnString(KeyboardState kb)
        {
            string ans = "";
            foreach (string text in texts)
            {
                if (text != "")
                {
                    ans += "<" + text + ">\n";
                }
                else
                {
                    ans += "Error";
                }
            }
            return ans;
        }

    }
}
