using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



public static class Input
{
    public static KeyboardState currentKeyboard;
    public static KeyboardState prevKeyboard;
    public static Vector3 goalPosition = new Vector3(15.5f, 1, -8.5f);
    public static float goalDistance;

    public static bool isClicked(Keys key)
    {

       // return currentKeyboard.IsKeyDown(key) && prevKeyboard.IsKeyUp(key) ? true : false;

        if (prevKeyboard.IsKeyUp(key) )
        {
            Console.Out.WriteLine("step1");
            if (currentKeyboard.IsKeyDown(key))
            {
                Console.Out.WriteLine("step2");
                Console.Out.WriteLine("True");
                return true;
            }
           
        }
        return false;
    }

    public static bool isPressed(Keys key)
    {
        return currentKeyboard.IsKeyDown(key) ? true : false;
    }
    
    public static float calculateDistance(Vector3 start, Vector3 goal)
    {
        return Math.Abs(Math.Abs(start.X) - Math.Abs(goal.X)) +
            Math.Abs(Math.Abs(start.Y) - Math.Abs(goal.Y))  + Math.Abs(Math.Abs(start.Z)
            - Math.Abs(goal.Z));
    }

}

