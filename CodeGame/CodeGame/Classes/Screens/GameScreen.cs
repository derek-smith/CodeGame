using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CodeGame.Classes.Screens {

    enum CardType { Null, Code, Bug, Debug, Hacker, Crash, SickDay, Virus }

    class Card {
        CardType type = CardType.Null;
        // Lines of code (only used if type == CardType.Code)
        int loc = -1;

        public CardType Type { get { return type; } }
        public int LOC { get { return loc; } }

        public Card(CardType type) {
            this.type = type;
        }

        public Card(int loc) {
            type = CardType.Code;
            this.loc = loc;
        }
    }

    class Deck {
        Card[] deck = new Card[52];

        public Deck() {

            int i = 0;

            for (; i < 10; i++)
                deck[i] = new Card(1);

            for (; i < 20; i++)
                deck[i] = new Card(2);



            Shuffle();
        }

        public void Shuffle() {

        }
    }

    class GameScreen {

        ScreenManager mgr;

        string[] nicks = null;
        Card[,] publicCards = null;
        Card[] privateCards = null;

        public GameScreen(ScreenManager mgr) {
            this.mgr = mgr;
            nicks = mgr.LobbyScreen.Nicks;

            publicCards = new Card[nicks.Length, 5];
            privateCards = new Card[6];
        }

        public void Update(GameTime gameTime) {

        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            
            graphics.Clear(Color.CornflowerBlue);

            batch.Begin();



            batch.End();
        }
    }
}
