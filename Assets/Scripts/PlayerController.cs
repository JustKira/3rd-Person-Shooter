using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotateSpeed = 10f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI alertTextTMP;

    [Header("Boundary Detection")]
    [SerializeField] private float maxDistance = 0.1f; // 10cm
    private bool closeToBorder = false;
    private float distanceFromBorder = 0f;
    private LayerMask boundaryLayer;

    [Header("Gun")]
    [SerializeField] private Gun gun;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private InputAction moveAction;
    private InputAction shootAction;
    private GameObject bulletsHolder;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        CreateBulletsHolder();
    }

    void Update()
    {
        ProcessMovement();
        ProcessRotation();
        CheckProximityToBoundary();
        UpdateUI();
    }


    private void InitializeComponents()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        shootAction = playerInput.actions["Fire"];
        Cursor.lockState = CursorLockMode.Locked;
        boundaryLayer = LayerMask.GetMask("Boundary");
        controller.enabled = true;
    }

    private void OnEnable()
    {
        shootAction.performed += _ => gun.Shoot(cameraTransform);
        GameManager.instance.OnGameWon += StopInputs;
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => gun.Shoot(cameraTransform);
    }

    private void StopInputs()
    {
        controller.enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }


    private void ProcessMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void ProcessRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    private void CreateBulletsHolder()
    {
        if (bulletsHolder == null)
        {
            bulletsHolder = new GameObject("BulletsHolder");
        }
    }

    private void CheckProximityToBoundary()
    {

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, maxDistance, transform.forward, maxDistance, boundaryLayer);
        closeToBorder = hits.Length > 0;
        if (closeToBorder)
        {
            distanceFromBorder = Mathf.Infinity;
            foreach (RaycastHit hit in hits)
            {
                if (hit.distance < distanceFromBorder)
                {
                    distanceFromBorder = hit.distance;
                }
            }
        }
    }

    private void UpdateUI()
    {
        if (closeToBorder)
        {
            alertTextTMP.text = $"LEAVING BOUNDS";
        }
        else
        {
            alertTextTMP.text = "";
        }
    }

}
