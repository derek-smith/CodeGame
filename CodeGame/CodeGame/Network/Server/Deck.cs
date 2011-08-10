using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CodeGame.Controls;

namespace CodeGame.Network.Server {

    class Deck {
        Queue<Card> deck = new Queue<Card>(); // 51

        public Deck() {

            for (int i = 0; i < 10; i++)
                deck.Enqueue(Card.Code1);

            for (int i = 0; i < 10; i++)
                deck.Enqueue(Card.Code2);

            for (int i = 0; i < 6; i++)
                deck.Enqueue(Card.Code3);

            for (int i = 0; i < 3; i++)
                deck.Enqueue(Card.Code4);

            deck.Enqueue(Card.Code6);

            for (int i = 0; i < 5; i++)
                deck.Enqueue(Card.Bug);

            for (int i = 0; i < 5; i++)
                deck.Enqueue(Card.Debug);

            for (int i = 0; i < 5; i++)
                deck.Enqueue(Card.Hacker);

            for (int i = 0; i < 3; i++)
                deck.Enqueue(Card.Crash);

            for (int i = 0; i < 2; i++)
                deck.Enqueue(Card.SickDay);

            deck.Enqueue(Card.Virus);

            Shuffle();
        }

        public void Shuffle() {
            List<Card> cards = new List<Card>(deck);
            Queue<Card> newDeck = new Queue<Card>(deck.Count);
            Random r = new Random(DateTime.Now.Millisecond);

            int n = -1;

            while (cards.Count > 0) {
                n = r.Next(cards.Count);
                newDeck.Enqueue(cards[n]);
                cards.RemoveAt(n);
            }

            deck = newDeck;
        }

        public Card GetNextCard() {
            return deck.Dequeue();
        }

        public int Count() {
            return deck.Count;
        }
    }
}
