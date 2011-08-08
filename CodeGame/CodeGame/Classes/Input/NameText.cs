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

    interface ITextBox {
        int GetWidth();
        int GetHeight();
        void SetPosition(Vector2 newPosition);
        void Draw(SpriteBatch batch);
    }

    class TextBox : ITextBox {
        Vector2 boxPosition = Vector2.Zero;
        Texture2D boxTexture = null;

        SpriteFont font = null;

        string text = "";
        Vector2 textPosition = Vector2.Zero;

        const int PAD = 10;

        int width = -1;
        int height = -1;

        public TextBox(ScreenManager mgr, string text = "", int width = 200) {
            font = mgr.Content.Load<SpriteFont>("ButtonFont");
            textPosition = CalculateTextPosition();

            float fontHeight = (font.MeasureString("DS")).Y;

            this.text = text;
            this.width = width;
            height = PAD + (int)fontHeight + PAD;

            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.Gray * 0.2f;
            }
            boxTexture = new Texture2D(mgr.Game.GraphicsDevice, width, height);
            boxTexture.SetData<Color>(pixels);
        }

        public void SetPosition(Vector2 newPosition) {
            boxPosition = newPosition;
            textPosition = CalculateTextPosition();
        }


        private Vector2 CalculateTextPosition() {
            return new Vector2(boxPosition.X + PAD, boxPosition.Y + PAD);
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(boxTexture, boxPosition, Color.White);
            batch.DrawString(font, text, textPosition, Color.White);
        }

        public bool Update(GameTime gameTime) {

            return false;
        }

        public int GetWidth() {
            return width;
        }

        public int GetHeight() {
            return height;
        }
    }
    
    class NameText {
        InputManager _input;
        Texture2D _cursor;
        Vector2 _cursorPosition;
        StringBuilder _text;
        Vector2 _textPosition;
        bool _isEditMode = false;
        SpriteFont _font;
        Color _color;
        //string _startText;

        public NameText(ScreenManager screen, SpriteFont font, Vector2 position, Color color) {
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
                    if (keys[i] == Keys.LeftShift || keys[i] == Keys.RightShift) continue; 
                    
                    if (keys[i] == Keys.Enter) {
                        IsEditMode = false;
                        return;
                    }
                    else if (keys[i] == Keys.OemPeriod) {
                        _text.Append(".");
                        UpdateCursor();
                    }
                    else if (keys[i] == Keys.Back) {
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
            // Possible Exception: character not in the font (Type: ArgumentException)
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
