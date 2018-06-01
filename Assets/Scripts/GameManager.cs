using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		start,
		play,
		gameOver
	}

	public DataGame dataGame;
	public PlayerControl player;
	public UiManager uiManager;
	public CameraControl cam;
	public GameState gameState = GameState.start;
	public Vector2 deltaPlatform;
	Animation animaLisPlatform;

	Transform listPlatform;

	void Awake ()
	{
		int purchased = PlayerPrefs.GetInt ("purchased");
		if (purchased == 1) {
			hideAdsButtons ();
		}

		listPlatform = GetComponent<Transform> ();
		animaLisPlatform = GetComponent<Animation> ();
		animaLisPlatform.enabled = false;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		if(Input.GetMouseButtonDown (0) && uiManager.animaUI.GetBool ("play") && !WasAButton ())
		{
			TouchGame ();
		}
	}

	private Button GetRestoreButton()
	{
		GameObject restoreButtonGameObject = GameObject.Find("ButtonRestore");
		if (restoreButtonGameObject != null)
		{
			return restoreButtonGameObject.GetComponent<Button>();
		}
		else
		{
			return null;
		}
	}

	private Button GetRemoveAdsButton()
	{
		GameObject restoreButtonGameObject = GameObject.Find("ButtonRemoveAds");
		if (restoreButtonGameObject != null)
		{
			return restoreButtonGameObject.GetComponent<Button>();
		}
		else
		{
			return null;
		}
	}

	private void hideAdsButtons(){
		GetRemoveAdsButton ().gameObject.SetActive (false);
		GetRestoreButton ().gameObject.SetActive (false);
	}

	private bool WasAButton()
	{
		UnityEngine.EventSystems.EventSystem ct
		= UnityEngine.EventSystems.EventSystem.current;

		if (! ct.IsPointerOverGameObject() ) return false;
		if (! ct.currentSelectedGameObject ) return false;
		if (ct.currentSelectedGameObject.GetComponent<Button>() == null )
			return false;

		return true;
	}

	public void TouchGame ()
	{
		if (gameState == GameState.play) {
			if (animaLisPlatform.enabled) {
				animaLisPlatform.enabled = false;
				cam.enabled = true;
			}
			player.Jump ();
		}	
	}

	public void PlayGame ()
	{
		player.gameObject.SetActive (true);
		animaLisPlatform.gameObject.SetActive (true);
		animaLisPlatform.enabled = true;
		if (animaLisPlatform.isPlaying) {
			animaLisPlatform.Stop ();
		}
		animaLisPlatform.Play ();
		dataGame.platformActive = Random.Range (0, dataGame.platform.Length);
		for (int i = 0; i < listPlatform.childCount; i++) {
			Platform platform = listPlatform.GetChild (i).GetComponent<Platform> ();
			platform.enabled = true;
			platform.SetRadius ();
		}
		cam.Start ();
		player.Start ();
	}

	public void RestartGame ()
	{
		gameState = GameState.start;
		PlayGame ();
	}

	public void BeginPlay ()                               // chay xong animation BeginGame;      
	{
		gameState = GameState.play;
		player.enabled = true;
		SetPlatform ();
	}

	public void SetPlatform ()
	{
		listPlatform.gameObject.SetActive (true);
		for (int i = 0; i < listPlatform.childCount; i++) {
			Platform platform = listPlatform.GetChild (i).GetComponent<Platform> ();
			platform.enabled = true;
			platform.ResetIndex ();
			if (i >= 3) {
				platform.deltaPlatform = deltaPlatform;
				platform.SetPlatform ();
			}
		}
	}
}
