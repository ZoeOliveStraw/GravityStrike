using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement stats")]
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float thrusterStrength;
    [SerializeField] private float angleToThruster;
    [SerializeField] private float rotationSpeed;
    
    [Header("Thruster Particle system")]
    [SerializeField] private ParticleSystem thrusterParticles;
    [SerializeField] private Vector2 minMaxParticleRate;
    [SerializeField] private Vector2 minMaxParticleLifetime;
    
    private Vector2 _moveVector;
    
    void Update()
    {
        _moveVector = InputManager.MoveVector;
        ProcessMovement();
        RenderParticles();
    }

    private void ProcessMovement()
    {
        float a = AngleToStickDirection();
        if (_moveVector.magnitude < 0.1f) return;
        RotateTowardsMoveDir();
        if(a < angleToThruster) ApplyThrusters();
    }

    private float AngleToStickDirection()
    {
        float result = Vector2.Angle(transform.forward, _moveVector);
        Debug.LogWarning($"RESULT: {result}");
        return result;
    }

    private void RotateTowardsMoveDir()
    {
        Vector2 currentDirection = transform.up;
        float angle = Vector2.SignedAngle(currentDirection, _moveVector);
        float rotationAmount = Mathf.LerpAngle(0, angle, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationAmount);
    }

    private void ApplyThrusters()
    {
        
    }

    private void RenderParticles()
    {
        bool isMoving = _moveVector.magnitude > 0.1f;
        if (isMoving && AngleToStickDirection() < angleToThruster)
        {
            if (!thrusterParticles.isPlaying)
            {
                thrusterParticles.Clear();
                thrusterParticles.Play();
            }
            float partStrength = _moveVector.magnitude;
            float partLifetime = minMaxParticleLifetime.x + (minMaxParticleLifetime.y - minMaxParticleLifetime.x) * partStrength;
            float partRate = minMaxParticleRate.x + (minMaxParticleRate.y - minMaxParticleRate.x) * partStrength;
            var main = thrusterParticles.main;
            var emission = thrusterParticles.emission;
            main.startLifetime = partLifetime;
            emission.rateOverTime = partRate;
        }
        else if(thrusterParticles.isPlaying) thrusterParticles.Stop();
    }
}   
