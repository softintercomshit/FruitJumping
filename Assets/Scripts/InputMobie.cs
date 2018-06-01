using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputMobie : MonoBehaviour
{
	public float speedScale = 0.2f;
	public ScrollView scroll;
	public bool eventTrigger = true;
	float beginX;
	float camWith;

	void Awake ()
	{
		camWith = Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			scroll.touchOn = true;
			scroll.velocityInput = Vector3.zero;
			beginX = Input.mousePosition.x;
		} else if (Input.GetMouseButton (0)) {
			scroll.velocityInput = new Vector3 ((Input.mousePosition.x - beginX) * speedScale / camWith, 0, 0);
			beginX = Input.mousePosition.x;	
		} else if (Input.GetMouseButtonUp (0)) {
			scroll.TouchEnd ();
			scroll.velocityInput = Vector3.zero;
		}
	}

}
