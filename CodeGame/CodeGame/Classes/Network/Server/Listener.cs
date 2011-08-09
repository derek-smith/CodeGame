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
        Interpreter interpreter = null;
        Thread thread = null;

        public Listener() {
        }

        public void Start() {
            interpreter = new Interpreter();
            thread = new Thread(ListenerLoop);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop() {
            thread.Abort();
        }

        public void ListenerLoop() {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 12345));
            socket.Listen(4);

            while (true) {
                // Blocks until an incoming connection connects
                Socket transmitterSocket = socket.Accept();

                // Create new transmitter and assign socket
                Reader reader = new Reader(transmitterSocket, interpreter);
                reader.Start();
            }
        }
    }
}
