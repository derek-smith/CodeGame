using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CodeGame.Classes.Network.Server {

    class Reader {
        Thread thread = null;
        BinaryReader reader = null;
        BinaryWriter writer = null;
        Interpreter interpret = null;

        int id = -1;

        public BinaryWriter W { get { return writer; } }

        public Reader(Socket socket, Interpreter interpreter) {
            NetworkStream stream = new NetworkStream(socket);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            this.interpret = interpreter;
        }

        public void Start() {
            thread = new Thread(ReaderLoop);
            thread.Start();
        }

        public void Stop() {
            thread.Abort();
        }

        private void ReaderLoop() {

            int cmd = -1;

            while (true) {

                cmd = reader.ReadInt32();

                switch ((Command)cmd) {

                    case Command.JoinGame:

                        string nick = reader.ReadString();
                        id = interpret.JoinGame(this, nick);
                        break;

                    case Command.Ready:

                        interpret.Ready(id);
                        break;
                }
            }
        }
    }
}
