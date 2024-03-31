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


	  // Add these fields to adjust the camera's FOV
    [SerializeField] private Camera playerCamera;

	void Awake ()
	{
		ToggleSelectedCursor (false);
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

 if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay, ~LayerMask.GetMask("SeeThrough"))) 
    {
        InteractiveObjectBase interactableObject = hit.transform.GetComponent<InteractiveObjectBase>();
        
        if (interactableObject != null)
        {
            ToggleSelectedCursor(true); // Show that the object is interactable
            
            if (Input.GetKeyDown(KeyCode.E)) // Check for 'E' key to interact
            {
                interactableObject.Interact(); // Call the interact function on the object
            }
        }
        else
        {
            ToggleSelectedCursor(false);
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





		

}
