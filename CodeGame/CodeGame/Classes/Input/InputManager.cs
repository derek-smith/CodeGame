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

// This should be rewritten to use delegates/events with Textbox as a subscriber

namespace CodeGame.Classes.Input {
    class InputManager {
        Keys[] _keyState;
        Keys[] _prevKeyState;
        MouseState _mouseState, _prevMouseState;
        bool _isProcessingKeys = false;
        bool _isShiftPressed = false;
        Dictionary<Keys, bool> _lockedKeys = new Dictionary<Keys, bool>();
        bool _boxHasFocus = false;

        public bool BoxHasFocus { get { return _boxHasFocus; } set { _boxHasFocus = value; } }

        public InputManager() {

        }

        public void UpdateBegin() {
            if (_isProcessingKeys) _keyState = Keyboard.GetState().GetPressedKeys();
            _mouseState = Mouse.GetState();
        }

        public void UpdateEnd() {
            if (_isProcessingKeys) _prevKeyState = _keyState;
            _prevMouseState = _mouseState;
        }

        public int MouseX { get { return _mouseState.X; } }
        public int MouseY { get { return _mouseState.Y; } }

        public bool IsMouseClicked() {
            return (_prevMouseState.LeftButton == ButtonState.Pressed &&
                    _mouseState.LeftButton == ButtonState.Released);
        }

        public bool IsProcessingKeys
        { 
            get {
                return _isProcessingKeys;
            }
            set {
                _isProcessingKeys = value;

                if (_isProcessingKeys)
                    InitializeKeys();
            }
        }
        
        public bool IsShiftPressed { get { return _isShiftPressed; } }
        
        public Keys[] GetKeys() {
            // Shouldn't have to worry about valid keys here.
            for (int i = 0; i < _prevKeyState.Length; i++) {
                if (!_keyState.Contains(_prevKeyState[i])) {
                    _lockedKeys[_prevKeyState[i]] = false;
                }
            }

            List<Keys> returnKeys = new List<Keys>();

            for (int i = 0; i < _keyState.Length; i++) {
                if (_keyState[i] == Keys.Enter) {
                    returnKeys.Add(Keys.Enter);
                    return returnKeys.ToArray();
                }
                
                if (_keyState[i] == Keys.LeftShift || _keyState[i] == Keys.RightShift) {
                    _isShiftPressed = true;
                    continue;
                }
                _isShiftPressed = false;

                if (IsValidKey(_keyState[i]) && !_lockedKeys[_keyState[i]]) {
                    returnKeys.Add(_keyState[i]);
                    _lockedKeys[_keyState[i]] = true;
                }
            }
            return returnKeys.ToArray();
        }

        private void InitializeKeys() {
            _lockedKeys[Keys.Space]         = false;
            _lockedKeys[Keys.Back]          = false;
            _lockedKeys[Keys.OemPeriod]     = false;
            for (int i = 48; i <= 57; i++) _lockedKeys[(Keys)i] = false;
            for (int i = 65; i <= 90; i++) _lockedKeys[(Keys)i] = false;
            _keyState = new Keys[0];
            _prevKeyState = new Keys[0];
        }

        private bool IsValidKey(Keys key) {
            int n = (int)key;
            return (key == Keys.Space       ||
                    key == Keys.Back        ||
                    key == Keys.OemPeriod   ||
                    (n >= 48 && n <= 57)    ||      // Numbers
                    (n >= 65 && n <= 90));          // Letters
        }
    }
}
