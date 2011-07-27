using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace CodeGame.Classes.Network.Server {
    class Server {
        ServerShared _shared;

        public Server(ServerShared shared) {
            _shared = shared;

            Thread listenThread = new Thread(Run);
            //listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void Run() {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            listenSocket.Listen(3);

            Socket clientSocket = listenSocket.Accept();

            BinaryReader br = new BinaryReader(new NetworkStream(clientSocket));
            string s = br.ReadString();
            Debug.WriteLine(s);
            

            br.Close();
        }
    }
}
