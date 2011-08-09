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
        string _username;
        SpriteFont font;
        //Button _backButton;
        Listener _listener;
        Client _client;
        Commands _commands;
        string[] _playerNames = new string[0];

        MouseState mouse = new MouseState();
        MouseState prevMouse = new MouseState();
        Button btnBack = null;
        List<string> nicks = new List<string>(4);

        public LobbyScreen(ScreenManager mgr) {
            this.mgr = mgr;
            font = mgr.Content.Load<SpriteFont>("ButtonFont");
            //_backButton = new Button(_manager, "Back-Normal", new Vector2(20, 510), Color.Cyan, false);

            btnBack = new Button(mgr, new Vector2(40, 600 - 40 - 49), "Back");
        }

        public void ChangeFromMenuScreen(string nick) {
            nicks.Add(nick);
            //_manager.StatusBar.Text = status;
            //_username = username;
            //_listener = new Listener();
            //_listener.Start();
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



            //if (_serverShared.HasSomethingChanged) {
            //    while (_serverShared.HasCommands()) {
            //        switch (_serverShared.GetNextCommand()) {
            //            case Network.Command.NewPlayer:
            //                _playerNames = _serverShared.GetPlayerNames();
            //                break;
            //        }
            //    }
            //}

            //_backButton.Update();

            //if (_backButton.IsClicked()) {
            //    _listener.Stop();
            //    _manager.ChangeToMenuScreen();
            //    return;
            //}
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            graphics.Clear(Color.Black);

            batch.Begin();
            for (int i = 0; i < nicks.Count; i++) {
                batch.DrawString(font, nicks[i], new Vector2(40, (i * 30) + 90), Color.White);
            }

            btnBack.Draw(batch);

            batch.End();

            //batch.DrawString(_font, _username, new Vector2(20, 90), Color.White);
            //_backButton.Draw(batch);
            // Leave this out - is called in ScreenManager
            //batch.End();
        }

        private bool HoveringOver(Button btn) {
            if (mouse.X >= btn.X1 && mouse.X <= btn.X2 && mouse.Y >= btn.Y1 && mouse.Y <= btn.Y2)
                return true;
            else
                return false;
        }

        private bool ClickedOn(Button btn) {
            //if (mouse.LeftButton == ButtonState.Pressed &&
            //    prevMouse.LeftButton == ButtonState.Released)
            //    return true;
            //else
            //    return false;

            if (prevMouse.LeftButton == ButtonState.Pressed &&
                mouse.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }
    }
}
