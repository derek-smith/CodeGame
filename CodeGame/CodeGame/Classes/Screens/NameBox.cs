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

    class NameBox {

        ScreenManager _manager;
        Texture2D _blackTexture;
        Texture2D _boxTexture;
        Vector2 _boxPosition = new Vector2(100, 100);
        bool _active = false;
        SpriteFont _font;
        Color _clrTransparent = Color.White * 0.7f;
        //Button _btnOkay, _btnCancel;
        NameText _txtName;

        public NameBox(ScreenManager manager) {
            _manager = manager;
            _boxTexture = _manager.Content.Load<Texture2D>("MessageBox");
            _font = _manager.Content.Load<SpriteFont>("MenuFont");

            int width = _manager.Game.GraphicsDevice.Viewport.Width;
            int height = _manager.Game.GraphicsDevice.Viewport.Height;
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.Black;
            }
            _blackTexture = new Texture2D(_manager.Game.GraphicsDevice, width, height);
            _blackTexture.SetData<Color>(pixels);

            _txtName = new NameText(_manager, _font, new Vector2(140, 220), Color.Black);
            //_btnOkay = new Button(_manager, "Test-Normal", new Vector2(520, 420), Color.SaddleBrown, true);
            //_btnCancel = new Button(_manager, "Test-Normal", new Vector2(120, 420), Color.SaddleBrown, true);
        }

        public void Show() {
            _active = true;
            _manager.InputManager.BoxHasFocus = true;
            _txtName.IsEditMode = true;

            //_txtName.Text = _manager.MenuScreen.PlayerName;
        }

        private void Hide(bool success) {
            _active = false;
            _manager.InputManager.BoxHasFocus = false;
            _txtName.IsEditMode = false;

            //if (success)
                //_manager.MenuScreen.PlayerName = _txtName.Text;
        }

        public void Update(GameTime gameTime) {
            _txtName.Update(gameTime);
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
            batch.DrawString(_font, "Type a name", new Vector2(120, 120), Color.Black);
            //_btnOkay.Draw(batch);
            //_btnCancel.Draw(batch);
            _txtName.Draw(batch);
        }

        public bool Active { get { return _active; } }
    }
}
