using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement stats")]
    [SerializeField] private Rigidbody2D _rigidBody2D;
    [SerializeField] private float thrusterStrength;
    [SerializeField] private float angleToThruster;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float gravityScale;
    
    [Header("Thruster Particle system")]
    [SerializeField] private ParticleSystem thrusterParticles;
    [SerializeField] private Vector2 minMaxParticleRate;
    [SerializeField] private Vector2 minMaxParticleLifetime;
    
    private Vector2 _moveVector;
    private bool _isMoving;
    
    void Update()
    {
        _moveVector = CalculateMovement();
        _isMoving = _moveVector.magnitude > 0.1f;
        ProcessMovement();
        RenderParticles();
        ApplyGravity();
    }

    public Vector2 CalculateMovement() {

        float threshold = 2;
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        // if in a gravity hotspot, move away
        Vector2 current_gravity = GameManager.Instance.GravityFromPosition(position);
        if (current_gravity.magnitude > threshold) {
            return new Vector2(0,0); // temporary
            return - current_gravity;
        } else { 
            // find path to player
            return new Vector2(0,0);
        }
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
        _rigidBody2D.AddForce(_moveVector * (thrusterStrength * Time.deltaTime), ForceMode2D.Impulse);
        if (_rigidBody2D.linearVelocity.magnitude > maxVelocity)
        {
            _rigidBody2D.linearVelocity = _rigidBody2D.linearVelocity.normalized * maxVelocity;
        }
    }
    
    private void ApplyGravity()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 gravityForce = GameManager.Instance.GravityFromPosition(currentPosition) * (gravityScale * Time.deltaTime);
        _rigidBody2D.AddForce(gravityForce, ForceMode2D.Impulse);
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
