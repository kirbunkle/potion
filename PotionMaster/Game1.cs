using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.BitmapFonts;
using System;

namespace PotionMaster
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private PlayerCharacter playerboi;
        private Character binch;

        // global variables
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static KeyboardState keyboardInput;
        public static int dt; // deltatime
        public static GameTime gt;
        public static ContentManager content;
        public static TiledMapRenderer mapRenderer;
        public static Camera2D camera;
        public static int screenW;
        public static int screenH;
        public static BitmapFont font;
        public static int tileSize;
        public static Location currentLocation;

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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mapRenderer = new TiledMapRenderer(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            screenW = 800;
            screenH = 480;
            camera = new Camera2D(new BoxingViewportAdapter(Window, GraphicsDevice, screenW, screenH));
            //camera.Zoom = 2;
            tileSize = 32;

            content = Content;
            playerboi = new PlayerCharacter();
            binch = new Character();
            font = Content.Load<BitmapFont>("fonts/font1");
            currentLocation = new Location();
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

            currentLocation.Update();
            playerboi.Update();
            binch.Update();
            
            var oldPos = camera.Position;
            camera.LookAt(playerboi.GetPosition());
            float diffX = Math.Abs(camera.Position.X - oldPos.X);
            float diffY = Math.Abs(camera.Position.Y - oldPos.Y);
            if ((diffX > 0.50) || (diffY > 0.50))
            {
                oldPos += (camera.Position - oldPos) / 12; 
                oldPos.X = (float)Math.Ceiling(oldPos.X);  // snap to nearest pixel
                oldPos.Y = (float)Math.Ceiling(oldPos.Y);
                camera.Position = oldPos;
            }
            oldPos = camera.Position;
            if (oldPos.X < 0)
                oldPos.X = 0;
            if (oldPos.X > (currentLocation.WidthInPixels() - screenW))
                oldPos.X = currentLocation.WidthInPixels() - screenW;
            if (oldPos.Y < 0)
                oldPos.Y = 0;
            if (oldPos.Y > (currentLocation.HeightInPixels() - screenH))
                oldPos.Y = currentLocation.HeightInPixels() - screenH;
            camera.Position = oldPos;
            //camera.ZoomIn(camera.Zoom*dt*0.01f);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // camera draw
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            currentLocation.Draw();
            playerboi.Draw();
            binch.Draw();
            spriteBatch.End();

            // hud draw
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            currentLocation.DrawHud();
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}