using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(pos: (Vector3)Mouse.current.position.ReadValue()));

        if (!rayHit.collider) return;

        Debug.Log(rayHit.collider.gameObject.name);
    }
}
