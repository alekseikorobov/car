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
    /// Это игровой компонент, реализующий интерфейс IUpdateable.
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix view { get; set; }
        public Matrix projection { get; set; }

        public Vector3 cameraPosition { get; set; }
        private Vector3 cameraDirection;
        private Vector3 _cameraUp;
        public Vector3 cameraUp
        {
            get
            {
                return _cameraUp;
            }
            set
            {
                _cameraUp = value;
                CreateLookAt();
            }
        }

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                        (float)Game.Window.ClientBounds.Width /
                        (float)Game.Window.ClientBounds.Height,
                        0.000001f, 300);
            // ЗАДАЧА: здесь создаются дочерние компоненты
        }

        void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }

        /// <summary>
        /// Позволяет игровому компоненту выполнить необходимую инициализацию перед\r\запуском.  Здесь можно запросить нужные службы и загрузить контент.
        /// 
        /// </summary>
        public override void Initialize()
        {
            // ЗАДАЧА: добавьте здесь код инициализации

            base.Initialize();
        }

        /// <summary>
        /// Позволяет игровому компоненту обновиться.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        public override void Update(GameTime gameTime)
        {
            // ЗАДАЧА: добавьте здесь код обновления

            base.Update(gameTime);
        }
    }
}
