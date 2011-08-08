﻿using System;
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
        ScreenManager _manager;
        string _username;
        SpriteFont _font;
        //Button _backButton;
        Listener _listener;
        Client _client;
        Commands _commands;
        string[] _playerNames = new string[0];

        public LobbyScreen(ScreenManager manager) {
            _manager = manager;
            _font = _manager.Content.Load<SpriteFont>("MenuFont");
            //_backButton = new Button(_manager, "Back-Normal", new Vector2(20, 510), Color.Cyan, false);
        }

        public void ChangeFromMenuScreen(string status, string username) {
            _manager.StatusBar.Text = status;
            _username = username;
            _listener = new Listener();
            _listener.Start();
        }

        public void Update(GameTime gameTime) {
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
            graphics.Clear(Color.CornflowerBlue);
            batch.Begin();
            for (int i = 0; i < _playerNames.Length; i++) {
                batch.DrawString(_font, _playerNames[i], new Vector2(20, (i * 30) + 90), Color.White);
            }
            

            //batch.DrawString(_font, _username, new Vector2(20, 90), Color.White);
            //_backButton.Draw(batch);
            // Leave this out - is called in ScreenManager
            //batch.End();
        }
    }
}
