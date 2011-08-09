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

        public Interpreter2() {
        }

        private void Broadcast() {

        }

        public void JoinGame(Transmitter2 transmitter, string nick) {
            lock (theInterpreter) {
                transmitters.Add(transmitter);
                nicks.Add(nick);
            }
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
