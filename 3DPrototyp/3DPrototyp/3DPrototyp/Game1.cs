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

        int[,] floorPlan;

        Camera camera;


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
            device = graphics.GraphicsDevice;
            camera = new Camera(new Vector3(2, 4, -6), 0.05f, 0.005f, device);
            LoadFloorPlan();        // Durch einlesen von einen Bild ersetzen
            SetUpVertices();        // Braucht noch Texturen

            // TODO: use this.Content to load your game content here
        }
        //Laden der Map
        private void LoadFloorPlan()
        {
            floorPlan = new int[,]
             {
                 {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                 {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,0,1,1,0,0,0,1,1,0,0,1,0,2},
                 {2,0,0,1,1,0,0,0,1,0,0,0,1,0,2},
                 {2,0,0,0,1,1,0,1,1,0,0,0,0,0,2},
                 {2,0,0,0,0,0,0,0,0,0,0,1,0,0,2},
                 {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,1,1,0,0,0,1,0,0,0,0,0,0,2},
                 {2,0,1,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
                 {2,0,0,0,0,1,0,0,0,0,0,0,0,0,2},
                 {2,0,0,0,0,1,0,0,0,1,0,0,0,0,2},
                 {2,0,1,0,0,0,0,0,0,1,0,0,0,0,2},
                 {2,0,1,1,0,0,0,0,1,1,0,0,0,1,2},
                 {2,0,0,0,0,0,0,0,1,1,0,0,0,1,2},
                 {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
             };

        }

        //Erstellen der Vertices aus der Map, braucht Texturen!
        private void SetUpVertices()
        {
            float imagesInTexture = 1 + 10;              //Ändern wen andere Textur gewählt wird, "+ Zahl" ist die Anzahl der Texturen -1

            int mapWidth = floorPlan.GetLength(0);
            int mapLength = floorPlan.GetLength(1);


            List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();
            for (int x = 0; x < mapWidth; x++)
            {
                for (int z = 0; z < mapLength; z++)
                {
                    int currentbuilding = 1;  //geändert das erstmal nur 2 Texturen (Boden und Wand) gezeichnet werden
                    //int currentbuilding = floorPlan[x, z];

                    //Ein Dreieck besteht je aus 3 Vertices
                    //1. Vector3(Position) 2. Vector3(Normal) für das Licht 3. Vector2(Textur position)
                    
                    //Momentan werden Wände zwischen zwei Blöcken gezeichnet                --> Wenn Zeit das optimieren
                    //Momentan wird die Textur nur gestreckt                                --> Wenn Zeit mehrer Blöcke generieren staht einen zu strecken
                    //Momentan können nur Blöcke mit verschiedenen höhen generiert werden   --> Wenn Zeit schwebende Platformen

                    //floor or ceiling

                    /*
                     * Auskommentiert da bei currentbuilding = 1 andere Textur gezeichnet würde
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2(currentbuilding * 2 / imagesInTexture, 1)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));

                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));
                    */
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2(0, 1)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2(0, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2(1/imagesInTexture, 1)));

                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2(0, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2(1 / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2(1 / imagesInTexture, 1)));


                    if (currentbuilding != 0)
                    {

                        //front wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z - 1), new Vector3(0, 0, -1), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));

                        //back wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(0, 0, 1), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));

                        //left wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z - 1), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, 0, -z), new Vector3(-1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));

                        //right wall
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z - 1), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z - 1), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 1)));

                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z - 1), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2 - 1) / imagesInTexture, 0)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, 0, -z), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 1)));
                        verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(1, 0, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                    }
                }
            }

            mapVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, verticesList.Count, BufferUsage.WriteOnly);

            mapVertexBuffer.SetData<VertexPositionNormalTexture>(verticesList.ToArray());
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
            base.Draw(gameTime);
        }

        //Zeichnen der Map, abändern in BasicEffects
        private void DrawMap()
        {
            //Klappt das auch in Basic Effects?
            effect.TextureEnabled = true;
            //effect.EnableDefaultLighting();
            effect.View = camera.viewMatrix;
            effect.Projection = camera.projectionMatrix;
            effect.Texture = mapTexture;
            effect.World = Matrix.Identity;
            //effect.CurrentTechnique = effect.Techniques["Textured"];
            //effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            //effect.Parameters["xView"].SetValue(viewMatrix);
            //effect.Parameters["xProjection"].SetValue(projectionMatrix);
            //effect.Parameters["xTexture"].SetValue(mapTexture);

            //Das muss nicht geändert werden!
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(mapVertexBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, mapVertexBuffer.VertexCount / 3);
            }
        }
    }
}

