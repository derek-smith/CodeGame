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

namespace CodeGame.Classes.Screens {
    class GameScreen {
        ScreenManager _manager;

        public GameScreen(ScreenManager manager) {
            _manager = manager;
        }

        public void Update(GameTime gameTime) {

        }

        public void Draw(GraphicsDevice graphics, SpriteBatch batch) {
            graphics.Clear(Color.CornflowerBlue);
            batch.Begin();

            // Leave this out - is called in ScreenManager
            //batch.End();
        }
    }
}
