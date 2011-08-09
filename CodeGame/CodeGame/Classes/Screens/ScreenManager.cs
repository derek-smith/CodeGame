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
using CodeGame.Classes.Network.Server;
using CodeGame.Classes.Network.Client;

namespace CodeGame.Classes.Screens {
    enum Screen { Menu, Lobby, Game }

    class ScreenManager {
        Game game;

        MenuScreen menuScreen;
        LobbyScreen lobbyScreen;
        GameScreen gameScreen;
        
        Screen activeScreen = Screen.Menu;

        StatusBar2 statusBar = null;
        string statusBarStartText = "So you want to play my game, huh? Swwweeeeeet!";

        Listener2 listener = null;
        Client2 client = null;

        public ScreenManager(Game game) {
            this.game = game;

            menuScreen = new MenuScreen(this);
            lobbyScreen = new LobbyScreen(this);
            gameScreen = new GameScreen(this);

            statusBar = new StatusBar2(this);
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
        public StatusBar2 StatusBar { get { return statusBar; } }
        public Game Game { get { return game; } }
        public MenuScreen MenuScreen { get { return menuScreen; } }
        public LobbyScreen LobbyScreen { get { return lobbyScreen; } }
        public GameScreen GameScreen { get { return gameScreen; } }
        public int Width { get { return game.GraphicsDevice.Viewport.Width; } }
        public int Height { get { return game.GraphicsDevice.Viewport.Height; } }

        public void Close() {
            game.Exit();
        }

        //
        // Change from Menu screen to Lobby screen
        //

        public void ChangeToLobbyScreen(bool hosting) {
            statusBar.Text = "You really think someone is going to play YOU?";
            activeScreen = Screen.Lobby;

            if (hosting) {
                listener = new Listener2();
                listener.Start();
            }

            client = new Client2(this);
        }

        //
        // Change from Lobby screen to Menu screen
        //

        public void ChangeToMenuScreen() {
            statusBar.Text = statusBarStartText;
            activeScreen = Screen.Menu;
        }
    }
}
