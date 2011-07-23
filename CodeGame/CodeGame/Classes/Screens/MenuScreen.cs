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
        ScreenManager _screen;
        Button _hostButton, _joinButton, _quitButton, _changeButton;
        SpriteFont _menuFont;

        string _name;
        string _lblName;
        Vector2 _lblNamePosition = new Vector2(350, 90);
        string _ipAddress;

        public string PlayerName { get { return _name; } set { _name = value; _lblName = "Name: " + _name; } }
        public string IPAddress { get { return _ipAddress; } set { _ipAddress = value; } }

        public MenuScreen(ScreenManager screen) {

            PlayerName = "dsmith";
            IPAddress = "127.0.0.1";

            _screen = screen;
            _hostButton = new Button(_screen, "Host-Normal", new Vector2(20, 80), Color.Red, false);
            _joinButton = new Button(_screen, "Join-Normal", new Vector2(20, 160), Color.Blue, false);
            _quitButton = new Button(_screen, "Quit-Normal", new Vector2(20, 510), Color.Purple, false);
            _changeButton = new Button(_screen, "Change-Normal", new Vector2(350, 160), Color.Yellow, false);
            _menuFont = _screen.ContentManager.Load<SpriteFont>("MenuFont");
        }

        public void Update(GameTime gameTime) {
            UpdateControls();
            HandleButtonActions();
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            graphics.Clear(Color.CornflowerBlue);
            batch.Begin();

            _hostButton.Draw(batch);
            _joinButton.Draw(batch);
            _quitButton.Draw(batch);
            _changeButton.Draw(batch);
            batch.DrawString(_menuFont, _lblName, _lblNamePosition, Color.Black);

            // Leave this out - is called in ScreenManager
            //batch.End();
        }

        private void UpdateControls() {
            _hostButton.Update();
            _joinButton.Update();
            _quitButton.Update();
            _changeButton.Update();
        }

        private void HandleButtonActions() {
            if (_quitButton.IsClicked()) {
                _screen.Close();
            }
            else if (_changeButton.IsClicked()) {
                _screen.NameBox.Show();
            }
            else if (_hostButton.IsClicked()) {
                _screen.ChangeToLobbyScreen(_name);
            }
            else if (_joinButton.IsClicked()) {
                _screen.IPBox.Show();
            }
        }
    }
}
