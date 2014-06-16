using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PositronNova.Class
{
    [Serializable()]
    class Client
    {   
        private List<Unit.Unit> enn;
        public List<Unit.Unit>Ennemies
        {
            get { return enn; }
        }
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
            BinaryFormatter format = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            format.Serialize(ms, unit);
            byte[] bytes = new byte[ms.Capacity];
            ms.Seek(0, SeekOrigin.Begin);
            bytes = ms.GetBuffer();
            bool Send = false;
            foreach (var b in bytes)
            {
                Send = Send || b != 0;
            }
            if (Send)
                sock.Send(bytes);
        }
        public void Receive()
        {
            while (true)
            {
                //try
                //{
                    BinaryFormatter format = new BinaryFormatter();
                    Unit.Unit[] units;
                    byte[] buffer = new byte[2048 * 128];
                    sock.Receive(buffer);
                if (buffer != null)
                {
                    MemoryStream mem = new MemoryStream(buffer);
                    mem.Seek(0, SeekOrigin.Begin);
                    units = (Unit.Unit[]) format.Deserialize(mem);
                    lock (enn)
                    {
                        enn.Clear();
                        for (int i = 0; i < units.Length; i++)
                        {
                            units[i].Friendly = false;
                            enn.Add(units[i]);
                        }
                    }
                }
                //}
                //catch
                //{
                //    try
                //    {
                //        string foo;
                //        foo = clientReader.ReadLine();
                //        if (foo != "")
                //        {
                //            chat.addString(foo);
                //        }
                //    }
                //    catch
                //    {
                //        break;
                //    }
                //}
            }
        }
        public void Close()
        {
            //Est appele quand multijoueur est ferme
            sock.Close();
        }
    }
}