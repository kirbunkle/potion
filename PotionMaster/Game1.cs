using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;


namespace PotionMaster
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private Texture2D playersprite;
        private PlayerCharacter playerboi;

        // global variables
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static KeyboardState keyboardInput;
        public static int dt; // datetime
        public static ContentManager content;
        public static TiledMap mahMap;
        public static TiledMapRenderer mapRenderer;

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

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            content = Content;
            playersprite = Content.Load<Texture2D>("dumb_grass");
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
            
            mapRenderer.Update(mahMap, gameTime);
            playerboi.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            mapRenderer.Draw(mahMap);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(playersprite, new Vector2(400, 200), Color.White);
            spriteBatch.End();
            
            playerboi.Draw();

            base.Draw(gameTime);
        }
    }
}