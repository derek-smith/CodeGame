#if DEBUG
#define FPS_COUNTER
#endif

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
using CodeGame.Classes.Utilities;

namespace CodeGame {
    public class Game : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        ScreenManager _manager;

#if FPS_COUNTER
        FPS _fps;
#endif

        public Game() {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _manager = new ScreenManager(this);
#if FPS_COUNTER
            _fps = new FPS(Content, Window.ClientBounds, FPS.Display.BottomRight);
#endif
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            _manager.Update(gameTime);

#if FPS_COUNTER
            _fps.Update(gameTime);
#endif

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // This line is very temporary
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _manager.Draw(GraphicsDevice, _spriteBatch);
#if FPS_COUNTER
            _fps.Draw(_spriteBatch);
#endif

            base.Draw(gameTime);
        }
    }
}
