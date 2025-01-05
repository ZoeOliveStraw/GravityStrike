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


    private IEnumerator Start()
    {
        while (GameManager.Instance == null) yield return null;
        while (GameManager.Instance.player == null) yield return null;
        _player = GameManager.Instance.player.transform;
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
        
        Vector2 vectorToPlayer = (transform.position - _player.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            vectorToPlayer,
            range);
        if (hit.transform.tag == "Player") return true;
        return false;
        
    }

    private void ShootPlayer()
    {
        Debug.LogWarning("SHOOTING PLAYER");
        if (_currentCooldown <= 0)
        {
            Debug.LogWarning("SHOOTING PLAYER");
            Vector2 vectorToPlayer = transform.position - _player.position;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, (Vector2) transform.position + vectorToPlayer);
            _currentCooldown = rateOfFire;
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
        Color startColor = lineRenderer.startColor;
        startColor.a = alpha;

        Color endColor = lineRenderer.endColor;
        endColor.a = alpha;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }
}
