using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
public class PlayerInteractionStore : MonoBehaviour
{
    public Image crosshairDefault;
    public Image crosshairSelected;
    public GraphicRaycaster graphicRaycaster;
    public FirstPersonController firstPersonController;
    [SerializeField] private Camera playerCamera;
    public float interactionDistance = 2f; // Distance within which a player can interact
    private Interactable currentInteractable;

    void Awake()
    {
        ToggleSelectedCursor(false);
        

    }

    void Start()
    {
        GameManager.Instance.TriggerObjective("Check the note left by the manager on the computer.");

    }

    void Update()
    {
        PhysicsRaycasts();
        GraphicsRaycasts();
        HandleInteraction();

    }

    public void ToggleSelectedCursor(bool showSelectedCursor)
    {
        crosshairDefault.enabled = !showSelectedCursor;
        crosshairSelected.enabled = showSelectedCursor;
    }



    void HandleInteraction()
    {
        // Change KeyCode.E to mouse button detection
        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            currentInteractable.Interact();
            currentInteractable = null; // Optionally reset after interaction
        }
    }

    void PhysicsRaycasts()
{
    Vector3 centreOfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    Ray centreOfScreenRay = playerCamera.ScreenPointToRay(centreOfScreen);
    RaycastHit hit;

    if (Physics.Raycast(centreOfScreenRay, out hit, interactionDistance))
    {
        Interactable interactable = hit.collider.GetComponent<Interactable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            ToggleSelectedCursor(true); // Show that the object is interactable
        }
        else
        {
            ToggleSelectedCursor(false);
            currentInteractable = null;
        }
    }
    else
    {
        ToggleSelectedCursor(false);
        currentInteractable = null;
    }
}


    void GraphicsRaycasts()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        bool hitButton = false;

        if (results.Count > 0)
        {
            for (int i = 0; i < results.Count; i++)
            {
                Button button = results[i].gameObject.GetComponent<Button>();

                if (button != null)
                {
                    hitButton = true;

                    if (Input.GetMouseButtonDown(0)) button.onClick.Invoke();
                }
            }

            if (hitButton) ToggleSelectedCursor(true);
        }
    }
}