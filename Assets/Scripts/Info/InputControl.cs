using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Bindings;
using UnityEngine.Internal;

public class InputControl
{
    //[NativeThrows]
    //private static extern bool GetKeyDownInt(KeyCode key);


    //Input down = Input.GetKeyDown(KeyCode.Escape);



    public static bool Up()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            return true;
        }
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            return true;
        }
        return false;
    }

    public static bool Down()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            return true;
        }
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            return true;
        }
        return false;
    }

    public static bool Left()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            return true;
        }
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            return true;
        }
        return false;
    }

    public static bool Right()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            return true;
        }
        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            return true;
        }
        return false;
    }

    public static bool Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            return true;
        }

        return false;
    }
    public static bool Fireball()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            return true;
        }
        
        
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            return true;
        }
        return false;
    }

    public static bool Hook()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            return true;
        }


        if (Input.GetAxisRaw("Hook") == 1)
        {
            return true;
        }
        return false;
    }

    public static bool Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            return true;
        }


        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            return true;
        }
        return false;
    }

    public static bool Esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            return true;
        }


        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            return true;
        }
        return false;
    }

    // Controles de interaccion

    public static bool Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }


        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            return true;
        }
        return false;
    }


}
