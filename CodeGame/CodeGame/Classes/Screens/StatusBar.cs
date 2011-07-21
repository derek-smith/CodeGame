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
