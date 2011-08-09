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

    class StatusBar2 {
        ScreenManager mgr = null;
        Texture2D bgTexture = null;

        SpriteFont font = null;
        string text = "";
        Vector2 textPosition = new Vector2(20, 7);

        public StatusBar2(ScreenManager mgr) {
            this.mgr = mgr;
            font = mgr.Content.Load<SpriteFont>("ButtonFont");

            int width = mgr.Width;
            int height = 40;
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.Red;
            }
            bgTexture = new Texture2D(mgr.Game.GraphicsDevice, width, height);
            bgTexture.SetData<Color>(pixels);
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(bgTexture, Vector2.Zero, Color.White * 0.4f);
            batch.DrawString(font, text, textPosition, Color.White * 0.7f);
        }

        public string Text { get { return text; } set { text = value; } }
    }


    class StatusBar {
        Texture2D _texture;
        Vector2 _texturePostion = new Vector2(20, 0);
        SpriteFont _font;
        string _text;
        Vector2 _textPostion = new Vector2(35, 7);

        public StatusBar(ContentManager content, string startText) {
            _texture = content.Load<Texture2D>("StatusBar");
            _font = content.Load<SpriteFont>("MenuFont");
            _text = startText;
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(_texture, _texturePostion, Color.White);
            batch.DrawString(_font, _text, _textPostion, Color.Black);
        }

        public string Text { get { return _text; } set { _text = value; } }
    }
}
