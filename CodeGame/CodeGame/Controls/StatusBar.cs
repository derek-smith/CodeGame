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

using CodeGame.Screens;

namespace CodeGame.Controls {

    class StatusBar {
        ScreenManager mgr = null;
        Texture2D bgTexture = null;

        public static Color MenuColor = Color.Blue * 0.5f;
        public static Color LobbyColor = Color.Red * 0.5f;
        public static Color LobbyColorReady = Color.Green * 0.5f;

        Color bgColor = MenuColor;

        SpriteFont font = null;
        string text = "";
        Vector2 textPosition = new Vector2(40, 7);

        public StatusBar(ScreenManager mgr) {
            this.mgr = mgr;
            font = mgr.Content.Load<SpriteFont>("MainFont");

            int width = mgr.Width;
            int height = 40;
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = Color.White;
            }
            bgTexture = new Texture2D(mgr.Game.GraphicsDevice, width, height);
            bgTexture.SetData<Color>(pixels);
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(bgTexture, Vector2.Zero, bgColor);
            batch.DrawString(font, text, textPosition, Color.White * 0.8f);
        }

        public string Text { get { return text; } set { text = value; } }
        public Color Color { get { return bgColor; } set { bgColor = value; } }
    }
}
