using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class RandomBackground
{
	public GameObject[] ListBackGround;
	public Color[] ListColor;
	public Text[] ListTextColor;

	public void RandomGround ()
	{
		int ran = Random.Range (0, ListBackGround.Length);
		for (int i = 0; i < ListBackGround.Length; i++) {
			if (i == ran) {
				ListBackGround [i].SetActive (true);
			} else {
				ListBackGround [i].SetActive (false);
			}
		}
		for (int i = 0; i < ListTextColor.Length; i++) {
			ListTextColor [i].color = ListColor [ran];
		}
	}
}

public class CameraControl : MonoBehaviour
{
	public RandomBackground BackGround;
	public float speed;
	[Range (0, 1)]
	public float speedLerpPlayer;
	public PlayerControl player;
	public GameManager gameManager;
	public float widthCamera;

	float deltaPlayer;
	Transform _transform;
	Vector3 velocity = Vector3.zero;
	Vector3 currentPosition;

	// Use this for initialization
	void Awake ()
	{
		_transform = GetComponent<Transform> ();
		widthCamera = Camera.main.orthographicSize * (float)Screen.width / (float)Screen.height;
		deltaPlayer = widthCamera / 3;
	}

	public void Start ()
	{
		_transform.position = new Vector3 (0, 0, _transform.position.z);
		this.enabled = false;
	}

	public void OnDisable ()
	{
		Palalat.deltaPositionX = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveCamera ();
		if (player.transform.position.x - _transform.position.x > deltaPlayer) {
			Lerpplayer ();
		}
		Palalat.deltaPositionX = _transform.position.x - currentPosition.x;
	}

	void LateUpdate ()
	{
		currentPosition = _transform.position;
	}

	void MoveCamera ()
	{
		_transform.position = new Vector3 (_transform.position.x + speed * Time.deltaTime, _transform.position.y, _transform.position.z);
	}

	public void LerpCamera (GameObject target)
	{
		_transform.position = Vector3.SmoothDamp (_transform.position, new Vector3 (_transform.position.x, target.transform.position.y, _transform.position.z), ref velocity, speedLerpPlayer);
	}

	void Lerpplayer ()
	{
		_transform.position = Vector3.Lerp (_transform.position, new Vector3 (player.transform.position.x - deltaPlayer, _transform.position.y, _transform.position.z), speedLerpPlayer);
	}
}
