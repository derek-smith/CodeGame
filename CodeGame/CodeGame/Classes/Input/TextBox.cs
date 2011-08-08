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
        string GetText();
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

        Keys[] keys;
        Keys[] prevKeys;
        bool isShiftPressed = false;
        Dictionary<Keys, bool> lockedKeys = new Dictionary<Keys, bool>();

        Texture2D cursor = null;
        Vector2 cursorPosition = Vector2.Zero;

        // False means Escape was pressed
        // but this flag is only checked AFTER the box has been closed.
        bool enterPressed = false;
        public bool EnterPressed { get { return enterPressed; } }

        public TextBox(ScreenManager mgr, int width = 200) {
            font = mgr.Content.Load<SpriteFont>("ButtonFont");
            textPosition = CalculateTextPosition();

            float fontHeight = (font.MeasureString("DS")).Y;

            this.width = width;
            height = PAD + (int)fontHeight + PAD;

            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.Gray * 0.3f;
            }
            boxTexture = new Texture2D(mgr.Game.GraphicsDevice, width, height);
            boxTexture.SetData<Color>(pixels);

            int cursorWidth = 2;
            pixels = new Color[cursorWidth * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.White;
            }
            cursor = new Texture2D(mgr.Game.GraphicsDevice, cursorWidth, height);
            cursor.SetData<Color>(pixels);

            ClearKeys();
        }

        // This is assumed to be called right before
        // displaying this TextBox
        public void SetText(string text) {
            this.text = text;
            ClearKeys();
            UpdateCursorPosition();
        }

        private void ClearKeys() {
            lockedKeys[Keys.Enter] = true;
            lockedKeys[Keys.Escape] = true;
            lockedKeys[Keys.Back] = false;
            for (int i = 48; i <= 57; i++) lockedKeys[(Keys)i] = false;
            for (int i = 65; i <= 90; i++) lockedKeys[(Keys)i] = false;

            keys = new Keys[0];
            prevKeys = new Keys[0];
        }

        private bool IsValidKey(Keys key) {
            int n = (int)key;
            return (key == Keys.Back        ||
                    key == Keys.Enter       ||
                    key == Keys.Escape      ||
                    (n >= 48 && n <= 57)    ||  // Numbers
                    (n >= 65 && n <= 90));      // Letters
        }

        public bool Update(GameTime gameTime) {
            keys = Keyboard.GetState().GetPressedKeys();

            // Clear lockedKeys for released keys
            for (int i = 0; i < prevKeys.Length; i++) {
                if (!keys.Contains(prevKeys[i])) {
                    lockedKeys[prevKeys[i]] = false;
                }
            }

            // Shift is not part of the ValidKeys check, but is still valid!
            if (keys.Contains(Keys.LeftShift) || keys.Contains(Keys.RightShift)) {
                     isShiftPressed = true;
            }

            for (int i = 0; i < keys.Length; i++) {
                if (IsValidKey(keys[i]) && !lockedKeys[keys[i]]) {

                    lockedKeys[keys[i]] = true;

                    if (keys[i] == Keys.Enter) {
                        if (text != "") {
                            enterPressed = true;
                        }
                        else {
                            enterPressed = false;
                        }
                        return true;
                    }
                    else if (keys[i] == Keys.Escape) {
                        enterPressed = false;
                        return true;
                    }
                    else if (keys[i] == Keys.Back) {
                        if (text != "") {
                            text = text.Substring(0, text.Length - 1);
                        }
                    }
                    else {
                        int n = (int)keys[i];
                        if (!isShiftPressed) n += 32;

                        text += (char)n;
                    }

                    UpdateCursorPosition();

                }
                
            }

            isShiftPressed = false;
            prevKeys = keys;

            return false;
        }
        
        public void SetPosition(Vector2 newPosition) {
            boxPosition = newPosition;
            textPosition = CalculateTextPosition();
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition() {
            Vector2 textDimensions = font.MeasureString(text);
            cursorPosition.Y = boxPosition.Y;
            cursorPosition.X = boxPosition.X + textDimensions.X + PAD + 2;
        }

        private Vector2 CalculateTextPosition() {
            return new Vector2(boxPosition.X + PAD, boxPosition.Y + PAD);
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(boxTexture, boxPosition, Color.White);
            batch.DrawString(font, text, textPosition, Color.White);
            batch.Draw(cursor, cursorPosition, Color.White);
        }

        public int GetWidth() {
            return width;
        }

        public int GetHeight() {
            return height;
        }

        public string GetText() {
            return text;
        }
    }
}