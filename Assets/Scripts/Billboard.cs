using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class Billboard : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float yOffset;

    [SerializeField] private TextMeshProUGUI text;

    private Vector3 initialScale, thresholdScale;
    private float healthYLimit;

    private Enemy enemy;



    void Start()
    {
        cam = Camera.main;
        initialScale = transform.localScale;

        thresholdScale = new(
            initialScale.x * 0.25f,
            initialScale.y * 0.25f,
            initialScale.z * 0.25f
        );


        if (target != null)
        {
            enemy = target.parent.GetComponent<Enemy>();
            enemy.OnDeath += DestroySelf;
        }

    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = Vector3.zero;

            if (enemy)
            {
                float healthRatio = (float)enemy.health / enemy.maxHealth;

                Vector3 newScale = new(
                    initialScale.x * healthRatio,
                    initialScale.y * healthRatio,
                    initialScale.z * healthRatio
                );
                bool isAboveThreshold = newScale.x >= thresholdScale.x && newScale.y >= thresholdScale.y && newScale.z >= thresholdScale.z;

                if (isAboveThreshold)
                {
                    transform.localScale = newScale;
                    targetPosition = new(target.position.x, target.position.y + yOffset * healthRatio, target.position.z);
                    healthYLimit = yOffset * healthRatio;
                }
                else
                {
                    targetPosition = new(target.position.x, target.position.y + healthYLimit, target.position.z);
                }


                text.text = $"HP: {enemy.health} /  {enemy.maxHealth}";

            }

            transform.position = targetPosition;
        }
    }

    void LateUpdate()
    {
        // Ensure the canvas always faces the camera
        transform.LookAt(transform.position + cam.transform.forward);
    }
}
