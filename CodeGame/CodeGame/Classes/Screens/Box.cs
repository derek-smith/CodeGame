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

    class Box {

        Texture2D[] boxTextures = null;                 // Calculated
        Vector2[] boxPositions = new Vector2[8];

        SpriteFont font = null;                         // Calculated

        List<string> lines = new List<string>();
        Vector2[] linePositions = null;                 // Calculated

        int screenWidth = -1;                           // All calculated 
        int screenHeight = -1;                          // 
        int boxWidth = -1;                              //
        int boxHeight = -1;                             //
        int cornerWidth = -1;                           //
        int cornerHeight = -1;                          //

        const int PAD = 20;

        int x1 = -1;
        int y1 = -1;
        int x2 = -1;
        int y2 = -1;

        Texture2D screenOverlay = null;
        Color screenOverlayColor = Color.Black;
        Color transparent = Color.White * 0.7f;
        Texture2D boxBackground = null;
        Color boxBackgroundColor = Color.Black;

        Button[] btns = null;
        ITextBox textBox = null;

        public Box(ScreenManager mgr, string text, int width, Button[] buttons)
            : this(mgr, text, width, buttons, null) {

        }

        public Box(ScreenManager mgr, string text, int width, Button[] buttons, ITextBox textbox) {
            // Load font
            font = mgr.Content.Load<SpriteFont>("MainFont");

            textBox = textbox;

            // Starts at top left and rotates clockwise
            boxTextures = new Texture2D[8] {
                mgr.Content.Load<Texture2D>(@"Box\Box-Top-Left"),
                mgr.Content.Load<Texture2D>(@"Box\Box-Top"),
                mgr.Content.Load<Texture2D>(@"Box\Box-Top-Right"),
                mgr.Content.Load<Texture2D>(@"Box\Box-Right"),
                mgr.Content.Load<Texture2D>(@"Box\Box-Bottom-Right"),
                mgr.Content.Load<Texture2D>(@"Box\Box-Bottom"),
                mgr.Content.Load<Texture2D>(@"Box\Box-Bottom-Left"),
                mgr.Content.Load<Texture2D>(@"Box\Box-Left")
            };

            //
            // Dimensions
            //

            screenWidth = mgr.Width;
            screenHeight = mgr.Height;
            boxWidth = width;
            //boxHeight needs to be calculated
            cornerWidth = boxTextures[0].Width;
            cornerHeight = boxTextures[0].Height;

            //
            // Calculate boxHeight
            //

            Vector2 textDim = font.MeasureString(text);
            int innerBoxWidth = boxWidth - (PAD * 2);
            // Need word wrap?
            if (textDim.X > innerBoxWidth) {
                string[] tokens = text.Split(' ');
                StringBuilder lineBuilder = new StringBuilder();

                for (int i = 0; i < tokens.Length; i++) {
                    lineBuilder.Append(tokens[i]);
                    textDim = font.MeasureString(lineBuilder.ToString());

                    lineBuilder.Append(" ");

                    // Breaking point?
                    if (textDim.X > innerBoxWidth) {
                        // Remove last token (and space) because it went over boxWidth
                        lineBuilder.Remove(lineBuilder.Length - (tokens[i].Length + 1), tokens[i].Length + 1);
                        // Save newly-created line
                        lines.Add(lineBuilder.ToString());
                        // Clear for next line
                        lineBuilder.Clear();
                        // Start new line with last word that didn't fit (and space)
                        lineBuilder.Append(tokens[i]);
                        lineBuilder.Append(" ");
                    }
                }
                // Add left over as last line
                lines.Add(lineBuilder.ToString());
            }
            // No word wrap needed
            else {
                lines.Add(text);
            }
            // Add up all of this stuff to get the total height:
            //   number of lines of text * the height of a text letter
            // + box padding * 4 (top, middle, ... , middle, bottom)
            // + the height of a button
            // + the height of a textbox
            int textBoxHeight = 0;
            if (textBox != null) textBoxHeight = textBox.GetHeight() + (PAD * 2);
            boxHeight = (lines.Count * (int)textDim.Y) + (PAD * 3) + Button.Height + textBoxHeight;

            x1 = (screenWidth - boxWidth) / 2;
            x2 = x1 + boxWidth;
            y1 = (screenHeight - boxHeight) / 2;
            y2 = y1 + boxHeight;

            //
            // Buttons
            //

            this.btns = buttons;

            if (btns.Length >= 1) {
                btns[0].Position = new Vector2(x2 - PAD - Button.Width, y2 - PAD - Button.Height);

                if (btns.Length == 2) {
                    btns[1].Position = new Vector2(x1 + PAD, y2 - PAD - Button.Height);
                }
            }

            if (textBox != null)
                textBox.SetPosition(new Vector2(x1 + PAD, y2 - (PAD * 3) - Button.Height - textBox.GetHeight()));

            // Corners
            boxPositions[0] = new Vector2(x1, y1);
            boxPositions[2] = new Vector2(x2 - cornerWidth, y1);
            boxPositions[4] = new Vector2(x2 - cornerWidth, y2 - cornerHeight);
            boxPositions[6] = new Vector2(x1, y2 - cornerHeight);

            linePositions = new Vector2[lines.Count];
            for (int i = 0; i < linePositions.Length; i++) {
                linePositions[i] = new Vector2(x1 + PAD, y1 + PAD + (i * (int)textDim.Y));
            }

            // screenOverlay
            Color[] pixels = new Color[screenWidth * screenHeight];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = screenOverlayColor;
            }
            screenOverlay = new Texture2D(mgr.Game.GraphicsDevice, screenWidth, screenHeight);
            screenOverlay.SetData<Color>(pixels);

            // boxBackground
            pixels = new Color[boxWidth * boxHeight];
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = boxBackgroundColor;
            }
            boxBackground = new Texture2D(mgr.Game.GraphicsDevice, boxWidth, boxHeight);
            boxBackground.SetData<Color>(pixels);
        }

        public void Draw(SpriteBatch batch) {

            // Draw overlay/box background
            batch.Draw(screenOverlay, Vector2.Zero, transparent);
            batch.Draw(boxBackground, new Vector2(x1, y1), Color.White);

            // Draw corners
            for (int i = 0; i < boxTextures.Length; i += 2) {
                batch.Draw(boxTextures[i], boxPositions[i], Color.White);
            }

            // Draw top and bottom
            for (int x = x1 + cornerWidth; x < x2 - cornerHeight; x++) {
                batch.Draw(boxTextures[1], new Vector2(x, y1), Color.White);
                batch.Draw(boxTextures[5], new Vector2(x, y2 - cornerHeight), Color.White);
            }

            // Draw right and left
            for (int y = y1 + cornerHeight; y < y2 - cornerHeight; y++) {
                batch.Draw(boxTextures[3], new Vector2(x2 - cornerWidth, y), Color.White);
                batch.Draw(boxTextures[7], new Vector2(x1, y), Color.White);
            }

            // Draw text 
            for (int i = 0; i < lines.Count; i++) {
                batch.DrawString(font, lines[i], linePositions[i], Color.White * 0.95f);
            }

            // Draw textbox
            if (textBox != null)
                textBox.Draw(batch);

            // Draw Buttons
            for (int i = 0; i < btns.Length; i++) {
                btns[i].Draw(batch);
            }
        }
    }
}