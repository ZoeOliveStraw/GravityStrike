using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float thrusterStrength;
    
    private Vector2 _moveVector;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        _moveVector = InputManager.MoveVector;
        Debug.LogWarning($"MOVE VECTOR: {_moveVector}");
    }
}
