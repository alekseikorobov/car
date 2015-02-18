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
    public class ModeleCar : DrawableGameComponent
    {
        
        public void SetCarPosition(
           Vector3 setNewCarPosition,
           Vector3 setDirection,
           Vector3 setUp)
        {
            // Add car height to make camera look at the roof and not at the street.
            carPos = setNewCarPosition;
            carDir = setDirection;
            carUp = setUp;
        }

        Vector3 carPos;

        /// <summary>
        /// Вектор направления автомабиля.
        /// </summary>
        Vector3 carDir;

        /// <summary>
        /// Speed of our car, just in the direction of our car.
        /// Sliding is a nice feature, but it overcomplicates too much and
        /// for this game sliding would be really bad and make it much harder
        /// to drive!
        /// </summary>
        float speed;

        /// <summary>
        /// Car up vector for orientation.
        /// </summary>
        Vector3 carUp;
        public Vector3 CarRight
        {
            get
            {
                return Vector3.Cross(carDir, carUp);
            }
        }

        public Matrix UpdateCarMatrix()
        {
            // Get car matrix with help of the current car position, dir and up
            Matrix carMatrix = Matrix.Identity;
            carMatrix.Right = CarRight;
            carMatrix.Up = carUp;
            carMatrix.Forward = carDir;
            carMatrix.Translation = carPos;

            return carMatrix;
        }

        public ModeleCar(Game game)
            : base(game)
        {
            SetCarPosition(new Vector3(0, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1f, 0));
        }

        public Model skillet { get; set; }
        public Model lastCircle { get; set; }
        public Model firstCircleRight { get; set; }
        public Model firstCircleLeft { get; set; }

        /// <summary>
        /// Позволяет игровому компоненту выполнить необходимую инициализацию перед\r\запуском.  Здесь можно запросить нужные службы и загрузить контент.
        /// 
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            skillet = Game.Content.Load<Model>(@"Modele\UserCar1\skillet");
            lastCircle = Game.Content.Load<Model>(@"Modele\UserCar1\lastCircle");
            firstCircleRight = Game.Content.Load<Model>(@"Modele\UserCar1\firstCircleRight");
            firstCircleLeft = Game.Content.Load<Model>(@"Modele\UserCar1\firstCircleLeft");

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            MyDraw(lastCircle);
            MyDraw(firstCircleLeft, true);
            MyDraw(firstCircleRight, true);
            MyDraw(skillet);
            base.Draw(gameTime);
        }

        //private Matrix rotate1 = Matrix.Identity;
        private void MyDraw(Model model, bool isrot = false)
        {
            Matrix[] m = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(m);

            //foreach ( in )
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                ModelMesh mesh = model.Meshes[i];
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = ((Game1)Game).camera.projection;
                   
                    effect.View = ((Game1)Game).camera.view;
                    if (isrot)
                    {
                        effect.World = mesh.ParentBone.Transform*UpdateCarMatrix();
                    }
                    else
                    {
                        effect.World = mesh.ParentBone.Transform * UpdateCarMatrix();
                    }

                }
                mesh.Draw();
            }
        }

        private float dx, dz;
        public Matrix RotateMatrixWheel = Matrix.CreateRotationY(0);
        public Matrix RotateMatrixAll = Matrix.CreateRotationY(0);
        public Matrix TransformMatrix = Matrix.Identity;
        public Matrix TransformMatrixAll = Matrix.CreateTranslation(0, 0, 0);

        //private Vector3 transform;
        public float transform;
        public float Transform
        {
            get
            {
                return transform;
            }
            set
            {
                transform = value;
                TransformMatrix = Matrix.CreateTranslation(transform,0,0);
            }

        }

        private int maxAngleWheel = 30;
        private float rotation = 0;
        public float Rotate
        {
            get { return (float)Math.Round(rotation, 2); }
            set
            {
                //while (value >= MathHelper.TwoPi)
                //{
                //    value -= MathHelper.TwoPi;
                //}
                //while (value < 0)
                //{
                //    value += MathHelper.TwoPi;
                //}
                if (rotation != value)
                {
                    rotation = value;
                    RotateMatrixWheel = Matrix.CreateRotationY(rotation);
                    TransformMatrixAll = Matrix.CreateTranslation(200, 0, 100);
                }
            }
        }

        private float rotationall = 0;
        public float RotateAll
        {
            get { return (float)Math.Round(rotationall, 2); }
            set
            {
                //while (value >= MathHelper.TwoPi)
                //{
                //    value -= MathHelper.TwoPi;
                //}
                //while (value < 0)
                //{
                //    value += MathHelper.TwoPi;
                //}
                if (rotationall != value)
                {
                    rotationall = value;
                    RotateMatrixAll = Matrix.CreateRotationY(rotationall);
                }
            }
        }

        private float speedInertion = 1;
        private int accert = 0;
        public bool isMoveing { get; set; }
        private float interpolatedRotationChange;
        /// <summary>
        /// Позволяет игровому компоненту обновиться.
        /// </summary>
        /// <param name="gameTime">Предоставляет моментальный снимок значений времени.</param>
        public override void Update(GameTime gameTime)
        {
            isMoveing = false;
            KeyboardState k = Keyboard.GetState();

            if (k.IsKeyDown(Keys.Left) && (k.IsKeyDown(Keys.Down) || k.IsKeyDown(Keys.Up)))
            {
                interpolatedRotationChange += 0.02f;
                if ((k.IsKeyDown(Keys.Down)))
                {
                    interpolatedRotationChange = -interpolatedRotationChange;
                }
                carDir = Vector3.TransformNormal(carDir,
                        Matrix.CreateFromAxisAngle(carUp, interpolatedRotationChange));
            }
            else
            {
                interpolatedRotationChange = 0;
            }
            if (k.IsKeyDown(Keys.Right) && (k.IsKeyDown(Keys.Down) || k.IsKeyDown(Keys.Up)))
            {
                interpolatedRotationChange -= 0.02f;
                if ((k.IsKeyDown(Keys.Down)))
                {
                    interpolatedRotationChange = -interpolatedRotationChange;
                }
                carDir = Vector3.TransformNormal(carDir,Matrix.CreateFromAxisAngle(carUp, interpolatedRotationChange));
            }
            else
            {
                interpolatedRotationChange = 0;
            }
            if (k.IsKeyDown(Keys.Down))
            {
                //speedInertion -= ;
                //speedInertion -= speedInertion - 0.1f < 2 ? 0 : 0.1f;
                carPos -= carDir * 5.75f * -speedInertion;
            }
            if (k.IsKeyDown(Keys.Up))
            {
                //speedInertion += 0.1f;
                //speedInertion += speedInertion + 0.1f > 2 ? 0 : 0.1f;
                carPos += carDir * 5.75f * speedInertion;

            }

            if (!k.IsKeyDown(Keys.Up) && !k.IsKeyDown(Keys.Down) && (speedInertion > 0 || speedInertion <0))
            {
                //if (speedInertion > 0)
                //{
                //    //speedInertion -= 0.05f;
                //    carPos += carDir * 5.75f * speedInertion;
                //}
                //if (speedInertion < 0)
                //{
                //    //speedInertion += 0.05f;
                //    carPos -= carDir * 5.75f * -speedInertion;
                //}
            }

            base.Update(gameTime);
        }
    }
}
