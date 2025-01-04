using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxShotForce;
    
    private Vector2 _aimVector;
    private float _currentShotForce = 0;

    void Update()
    {
        _aimVector = InputManager.AimVector;
        Debug.LogWarning($"AIM VECTOR: {_aimVector}");
        if(InputManager.Controls.Player.Attack.IsPressed()) Mathf.Clamp(_currentShotForce += Time.deltaTime, 0, minMaxShotForce.y);
        if(InputManager.Controls.Player.Attack.WasReleasedThisFrame()) FireReleased();
    }

    private void FireReleased()
    {
        
    }
}
