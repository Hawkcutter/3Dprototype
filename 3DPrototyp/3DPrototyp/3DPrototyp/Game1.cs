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



public class Game1 : Microsoft.Xna.Framework.Game
{
    public static GraphicsDeviceManager graphics;
    public static SpriteBatch spriteBatch;
    public static GameTime gameTime;

    GraphicsDevice device;
    VertexBuffer mapVertexBuffer;

    BasicEffect effect;

    Texture2D mapTexture;

    Matrix viewMatrix;
    Matrix projectionMatrix;
    int[,] floorPlan;

    //Muss später geändert werden
    float rotation;

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

        mapTexture = Content.Load<Texture2D>("TextureFile");

        rotation = 0.5f;

        device = graphics.GraphicsDevice;
        SetUpCamera();
        LoadFloorPlan();        // Durch einlesen von einen Bild ersetzen
        SetUpVertices();        // Braucht noch Texturen

        // TODO: use this.Content to load your game content here
    }

    private void SetUpCamera()
    {

        viewMatrix = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
    }

    //Laden der Map
    private void LoadFloorPlan()
    {
        floorPlan = new int[,]
             {
                 {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                 {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,0,1,1,0,0,0,1,1,0,0,1,0,1},
                 {1,0,0,1,1,0,0,0,1,0,0,0,1,0,1},
                 {1,0,0,0,1,1,0,1,1,0,0,0,0,0,1},
                 {1,0,0,0,0,0,0,0,0,0,0,1,0,0,1},
                 {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,1,1,0,0,0,1,0,0,0,0,0,0,1},
                 {1,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                 {1,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
                 {1,0,0,0,0,1,0,0,0,1,0,0,0,0,1},
                 {1,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
                 {1,0,1,1,0,0,0,0,1,1,0,0,0,1,1},
                 {1,0,0,0,0,0,0,0,1,1,0,0,0,1,1},
                 {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
             };

    }

    //Erstellen der Vertices aus der Map, braucht Texturen!
    private void SetUpVertices()
    {
        //int differentBuildings = buildingHeights.Length - 1;
        float imagesInTexture = 1;              //braucht änderung, momentan keine Textur vorhanden

        int mapWidth = floorPlan.GetLength(0);
        int mapLength = floorPlan.GetLength(1);


        List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapLength; z++)
            {
                int currentbuilding = floorPlan[x, z];

                //Ein Dreieck besteht je aus 3 Vertices
                //1. Vector3(Position) 2. Vector3(Normal) für das Licht 3. Vector2(Textur position)

                //floor or ceiling
                verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2(currentbuilding * 2 / imagesInTexture, 1)));
                verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));

                verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2) / imagesInTexture, 0)));
                verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 0)));
                verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2((currentbuilding * 2 + 1) / imagesInTexture, 1)));

                if (currentbuilding != 0)
                {
                    //ineffektiv, da auch wände zwischen objekten gezeichnet werden

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

        //Rotieren um die Y-Achse, zum testen, sollte später geändert werden
        if (Input.isPressed(Keys.Right))
            rotation++;
        if (Input.isPressed(Keys.Left))
            rotation--;

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
        effect.View = viewMatrix * Matrix.CreateRotationY(rotation);  //Keine Ahnung ob das geht
        effect.Projection = projectionMatrix;
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

