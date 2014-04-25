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


namespace _3DPrototyp
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static GameTime gameTime;

        GraphicsDevice device;

        //Speichert ansich die Map
        VertexBuffer mapVertexBuffer;

        BasicEffect effect;

        Texture2D endingTexture;
        public static bool reachedGoal = false;
        Vector2 goalTextPosition = new Vector2(80,40);

        Vector3 goalPosition = new Vector3(19.5f, 6, -7.5f);
        

        Texture2D mapTexture;
        Model model;
        //Model bewegung test
        Vector3 modelPosition = new Vector3(20, 5, -5);
        bool forward = false;

        //verwalten Camera Bewegung und Map Erstellung
        Camera camera;
        Map map;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            this.Window.Title = "3D Prototype";

            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {

            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            mapTexture = Content.Load<Texture2D>("texturemap");
            endingTexture = Content.Load<Texture2D>("theEnd");
            model = LoadModel("xwing");

            device = graphics.GraphicsDevice;

            camera = new Camera(new Vector3(2, 4, -6), 0.05f, 0.005f, device); // Camera((X,Y,Z),moveSpeed,rotationSpeed,GraphicsDevice)
            map = new Map();
            map.LoadFloorPlan();        // Durch einlesen von einen Bild ersetzen
            mapVertexBuffer = map.SetUpVertices(device);     // Braucht noch Texturen
            map.SetUpBoundingBoxes();
            map.setGoal();

            Console.Out.WriteLine("Steuerung: ");
            Console.Out.WriteLine("WASD standard ");
            Console.Out.WriteLine("Leertaste springen ");
            Console.Out.WriteLine("Bild Hoch/Runter: Kamera hoch oder runter ");

        }
        private Model LoadModel(string assetName)
        {

            Model newModel = Content.Load<Model>(assetName); 
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();
            return newModel;
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (camera.reachedGoal)
                reachedGoal = true;

            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();

            if (Input.isPressed(Keys.Escape))
                this.Exit();

            camera.update(map);
            //Kolision testen
            /*
            BoundingSphere playerSphere = new BoundingSphere(camera.cameraPosition, 0.04f);
            if (map.CheckCollision(playerSphere) != map.GetCollisionType("None"))
            {
                //xwingPosition = new Vector3(8, 1, -3);
                //xwingRotation = Quaternion.Identity;
                //gameSpeed /= 1.1f;

                Console.Out.WriteLine("Colision detected" + map.CheckCollision(playerSphere));
            }
            */
            //Testen und ausprobieren von Modelbewegung
            /*if (modelPosition.Z >= -14f && forward)
            {
                modelPosition.Z -= 0.1f;
                modelPosition.X -= 0.1f;
            } else
            {
                forward = false;
                modelPosition.Z += 0.1f;
                modelPosition.Y += 0.1f;
                if (modelPosition.Z >= -1f)
                {
                    forward = true;
                }
            } 

            //Console.Out.WriteLine("modelPosition: " + modelPosition);*/
            // TODO: Add your update logic here

            

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            

            // TODO: Add your drawing code here
            DrawMap();
            DrawModel();
            if (reachedGoal)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                spriteBatch.Draw(endingTexture, goalTextPosition, Color.White);
                spriteBatch.End();
            }
            

            base.Draw(gameTime);
        }

        //Zeichnen der Map, abändern in BasicEffects
        private void DrawMap()
        {
            effect.TextureEnabled = true;
            effect.View = camera.viewMatrix;
            effect.Projection = camera.projectionMatrix;
            effect.Texture = mapTexture;
            effect.World = Matrix.Identity;
            /*
            RasterizerState rs = new RasterizerState();
            rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;  
             */
            //effect.EnableDefaultLighting();
            //Das muss nicht geändert werden!
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(mapVertexBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, mapVertexBuffer.VertexCount / 3);
            }


        }
        private void DrawModel()
        {
            Matrix worldMatrix = Matrix.CreateScale(0.005f, 0.005f, 0.005f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(Input.goalPosition);

            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    currentEffect.EnableDefaultLighting();
                    currentEffect.World = modelTransforms[mesh.ParentBone.Index] * worldMatrix;
                    currentEffect.View = camera.viewMatrix;
                    currentEffect.Projection = camera.projectionMatrix;
                    currentEffect.VertexColorEnabled = true;
                }
                mesh.Draw();
            }
        }
        

    }
}

