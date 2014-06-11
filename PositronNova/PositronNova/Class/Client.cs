using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace PositronNova.Class
{
    class Client
    {
        public String name { get; private set; }
        String host;
        int port;
        public Socket sock;
        StreamReader clientReader;
        StreamWriter clientWriter;
        public Client(String name, String host, int port)
        {
            this.name = name;
            this.host = host;
            this.port = port;
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(host, port);
            this.clientReader = new StreamReader(new NetworkStream(sock));
            this.clientWriter = new StreamWriter(new NetworkStream(sock));
        }
        //Methode de connection au serveur
        public void Connect()
        {
            sock.Connect(host, 1234);
            if (Receive() == "accepted")
            {
                Send(name);
            }
            else
            {
                System.Threading.Thread.Sleep(1000);
                Connect();
            }
        }
        public void Send(string message)
        {
            clientWriter.WriteLine(message);
            clientWriter.Flush();
        }
        public string Receive()
        {
            string message;
            try
            {
                message = clientReader.ReadLine();
                return message;
            }
            catch
            {
                return null;
            }
        }
    }
}
