using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace CodeGame.Network.Server {
    
    class Player {

        Reader reader = null;
        string nick = "defaultNick";
        bool isReady = false;
        List<Card> cards = new List<Card>(6);

        public string Nick { get { return nick; } }
        public bool IsReady { get { return isReady; } set { isReady = value; } }
        public Card[] Cards { get { return cards.ToArray(); } set { cards = new List<Card>(value); } }        

        public Player(Reader reader, string nick) {
            this.reader = reader;
            this.nick = nick;
        }

        public void Write(int i) {
            reader.W.Write(i);
        }

        public void Write(string s) {
            reader.W.Write(s);
        }

        public void Write(bool b) {
            reader.W.Write(b);
        }
    }
}
