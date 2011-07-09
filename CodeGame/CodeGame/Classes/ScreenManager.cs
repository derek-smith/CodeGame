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
        ActiveScreen active = ActiveScreen.Menu;

        public ScreenManager(ContentManager content) {
            _menu = new MenuScreen(this, content);
            _lobby = new LobbyScreen(this, content);
            _game = new GameScreen(this, content);
        }

        public void Update(GameTime gameTime) {
            switch (active) {
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
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            switch (active) {
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
    }
}
