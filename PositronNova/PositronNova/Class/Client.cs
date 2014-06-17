using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PositronNova.Class
{
    [Serializable()]
    class Client
    {
        private List<Unit.Unit> enn;
        public List<Unit.Unit> Ennemies
        {
            get { return enn; }
        }
        bool end = true;
        public Chat chat;
        private Thread Writer;
        public String name { get; private set; }
        public Socket sock;
        StreamReader clientReader;
        StreamWriter clientWriter;
        //public BinaryFormatter format;
        public Client(String name, String host, int port, Game game)
        {
            //format = new BinaryFormatter();
            chat = new Chat(game);
            enn = new List<Unit.Unit>();
            this.name = name;
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(host, port);
            this.clientReader = new StreamReader(new NetworkStream(sock));
            this.clientWriter = new StreamWriter(new NetworkStream(sock));
            Writer = new Thread(new ThreadStart(Receive));
            Writer.IsBackground = true;
            Writer.Start();
        }
        //Methode de connection au serveur
        public void KBInput(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Enter) && chat.Input != "")
            {
                Send(chat.Input);
                chat.Enter();
            }
            else
            {
                chat.KBInput(ks);
            }
        }
        public void Connect()
        {
            Send(name);
        }
        public void Send(string message)
        {
            clientWriter.WriteLine(message);
            clientWriter.Flush();
        }
        public void SendUnit(List<Unit.Unit> unit)
        {
            //On serialise la liste d'unites
            XmlSerializer format = new XmlSerializer(typeof(List<Unit.Unit>));
            format.Serialize(clientWriter, unit);
            clientWriter.Flush();
            //byte[] bytes = new byte[ms.Capacity];
            //ms.Position = 0;
            //bytes = ms.GetBuffer();
            //sock.Send(bytes);
        }

        public void Receive()
        {
            while (true)
            {
                //try
                //{
                    XmlSerializer format = new XmlSerializer(typeof(List<Unit.Unit>));
                    List<Unit.Unit> units;
                    //byte[] buffer = new byte[2048 * 128];
                    //sock.Receive(buffer);
                    //MemoryStream mem = new MemoryStream(buffer);
                    //mem.Position = 0;
                    units = (List<Unit.Unit>) format.Deserialize(clientReader);
                    clientReader.DiscardBufferedData();
                    lock (enn)
                    {
                        for (int i = 0; i < units.Count; i++)
                        {
                            units[i].Friendly = false;
                            enn.Add(units[i]);
                        }
                    }
                }
                //}
                //catch
                //{

                //}
            //}
            //catch
            //{
            //    //try
            //    //{
            //    //    string foo;
            //    //    foo = clientReader.ReadLine();
            //    //    if (foo != "")
            //    //    {
            //    //        chat.addString(foo);
            //    //    }
            //    //}
            //    //catch
            //    //{
            //    //    
            //    //}
            //}
            //}
        }
        public void Close()
        {
            //Est appele quand multijoueur est ferme
            sock.Close();
        }
    }
}