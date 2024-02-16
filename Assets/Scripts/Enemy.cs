using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Properties")]
    public int maxHealth = 100;
    public int health;
    private Vector3 initialScale;
    private Vector3 thresholdScale;

    [Header("Movement Properties")]
    public Vector3 center = Vector3.zero;
    public float squareRadius = 2f;
    public float moveSpeed = 2.0f;
    public float moveThreshold = 1f;

    private Vector3 targetPosition;

    public event Action OnDeath;

    void Start()
    {
        health = maxHealth;
        initialScale = transform.localScale;
        thresholdScale = new Vector3(
            initialScale.x * 0.1f,
            initialScale.y * 0.1f,
            initialScale.z * 0.1f
        );

        GenerateNewPosition();
    }

    void Update()
    {
        MoveToPosition();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        float healthRatio = (float)health / maxHealth;
        Vector3 newScale = new Vector3(
            initialScale.x * healthRatio,
            initialScale.y * healthRatio,
            initialScale.z * healthRatio
        );

        bool isAboveThreshold = newScale.x >= thresholdScale.x && newScale.y >= thresholdScale.y && newScale.z >= thresholdScale.z;
        if (isAboveThreshold)
        {
            transform.localScale = newScale;
        }

        if (health <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject, 0.5f);
        }
    }

    private void MoveToPosition()
    {
        if (Vector3.Distance(transform.position, targetPosition) < moveThreshold)
        {
            GenerateNewPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void GenerateNewPosition()
    {
        float safeZoneMargin = squareRadius * 0.05f;

        float minBoundary = center.x - (squareRadius - safeZoneMargin);
        float maxBoundary = center.x + (squareRadius - safeZoneMargin);

        float randomX = UnityEngine.Random.Range(minBoundary, maxBoundary);
        float randomZ = UnityEngine.Random.Range(minBoundary, maxBoundary);

        targetPosition = new Vector3(randomX, transform.position.y, randomZ);

        if (Vector3.Distance(center, targetPosition) > squareRadius - safeZoneMargin)
        {
            GenerateNewPosition();
        }
    }
}
