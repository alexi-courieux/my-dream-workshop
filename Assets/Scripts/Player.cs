using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private const string LayerStation = "Station";
    private const string LayerResourceNode = "ResourceNode";

    private int _stationMask;
    private int _resourceNodeMask;
    public static Player Instance { get; private set; }

    public EventHandler OnMove;
    public EventHandler OnStopMove;
    public EventHandler OnPlayerInteract;
    public EventHandler OnPlayerInteractAlt;

    public PlayerItemHandlingSystem HandleSystem { get; private set; }

    private const float MaxInteractionDistance = 1f;

    [SerializeField] private float movementSpeed = 6f;

    [SerializeField] private float characterRotationSpeed = 8f;
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform playerPositionForRaycast;

    private CharacterController _characterController;
    private IFocusable _focusedObject;

    private void Awake()
    {
        _stationMask = LayerMask.GetMask(LayerStation);
        _resourceNodeMask = LayerMask.GetMask(LayerResourceNode);

        Instance = this;
        _characterController = GetComponent<CharacterController>();
        HandleSystem = GetComponent<PlayerItemHandlingSystem>();
    }

    private void Start()
    {
        InputManager.Instance.OnInteract += InputManager_OnInteract;
        InputManager.Instance.OnUse += InputManager_OnInteractAlt;
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
        RotateTowardsNearestStation();
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
    
    private void RotateTowardsNearestStation()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        int[] poiLayers = {_stationMask, _resourceNodeMask};
        
        if (hitColliders.Length == 0) return;
        float minDistance = float.MaxValue;
        Transform nearestPoi = null;
        foreach (Collider hitCollider in hitColliders)
        {
            if (!Array.Exists(poiLayers, layer => layer == 1 << hitCollider.gameObject.layer)) continue;
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (!(distance < minDistance)) continue;
            
            minDistance = distance;
            nearestPoi = hitCollider.transform;
        }

        if (nearestPoi is null) return;
        
        Vector3 directionToStation = (nearestPoi.position - transform.position).normalized;
        directionToStation.y = 0; // Keep the rotation on the horizontal plane
        Quaternion targetRotation = Quaternion.LookRotation(directionToStation);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, characterRotationSpeed/2 * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 finalMovement = Vector3.zero;
        Vector2 movementInput = InputManager.Instance.GetMovementVectorNormalized() * -1;
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
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
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
                new[] {_stationMask, _resourceNodeMask})) return;
        if (hitInfo.transform.TryGetComponent(out IUseable interactableComponent))
        {
            interactableComponent.Use();
            OnPlayerInteractAlt?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InputManager_OnNext(object sender, EventArgs e)
    {
        if (!CheckForRaycastHit(out RaycastHit hitInfo, new[] {_stationMask})) return;
        
        if (hitInfo.transform.TryGetComponent(out IInteractableNext interactableComponent))
        {
            interactableComponent.InteractNext();
        }
    }

    private void InputManager_OnPrevious(object sender, EventArgs e)
    {
        if (!CheckForRaycastHit(out RaycastHit hitInfo, new[] {_stationMask})) return;
        
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