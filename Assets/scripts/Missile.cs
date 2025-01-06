using System;
using Unity.VisualScripting;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float gravityScale;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float homingForce;
    [SerializeField] private float homingRange;
    
    private bool _homing = false;
    private Transform _homingTarget;
    
    public void FireZeMissile(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        CheckForEnemiesInRange();
        if (_homing)
        {
            Debug.LogWarning("HOMING");
            Vector2 homingDirection = _homingTarget.position - transform.position;
            rb.AddForce(homingDirection * homingForce, ForceMode2D.Impulse);
        }
        ApplyGravity();
        RotateMissile();
    }

    private void ApplyGravity()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 gravityForce = GameManager.Instance.GravityFromPosition(currentPosition) * (gravityScale * Time.deltaTime);
        rb.AddForce(gravityForce, ForceMode2D.Impulse);
    }

    private void RotateMissile()
    {
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
        rb.rotation = angle -90;
    }

    private void CheckForEnemiesInRange()
    {
        // Perform a circle check and get all colliders within the radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, homingRange);

        foreach (Collider2D collider in colliders)
        {
            // Check if the object has the desired tag
            if (collider.CompareTag("Enemy"))
            {
                _homingTarget = collider.transform;
                _homing = true;
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            other.transform.gameObject.GetComponent<EnemyHurtBox>().death();
        }
        ExplodeMissile();
    }

    private void ExplodeMissile()
    {
        GameManager.Instance.PlaySoundEffect(GameManager.SoundEffects.MissileExplode);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        GameManager.Instance.ShakeCamera(0.2f, 0.5f);
        Destroy(gameObject);
    }
}
