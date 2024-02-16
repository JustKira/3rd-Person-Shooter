using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float speed = 30f;
    private float timeToDestroy = 5f;
    [SerializeField]
    private float raycastLength = 1f; // Length of the raycast behind the bullet

    public Vector3 target;
    public bool hit;

    private bool targetReached;
    private Vector3 direction;
    private RaycastHit[] hits = new RaycastHit[10]; // Buffer for multiple hits
    private LayerMask enemyLayer;

    private void Start()
    {
        direction = (target - transform.position).normalized;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        if (!targetReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.5f)
            {
                targetReached = true;
            }
        }
        else
        {
            transform.position += direction * speed * Time.deltaTime;
        }


        int numHits = Physics.RaycastNonAlloc(transform.position - direction * raycastLength, direction, hits, raycastLength + 0.1f, enemyLayer);
        for (int i = 0; i < numHits; i++)
        {
            // Ensure we don't hit ourselves
            if (hits[i].collider.gameObject != gameObject)
            {
                Enemy enemy = hits[i].collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}
