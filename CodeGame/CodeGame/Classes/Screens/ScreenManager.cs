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
    enum Screen { Menu, Lobby, Game }

    class ScreenManager {
        Game _gameObject;

        InputManager _input = new InputManager();
        MenuScreen _menu;
        LobbyScreen _lobby;
        GameScreen _game;
        StatusBar _statusBar;
        
        Screen _activeScreen = Screen.Menu;

        //KeyboardState _keyState = new KeyboardState();
        //KeyboardState _prevKeyState = new KeyboardState();

        //MouseState _mouseState = new MouseState();
        //MouseState _prevMouseState = new MouseState();

        public ScreenManager(Game game) {
            _gameObject = game;
            _menu = new MenuScreen(this);
            _lobby = new LobbyScreen(this);
            _game = new GameScreen(this);
            _statusBar = new StatusBar(_gameObject.Content, "Welcome to Code!");
        }

        public void Update(GameTime gameTime) {
            //_keyState = Keyboard.GetState();
            //_mouseState = Mouse.GetState();

            _input.UpdateBegin();
            switch (_activeScreen) {
                case Screen.Menu:
                    _menu.Update(gameTime);
                    break;
                case Screen.Lobby:
                    _lobby.Update(gameTime);
                    break;
                case Screen.Game:
                    _game.Update(gameTime);
                    break;
            }
            _input.UpdateEnd();

            //_prevKeyState = _keyState;
            //_prevMouseState = _mouseState;
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            switch (_activeScreen) {
                case Screen.Menu:
                    _menu.Draw(graphics, batch);
                    break;
                case Screen.Lobby:
                    _lobby.Draw(graphics, batch);
                    break;
                case Screen.Game:
                    _game.Draw(graphics, batch);
                    break;
            }
            _statusBar.Draw(batch);
            batch.End();
        }

        public InputManager InputManager { get { return _input; } }
        public ContentManager ContentManager { get { return _gameObject.Content; } }
        public StatusBar StatusBar { get { return _statusBar; } }

        public void Exit() {
            _gameObject.Exit();
        }

        public void ChangeToLobbyScreen(string username) {
            _lobby.ChangeFromMenuScreen("Waiting for players...", username);
            _activeScreen = Screen.Lobby;
        }

        public void ChangeToMenuScreen() {
            _statusBar.Text = "Welcome to Code!";
            _activeScreen = Screen.Menu;
        }
    }
}
