using UnityEngine;
using UnityEngine.InputSystem;

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
            if (Controls.Player.Look.ReadValue<Vector2>() != Vector2.zero)
            {
                return Controls.Player.Look.ReadValue<Vector2>();
            }
            if (Controls.Player.Aim.IsPressed() && GameManager.Instance != null)
            {
                if (GameManager.Instance.player != null)
                {
                    Vector3 playerPos = 
                        Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);
                    Vector2 pointerPosition = Pointer.current.position.ReadValue();
                    Vector2 direction = (pointerPosition - (Vector2)playerPos).normalized;
                    return direction;
                }
                return Vector2.zero;
            }
            return Vector2.zero;
        }
    }

    private static void AssignActions()
    {
        
    }
}
