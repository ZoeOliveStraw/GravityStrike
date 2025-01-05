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

    public int gravity_threshold_1 = 20; // amount of gravity that will cause enemy to move perpendicular to reach player
    public int gravity_threshold_2 = 40; // amount of gravity that will cause enemy to flee gravity completely
    
    void Update()
    {
        _moveVector = CalculateMovement();
        _isMoving = _moveVector.magnitude > 0.1f;
        ProcessMovement();
        RenderParticles();
        ApplyGravity();
    }

    public Vector2 CalculateMovement() {

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        // if in a gravity hotspot, move away
        Vector2 current_gravity = GameManager.Instance.GravityFromPosition(position);
        if (GameManager.Instance.player == null) return Vector2.zero;
        Vector2 player_position = GameManager.Instance.player.transform.position;
        Vector2 player_direction = (player_position - position).normalized;
        Vector2 gravity_direction = current_gravity.normalized;

        if (current_gravity.magnitude > gravity_threshold_2) {
            return - current_gravity.normalized * maxVelocity;
        } else if (current_gravity.magnitude > gravity_threshold_1) { 
            // Check if the angle between the vectors is less than 90 degrees
            float dotProduct = Vector2.Dot(player_direction, gravity_direction);

            if (dotProduct > 0) 
            {
                Vector2 perpendicular1 = new Vector2(-gravity_direction.y, gravity_direction.x); // +90 degrees
                Vector2 perpendicular2 = new Vector2(gravity_direction.y, -gravity_direction.x); // -90 degrees

                // Determine which perpendicular vector is closer to the player's direction
                float dot1 = Vector2.Dot(player_direction, perpendicular1);
                float dot2 = Vector2.Dot(player_direction, perpendicular2);

                Vector2 closerPerpendicular = dot1 > dot2 ? perpendicular1 : perpendicular2;

                return closerPerpendicular.normalized * maxVelocity * 0.5f;
            } else {
                return - current_gravity.normalized * maxVelocity;
            }
        } else {
            return player_direction * maxVelocity * 0.5f;
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
