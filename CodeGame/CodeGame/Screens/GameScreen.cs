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

using CodeGame.Controls;

namespace CodeGame.Screens {

    class GameScreen {

        ScreenManager mgr = null;
        SpriteFont font = null;

        Chat chatBox = null;        
        Button btnDiscard = null;

        string[] nicks = null;
        int[] scores = null;
        // Public cards
        Card[,] pubCards = null;

        // Private cards
        Card[] priCards = new Card[6];

        Texture2D[] cardTextures = null;
        int cardWidth = -1;
        int cardHeight = -1;

        MouseState mouse = new MouseState();
        MouseState prevMouse = new MouseState();

        Texture2D activeTexture = null;
        Vector2 activePosition = new Vector2(20, 75);
        int activeWidth = 153;
        int activeHeight = 50;

        public GameScreen(ScreenManager mgr) {
            this.mgr = mgr;
            font = mgr.Content.Load<SpriteFont>("MainFont");

            chatBox = new Chat(mgr);

            Color[] pixels = new Color[activeWidth * activeHeight];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.White;
            }
            activeTexture = new Texture2D(mgr.Game.GraphicsDevice, activeWidth, activeHeight);
            activeTexture.SetData<Color>(pixels);
            
            // enum Card { None, Code1, Code2, Code3, Code4, Code6 = 6, Bug, Debug, Hacker, Crash, SickDay, Virus }

            List<Texture2D> textures = new List<Texture2D>(13);

            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Blank"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Code1"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Code2"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Code3"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Code4"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Blank"));   // Not used
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Code6"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Bug"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Debug"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Hacker"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Crash"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\SickDay"));
            textures.Add(mgr.Content.Load<Texture2D>(@"Cards\Virus"));

            cardTextures = textures.ToArray();

            cardWidth = cardTextures[0].Width;
            cardHeight = cardTextures[0].Height;

            btnDiscard = new Button("Discard", new Vector2(20, 600 - 20 - cardHeight + 17));
        }

        // TODO: Reset everything for a brand new use
        public void Start() {
            nicks = mgr.LobbyScreen.Nicks;
            scores = new int[nicks.Length];
            pubCards = new Card[nicks.Length, 5];
        }

        public void Update(GameTime gameTime) {

            mouse = Mouse.GetState();

            if (HoveringOver(btnDiscard)) {
                btnDiscard.IsHover = true;
            }
            else {
                btnDiscard.IsHover = false;
            }

            prevMouse = mouse;
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            
            graphics.Clear(Color.Black);

            batch.Begin();

            batch.Draw(activeTexture, activePosition, Color.Green * 0.5f);

            for (int i = 0; i < nicks.Length; i++) {
                batch.DrawString(font, nicks[i], new Vector2(25, (i * (cardHeight + 4)) + 77), Color.White * 0.8f);
                batch.DrawString(font, scores[i].ToString(), new Vector2(25, (i * (cardHeight + 4)) + 100), Color.White);
            }

            for (int i = 0; i < nicks.Length; i++) {
                for (int n = 0; n < 5; n++) {
                    batch.Draw(cardTextures[(int)pubCards[i, n]], new Vector2(193 + (n * cardWidth) + (n * 4), 60 + (i * cardHeight) + (i * 4)), Color.White * 0.7f);
                }
            }

            for (int n = 0; n < 6; n++) {
                batch.Draw(cardTextures[(int)priCards[n]], new Vector2(193 + (n * cardWidth) + (n * 4), 600 - 20 - cardHeight), Color.White * 0.7f);
            }

            chatBox.Draw(batch);
            btnDiscard.Draw(batch);

            batch.End();
        }

        //
        // Helper methods
        //

        private bool HoveringOver(Button btn) {
            if (mouse.X >= btn.X1 && mouse.X <= btn.X2 && mouse.Y >= btn.Y1 && mouse.Y <= btn.Y2)
                return true;
            else
                return false;
        }

        private bool ClickedOn(Button btn) {
            if (prevMouse.LeftButton == ButtonState.Pressed &&
                mouse.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }
    }
}
