// This is a helper class for event sharing in a
// non-event-based environment.

using System;
using System.Collections.Generic;       // List, Queue support
using System.Text;

namespace CodeGame.Classes.Network {
    public enum Command {
        Null, JoinGame, JoinGameSuccess, ReadyYes, ReadyNo, PlayerJoin, PlayerExit, GameBegin, DrawCard, YourTurn, PlayersTurn, CardPlayed
    }





    class Commands {
        // This signals that something has changed.
        public volatile bool HasSomethingChanged = false;
        // This is used for multi-threaded locking.
        private object _key = new object();
        // This holds all player names, max = 4 (first name is always the host).
        private List<string> _playerNames = new List<string>(4);


        private Queue<Command> _commands = new Queue<Command>();

        public Commands() {
        }

        // This method is for the network-side classes.
        public void AddPlayerName(string name) {
            lock (_key) {
                _playerNames.Add(name);
                _commands.Enqueue(Command.JoinGame);
            }
            HasSomethingChanged = true;
        }

        // This method is for the game-side classes.
        public string[] GetPlayerNames() {
            lock (_key) {
                return _playerNames.ToArray();
            }
        }

        public bool HasCommands() {
            lock (_key) {
                if (_commands.Count > 0) {
                    return true;
                }
                else {
                    HasSomethingChanged = false;
                    return false;
                }
            }
        }

        public Command GetNextCommand() {
            lock (_key) {
                return _commands.Dequeue();
            }
        }
    }
}
