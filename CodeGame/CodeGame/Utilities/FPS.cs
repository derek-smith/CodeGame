using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace CodeGame.Screens {
    class FPS
    {
        public enum Display { TopLeft, TopRight, BottomRight, BottomLeft }
        SpriteFont _font;
        Vector2 _vector;
        double _elapsed;
        int _counter;
        int _snapshot;
        const int _margin = 5;

        public FPS(ContentManager content, Rectangle window, Display display)
        {
            _font = content.Load<SpriteFont>("MainFont");
            switch (display)
            {
                case Display.TopLeft:
                    _vector = new Vector2(_margin, _margin);
                    break;
                case Display.TopRight:
                    _vector = new Vector2(window.Width - 40 - _margin, _margin);
                    break;
                case Display.BottomRight:
                    Debug.WriteLine("screen.Width: " + window.Width.ToString());
                    Debug.WriteLine("screen.Height: " + window.Height.ToString());
                    _vector = new Vector2(window.Width - 40 - _margin, window.Height - 20 - _margin);
                    break;
                case Display.BottomLeft:
                    _vector = new Vector2(_margin, window.Height - 20 - _margin);
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsed >= 1000.0)
            {
                _snapshot = _counter;
                _counter = 0;
                _elapsed -= 1000.0;
            }
            else
                _counter++;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            batch.DrawString(_font, _snapshot.ToString(), _vector, Color.Red);
            batch.End();
        }
    }
}
