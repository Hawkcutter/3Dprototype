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

}

