using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }

    public event EventHandler OnSpacebarAction;
    public event EventHandler OnDashAction;

    private PlayerController playerController;

    private void Awake() {
        Instance = this;
        playerController = new PlayerController();
    }

    private void OnEnable() {
        playerController.Enable();

        playerController.Player.Space.performed += Space_performed;
        playerController.Player.Dash.performed += Dash_performed;
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void Space_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSpacebarAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable() {
        playerController.Disable();
    }

    // Get InputVector Values for x axis from PlayerController Class
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerController.Player.Movement.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }
}
