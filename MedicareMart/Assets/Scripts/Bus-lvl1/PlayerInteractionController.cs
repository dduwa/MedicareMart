using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
 
public class PlayerInteractionController : MonoBehaviour {
     
	public Image crosshairDefault;
	public Image crosshairSelected;
	public GraphicRaycaster graphicRaycaster;
	public FirstPersonController firstPersonController;
 

	// Add these fields to adjust the camera's FOV
    [SerializeField] private Camera playerCamera;
	[SerializeField] private Camera busCam; // Reference to the BusCam camera

    private float originalFOV; // To store the original FOV value

    void Awake ()
	{
		ToggleSelectedCursor (false);
		originalFOV = playerCamera.fieldOfView; // Store the original FOV value
	}

    void Update () 
    {
        PhysicsRaycasts ();
		GraphicsRaycasts ();
    }
     
	public void ToggleSelectedCursor (bool showSelectedCursor)
	{
		crosshairDefault.enabled  = !showSelectedCursor;
		crosshairSelected.enabled = showSelectedCursor;
	}


void PhysicsRaycasts() 
{
    Vector3 centreOfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    float distanceToFireRay = 20;
    Ray centreOfScreenRay = Camera.main.ScreenPointToRay(centreOfScreen);
    RaycastHit hit;

    // Perform the raycast first
    if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay)) 
    {
        // Now that we've confirmed a hit, we can safely access hit.transform
        if (hit.transform.CompareTag("BusSeat"))
        {
            ToggleSelectedCursor(true); // Show that the seat is interactable
            if (Input.GetMouseButtonDown(0)) // On click, sit down or stand up
            {

                SitDown(hit.transform); // Pass the transform of the seat to sit down on
				UIManager.Instance.DisableObjectivePanel(); // Disable the objective panel
                }
        }
        else
        {
            // Check if hit object is an interactable object
            InteractiveObjectBase iob = hit.transform.GetComponent<InteractiveObjectBase>();
            ToggleSelectedCursor(iob != null);
            if (iob != null && Input.GetMouseButtonDown(0))
            {
                iob.Interact();
            }
        }
    }
    else
    {
        ToggleSelectedCursor(false);
    }
}


	void GraphicsRaycasts() 
	{
		PointerEventData eventData = new PointerEventData (EventSystem.current);

		eventData.position = new Vector2 (Screen.width * 0.5f, Screen.height * 0.5f);

		List<RaycastResult> results = new List<RaycastResult> ();

		graphicRaycaster.Raycast (eventData, results);

		bool hitButton = false;

		if (results.Count > 0) 
		{
			for (int i = 0; i < results.Count; i++) 
			{
				Button button = results [i].gameObject.GetComponent<Button> ();

				if (button != null) 
				{
					hitButton = true;

					if (Input.GetMouseButtonDown (0)) button.onClick.Invoke ();
				} 
			}

			if (hitButton) ToggleSelectedCursor (true);
		}
	}

	public void SitDown(Transform seatPosition)
	{
		// Move the player to the seat
		transform.position = seatPosition.position;
   		Quaternion desiredRotation = Quaternion.Euler(-50f, 115f, 10f);
		transform.rotation = desiredRotation;
		// Disable player movement here
		firstPersonController.enabled = false;
		
		playerCamera.transform.position = new Vector3(-60.58f, 3.21f, -3.18f);
    	playerCamera.transform.rotation = Quaternion.Euler(-0.996f, -271.635f, 0.006f);
		
		// Start the second cutscene after a delay
		StartCoroutine(BeginCutsceneAfterDelay(2));
	}

	IEnumerator BeginCutsceneAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		CutsceneController.Instance.StartCutscene(2); // Assuming CutsceneController is a singleton
	}
}