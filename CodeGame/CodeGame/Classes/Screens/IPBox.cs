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
using CodeGame.Classes.Input;

namespace CodeGame.Classes.Screens {
    
    class IPBox {

        ScreenManager _screen;
        Texture2D _blackTexture;
        Texture2D _boxTexture;
        Vector2 _boxPosition = new Vector2(100, 100);
        bool _active = false;
        SpriteFont _font;
        Color _clrTransparent = Color.White * 0.7f;
        //Button _btnOkay, _btnCancel;
        NameText _txtIP;

        public IPBox(ScreenManager manager) {
            _screen = manager;
            _boxTexture = _screen.Content.Load<Texture2D>("MessageBox");
            _font = _screen.Content.Load<SpriteFont>("MenuFont");

            int width = _screen.Game.GraphicsDevice.Viewport.Width;
            int height = _screen.Game.GraphicsDevice.Viewport.Height;
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.Black;
            }
            _blackTexture = new Texture2D(_screen.Game.GraphicsDevice, width, height);
            _blackTexture.SetData<Color>(pixels);

            _txtIP = new NameText(_screen, _font, new Vector2(140, 220), Color.Black);
            //_btnOkay = new Button(_screen, "Test-Normal", new Vector2(520, 420), Color.SaddleBrown, true);
            //_btnCancel = new Button(_screen, "Test-Normal", new Vector2(120, 420), Color.SaddleBrown, true);
        }

        public void Show() {
            _active = true;
            _screen.InputManager.BoxHasFocus = true;
            _txtIP.IsEditMode = true;

            _txtIP.Text = _screen.MenuScreen.IPAddress;
        }

        public void Hide(bool success) {
            _active = false;
            _screen.InputManager.BoxHasFocus = false;
            _txtIP.IsEditMode = false;

            if (success)
                _screen.MenuScreen.IPAddress = _txtIP.Text;
        }

        public void Update(GameTime gameTime) {
            _txtIP.Update(gameTime);
            //_btnOkay.Update();
            //_btnCancel.Update();

            //if (_btnOkay.IsClicked()) {
            //    Hide(true);
            //}
            //else if (_btnCancel.IsClicked()) {
            //    Hide(false);
            //}
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(_blackTexture, Vector2.Zero, _clrTransparent);
            batch.Draw(_boxTexture, _boxPosition, Color.White);
            batch.DrawString(_font, "Type an IP:", new Vector2(120, 120), Color.Black);

            //_btnOkay.Draw(batch);
            //_btnCancel.Draw(batch);
            _txtIP.Draw(batch);
        }

        public bool Active { get { return _active; } }
    }
}
