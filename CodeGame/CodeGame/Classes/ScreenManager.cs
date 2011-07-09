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

namespace CodeGame.Classes {
    class ScreenManager {
        MenuScreen _menu;
        LobbyScreen _lobby;
        GameScreen _game;

        enum ActiveScreen { Menu, Lobby, Game }
        ActiveScreen _active = ActiveScreen.Menu;

        KeyboardState _keyState = new KeyboardState();
        KeyboardState _prevKeyState = new KeyboardState();

        MouseState _mouseState = new MouseState();
        MouseState _prevMouseState = new MouseState();

        public ScreenManager(ContentManager content) {
            _menu = new MenuScreen(this, content);
            _lobby = new LobbyScreen(this, content);
            _game = new GameScreen(this, content);
        }

        public void Update(GameTime gameTime) {
            _keyState = Keyboard.GetState();
            _mouseState = Mouse.GetState();
            switch (_active) {
                case ActiveScreen.Menu:
                    _menu.Update(gameTime);
                    break;
                case ActiveScreen.Lobby:
                    _lobby.Update(gameTime);
                    break;
                case ActiveScreen.Game:
                    _game.Update(gameTime);
                    break;
            }
            _prevKeyState = _keyState;
            _prevMouseState = _mouseState;
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            switch (_active) {
                case ActiveScreen.Menu:
                    _menu.Draw(graphics, batch);
                    break;
                case ActiveScreen.Lobby:
                    _lobby.Draw(graphics, batch);
                    break;
                case ActiveScreen.Game:
                    _game.Draw(graphics, batch);
                    break;
            }
        }

        public bool WasKeyPressed(Keys key) {
            if (_prevKeyState.IsKeyDown(key) && _keyState.IsKeyUp(key))
                return true;
            else
                return false;
        }

        public bool WasMouseClicked(out int x, out int y) {
            x = y = -1;
            if (_prevMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released) {
                x = _mouseState.X;
                y = _mouseState.Y;
                return true;
            }
            else
                return false;
        }
    }
}
