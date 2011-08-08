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
using CodeGame.Classes.Screens;
using System.Text;

namespace CodeGame.Classes.Input {
    class IPText {
        InputManager _input;
        Texture2D _cursor;
        Vector2 _cursorPosition;
        StringBuilder _text;
        Vector2 _textPosition;
        bool _isEditMode = false;
        SpriteFont _font;
        Color _color;
        //string _startText;

        public IPText(ScreenManager screen, SpriteFont font, Vector2 position, Color color) {
            _input = screen.InputManager;
            _font = font;
            //_startText = startText;
            _text = new StringBuilder();
            _textPosition = position;
            _color = color;
            _cursor = screen.Content.Load<Texture2D>("Cursor");
            _cursorPosition = position;
            _cursorPosition.Y -= 6;
            UpdateCursor();
        }

        public void Update(GameTime gameTime) {
            if (_isEditMode) {
                
                Keys[] keys = _input.GetKeys();
                if (keys.Length == 0) return;

                for (int i = 0; i < keys.Length; i++) {
                    if (keys[i] == Keys.Enter) {
                        IsEditMode = false;
                        return;
                    }

                    if (keys[i] == Keys.LeftShift || keys[i] == Keys.RightShift) continue;

                    if (keys[i] == Keys.Back) {
                        if (_text.Length > 0) {
                            _text.Remove(_text.Length - 1, 1);
                            UpdateCursor();
                        }
                    }
                    else {
                        int n = (int)keys[i];
                        if (n >= 65 && n <= 90) {
                            if (!_input.IsShiftPressed) n += 32;
                        }
                        _text.Append((char)n);
                        UpdateCursor();
                    }
                }
            }
        }

        private void UpdateCursor() {
            Vector2 textMeasurements = _font.MeasureString(_text);
            _cursorPosition.X = _textPosition.X + textMeasurements.X + 3;
        }

        public void Draw(SpriteBatch batch) {
            batch.DrawString(_font, _text.ToString(), _textPosition, _color);

            if (_isEditMode) {
                batch.Draw(_cursor, _cursorPosition, Color.Black);
            }
        }

        public string Text {
            get {
                return _text.ToString();
            }
            set {
                _text = new StringBuilder(value);
                UpdateCursor();
            }
        }

        public bool IsEditMode {
            get {
                return _isEditMode;
            }
            set {
                _isEditMode = value;
                _input.IsProcessingKeys = value;

                //if (_isEditMode == false && _text.Length == 0) {
                //    _text.Append(_startText);
                //    UpdateCursor();
                //}
            }
        }
    }
}
