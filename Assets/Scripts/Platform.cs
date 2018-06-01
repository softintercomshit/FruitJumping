using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
	public DataGame dataGame;
	public float speed = 90;
	[HideInInspector]
	public Vector2 deltaPlatform;
	public bool randomAngle = true;
	[HideInInspector]
	public int index;
	public GameObject listCoin;
	public float highCoinRandom = 1;
	[HideInInspector]
	public float radius;
	public SpriteRenderer render;

	Transform _transform;
	CameraControl cam;
	Transform lastPlatform;
	CircleCollider2D collider;

	// Use this for initialization
	void Awake ()
	{
		cam = Camera.main.GetComponent<CameraControl> ();

		_transform = GetComponent<Transform> ();
		collider = GetComponent<CircleCollider2D> ();
		if (randomAngle) {
			_transform.eulerAngles = new Vector3 (_transform.eulerAngles.x, _transform.eulerAngles.y, Random.Range (0, 360));
		}
		SetRadius ();
	}

	public void SetRadius ()
	{
		int ran;
		if(dataGame.randomPlatform)
		{
			ran = Random.Range (0, dataGame.platform.Length);
			render.sprite = dataGame.platform [ran].sprite;
			speed = dataGame.platform [ran].speed;
		}
		else
		{
			ran = dataGame.platformActive;
			render.sprite = dataGame.platform [dataGame.platformActive].sprite;
			speed = dataGame.platform [dataGame.platformActive].speed;
		}
		if (dataGame.fixRadiusPlatform) {
			collider.radius = dataGame.radiusFix;
			float scale = collider.radius / (render.sprite.bounds.size.y / 2);
			render.transform.localScale = new Vector3 (scale, scale, render.transform.localScale.z);
		}
		else
		{
			render.transform.localScale = new Vector3(dataGame.platform [ran].scale, dataGame.platform [ran].scale, render.transform.localScale.z);
			collider.radius = dataGame.platform [ran].scale * render.sprite.bounds.size.y / 2;
		}
		radius = collider.radius;
	}

	void Start ()
	{
		this.enabled = false;
	}

	public void ResetIndex ()
	{
		index = _transform.GetSiblingIndex ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Rotate ();
		CheckIndex ();
	}

	void Rotate ()
	{
		_transform.eulerAngles = new Vector3 (_transform.eulerAngles.x, _transform.eulerAngles.y, _transform.eulerAngles.z + speed * Time.deltaTime);
	}

	void CheckIndex ()
	{
		if (_transform.GetSiblingIndex () == 3 && _transform.position.x < cam.transform.position.x - cam.widthCamera - radius - 10) {
			PlatformPalalat ();
		}
	}

	void PlatformPalalat ()
	{
		lastPlatform = _transform.parent.GetChild (_transform.parent.childCount - 1);
		_transform.position = new Vector3 (lastPlatform.position.x + deltaPlatform.x, lastPlatform.position.y + Random.Range (-deltaPlatform.y, deltaPlatform.y), _transform.position.z);
		_transform.SetSiblingIndex (_transform.parent.childCount);
		index += _transform.parent.childCount - 3;
		InstateCoin ();
	}

	public void SetPlatform ()
	{
		SetRadius ();
		lastPlatform = _transform.parent.GetChild (_transform.GetSiblingIndex () - 1);
		_transform.position = new Vector3 (lastPlatform.position.x + deltaPlatform.x, lastPlatform.position.y + Random.Range (-deltaPlatform.y, deltaPlatform.y), _transform.position.z);
		InstateCoin ();
	}

	void InstateCoin ()
	{
		if (Random.Range (0, 2) % 2 == 1) {
			for (int i = 0; i < listCoin.transform.childCount; i++) {
				GameObject coin = listCoin.transform.GetChild (i).gameObject;
				if (!coin.activeSelf) {
					coin.SetActive (true);
					coin.transform.position = new Vector3 ((_transform.position.x + lastPlatform.position.x) / 2, (_transform.position.y + lastPlatform.position.y) / 2 + Random.Range (0, highCoinRandom), coin.transform.position.z);
					return;
				}
			}
		}
	}

}
