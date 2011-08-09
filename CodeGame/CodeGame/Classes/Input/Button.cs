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
        //InputManager input = null;

        SpriteFont font = null;

        Texture2D textureNormal = null;
        Texture2D textureHover = null;
        Vector2 texturePosition = Vector2.Zero;

        string text = "";
        Vector2 textPostion = Vector2.Zero;

        bool isHover = false;

        public Button(ScreenManager mgr, string text) : this(mgr, Vector2.Zero, text) {
            // Just a short-cut overload
        }

        public Button(ScreenManager screen, Vector2 position, string text) {
            //input = screen.InputManager;

            font = screen.Content.Load<SpriteFont>("ButtonFont");

            textureNormal = screen.Content.Load<Texture2D>("Button-Normal");
            textureHover = screen.Content.Load<Texture2D>("Button-Hover");
            texturePosition = position;
            
            this.text = text;
            textPostion = CalculateTextPosition();
        }

        public void Draw(SpriteBatch batch) {
            if (isHover) {
                batch.Draw(textureHover, texturePosition, Color.White);
                batch.DrawString(font, text, textPostion, Color.White);
            }
            else {
                batch.Draw(textureNormal, texturePosition, Color.White);
                batch.DrawString(font, text, textPostion, Color.White * 0.7f);
            }
        }

        private Vector2 CalculateTextPosition() {
            // MeasureString uses a Vector2 to represent width/height via x/y
            Vector2 str = font.MeasureString(text);
            float offset = (textureNormal.Width - str.X) / 2;
            return new Vector2(texturePosition.X + offset, texturePosition.Y + 11);
        }

        public bool IsHover { get { return isHover; } set { isHover = value; } }

        public int X1 { get { return (int)texturePosition.X; } set { texturePosition.X = value; } }
        public int Y1 { get { return (int)texturePosition.Y; } set { texturePosition.X = value; } }
        public int X2 { get { return (int)texturePosition.X + textureNormal.Width; } }
        public int Y2 { get { return (int)texturePosition.Y + textureNormal.Height; } }

        public Vector2 Position { 
            get { return texturePosition; }
            set {
                texturePosition = value;
                textPostion = CalculateTextPosition();
            }   
        }

        public static int Width { get { return 153; } }
        public static int Height { get { return 49; } }
    }
    





    //class Button {
    //    InputManager _input;
    //    Texture2D _texture;
    //    Vector2 _postion;
    //    Color _color, _hoverColor;
    //    bool _forceUpdate;

    //    public Button(ScreenManager screen, string texture, Vector2 position, Color hoverColor, bool forceUpdate) {
    //        _input = screen.InputManager;
    //        _texture = screen.Content.Load<Texture2D>(texture);
    //        _postion = position;
    //        _hoverColor = hoverColor;
    //        _color = Color.White;
    //        _forceUpdate = forceUpdate;
    //    }

    //    public void Update() {
    //        if (_input.BoxHasFocus && !_forceUpdate) return;

    //        if (IsHover())
    //            _color = _hoverColor;
    //        else
    //            _color = Color.White;
    //    }

    //    public void Draw(SpriteBatch batch) {
    //        batch.Draw(_texture, _postion, _color);
    //    }

    //    public bool IsClicked() {
    //        if ((!_input.BoxHasFocus || _forceUpdate) && IsHover() && _input.IsMouseClicked())
    //            return true;
    //        else
    //            return false;
    //    }

    //    private bool IsHover() {
    //        if (_input.MouseX >= (int)_postion.X &&
    //            _input.MouseX <= (int)_postion.X + _texture.Width &&
    //            _input.MouseY >= (int)_postion.Y &&
    //            _input.MouseY <= (int)_postion.Y + _texture.Height)
    //            return true;
    //        else
    //            return false;
    //    }
    //}
}
