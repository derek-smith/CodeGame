using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using CodeGame.Classes.Utilities;
using CodeGame.Classes.Network;

namespace CodeGame.Classes.Network.Server {
    class Server {
        Queue<Action> _actions = null;
        Object _actionLock = new Object();
        Thread _thread = null;
        List<Transmitter> _netIO = null;
        List<Player> _players = null;

        public Server() {
        }

        public void Start() {
            _netIO = new List<Transmitter>(4);
            _actions = new Queue<Action>();
            _players = new List<Player>(4);

            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop() {
            foreach (Transmitter receiver in _netIO) {
                receiver.Stop();
            }
            _thread.Abort();
        }

        private void Run() {
            Action action = null;

            while (true) {
                lock (_actionLock) {
                    if (_actions.Count > 0) {
                        action = _actions.Dequeue();
                    }
                }

                if (action != null) {
                    switch (action.Command) {

                        case Command.JoinGame:

                            _players.Add(new Player(action.PlayerName));
                            Broadcast(Command.JoinGameSuccess);
                            Broadcast(_players.Count);
                            foreach (Player player in _players) {
                                Broadcast(player.Name);
                            }
                            break;
                    }
                    action = null;
                }
                Thread.Sleep(100);
            }
        }

        private void Broadcast(Command cmd) {
            Broadcast((int)cmd);
        }

        private void Broadcast(int i) {
            foreach (Transmitter netIO in _netIO) {
                //netIO.Send(i);
            }
        }

        private void Broadcast(string s) {
            foreach (Transmitter netIO in _netIO) {
                //netIO.Send(s);
            }
        }

        public void PushConnection(Socket socket) {
            //_netIO.Add(new Transmitter(this, socket));
        }

        public void PushCommand(Action action) {
            lock (_actionLock) {
                _actions.Enqueue(action);
            }
        }

    }

    public class Player {
        private static int NewID = 0;

        int _id = -1;
        string _name = String.Empty;

        public Player(string name) {
            _id = Player.NewID;
            Player.NewID++;
            _name = name;
        }

        public int ID { get { return _id; } }
        public string Name { get { return _name; } }
    }

    public class Action {
        Command _cmd = Command.Null;
        int _playerID = -1;
        string _playerName = String.Empty;
        int _againstID = -1;

        public Action(Command cmd) {
            _cmd = cmd;
        }

        public Action(Command cmd, int playerID) : this(cmd) {
            _playerID = playerID;
        }

        public Action(Command cmd, int playerID, int againstID) : this(cmd, playerID) {
            _againstID = againstID;
        }

        public Action(Command cmd, string playerName) : this(cmd) {
            _playerName = playerName;
        }

        public Command Command { get { return _cmd; } }
        public int PlayerID { get { return _playerID; } }
        public string PlayerName { get { return _playerName; } }
        public int AgainstID { get { return _againstID; } }
    }
}
