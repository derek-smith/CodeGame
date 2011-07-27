using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CodeGame.Classes.Network.Server {
    class Server {

        public Server() {
            Thread listenThread = new Thread(Run);
            //listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void Run() {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            listenSocket.Listen(3);

            while (true) {
                Socket clientSocket = listenSocket.Accept();

                BinaryWriter bw = new BinaryWriter(new NetworkStream(clientSocket));

                bw.Write("Hello World!");

                bw.Close();
            }
        }
    }
}
