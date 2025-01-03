using UnityEngine;

public class InputManager
{
    private static InputSystem_Actions _controls;
    public static InputSystem_Actions Controls {
        get
        {
            if (_controls == null)
            {
                _controls = new InputSystem_Actions();
                _controls.Enable();
                AssignActions();
            }
            return _controls;
        }
    }

    public static Vector2 MoveVector
    {
        get
        {
            return Controls.Player.Move.ReadValue<Vector2>();
        }
    }
    
    public static Vector2 AimVector
    {
        get
        {
            return Controls.Player.Look.ReadValue<Vector2>();
        }
    }

    private static void AssignActions()
    {
        
    }
}
