using System;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PositronNova.Class
{
    class Client
    {
        public Chat chat;
        private NetworkStream ns;
        private Thread Writer, Reader;
        public String name { get; private set; }
        String host;
        int port;
        public Socket sock;
        StreamReader clientReader;
        StreamWriter clientWriter;
        private BinaryFormatter format;
        public Client(String name, String host, int port, Game game)
        {
            format = new BinaryFormatter();
            chat = new Chat(game);
            this.name = name;
            this.host = host;
            this.port = port;
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(host, port);
            this.clientReader = new StreamReader(new NetworkStream(sock));
            this.clientWriter = new StreamWriter(new NetworkStream(sock));
            Writer = new Thread(new ThreadStart(Receive));
            //Writer = new Thread(new ThreadStart(Receive()));
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
        public void SendUnit(Unit.Unit unit)
        {
            //On serialise l'unite
            ns = new NetworkStream(sock);
            format.Serialize(ns, unit);
            ns.Flush();
        }
        public void Receive()
        {
            object data;
            try
            {
                string foo;
                foo = clientReader.ReadLine();
                if (foo != "")
                    chat.addString(foo);
            }
            catch
            {
                throw new EventLogReadingException("Pas de bol");
            }
        }
    }
}
