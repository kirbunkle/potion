using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace PotionMaster
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private PlayerCharacter playerboi;

        // global variables
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static KeyboardState keyboardInput;
        public static int dt; // deltatime
        public static GameTime gt;
        public static ContentManager content;
        public static TiledMap mahMap;
        public static TiledMapRenderer mapRenderer;
        public static Camera2D camera;
        public static int screenW;
        public static int screenH;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            mapRenderer = new TiledMapRenderer(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenW = 800;
            screenH = 480;
            camera = new Camera2D(new BoxingViewportAdapter(Window, GraphicsDevice, screenW, screenH));            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            content = Content;
            playerboi = new PlayerCharacter();
            mahMap = Content.Load<TiledMap>("tiledMaps/dumb_grass");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            Game1.spriteBatch.Dispose();
            Game1.mapRenderer.Dispose();
            Game1.content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardInput = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardInput.IsKeyDown(Keys.Escape))
                Exit();

            dt = gameTime.ElapsedGameTime.Milliseconds;
            gt = gameTime;
            
            mapRenderer.Update(mahMap, gameTime);
            playerboi.Update();
            
            var oldPos = camera.Position;
            camera.LookAt(playerboi.position);
            float diffX = Math.Abs(camera.Position.X - oldPos.X);
            float diffY = Math.Abs(camera.Position.Y - oldPos.Y);
            if ((diffX > 0.50) || (diffY > 0.50))
            {
                oldPos += (camera.Position - oldPos) / 8; // TODO: figure out how to do the sum of squares to get the best path to the player
                if (diffX > 4)
                {
                    oldPos.X = (float)Math.Round(oldPos.X);
                }                
                if (diffY > 4)
                { 
                    oldPos.Y = (float)Math.Round(oldPos.Y);
                }
                camera.Position = oldPos;
            }
            oldPos = camera.Position;
            if (oldPos.X < 0)
                oldPos.X = 0;
            if (oldPos.X > (mahMap.WidthInPixels - screenW))
                oldPos.X = mahMap.WidthInPixels - screenW;
            if (oldPos.Y < 0)
                oldPos.Y = 0;
            if (oldPos.Y > (mahMap.HeightInPixels - screenH))
                oldPos.Y = mahMap.HeightInPixels - screenH;
            camera.Position = oldPos;
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            mapRenderer.Draw(mahMap, camera.GetViewMatrix());
            playerboi.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}