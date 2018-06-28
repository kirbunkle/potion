using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Animations;

namespace PotionMaster
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // global variables
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
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
        public static Inventory inventory;
        public static GameDateTime gameDateTime;
        public static float zoom;
        public static GameInput input;
        public static PlayerCharacter playerCharacter;
        public static ItemManager itemManager;

        private float zoomCameraOffsetX;
        private float zoomCameraOffsetY;

        public static Vector2 CreatePositionWithSpriteOffset(Vector2 v)
        {
            return new Vector2(v.X + (Game1.tileSize * 0.5f), v.Y + (Game1.tileSize * 0.5f));
        }

        public static Vector2 CreatePositionWithSpriteOffset(Rectangle r)
        {
            return new Vector2(r.X + (r.Width * 0.5f), r.Y + (r.Height * 0.5f));
        }

        public static AnimatedSprite CreateAnimatedSprite(SpriteSheetAnimationFactory factory, string name)
        {
            AnimatedSprite a = new AnimatedSprite(factory, name);
            a.Origin = new Vector2(0, 0);
            return a;
        }

        public static SpriteSheetAnimationFactory CreateAnimationFactory(string textureName, string animationMapName)
        {
            var characterTexture = Game1.content.Load<Texture2D>(textureName);
            var characterMap = Game1.content.Load<Dictionary<string, Rectangle>>(animationMapName);
            var characterAtlas = new TextureAtlas("tempAtlas", characterTexture, characterMap);
            return new SpriteSheetAnimationFactory(characterAtlas);
        }

        public static AnimatedSprite CreateSingleAnimatedSprite(string textureName, string animationMapName,
            int[] frameIndicies, float frameDuration = 0.2F, bool isLooping = true, bool isReversed = false, bool isPingPong = false)
        {
            var factory = CreateAnimationFactory(textureName, animationMapName);
            factory.Add("temp", new SpriteSheetAnimationData(frameIndicies, frameDuration, isLooping, isReversed, isPingPong));
            return CreateAnimatedSprite(factory, "temp");
        }

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

            tileSize = 32;

            screenW = 1280;
            screenH = 720;
            Game1.graphics.PreferredBackBufferWidth = screenW;
            Game1.graphics.PreferredBackBufferHeight = screenH;
            Game1.graphics.ApplyChanges();

            camera = new Camera2D(new BoxingViewportAdapter(Window, GraphicsDevice, screenW, screenH));

            zoom = 2;
            camera.Zoom = zoom;
            
            zoomCameraOffsetX = (screenW - (screenW / zoom)) / 2;
            zoomCameraOffsetY = (screenH - (screenH / zoom)) / 2;

            content = Content;
            itemManager = new ItemManager();

            playerCharacter = new PlayerCharacter();
            font = Content.Load<BitmapFont>("fonts/font1");
            currentLocation = new Location();
            inventory = new Inventory();
            gameDateTime = new GameDateTime();
            input = new GameInput();
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
            input.SetKeyboardState(Keyboard.GetState());

            if (input.ButtonPressed(GameButtons.Esc))
                Exit();

            dt = gameTime.ElapsedGameTime.Milliseconds;
            gt = gameTime;

            currentLocation.Update();
            playerCharacter.Update();
            inventory.Update();
            gameDateTime.Update();
            
            var oldPos = camera.Position;
            camera.LookAt(playerCharacter.GetCenter());
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
            if (oldPos.X < -zoomCameraOffsetX)
                oldPos.X = -zoomCameraOffsetX;
            if (oldPos.X > ((currentLocation.WidthInPixels() + zoomCameraOffsetX) - screenW))
                oldPos.X = (currentLocation.WidthInPixels() + zoomCameraOffsetX) - screenW;
            if (oldPos.Y < -zoomCameraOffsetY)
                oldPos.Y = -zoomCameraOffsetY;
            if (oldPos.Y > ((currentLocation.HeightInPixels() + zoomCameraOffsetY) - screenH))
                oldPos.Y = (currentLocation.HeightInPixels() + zoomCameraOffsetY) - screenH;
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
            playerCharacter.Draw();
            spriteBatch.End();

            // hud draw
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            currentLocation.DrawHud();
            inventory.DrawHud();
            gameDateTime.DrawHud();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}