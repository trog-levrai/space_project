﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    class Chat
    {
        //Capte le texte
        private bool tab;
        private int rank;
        private string input;
        public string Input
        {
            get { return input; }
        }
        Game game;
        private KBInput kb;
        private string[] texts;
        public Chat(Game game)
        {
            tab = false;
            rank = 0;
            this.game = game;
            input = "";
            kb = new KBInput();
            texts = new string[10] {"", "", "", "", "", "", "", "", "", ""};
        }
        public void addString(string mess)
        {
            if (mess != "")
            {
                int i = 0;
                do
                {
                    i += 1;
                } while (i < texts.Length && texts[i] != "");
                if (i == texts.Length && mess != "")
                {
                    //On décale tout et on met mess à la dernière place car tout est remplis
                    for (int j = 1; j < texts.Length; j++)
                    {
                        texts[j - 1] = texts[j];
                    }
                    texts[texts.Length - 1] = mess;
                    rank = 10;
                }
                if (mess != "" && i < texts.Length)
                {
                    texts[i] = mess;
                    rank += 1;
                }
            }
        }
        public void Enter()
        {
            addString(input);
            input = "";
            kb.current = "";
            tab = false;
        }
        public void KBInput(KeyboardState keyboardState)
        {
            if (tab)
            {
                input += kb.GetString(keyboardState);
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
            position = new Vector2(Camera2d.Origine.X, game.Window.ClientBounds.Height - 14*rank + Camera2d.Origine.Y);
            return position;
        }
        //Génère le texte à afficher
        public string ReturnString()
        {
            string ans = "";
            foreach (string text in texts)
            {
                if (text != "")
                {
                    if (text.StartsWith("f:"))
                        ans += text.Substring(2) + " (Booo)\n";
                    else
                    {
                        if (text.StartsWith("e:"))
                            ans += text.Substring(2) + " (Yayy)\n";
                        else
                        {
                            ans += "<" + text + ">\n";
                        }
                    }
                }
            }
            return ans;
        }

    }
}
