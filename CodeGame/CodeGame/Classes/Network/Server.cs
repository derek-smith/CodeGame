using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace CodeGame.Classes.Network {
    class Server {
        TcpListener listener;

        public Server() {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 54321);
        }
    }
}
