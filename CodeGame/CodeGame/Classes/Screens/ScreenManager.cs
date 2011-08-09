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
        Game game;

        //InputManager _input = new InputManager();
        MenuScreen menuScreen;
        LobbyScreen lobbyScreen;
        GameScreen gameScreen;
        //StatusBar _statusBar;
        //NameBox _nameBox;
        //IPBox _ipBox;
        
        Screen activeScreen = Screen.Menu;

        //RenderTarget2D _renderTo;

        StatusBar2 statusBar = null;
        string statusBarStartText = "So you wanna play my game, huh? Swwweeeeeet!";

        public ScreenManager(Game game) {
            this.game = game;
            menuScreen = new MenuScreen(this);
            lobbyScreen = new LobbyScreen(this);
            gameScreen = new GameScreen(this);
            //_statusBar = new StatusBar(_g.Content, "Welcome to Code! the card game.");

            //int width = _g.GraphicsDevice.Viewport.Width;
            //int height = _g.GraphicsDevice.Viewport.Height;
            //_renderTo = new RenderTarget2D(_g.GraphicsDevice, width, height);

            //_nameBox = new NameBox(this);
            //_ipBox = new IPBox(this);

            statusBar = new StatusBar2(this);
            statusBar.Text = statusBarStartText;
        }

        public void Update(GameTime gameTime) {

            //_input.UpdateBegin();
            switch (activeScreen) {
                case Screen.Menu:
                    menuScreen.Update(gameTime);
                    break;
                case Screen.Lobby:
                    lobbyScreen.Update(gameTime);
                    break;
                case Screen.Game:
                    gameScreen.Update(gameTime);
                    break;
            }
            //if (_nameBox.Active)
            //    _nameBox.Update(gameTime);
            //else if (_ipBox.Active)
            //    _ipBox.Update(gameTime);

            //_input.UpdateEnd();
            
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            //_g.GraphicsDevice.SetRenderTarget(_renderTo);
            
            switch (activeScreen) {
                case Screen.Menu:
                    menuScreen.Draw(graphics, batch);
                    break;
                case Screen.Lobby:
                    lobbyScreen.Draw(graphics, batch);
                    break;
                case Screen.Game:
                    gameScreen.Draw(graphics, batch);
                    break;
            }

            batch.Begin();
            statusBar.Draw(batch);
            batch.End(); 

            //_statusBar.Draw(batch);
            //batch.End();

            //_g.GraphicsDevice.SetRenderTarget(null);

            //batch.Begin();
            //batch.Draw(_renderTo, Vector2.Zero, Color.White);

            //if (_nameBox.Active)
            //    _nameBox.Draw(batch);
            //else if (_ipBox.Active)
            //    _ipBox.Draw(batch);

            //batch.End();
        }

        //public InputManager InputManager { get { return _input; } }
        public ContentManager Content { get { return game.Content; } }
        public StatusBar2 StatusBar { get { return statusBar; } }
        public Game Game { get { return game; } }
        //public NameBox NameBox { get { return _nameBox; } }
        public MenuScreen MenuScreen { get { return menuScreen; } }
        public LobbyScreen LobbyScreen { get { return lobbyScreen; } }
        public GameScreen GameScreen { get { return gameScreen; } }
        //public IPBox IPBox { get { return _ipBox; } }
        public int Width { get { return game.GraphicsDevice.Viewport.Width; } }
        public int Height { get { return game.GraphicsDevice.Viewport.Height; } }

        public void Close() {
            game.Exit();
        }

        // Change from Menu to Lobby
        public void ChangeToLobbyScreen(string nick) {
            //_lobby.ChangeFromMenuScreen("Waiting for players...", username);
            statusBar.Text = "Waiting for players...";
            lobbyScreen.ChangeFromMenuScreen(nick);
            activeScreen = Screen.Lobby;
        }

        // Change from Lobby to Menu
        public void ChangeToMenuScreen() {
            //_statusBar.Text = "Welcome to Code!";
            statusBar.Text = statusBarStartText;
            activeScreen = Screen.Menu;
        }
    }
}
