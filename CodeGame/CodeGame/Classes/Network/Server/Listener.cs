// This class creates a simple TCP server for
// multi-player gaming. It runs in it's own thread
// and spawns a new thread per client. 4 clients max.

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
    class Listener {
        // This is global so an outsider can stop the thread.
        Thread _listenThread = null;

        Server _server = null;

        public Listener() {
        }

        public void Start() {
            _server = new Server();
            _server.Start();

            // Create a thread to listen for incoming connections.
            _listenThread = new Thread(Run);
            //_listenThread.IsBackground = true;     // May need this, but not sure yet.
            _listenThread.Start();
        }

        public void Stop() {
            _listenThread.Abort();
            _server.Stop();
        }

        // This method will eventually need error detection (try/catch etc)
        private void Run() {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            listenSocket.Listen(3);

            while (true) {
                Socket clientSocket = listenSocket.Accept();

                _server.PushConnection(clientSocket);
            }
        }
    }
}
