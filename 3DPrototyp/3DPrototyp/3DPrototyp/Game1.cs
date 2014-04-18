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
        VertexBuffer mapVertexBuffer;

        BasicEffect effect;

        Texture2D mapTexture;
        Model model;

        Camera camera;
        Map map;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            this.Window.Title = "3D Prototype";

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
            // TODO: Add your initialization logic here
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();


            effect = new BasicEffect(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mapTexture = Content.Load<Texture2D>("texturemap");
            model = LoadModel("xwing");

            device = graphics.GraphicsDevice;

            camera = new Camera(new Vector3(2, 4, -6), 0.05f, 0.005f, device); // Camera((X,Y,Z),moveSpeed,rotationSpeed,GraphicsDevice)
            map = new Map();
            map.LoadFloorPlan();        // Durch einlesen von einen Bild ersetzen
            mapVertexBuffer = map.SetUpVertices(device);     // Braucht noch Texturen

            // TODO: use this.Content to load your game content here
        }
        private Model LoadModel(string assetName)
        {

            Model newModel = Content.Load<Model>(assetName); 
            foreach (ModelMesh mesh in newModel.Meshes)
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    meshPart.Effect = effect.Clone();
            return newModel;
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();

            if (Input.isClicked(Keys.Escape))
                this.Exit();

            camera.update();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            DrawMap();
            DrawModel();
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
            Matrix worldMatrix = Matrix.CreateScale(0.005f, 0.005f, 0.005f) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(new Vector3(2, 3, -5));

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

