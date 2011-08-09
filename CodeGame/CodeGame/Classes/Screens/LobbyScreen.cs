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

        List<string> nicks = new List<string>(4);
        List<bool> ready = new List<bool>(4);

        public LobbyScreen(ScreenManager mgr) {
            this.mgr = mgr;
            font = mgr.Content.Load<SpriteFont>("MainFont");

            btnBack = new Button(mgr, new Vector2(40, 600 - 40 - 49), "Back");
        }

        public void ChangeFromMenuScreen(string nick) {
            // start client (and server, if host)
        }

        public void PlayerJoin(string nick) {
            nicks.Add(nick);
        }

        public void ReadyYes(int id) {
            ready[id] = true;
        }

        public void ReadyNo(int id) {
            ready[id] = false;
        }

        public void Update(GameTime gameTime) {

            mouse = Mouse.GetState();

            if (HoveringOver(btnBack)) {
                if (ClickedOn(btnBack)) {
                    ; // go back to Menu screen
                }
                else {
                    btnBack.IsHover = true;
                }
            }
            else {
                btnBack.IsHover = false;
            }
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            // Clear screen to black
            graphics.Clear(Color.Black);

            batch.Begin();
            
            // Draw nicknames
            for (int i = 0; i < nicks.Count; i++) {
                batch.DrawString(font, nicks[i], new Vector2(40, (i * 30) + 90), Color.White);
            }

            // Let back button draw itself
            btnBack.Draw(batch);

            batch.End();
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
