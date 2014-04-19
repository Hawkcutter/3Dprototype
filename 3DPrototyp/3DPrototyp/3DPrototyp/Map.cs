using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3DPrototyp
{
    class Map
    {

        //erstellen und erzeugen der Map, behandeln von Kolisionen mit der map
        //der MapVertexBuffer enthält ansich alle notwendigen Daten um die Map zu erzeugen daher der return dessen
        //Texturen werden hier nicht gebraucht, die Texturkoordinaten werden zwar zugewiesen, aber erst im Draw() angewendet
        public enum CollisionType { None, Building, Boundary, Target }
        public BoundingBox[] buildingBoundingBoxes;
        public BoundingBox completeCityBox;
        int[] buildingHeights = new int[] { 0, 1, 2, 3, 4, 5 };
        int[,] floorPlan;

        public Map() {} //Leer, da ich keinen momentan keine Daten aus der Game1.cs brauche

        public void LoadFloorPlan() 
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

        public VertexBuffer SetUpVertices(GraphicsDevice device)
        {
            VertexBuffer mapVertexBuffer;
            
            float imagesInTexture = 1 + 10;              //Ändern wen andere Textur gewählt wird, "+ Zahl" ist die Anzahl der Texturen -1

            int mapWidth = floorPlan.GetLength(0);
            int mapLength = floorPlan.GetLength(1);


            List<VertexPositionNormalTexture> verticesList = new List<VertexPositionNormalTexture>();
            for (int x = 0; x < mapWidth; x++)
            {
                for (int z = 0; z < mapLength; z++)
                {
                    int currentbuilding = 1;  //geändert das erstmal nur 2 Texturen (Boden und Wand) gezeichnet werden
                    //int currentbuilding = floorPlan[x, z];  //Original currentBuilding

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
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2(1 / imagesInTexture, 1)));

                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2(0, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z - 1), new Vector3(0, 1, 0), new Vector2(1 / imagesInTexture, 0)));
                    verticesList.Add(new VertexPositionNormalTexture(new Vector3(x + 1, floorPlan[x, z], -z), new Vector3(0, 1, 0), new Vector2(1 / imagesInTexture, 1)));


                    //if (currentbuilding != 0) // genutzt wen currentBuilding wieder einen nutzen hat
                    if (floorPlan[x,z] > 0) 
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

            return mapVertexBuffer;
        }

        public void SetUpBoundingBoxes()
        {
            int mapWidth = floorPlan.GetLength(0);
            int mapLength = floorPlan.GetLength(1);


            List<BoundingBox> bbList = new List<BoundingBox>(); for (int x = 0; x < mapWidth; x++)
            {
                for (int z = 0; z < mapLength; z++)
                {
                    int buildingType = floorPlan[x, z];
                    if (buildingType != 0)
                    {
                        int buildingHeight = buildingHeights[buildingType];
                        Vector3[] buildingPoints = new Vector3[2];
                        buildingPoints[0] = new Vector3(x, 0, -z);
                        buildingPoints[1] = new Vector3(x + 1, buildingHeight, -z - 1);
                        BoundingBox buildingBox = BoundingBox.CreateFromPoints(buildingPoints);
                        bbList.Add(buildingBox);
                    }
                }
            }
            buildingBoundingBoxes = bbList.ToArray();

            Vector3[] boundaryPoints = new Vector3[2];
            boundaryPoints[0] = new Vector3(0, 0, 0);
            boundaryPoints[1] = new Vector3(mapWidth, 20, -mapLength);
            completeCityBox = BoundingBox.CreateFromPoints(boundaryPoints);
        }

        public CollisionType CheckCollision(BoundingSphere sphere)
        {
            for (int i = 0; i < buildingBoundingBoxes.Length; i++)
                if (buildingBoundingBoxes[i].Contains(sphere) != ContainmentType.Disjoint)
                    return CollisionType.Building;

            if (completeCityBox.Contains(sphere) != ContainmentType.Contains)
                return CollisionType.Boundary;

            return CollisionType.None;
        }

        public CollisionType GetCollisionType(string Type)
        {
            if (Type == "Building")
                return CollisionType.Building;
            if (Type == "Boundary")
                return CollisionType.Boundary;
            if (Type == "Target")
                return CollisionType.Target;
            if(Type == "None")
                return CollisionType.None;

            return 0;
        }
    }
}
