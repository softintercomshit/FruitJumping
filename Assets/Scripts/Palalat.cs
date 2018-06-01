using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Palalat : MonoBehaviour
{
	public static float deltaPositionX;
	public float speedScale;
	public float speedStatic = 0;
	public float deltaPalalat;

	Transform _transform;
	float delta;

	void Awake ()
	{
		_transform = GetComponent<Transform> ();
		delta = Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height + GetComponent<SpriteRenderer> ().sprite.bounds.size.x / 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_transform.localPosition = new Vector3 (_transform.localPosition.x - deltaPositionX * speedScale + speedStatic * Time.deltaTime, _transform.localPosition.y, _transform.localPosition.z);
		if (_transform.localPosition.x < -delta) {
			_transform.localPosition = new Vector3 (_transform.localPosition.x + deltaPalalat, _transform.localPosition.y, _transform.localPosition.z);
		}
	}
}
