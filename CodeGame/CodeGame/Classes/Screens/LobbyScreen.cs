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
    class LobbyScreen {
        ScreenManager _manager;
        string _username;
        SpriteFont _font;
        Button _backButton;

        public LobbyScreen(ScreenManager manager) {
            _manager = manager;
            _font = _manager.ContentManager.Load<SpriteFont>("MenuFont");
            _backButton = new Button(_manager, 
        }

        public void ChangeFromMenuScreen(string status, string username) {
            _manager.StatusBar.Text = status;
            _username = username;
        }

        public void Update(GameTime gameTime) {

        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            graphics.Clear(Color.CornflowerBlue);
            batch.Begin();
            batch.DrawString(_font, _username, new Vector2(20, 90), Color.White);
            // Leave this out - is called in ScreenManager
            //batch.End();
        }
    }
}
