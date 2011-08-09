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
        Interpreter2 interpret = null;

        int id = -1;

        public BinaryWriter W { get { return writer; } }

        public Reader(Socket socket, Interpreter2 interpreter) {
            NetworkStream stream = new NetworkStream(socket);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            this.interpret = interpreter;
        }

        public void Start() {
            thread = new Thread(TransmitterLoop);
            thread.Start();
        }

        public void Stop() {
            thread.Abort();
        }

        private void TransmitterLoop() {

            int cmd = -1;

            while (true) {

                cmd = reader.ReadInt32();

                switch ((Command)cmd) {

                    case Command.JoinGame:

                        string nick = reader.ReadString();
                        id = interpret.JoinGame(this, nick);
                        break;

                    case Command.ReadyYes:

                        interpret.ReadyYes(id);
                        break;

                    case Command.ReadyNo:

                        interpret.ReadyNo(id);
                        break;
                }
            }
        }

    }




    class Transmitter {
        BinaryReader _reader = null;
        BinaryWriter _writer = null;
        Thread _thread = null;
        Interpreter _interpreter = null;

        int _id = -1;
        string _name = string.Empty;

        public Transmitter(Interpreter interpreter, Socket socket) {
            _interpreter = interpreter;
            _reader = new BinaryReader(new NetworkStream(socket));
            _writer = new BinaryWriter(new NetworkStream(socket));

            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop() {
            _thread.Abort();
        }

        private void Run() {
            int cmd = -1;

            while (true) {
                cmd = _reader.ReadInt32();

                switch ((Command)cmd) {
                    
                    case Command.JoinGame:

                        _name = _reader.ReadString();
                        //_id = _interpreter.RegisterTransmitter(this);
                        var tx = _interpreter.GetTransmitters();
                        Write(tx.Length);
                        foreach (var t in tx) {
                            
                        }
                        

                        break;
                }
            }
        }

        public void Write(Command cmd) {
            _writer.Write((int)cmd);
        }

        public void Write(string s) {
            _writer.Write(s);
        }

        public void Write(int i) {
            _writer.Write(i);
        }
    }
}
