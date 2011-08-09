using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace CodeGame.Classes.Network.Server {

    class Interpreter2 {
        // This is used to lock down the thread
        // so only 1 transmitter at a time can
        // "register an event"
        Object theInterpreter = new Object();

        // Need all the transmitters in order to send commands and broadcast
        List<Transmitter2> transmitters = new List<Transmitter2>(4);

        // Keep track of nicknames
        List<string> nicks = new List<string>(4);

        // Keep track of who is ready (only used while on the lobby screen)
        List<bool> ready = new List<bool>(4);

        public Interpreter2() {
        }

        // No lock here because it is assumed
        // the calling method has applied one
        private void Broadcast(int int32) {
            for (int i = 0; i < transmitters.Count; i++) {
                transmitters[i].W.Write(int32);
            }
        }
        private void Broadcast(string s) {
            for (int i = 0; i < transmitters.Count; i++) {
                transmitters[i].W.Write(s);
            }            
        }

        public int JoinGame(Transmitter2 transmitter, string nick) {
            lock (theInterpreter) {
                // Save for later use
                transmitters.Add(transmitter);
                nicks.Add(nick);

                // Send player count to new player
                transmitter.W.Write(transmitters.Count);
                // Assumption: nicks.Count == transmitters.Count
                for (int i = 0; i < transmitters.Count; i++) {
                    // Send the new player all nicks (including their own)
                    transmitter.W.Write(nicks[i]);

                    // And to all other players, skipping the new player...
                    if (transmitters[i] != transmitter) {
                        // Tell them who the new player is
                        transmitters[i].W.Write((int)Command.PlayerJoin);
                        transmitters[i].W.Write(nick);
                    }
                }

                return (transmitters.Count - 1);
            }
        }

        public void ReadyYes(int id) {
            ready[id] = true;
            Broadcast((int)Command.ReadyYes);
            Broadcast(nicks[id]);
        }

        public void ReadyNo(int id) {
            ready[id] = false;
            Broadcast((int)Command.ReadyNo);
            Broadcast(nicks[id]);
        }
    }



    class Interpreter {
        static int _id = -1;
        
        Object _theInterpreter = new Object();
        
        List<Transmitter> _transmitters = new List<Transmitter>(4);

        public Interpreter() {
        }

        public void RegisterTransmitter(Transmitter transmitter) {
            lock (_theInterpreter) {
                _id++;
                _transmitters.Add(transmitter);
            }
        }

        public Transmitter[] GetTransmitters() {
            lock (_theInterpreter) {
                return _transmitters.ToArray();
            }
        }

    }
}
