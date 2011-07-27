using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CodeGame.Classes.Network.Client {
    class Client {

        public Client() {
            Thread clientThread = new Thread(Run);
            clientThread.Start();
        }

        private void Run() {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));

            BinaryWriter bw = new BinaryWriter(new NetworkStream(clientSocket));
            bw.Write("Hello Server!");

            bw.Close();
        }
    }
}
