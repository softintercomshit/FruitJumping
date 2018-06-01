using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
	Animator animaUI;
	Transform _transform;
	CameraControl cam;
	float radius;

	// Use this for initialization
	void Awake ()
	{
		cam = Camera.main.GetComponent<CameraControl> ();
		_transform = GetComponent<Transform> ();
		radius = GetComponent<CircleCollider2D> ().radius * _transform.localScale.y;
		animaUI = GameObject.FindGameObjectWithTag ("UI").GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckIndex ();
	}

	void CheckIndex ()
	{
		if (_transform.position.x < cam.transform.position.x - cam.widthCamera - radius || animaUI.GetBool ("gameOver")) {
			gameObject.SetActive (false);
		}
	}
}
