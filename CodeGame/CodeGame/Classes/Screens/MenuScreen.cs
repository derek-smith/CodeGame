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

namespace CodeGame.Classes.Screens {
    class MenuScreen {
        ScreenManager _manager;
        Button _hostButton, _joinButton, _quitButton, _changeButton;
        Textbox _playerName;
        SpriteFont _menuFont;

        public MenuScreen(ScreenManager manager) {
            _manager = manager;
            _hostButton = new Button(_manager, "Host-Normal", new Vector2(10, 10), Color.Red);
            _joinButton = new Button(_manager, "Join-Normal", new Vector2(10, 110), Color.Blue);
            _quitButton = new Button(_manager, "Quit-Normal", new Vector2(10, 210), Color.Purple);
            _changeButton = new Button(_manager, "Change-Normal", new Vector2(350, 110), Color.Yellow);
            _menuFont = _manager.ContentManager.Load<SpriteFont>("MenuFont");
            _playerName = new Textbox(_manager, _menuFont, "PlayerName", new Vector2(350, 20), Color.White);
        }

        public void Update(GameTime gameTime) {
            _hostButton.Update();
            _joinButton.Update();
            _quitButton.Update();
            _changeButton.Update();

            if (_quitButton.IsClicked()) {
                _manager.Exit();
            }
            else if (_changeButton.IsClicked()) {
                _playerName.IsEditMode = !_playerName.IsEditMode;
            }
            else if (_hostButton.IsClicked()) {
                _manager.ChangeToLobbyScreen(_playerName.Text);
                return;
            }

            _playerName.Update(gameTime);
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            graphics.Clear(Color.CornflowerBlue);
            batch.Begin();
            _hostButton.Draw(batch);
            _joinButton.Draw(batch);
            _quitButton.Draw(batch);
            _playerName.Draw(batch);
            _changeButton.Draw(batch);
            batch.End();
        }
    }
}
