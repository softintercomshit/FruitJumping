using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
	public DataGame dataGame;
	public GameManager gameManager;
	public UiManager uiManager;
	public float speedJump;
	public float speedScalePlatform;
	public float speedTurn;
	public float gravity;
	public bool jump = false;
	public GameObject listPlatform;
	public GameObject listAddCoin;
	[HideInInspector]
	public float radius;
	public float deltaRadius = 0.95f;
	public SpriteRenderer render;
	public ParticleSystem particle;

	GameObject currentPlatform;
	Vector2 velocity;
	Transform _transform;
	CameraControl cam;
	AudioSource audio;
	CircleCollider2D collider;
	RaycastHit2D hit;
	Vector3 posi;

	void Awake ()
	{
		_transform = GetComponent<Transform> ();
		cam = Camera.main.GetComponent<CameraControl> ();
		collider = GetComponent<CircleCollider2D> ();
		audio = GetComponent<AudioSource> ();
		collider.enabled = false;
		_transform.GetChild (0).gameObject.SetActive (false);
	}

	public void Start ()
	{
		cam.BackGround.RandomGround ();
		StartGame ();
		this.enabled = false;
	}

	public void ActivePlayer ()     // purchase
	{
		render.enabled = true;
		render.sprite = dataGame.playerPurchase [dataGame.playerActive].playerSprite;
		particle.startColor = dataGame.playerPurchase [dataGame.playerActive].color;
		float scale = collider.radius / (render.sprite.bounds.size.y / 2) / deltaRadius;
		radius = collider.radius;
		render.transform.localScale = new Vector3 (scale, scale, render.transform.localScale.z);
	}

	// Use this for initialization
	void StartGame ()
	{
		ActivePlayer ();
		dataGame.score = 0;
		uiManager.SetScore ();
		SetParentPlayer (listPlatform.transform.GetChild (0).gameObject);
		_transform.GetChild (0).gameObject.SetActive (true);
		jump = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (jump) {
			MovePlay ();
		} else {
			cam.LerpCamera (_transform.parent.gameObject);
		}
		CheckTriggerCam ();
	}

	public void Jump ()
	{
		if (!jump) {
			velocity = (Vector2)_transform.right * (speedJump - _transform.GetComponentInParent <Platform>().speed * speedScalePlatform);
			PlayAudio (dataGame.audioData.jump);
			_transform.parent = null;
			jump = true;
		}
	}

	void MovePlay ()
	{
		if (_transform.position.y < cam.transform.position.y - Camera.main.orthographicSize - radius) {
			if (audio.clip != dataGame.audioData.die && uiManager.animaUI.GetBool ("play")) {
				PlayAudio (dataGame.audioData.die);
				PlayOver (false);
				uiManager.GameOver ();
				this.enabled = false;
			}
		}
		velocity = new Vector2 (velocity.x, velocity.y - gravity * Time.deltaTime);
		posi = new Vector3 (_transform.position.x + velocity.x * Time.deltaTime, _transform.position.y + velocity.y * Time.deltaTime, _transform.position.z);
		hit = Physics2D.CircleCast (posi, radius, Vector3.zero);
		if (hit.collider != null && hit.collider.tag == "Coin") {                  // add coin
			hit.collider.gameObject.SetActive (false);
			AddCoin (1, hit.collider.gameObject.transform.position);
		}
		if (hit.collider != null && hit.collider.tag == "Platform") {          // Trigger Platform
			SetParentPlayer (hit.collider.gameObject);
			return;
		}
		_transform.position = posi;
		_transform.eulerAngles = new Vector3 (_transform.eulerAngles.x, _transform.eulerAngles.y, _transform.eulerAngles.z + speedTurn * Time.deltaTime);

	}

	void SetParentPlayer (GameObject parent)
	{
		jump = false;
		currentPlatform = parent;
		_transform.eulerAngles = new Vector3 (-transform.eulerAngles.x, _transform.eulerAngles.y, Mathf.Atan2 (parent.transform.position.y - _transform.position.y, parent.transform.position.x - _transform.position.x) * Mathf.Rad2Deg + 90);
		_transform.parent = parent.transform;
		_transform.localPosition = _transform.localPosition.normalized * (radius + parent.GetComponent<Platform> ().radius) / parent.transform.localScale.x;
		dataGame.score = parent.GetComponent<Platform> ().index;
		uiManager.SetScore ();
		if (gameManager.gameState == GameManager.GameState.play) {
			PlayAudio (dataGame.audioData.tick);
		}
	}

	public void SetResume()
	{
		SetParentPlayer (currentPlatform);
		PlayOver (true);
		cam.transform.position = new Vector3 (currentPlatform.transform.position.x, cam.transform.position.y, cam.transform.position.z);
	}

	void CheckTriggerCam ()
	{
		if (_transform.position.x < cam.transform.position.x - cam.widthCamera && uiManager.animaUI.GetBool ("play")) {
			if (!jump) {
				jump = true;
			}
			velocity = Vector2.right * (speedJump + cam.speed);
			_transform.parent = null;
			listPlatform.SetActive (false);
			PlayAudio (dataGame.audioData.keng);
		}
	}

	void PlayOver(bool bol)
	{
		_transform.GetChild (0).gameObject.SetActive (bol);
		listPlatform.SetActive (bol);
		render.enabled = bol;
		cam.enabled = bol;
	}

	void PlayAudio (AudioClip clip)
	{
		if (audio.isPlaying) {
			audio.Stop ();
		}
		audio.clip = clip;
		audio.Play ();
	}

	void AddCoin (int coin, Vector3 posi)
	{
		dataGame.coin += coin;
		uiManager.SetCoin ();
		for (int i = 0; i < listAddCoin.transform.childCount; i++) {
			if (!listAddCoin.transform.GetChild (0).GetChild (0).gameObject.activeSelf) {
				listAddCoin.transform.GetChild (0).position = posi;
				listAddCoin.transform.GetChild (0).GetComponent<Animation> ().Play ();
				return;
			}
		}
	}
}
