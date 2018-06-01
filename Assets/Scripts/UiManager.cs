using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class UiManager: MonoBehaviour
{
	public DataGame dataGame;
	public GameManager gameManager;
	public PlayerControl playControl;
	public ReviveGame revive;
	public Text scorePlay;
	public Text coinPlay;
	public Text scoreOver;
	public Text bestOver;
	public Animator animaUI;
	public Text gameOverText;

	// Use this for initialization
	void Awake ()
	{
		animaUI = GetComponent<Animator> ();
		revive.NoRevive.AddListener (ShowGameOver);
		revive.ReviveCoin.AddListener (ReviveCoin);
		revive.ReviveVideo.AddListener (ReviveVideo);
	}
		
	void Start ()
	{
		animaUI.SetBool ("start", true);
		SetCoin ();
	}

	public void ButtonPlay ()
	{
		GoogleMobileAdsDemoScript.instance.ShowBanner ();
		revive.numberCoin = revive.coinRevive;
		animaUI.SetBool ("start", false);
		animaUI.SetBool ("play", true);
	}

	[DllImport ("__Internal")]
	private static extern void sendGameOver();

	public void GameOver ()
	{
		if(!CheckRevive ())
		{
			ShowGameOver ();
			try{
				sendGameOver ();
			}catch{}
		}
	}
		
	public void ShowGameOver()
	{
		animaUI.SetBool ("play", false);
		animaUI.SetBool ("gameOver", true);
		if (!PlayerPrefs.HasKey ("best") || PlayerPrefs.GetInt ("best") < dataGame.score) {
			PlayerPrefs.SetInt ("best", dataGame.score);
		}

		string scoreText = LanguageManager.Instance.localizedStringForKey (key: "Score");
		scoreText += " :" + dataGame.score.ToString ();
		scoreOver.text = scoreText;
//		scoreOver.text = "Score : " + dataGame.score.ToString ();

		string bestOverText = LanguageManager.Instance.localizedStringForKey (key: "Best");
		bestOverText += " :" + PlayerPrefs.GetInt ("best").ToString ();
//		bestOver.text = "Best : " + PlayerPrefs.GetInt ("best").ToString ();
		bestOver.text = bestOverText;

		gameOverText.text = LanguageManager.Instance.localizedStringForKey (key: "Game over");

		print(gameOverText);
		Service.instance.ReportScore (dataGame.score);
		GoogleMobileAdsDemoScript.instance.HideBanner ();
	}

	public void ReviveCoin()
	{
		dataGame.coin -= revive.numberCoin;
		SetCoin ();
		ResumeGame ();
	}

	public void ReviveVideo()
	{
		UnityAdsExample.Instance.ShowRewardedAd ();
		UnityAdsExample.Instance.Finish.AddListener (ResumeGame);
	}

	void ResumeGame()
	{
		playControl.enabled = true;
		playControl.SetResume ();
	}

	public bool CheckRevive()
	{
		GoogleMobileAdsDemoScript.instance.ShowInterstitial ();
		if(dataGame.score <=0)
		{
			revive.gameObject.SetActive (false);
			return false;
		}
		if(dataGame.coin >= revive.numberCoin && (UnityAdsExample.Instance.IsReady () && Random.Range (0,100)< revive.randomVideo))
		{
			revive.gameObject.SetActive (true);
			revive.SetRevive (ReviveType.Coin_Video, revive.numberCoin);
			return true;
		}
		else if(dataGame.coin >= revive.numberCoin)
		{
			revive.gameObject.SetActive (true);
			revive.SetRevive (ReviveType.Coin, revive.numberCoin);
			return true;
		}
		else if((UnityAdsExample.Instance.IsReady () && Random.Range (0,100)< revive.randomVideo))
		{
			revive.gameObject.SetActive (true);
			revive.SetRevive (ReviveType.Video);
			return true;
		}
		revive.gameObject.SetActive (false);
		return false;
	}

	public void ButtonRestart ()
	{
		revive.numberCoin = revive.coinRevive;
		GoogleMobileAdsDemoScript.instance.ShowBanner ();
		animaUI.SetBool ("gameOver", false);
		animaUI.SetBool ("play", true);
	}

	public void ButtoShop ()
	{
		animaUI.SetBool ("shop", true);
	}

	public void BackShop ()
	{
		animaUI.SetBool ("shop", false);
	}

	public void SetScore ()
	{
		scorePlay.text = dataGame.score.ToString ();
	}

	public void SetCoin ()
	{
		coinPlay.text = dataGame.coin.ToString ();
	}

	public void Pingcoin ()
	{
		animaUI.SetTrigger ("ping");
	}
}
