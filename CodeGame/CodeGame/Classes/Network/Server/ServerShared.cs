using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGame.Classes.Network.Server {
    class ServerShared {
        bool _isChanged = false;
        string _serverName = "dsmith";
        List<string> _clientNames = new List<string>(3);

        public ServerShared() {

        }

        public void AddClientName(string name) {
            _clientNames.Add(name);
            _isChanged = true;
        }

        public bool IsChanged { get { return _isChanged; } }
    }
}
