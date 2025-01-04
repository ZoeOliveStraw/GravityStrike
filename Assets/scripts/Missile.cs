using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float gravityScale;
    
    public void FireZeMissile(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Update()
    {
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
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
