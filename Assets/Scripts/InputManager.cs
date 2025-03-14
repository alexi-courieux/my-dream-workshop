using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public EventHandler OnInteract;
    public EventHandler OnInteractAlt;
    public EventHandler OnPause;
    public EventHandler OnNext;
    public EventHandler OnPrevious;
    public event EventHandler OnNextSlot;
    public event EventHandler OnPreviousSlot;

    public EventHandler OnRecipeBook;

    public EventHandler OnMenuCancel;
    public EventHandler OnMenuNext;
    public EventHandler OnMenuPrevious;

    private InputActions _inputActions;
    private InputDevice _lastUsedDevice;

    private void Awake()
    {
        Instance = this;
        _inputActions = new InputActions();
        _inputActions.Enable();

        _inputActions.Player.Interact.performed += Interact_OnPerformed;
        _inputActions.Player.InteractAlt.performed += InteractAlt_OnPerformed;
        _inputActions.Player.PreviousNextRecipe.performed += PreviousNextRecipe_OnPerformed;
        _inputActions.Player.PreviousNextSlot.performed += PreviousNextSlot_OnPerformed;
        
        _inputActions.MenuTransitions.Pause.performed += Pause_OnPerformed;
        _inputActions.MenuTransitions.RecipeBook.performed += RecipeBook_OnPerformed;
        
        _inputActions.Menu.Cancel.performed += MenuCancel_OnPerformed;
        _inputActions.Menu.PreviousNext.performed += MenuPreviousNext_OnPerformed;
        
        InputSystem.onAnyButtonPress.Call(InputSystem_OnAnyButtonPress);
    }

    private void OnDestroy()
    {
        _inputActions.Player.Interact.performed -= Interact_OnPerformed;
        _inputActions.Player.InteractAlt.performed -= InteractAlt_OnPerformed;
        _inputActions.Player.PreviousNextRecipe.performed -= PreviousNextRecipe_OnPerformed;
        _inputActions.Player.PreviousNextSlot.performed -= PreviousNextSlot_OnPerformed;
        
        _inputActions.MenuTransitions.RecipeBook.performed -= RecipeBook_OnPerformed;
        _inputActions.MenuTransitions.Pause.performed -= Pause_OnPerformed;
        
        _inputActions.Menu.Cancel.performed -= MenuCancel_OnPerformed;
        _inputActions.Menu.PreviousNext.performed -= MenuPreviousNext_OnPerformed;
        
        _inputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return _inputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    public Vector2 GetRotationVectorNormalized()
    {
        return _inputActions.Player.Rotate.ReadValue<Vector2>().normalized;
    }

    private void Interact_OnPerformed(InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlt_OnPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlt?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_OnPerformed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void PreviousNextRecipe_OnPerformed(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() > 0)
        {
            OnNext?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnPrevious?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void PreviousNextSlot_OnPerformed(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() > 0)
        {
            OnNextSlot?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnPreviousSlot?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void MenuCancel_OnPerformed(InputAction.CallbackContext obj)
    {
        OnMenuCancel?.Invoke(this, EventArgs.Empty);
    }
    
    private void MenuPreviousNext_OnPerformed(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() > 0)
        {
            OnMenuNext?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnMenuPrevious?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private void RecipeBook_OnPerformed(InputAction.CallbackContext obj)
    {
        OnRecipeBook?.Invoke(this, EventArgs.Empty);
    }
    
    public void DisableGameplayInput()
    {
        _inputActions.Player.Disable();
    }
    
    public void EnableGameplayInput()
    {
        _inputActions.Player.Enable();
    }
    
    public void EnableMenuInput()
    {
        _inputActions.Menu.Enable();
    }
    
    public void DisableMenuInput()
    {
        _inputActions.Menu.Disable();
    }
    
    public void EnableChangeSlotInput()
    {
        _inputActions.Player.PreviousNextSlot.Enable();
    }
    
    public void DisableChangeSlotInput()
    {
        Debug.Log("DisableChangeSlotInput");
        _inputActions.Player.PreviousNextSlot.Disable();
    }
    
    private void InputSystem_OnAnyButtonPress(InputControl obj)
    {
        _lastUsedDevice = obj.device;
    }
}