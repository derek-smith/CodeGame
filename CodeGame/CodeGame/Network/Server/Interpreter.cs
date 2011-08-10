using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace CodeGame.Network.Server {

    class Interpreter {
        // This is used to lock down the thread
        // so only 1 reader (each one with it's own thread)
        // at a time can use this object
        Object theInterpreter = new Object();

        // Need all the transmitters in order to send commands and broadcast
        //List<Reader> readers = new List<Reader>(4);

        // Keep track of nicknames
        //List<string> nicks = new List<string>(4);

        // Keep track of who is ready (only used while on the lobby screen)
        //List<bool> ready = new List<bool>(4);

        List<Player> players = new List<Player>(4);

        public Interpreter() {
        }

        // No lock here because it is assumed
        // the calling method has applied one
        private void Broadcast(int cmd) {
            for (int i = 0; i < players.Count; i++) {
                players[i].Write(cmd);
            }
        }
        private void Broadcast(string s) {
            for (int i = 0; i < players.Count; i++) {
                players[i].Write(s);
            }            
        }

        public int JoinGame(Reader reader, string nick) {
            lock (theInterpreter) {
                // Save for later use
                //readers.Add(reader);
                //nicks.Add(nick);
                //ready.Add(false);

                players.Add(new Player(reader, nick));

                // New player's ID
                int n = players.Count - 1;

                players[n].Write((int)Command.JoinGameSuccess);
                players[n].Write(players.Count);

                // Sucess
                //reader.W.Write((int)Command.JoinGameSuccess);
                // Send player count to new player
                //reader.W.Write(readers.Count);

                for (int i = 0; i < players.Count; i++) {
                    // Send the new player all nicks (including their own)
                    //reader.W.Write(nicks[i]);

                    players[n].Write(players[i].Nick);
                    players[n].Write(players[i].IsReady);

                    if (players[n] != players[i]) {
                        players[i].Write((int)Command.PlayerJoin);
                        players[i].Write(players[n].Nick);
                    }

                    // And to all other players, skipping the new player...
                    //if (readers[i] != reader) {
                        // Tell them who the new player is
                        //readers[i].W.Write((int)Command.PlayerJoin);
                        //readers[i].W.Write(nick);
                    //}
                }

                return (players.Count - 1); // ID (index)
            }
        }

        public void Ready(int id) {
            lock (theInterpreter) {
                // Toggle ready
                //ready[id] = !ready[id];
                players[id].IsReady = !players[id].IsReady;

                // Broadcast result of the toggle
                if (players[id].IsReady)
                    Broadcast((int)Command.ReadyYes);
                else
                    Broadcast((int)Command.ReadyNo);

                // This is the player that toggled
                Broadcast(id);
            }
        }

        public void GameBeginRequest() {
            lock (theInterpreter) {
                Broadcast((int)Command.GameBegin);
            }
        }


    }
}
