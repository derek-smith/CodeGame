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

namespace CodeGame.Classes.Screens {
    class MessageBox {
        ScreenManager _manager;
        Texture2D _blackTexture;
        Texture2D _boxTexture;
        Vector2 _boxPosition = new Vector2(100, 100);
        bool _active = false;
        string _text;
        SpriteFont _font;

        public MessageBox(ScreenManager manager) {
            _manager = manager;
            _boxTexture = _manager.ContentManager.Load<Texture2D>("MessageBox");
            _font = _manager.ContentManager.Load<SpriteFont>("MenuFont");

            int width = _manager.Game.GraphicsDevice.DisplayMode.Width;
            int height = _manager.Game.GraphicsDevice.DisplayMode.Height;
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.Black;
            }
            _blackTexture = new Texture2D(_manager.Game.GraphicsDevice, width, height);
            _blackTexture.SetData<Color>(pixels);
        }

        public void Show(string text) {
            _active = true;
            _text = text;
        }

        public void Hide() {
            _active = false;
        }

        public void Update() {

        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(_blackTexture, Vector2.Zero, Color.White * 0.7f);
            batch.Draw(_boxTexture, _boxPosition, Color.White);
            batch.DrawString(_font, _text, new Vector2(120, 120), Color.Black);
        }

        public bool Active { get { return _active; } }
    }
}
