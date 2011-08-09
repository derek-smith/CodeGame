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
using CodeGame.Classes.Input;
using CodeGame.Classes.Network;
using CodeGame.Classes.Network.Client;
using CodeGame.Classes.Network.Server;

namespace CodeGame.Classes.Screens {
    class LobbyScreen {
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

        public LobbyScreen(ScreenManager mgr) {
            this.mgr = mgr;
            font = mgr.Content.Load<SpriteFont>("MainFont");

            btnBack = new Button(mgr, new Vector2(40, 600 - 40 - 49), "Back");
            btnReady = new Button(mgr, new Vector2(mgr.Width - 40 - Button.Width, 600 - 40 - 49), "Ready");
            btnStart = new Button(mgr, new Vector2(mgr.Width - (40 * 2) - (Button.Width * 2), 600 - 40 - 49), "Start");
        }

        public void SetClient(Client client) {
            this.client = client;

            if (mgr.MenuScreen.IsHost)
                showStartButton = true;
        }

        //
        // PlayerJoin
        //

        public void PlayerJoin(string nick) {
            nicks.Add(nick);
            ready.Add(false);
            showReadyButton = true;
        }

        //
        // ReadyYes
        //

        public void ReadyYes(int id) {
            ready[id] = true;
        }

        //
        // ReadyNo
        //

        public void ReadyNo(int id) {
            ready[id] = false;
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

            else if (showStartButton && AllPlayersReady() && HoveringOver(btnStart)) {
                if (ClickedOn(btnStart)) {
                    // Clicked
                    ;
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
            
            // Draw nicknames
            for (int i = 0; i < nicks.Count; i++) {
                batch.DrawString(font, nicks[i], new Vector2(40, (i * 30) + 90), Color.White);
            }

            // Draw status (if ready or not)
            for (int i = 0; i < ready.Count; i++) {
                Color color = Color.Yellow;
                if (!ready[i]) color = Color.Gray;

                batch.DrawString(font, "Ready", new Vector2(250, (i * 30) + 90), color);
            }

            // Let Back button draw itself
            btnBack.Draw(batch);

            if (showReadyButton) {
                btnReady.Draw(batch);
            }

            if (showStartButton && AllPlayersReady()) {
                btnStart.Draw(batch);
            }


            batch.End();
        }

        //
        // Helper methods
        //

        private bool AllPlayersReady() {
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
