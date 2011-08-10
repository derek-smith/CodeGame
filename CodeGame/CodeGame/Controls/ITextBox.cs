using System;

using Microsoft.Xna.Framework;                  // Vector2
using Microsoft.Xna.Framework.Graphics;         // SpriteBatch

namespace CodeGame.Controls {

    interface ITextBox {
        int GetWidth();
        int GetHeight();
        string GetText();
        void SetPosition(Vector2 newPosition);
        void Draw(SpriteBatch batch);
    }

}
