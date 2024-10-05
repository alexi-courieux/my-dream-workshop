using System;
using AshLight.BakerySim;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string LayerStation = "Station";

    private int _stationMask;
    public static Player Instance { get; private set; }

    public EventHandler OnMove;
    public EventHandler OnStopMove;
    public EventHandler OnPlayerInteract;
    public EventHandler OnPlayerInteractAlt;

    public PlayerItemHandlingSystem HandleSystem { get; private set; }

    private const float MaxInteractionDistance = 1f;

    [SerializeField] private float movementSpeed = 6f;

    [SerializeField] private float characterRotationSpeed = 5f;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform playerPositionForRaycast;

    private CharacterController _characterController;
    private Camera _camera;
    private IFocusable _focusedObject;

    private void Awake()
    {
        _stationMask = LayerMask.GetMask(LayerStation);

        Instance = this;
        _characterController = GetComponent<CharacterController>();
        HandleSystem = GetComponent<PlayerItemHandlingSystem>();
        _camera = Camera.main;
    }

    private void Start()
    {
        InputManager.Instance.OnInteract += InputManager_OnInteract;
        InputManager.Instance.OnInteractAlt += InputManager_OnInteractAlt;
        InputManager.Instance.OnNext += InputManager_OnNext;
        InputManager.Instance.OnPrevious += InputManager_OnPrevious;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnInteract -= InputManager_OnInteract;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        if (CheckForRaycastHit(out RaycastHit hitInfo, new[] {_stationMask}))
        {
            if (hitInfo.transform.TryGetComponent(out IFocusable focusable))
            {
                if (focusable == _focusedObject) return;

                _focusedObject?.StopFocus();
                _focusedObject = focusable;
                focusable.Focus();
                return;
            }
        }

        _focusedObject?.StopFocus();
        _focusedObject = null;
    }

    private void Move()
    {
        Vector3 finalMovement = Vector3.zero;
        Vector2 movementInput = InputManager.Instance.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        const float minimalMovementMagnitude = 0.1f;
        if (movementInput.magnitude >= minimalMovementMagnitude)
        {
            OnMove?.Invoke(this, EventArgs.Empty);
            // Translate towards the camera direction
            moveDirection.y = 0;
            finalMovement += moveDirection * (movementSpeed * Time.fixedDeltaTime);

            // Rotate towards the movement direction
            Quaternion targetRotation =
                Quaternion.LookRotation(Vector3.ProjectOnPlane(moveDirection, Vector3.up), Vector3.up);
            visualTransform.rotation = Quaternion.Lerp(visualTransform.rotation, targetRotation,
                characterRotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            OnStopMove?.Invoke(this, EventArgs.Empty);
        }

        // Apply simplified gravity
        finalMovement += Physics.gravity * Time.fixedDeltaTime;

        _characterController.Move(finalMovement);
    }

    private void InputManager_OnInteract(object sender, EventArgs e)
    {
        if (!CheckForRaycastHit(out RaycastHit hitInfo,
                new[] {_stationMask})) return;
        if (hitInfo.transform.TryGetComponent(out IInteractable interactableComponent))
        {
            interactableComponent.Interact();
            OnPlayerInteract?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InputManager_OnInteractAlt(object sender, EventArgs e)
    {
        if (!CheckForRaycastHit(out RaycastHit hitInfo,
                new[] {_stationMask})) return;
        if (hitInfo.transform.TryGetComponent(out IInteractableAlt interactableComponent))
        {
            interactableComponent.InteractAlt();
            OnPlayerInteractAlt?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InputManager_OnNext(object sender, EventArgs e)
    {
        CheckForRaycastHit(out RaycastHit hitInfo, new[] {_stationMask});
        if (hitInfo.transform.TryGetComponent(out IInteractableNext interactableComponent))
        {
            interactableComponent.InteractNext();
        }
    }

    private void InputManager_OnPrevious(object sender, EventArgs e)
    {
        CheckForRaycastHit(out RaycastHit hitInfo, new[] {_stationMask});
        if (hitInfo.transform.TryGetComponent(out IInteractablePrevious interactableComponent))
        {
            interactableComponent.InteractPrevious();
        }
    }

    private bool CheckForRaycastHit(out RaycastHit hitInfo, int[] layers)
    {
        Ray ray = new Ray(playerPositionForRaycast.position, playerPositionForRaycast.forward);
        bool hit = Physics.Raycast(ray, out hitInfo, MaxInteractionDistance);
        if (!hit) return false;
        int targetLayer = 1 << hitInfo.transform.gameObject.layer;
        return Array.Exists(layers, layer => layer == targetLayer);
    }
}