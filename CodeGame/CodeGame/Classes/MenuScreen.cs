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

namespace CodeGame.Classes {
    class MenuScreen {
        ScreenManager _manager;
        string _string = "";

        public MenuScreen(ScreenManager manager, ContentManager content) {
            _manager = manager;
        }

        public void Update(GameTime gameTime) {
            _string += _manager.GetKeys();
        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            graphics.Clear(Color.CornflowerBlue);
            batch.Begin();
            batch.DrawString(_manager.Font, _string, new Vector2(10, 10), Color.Blue);
            batch.End();
        }
    }
}
