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
    class Chat {
        int width = 300;
        int height = 350;
        Texture2D boxTexture = null;
        Vector2 boxPosition = new Vector2(638, 60);

        public Chat(ScreenManager mgr) {
            Color[] pixels = new Color[width * height];
            Color color = Color.Gray * 0.3f;
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = color;
            }
            boxTexture = new Texture2D(mgr.Game.GraphicsDevice, width, height);
            boxTexture.SetData<Color>(pixels);
        }

        public void Update(GameTime gameTime) {

        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(boxTexture, boxPosition, Color.White);
        }
    }
}
