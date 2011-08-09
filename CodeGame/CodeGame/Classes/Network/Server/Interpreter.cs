using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace CodeGame.Classes.Network.Server {

    class Interpreter {
        // This is used to lock down the thread
        // so only 1 transmitter at a time can
        // "register an event"
        Object theInterpreter = new Object();

        // Need all the transmitters in order to send commands and broadcast
        List<Reader> readers = new List<Reader>(4);

        // Keep track of nicknames
        List<string> nicks = new List<string>(4);

        // Keep track of who is ready (only used while on the lobby screen)
        List<bool> ready = new List<bool>(4);

        public Interpreter() {
        }

        // No lock here because it is assumed
        // the calling method has applied one
        private void Broadcast(int int32) {
            for (int i = 0; i < readers.Count; i++) {
                readers[i].W.Write(int32);
            }
        }
        private void Broadcast(string s) {
            for (int i = 0; i < readers.Count; i++) {
                readers[i].W.Write(s);
            }            
        }

        public int JoinGame(Reader reader, string nick) {
            lock (theInterpreter) {
                // Save for later use
                readers.Add(reader);
                nicks.Add(nick);
                ready.Add(false);

                // Sucess
                reader.W.Write((int)Command.JoinGameSuccess);
                // Send player count to new player
                reader.W.Write(readers.Count);

                // Assumption: nicks.Count == transmitters.Count
                for (int i = 0; i < readers.Count; i++) {
                    // Send the new player all nicks (including their own)
                    reader.W.Write(nicks[i]);

                    // And to all other players, skipping the new player...
                    if (readers[i] != reader) {
                        // Tell them who the new player is
                        readers[i].W.Write((int)Command.PlayerJoin);
                        readers[i].W.Write(nick);
                    }
                }

                return (readers.Count - 1);
            }
        }

        public void Ready(int id) {

            lock (theInterpreter) {
                // Toggle ready
                ready[id] = !ready[id];

                // Broadcast result of the toggle
                if (ready[id])
                    Broadcast((int)Command.ReadyYes);
                else
                    Broadcast((int)Command.ReadyNo);

                // This is the player that toggled
                Broadcast(id);
            }
        }

        //public void ReadyNo(int id) {
        //    lock (theInterpreter) {
        //        ready[id] = false;
        //        Broadcast((int)Command.ReadyNo);
        //        Broadcast(nicks[id]);
        //    }
        //}
    }
}
