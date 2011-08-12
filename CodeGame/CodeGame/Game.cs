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

using CodeGame.Screens;

namespace CodeGame {
    public class Game : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch batch;

        Dictionary<string, object> g;

        ScreenManager mgr;

        #if FPS_COUNTER
        FPS fps;
        #endif

        public Game() {

            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 600;
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
            batch = new SpriteBatch(GraphicsDevice);

            g = Globals.Get();

            g["game"] = this;
            g["mgr"] = mgr;

            g["MainFont"] = Content.Load<SpriteFont>("MainFont");
            
            g["Button-Normal"] = Content.Load<Texture2D>("Button-Normal");
            g["Button-Hover"] = Content.Load<Texture2D>("Button-Hover");

            g["buttonWidth"] = ((Texture2D)g["Button-Normal"]).Width;
            g["buttonHeight"] = ((Texture2D)g["Button-Normal"]).Height;

            mgr = new ScreenManager(this);

            #if FPS_COUNTER
            fps = new FPS(Content, Window.ClientBounds, FPS.Display.BottomRight);
            #endif
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    this.Exit();

            TheMouse.Update();

            mgr.Update(gameTime);

            #if FPS_COUNTER
            fps.Update(gameTime);
            #endif

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            // This line is very temporary
            GraphicsDevice.Clear(Color.CornflowerBlue);

            mgr.Draw(GraphicsDevice, batch);
            
            #if FPS_COUNTER
            fps.Draw(batch);
            #endif

            base.Draw(gameTime);
        }
    }
}
