using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    class Chat
    {
        //Capte le texte
        //Le code sera à compléter pour l'implémentation de l'autocomplete pour shell
        private MatchCollection foo;
        private bool tab;
        private int rank;
        private string input;
        Game game;
        private Regex regex;
        private KBInput kb;
        private string[] texts;
        public Chat(Game game)
        {
            regex = new Regex(@"[;(\-)pdosx]*");
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
                //On remplace par des emoticones
                regex.Replace(mess, "\u1F601");
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
        public void KBInput(KeyboardState keyboardState)
        {
            if (tab)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    byte[] msg = Encoding.Default.GetBytes(input);
                    UdpClient udpClient = new UdpClient();
                    udpClient.Send(msg, msg.Length, "10.3.140.222", 1234);
                    udpClient.Close();
                    addString(input);
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
            position = new Vector2(Camera2d.Origine.X, game.Window.ClientBounds.Height - 14*rank + Camera2d.Origine.Y);
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
