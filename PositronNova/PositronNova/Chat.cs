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
        private Regex regex;
        private KBInput kb;
        private string[] texts;
        public Chat()
        {
            //regex = new Regex("[;(\-)pdosx]*");
            //regex.Options=
            tab = false;
            rank = 0;
            input = "";
            kb = new KBInput();
            texts = new string[10] {"", "", "", "", "", "", "", "", "", ""};
        }
        public void addString(string mess)
        {
            if (mess != "")
            {
                ////On remplace par des emoticones
                //foo = regex.Matches(mess);
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
                    udpClient.Send(msg, msg.Length, "10.3.5.1", 5035);
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
            }
            return ans;
        }

    }
}
