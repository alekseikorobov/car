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

namespace carrrr
{
    /// <summary>
    /// Это главный тип игры
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Позволяет игре выполнить инициализацию, необходимую перед запуском.
        /// Здесь можно запросить нужные службы и загрузить неграфический
        /// контент.  Вызов base.Initialize приведет к перебору всех компонентов и
        /// их инициализации.
        /// </summary>
        protected override void Initialize()
        {
            camera = new Camera(this, new Vector3(0, 1900, 0), Vector3.Zero, new Vector3(-1, 0, 0));
            car = new ModeleCar(this);

            Components.Add(car);
            Components.Add(camera);

            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            base.Initialize();
        }

        public ModeleCar car { get; set; }

        public Camera camera { get; set; }

        /// <summary>
        /// LoadContent будет вызываться в игре один раз; здесь загружается
        /// весь контент.
        /// </summary>
        protected override void LoadContent()
        {
            // Создайте новый SpriteBatch, который можно использовать для отрисовки текстур.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // ЗАДАЧА: используйте здесь this.Content для загрузки контента игры
        }

        /// <summary>
        /// UnloadContent будет вызываться в игре один раз; здесь выгружается
        /// весь контент.
        /// </summary>
        protected override void UnloadContent()
        {
            // ЗАДАЧА: выгрузите здесь весь контент, не относящийся к ContentManager
        }

        /// <summary>
        /// Позволяет игре запускать логику обновления мира,
        /// проверки столкновений, получения ввода и воспроизведения звуков.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        protected override void Update(GameTime gameTime)
        {
            // Позволяет выйти из игры
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            UpdateUnput();

            base.Update(gameTime);
        }

        private void UpdateUnput()
        {
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.A)) camera.cameraUp = new Vector3(1, 0, 0);
            if (k.IsKeyDown(Keys.S)) camera.cameraUp = new Vector3(0, 1, 0);
            if (k.IsKeyDown(Keys.D)) camera.cameraUp = new Vector3(0, 0, 1);
            if (k.IsKeyDown(Keys.Q)) camera.cameraUp = new Vector3(-1, 0, 0);
            if (k.IsKeyDown(Keys.W)) camera.cameraUp = new Vector3(0, -1, 0);
            if (k.IsKeyDown(Keys.E)) camera.cameraUp = new Vector3(0, 0, -1);
        }

        /// <summary>
        /// Вызывается, когда игра отрисовывается.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // ЗАДАЧА: добавьте здесь код отрисовки

            base.Draw(gameTime);
        }
    }
}
