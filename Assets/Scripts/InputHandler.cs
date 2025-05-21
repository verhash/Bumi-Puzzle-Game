using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;
    public int bookAmount = 3;
    public int backpackAmount = 3;

    public TextMeshProUGUI bookCount;
    public TextMeshProUGUI backpackCount;

    public GameObject bookCheckmark;
    public GameObject backpackCheckmark;

    void Awake()
    {
        mainCamera = Camera.main;
    }

public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(pos: (Vector3)Mouse.current.position.ReadValue()));

        if (!rayHit.collider) return;

        GameObject clickedObject = rayHit.collider.gameObject;

        switch (clickedObject.tag)
        {
            case "Book":
                if (bookAmount > 0)
                {
                    bookAmount--;
                    Destroy(clickedObject);
                }
                break;

            case "Backpack":
                if (backpackAmount > 0)
                {
                    backpackAmount--;
                    Destroy(clickedObject);
                }
                break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        // Book UI update
        if (bookAmount <= 0)
        {
            bookCount.gameObject.SetActive(false);
            bookCheckmark.SetActive(true);
        }
        else
        {
            bookCount.text = bookAmount.ToString();
        }

        // Backpack UI update
        if (backpackAmount <= 0)
        {
            backpackCount.gameObject.SetActive(false);
            backpackCheckmark.SetActive(true);
        }
        else
        {
            backpackCount.text = backpackAmount.ToString();
        }

        if (bookAmount <=0 && backpackAmount <= 0)
        {
            Debug.Log("Level finished!");
            // You can call SceneManager.LoadScene here when ready
        }
    }
}
