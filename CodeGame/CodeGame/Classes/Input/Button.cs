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
using CodeGame.Classes.Screens;

namespace CodeGame.Classes.Input {
    class Button {
        InputManager _input;
        Texture2D _texture;
        Vector2 _postion;
        Color _color, _hoverColor;
        bool _isEnabled = true;

        public Button(ScreenManager screen, string texture, Vector2 position, Color hoverColor) {
            _input = screen.InputManager;
            _texture = screen.ContentManager.Load<Texture2D>(texture);
            _postion = position;
            _hoverColor = hoverColor;
            _color = Color.White;
        }

        public void Update() {
            if (!_isEnabled) return;

            if (IsHover())
                _color = _hoverColor;
            else
                _color = Color.White;
        }

        public void Draw(SpriteBatch batch) {
            if (_isEnabled)
                batch.Draw(_texture, _postion, _color);
            else
                batch.Draw(_texture, _postion, Color.DarkGray);
        }

        public bool IsClicked() {
            if (_isEnabled && IsHover() && _input.IsMouseClicked())
                return true;
            else
                return false;
        }

        private bool IsHover() {
            if (_input.MouseX >= (int)_postion.X &&
                _input.MouseX <= (int)_postion.X + _texture.Width &&
                _input.MouseY >= (int)_postion.Y &&
                _input.MouseY <= (int)_postion.Y + _texture.Height)
                return true;
            else
                return false;
        }

        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; } }
    }
}
