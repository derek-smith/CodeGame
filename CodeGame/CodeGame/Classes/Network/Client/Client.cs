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
        Commands _commands = new Commands();
        BinaryWriter _writer = null;
        List<string> _playerNames = new List<string>(4);
        List<int> _playerIDs = new List<int>(4);

        public Client(string nickname) {
            Thread clientThread = new Thread(Run);
            clientThread.Start(nickname);
        }

        // Todo: Add error handling
        private void Run(object o) {
            string nickname = (string)o;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));
            NetworkStream stream = new NetworkStream(socket);
            BinaryReader reader = new BinaryReader(stream);
            _writer = new BinaryWriter(stream);

            _writer.Write((int)Command.JoinGame);
            _writer.Write(nickname);

            int cmd = -1;

            while (true) {
                cmd = reader.ReadInt32();

                switch ((Command)cmd) {
                    case Command.JoinGameSuccess:
                        int numPlayers = reader.ReadInt32();
                        for (int i = 0; i < numPlayers; i++) {
                            string name = reader.ReadString();
                            _playerNames.Add(name);
                        }
                        break;
                }
            }
        }
    }
}
