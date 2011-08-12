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
using CodeGame.Network;

namespace CodeGame.Screens {
    class LobbyScreen {
        const bool ALWAYS_SHOW_START_BUTTON = true;

        ScreenManager mgr;
        SpriteFont font;

        MouseState mouse = new MouseState();
        MouseState prevMouse = new MouseState();

        Button btnBack = null;
        Button btnReady = null;
        Button btnStart = null;

        List<string> nicks = new List<string>(4);
        List<bool> ready = new List<bool>(4);

        bool showReadyButton = false;
        bool showStartButton = false;

        Client client = null;
        
        public string[] Nicks { get { return nicks.ToArray(); } }

        // HACK: Need a dynamic way of getting this info
        const int cardWidth = 80;
        const int cardHeight = 80;

        public LobbyScreen(ScreenManager mgr) {
            this.mgr = mgr;
            font = mgr.Content.Load<SpriteFont>("MainFont");

            btnBack = new Button("Back", new Vector2(20, 600 - 40 - 49));
            btnReady = new Button("Ready", new Vector2(mgr.Width - 40 - Button.Width, 600 - 20 - 49));
            btnStart = new Button("Start", new Vector2(mgr.Width - (40 * 2) - (Button.Width * 2), 600 - 20 - 49));
        }

        public void SetClient(Client client) {
            this.client = client;

            //if (mgr.MenuScreen.IsHost)
                //showStartButton = true;
        }

        //
        // PlayerJoin
        //

        public void PlayerJoin(string nick, bool ready) {
            nicks.Add(nick);
            this.ready.Add(ready);
            showReadyButton = true;
        }

        //
        // ReadyYes
        //

        public void ReadyYes(int id) {
            ready[id] = true;

            if (AllPlayersReady()) {
                if (mgr.MenuScreen.IsHost) showStartButton = true;
                mgr.StatusBar.Color = StatusBar.LobbyColorReady;
            }
        }

        //
        // ReadyNo
        //

        public void ReadyNo(int id) {
            ready[id] = false;

            if (mgr.MenuScreen.IsHost) showStartButton = false;
            mgr.StatusBar.Color = StatusBar.LobbyColor;
        }

        public void GameBegin() {
            mgr.GameScreen.Start();
            mgr.ChangeToGameScreen();
        }

        //
        // Update
        //

        public void Update(GameTime gameTime) {

            // Save the mouse's state
            mouse = Mouse.GetState();

            //
            // Back button
            //

            if (HoveringOver(btnBack)) {
                if (ClickedOn(btnBack)) {
                    // Clicked
                    ; // go back to Menu screen
                }
                else {
                    // Hovering
                    btnBack.IsHover = true;
                }
            }

            //
            // Ready button
            //

            else if (showReadyButton && HoveringOver(btnReady)) {
                if (ClickedOn(btnReady)) {
                    // Clicked
                    client.Ready();
                }
                else {
                    // Hovering
                    btnReady.IsHover = true;
                }
            }

            //
            // Start button
            //

            else if (showStartButton /* && AllPlayersReady() */ && HoveringOver(btnStart)) {
                if (ClickedOn(btnStart)) {
                    // Clicked
                    client.GameBeginRequest();
                }
                else {
                    // Hovering
                    btnStart.IsHover = true;
                }
            }


            //
            // Else
            //

            else {
                // Neither clicking nor hovering
                // so lets reset the hover state
                btnBack.IsHover = false;
                btnReady.IsHover = false;
                btnStart.IsHover = false;
            }

            // Save the mouse's state for next Update()
            prevMouse = mouse;
        }

        //
        // Draw
        //

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            // Clear screen to black
            graphics.Clear(Color.Black);

            batch.Begin();

            for (int i = 0; i < nicks.Count; i++) {
                batch.DrawString(font, nicks[i], new Vector2(20, (i * (cardHeight + 4)) + 77), Color.White);
            }

            for (int i = 0; i < nicks.Count; i++) {
                batch.DrawString(font, "Ready", new Vector2(20, (i * (cardHeight + 4)) + 100), ready[i] ? Color.Yellow : Color.Gray);
            }

            // Let Back button draw itself
            btnBack.Draw(batch);

            if (showReadyButton) {
                btnReady.Draw(batch);
            }

            if (showStartButton /* && AllPlayersReady() */) {
                btnStart.Draw(batch);
            }


            batch.End();
        }

        //
        // Helper methods
        //

        private bool AllPlayersReady() {
            if (ALWAYS_SHOW_START_BUTTON) return true;

            if (ready.Count < 2) return false;

            for (int i = 0; i < ready.Count; i++) {
                if (!ready[i]) return false;
            }

            return true;
        }

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
