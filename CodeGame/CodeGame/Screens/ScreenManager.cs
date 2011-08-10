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

using CodeGame.Controls;
using CodeGame.Network;
using CodeGame.Network.Server;

namespace CodeGame.Screens {
    enum Screen { Menu, Lobby, Game }

    class ScreenManager {
        Game game;

        MenuScreen menuScreen;
        LobbyScreen lobbyScreen;
        GameScreen gameScreen;
        
        Screen activeScreen = Screen.Menu;

        StatusBar statusBar = null;
        string statusBarStartText = "Menu";

        Listener listener = null;
        Client client = null;

        public ScreenManager(Game game) {
            this.game = game;

            menuScreen = new MenuScreen(this);
            lobbyScreen = new LobbyScreen(this);
            gameScreen = new GameScreen(this);

            statusBar = new StatusBar(this);
            statusBar.Text = statusBarStartText;
        }

        public void Update(GameTime gameTime) {

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
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            
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
        }

        public ContentManager Content { get { return game.Content; } }
        public StatusBar StatusBar { get { return statusBar; } }
        public Game Game { get { return game; } }
        public MenuScreen MenuScreen { get { return menuScreen; } }
        public LobbyScreen LobbyScreen { get { return lobbyScreen; } }
        public GameScreen GameScreen { get { return gameScreen; } }
        public Client Client { get { return client; } }
        public int Width { get { return game.GraphicsDevice.Viewport.Width; } }
        public int Height { get { return game.GraphicsDevice.Viewport.Height; } }

        public void Close() {
            game.Exit();
        }

        //
        // Change from Menu screen to Lobby screen
        //

        public void ChangeToLobbyScreen(bool hosting) {
            statusBar.Text = "Lobby";
            statusBar.Color = StatusBar.LobbyColor;
            activeScreen = Screen.Lobby;

            if (hosting) {
                listener = new Listener();
                listener.Start();
            }

            client = new Client(this);
            lobbyScreen.SetClient(client);
        }

        //
        // Change from Lobby screen to Menu screen
        //

        public void ChangeToMenuScreen() {
            statusBar.Text = statusBarStartText;
            statusBar.Color = StatusBar.MenuColor;
            activeScreen = Screen.Menu;
        }

        public void ChangeToGameScreen() {
            statusBar.Text = "Game";
            activeScreen = Screen.Game;
        }
    }
}
