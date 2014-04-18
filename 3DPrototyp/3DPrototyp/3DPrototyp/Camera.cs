using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3DPrototyp
{
    class Camera
    {
        //Standart Variablen für Kamera, ziemlich selbsterklärend
        public Vector3 cameraPosition;
        public float   moveSpeed, rotateSpeed; //Individuelle Belegung dieser
        public Matrix  viewMatrix, projectionMatrix; //Muss immernoch in der Draw Methode aufgerufen werden

        GraphicsDevice device;

        //wichtig zur bewegung der Kamera
        Matrix rotation;

        float yaw   = 0f;
        float pitch = 0f;

        //Variablen für die Kamerarichtung
        int oldX, oldY;

        //Standart Konstruktor
        public Camera(Vector3 position, float moveSpeed, float rotateSpeed, GraphicsDevice device){
            
            this.cameraPosition = position;
            this.moveSpeed      = moveSpeed;
            this.rotateSpeed    = rotateSpeed;
            this.device         = device;

            viewMatrix          = Matrix.CreateLookAt(cameraPosition,Vector3.Zero,Vector3.Up);
            projectionMatrix    = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), device.Viewport.AspectRatio, 0.01f, 1000.0f);
            ResetMousCursor();
        }

        //Update Methode der Camera.cs, muss in der Update Methode der Game1.cs aufgerufen werden
        public void update()
        {
            //Übernommen aus dem standart generierten Game1.cs
            Input.prevKeyboard = Input.currentKeyboard;
            Input.currentKeyboard = Keyboard.GetState();

            //WASD Steuerung der Kamera
            if (Input.isPressed(Keys.W))
            {
                Vector3 v = new Vector3(0, 0, -1) * moveSpeed;
                Move(v);
            }
            if (Input.isPressed(Keys.S))
            {
                Vector3 v = new Vector3(0, 0, 1) * moveSpeed;
                Move(v);
            }
            if (Input.isPressed(Keys.A))
            {
                Vector3 v = new Vector3(-1, 0, 0) * moveSpeed;
                Move(v);
            }
            if (Input.isPressed(Keys.D))
            {
                Vector3 v = new Vector3(1, 0, 0) * moveSpeed;
                Move(v);
            }
            //Sorgt dafür das du nicht einmal 360° oben oder nach unten drehen kannst
            pitch = MathHelper.Clamp(pitch, -1.5f, 1.5f);
            //Abfangen der Maus
            MouseState mState = Mouse.GetState();
            //Berechnung aus den Koordinaten der maus die Bewegung derer
            int dx = mState.X - oldX;
            yaw -= rotateSpeed * dx;

            int dy = mState.Y - oldY;
            pitch -= rotateSpeed * dy;

            ResetMousCursor();
            UpdateMatrices();
        }

        //Maus in der Fenstermitte Fangen
        private void ResetMousCursor()
        {
            int centerX = device.Viewport.Width  / 2;
            int centerY = device.Viewport.Height / 2;

            Mouse.SetPosition(centerX, centerY);
            oldX = centerX;
            oldY = centerY;

        }

        //Aktuelle Berechnung der Kamera Bewegung
        private void UpdateMatrices()
        {
            rotation = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw);
            Vector3 transformedReference = Vector3.Transform(new Vector3(0, 0, -1), rotation);
            Vector3 lookAt = cameraPosition + transformedReference;

            viewMatrix = Matrix.CreateLookAt(cameraPosition, lookAt, Vector3.Up);
        }

        //Verschiebung der Kamera
        private void Move(Vector3 v)
        {
            Matrix yRotation = Matrix.CreateRotationY(yaw);
            v = Vector3.Transform(v, yRotation);

            cameraPosition += v;
        }


    }
}
