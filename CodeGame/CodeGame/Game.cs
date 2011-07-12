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
using CodeGame.Classes;
using CodeGame.Classes.Utilities;

namespace CodeGame {
    public class Game : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager manager;

#if DEBUG
        FPS fps;
#endif

        public Game() {
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            manager = new ScreenManager(Content);
#if DEBUG
            fps = new FPS(Content, Window.ClientBounds, FPS.Display.BottomRight);
#endif
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            manager.Update(gameTime);
#if DEBUG
            fps.Update(gameTime);
#endif

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // This line is very temporary
            GraphicsDevice.Clear(Color.CornflowerBlue);

            manager.Draw(GraphicsDevice, spriteBatch);
#if DEBUG
            fps.Draw(spriteBatch);
#endif

            base.Draw(gameTime);
        }
    }
}
