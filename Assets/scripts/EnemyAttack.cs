using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float rateOfFire;
    [SerializeField] private LineRenderer lineRenderer;

    private float _currentCooldown;
    private Transform _player;
    private Collider2D _myCollider;

    private IEnumerator Start()
    {
        while (GameManager.Instance == null) yield return null;
        while (GameManager.Instance.player == null) yield return null;
        _player = GameManager.Instance.player.transform;
        
        _myCollider = GetComponent<Collider2D>();

        // Ignore collisions between this object's collider and itself
        Physics2D.IgnoreCollision(_myCollider, _myCollider, true);
    }
    
    void Update()
    {
        if (_player == null)
        {
            return;
        }
        if (CanSeePlayer()) ShootPlayer();
        _currentCooldown -= Time.deltaTime;
        RenderShotReticle();
    }

    private bool CanSeePlayer()
    {
        float distanceToPlayer = Vector2.Distance(_player.position, transform.position);
        if (distanceToPlayer > range) return false;
        
        Vector2 vectorToPlayer = (_player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            vectorToPlayer.normalized,
            range);
        if (hit.collider != null && hit.collider.CompareTag("Player")) return true;
        return false;
    }

    private void ShootPlayer()
    {
        if (_currentCooldown <= 0)
        {
            Vector2 vectorToPlayer = transform.position - _player.position;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2) transform.position - vectorToPlayer);
            _currentCooldown = rateOfFire;
            _player.gameObject.GetComponent<PlayerHurtbox>().TakeDamage(damage);
        }
    }

    private void RenderShotReticle()
    {
        if (_currentCooldown <= 0f)
        {
            lineRenderer.enabled = false;
            return;
        }
        lineRenderer.enabled = true;
        
        float alpha = Mathf.Clamp01(_currentCooldown / rateOfFire);
        Color color = lineRenderer.startColor;
        color.a = alpha;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}
