using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace CodeGame.Classes.Network.Server {
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
