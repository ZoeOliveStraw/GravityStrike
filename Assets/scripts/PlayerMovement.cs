using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement stats")]
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float thrusterStrength;
    [SerializeField] private float angleToThruster;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxVelocity;
    
    [Header("Thruster Particle system")]
    [SerializeField] private ParticleSystem thrusterParticles;
    [SerializeField] private Vector2 minMaxParticleRate;
    [SerializeField] private Vector2 minMaxParticleLifetime;
    
    private Vector2 _moveVector;
    private bool _isMoving;
    
    void Update()
    {
        _moveVector = InputManager.MoveVector;
        _isMoving = _moveVector.magnitude > 0.1f;
        ProcessMovement();
        RenderParticles();
    }

    private void ProcessMovement()
    {
        float a = AngleToStickDirection();
        if (_moveVector.magnitude < 0.1f) return;
        RotateTowardsMoveDir();
        if(a < angleToThruster && _isMoving) ApplyThrusters();
    }

    private float AngleToStickDirection()
    {
        float result = Vector2.Angle(transform.up, _moveVector);
        Debug.LogWarning($"MOVEVECTOR: {_moveVector}, FORWARD: {transform.forward} RESULT: {result}");
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
        _rigidBody2D.AddForce(_moveVector * thrusterStrength, ForceMode2D.Impulse);
        if (_rigidBody2D.velocity.magnitude > maxVelocity)
        {
            _rigidBody2D.velocity = _rigidBody2D.velocity.normalized * maxVelocity;
        }
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
