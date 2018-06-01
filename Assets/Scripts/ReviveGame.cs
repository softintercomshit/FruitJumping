using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public enum ReviveType
{
	Coin,
	Video,
	Coin_Video
}

public class ReviveGame : MonoBehaviour
{
	public int coinRevive = 50;
	[Range(0, 100)]
	public float randomVideo = 50;
	public GameObject coin;
	public GameObject video;
	public RectTransform timeCurrent;
	public Text textCoin;

	public Text reviveText;
	public Text watchVideoText;
	public Text payCoinText;

	public float timeRevive = 4;
	public UnityEvent NoRevive;
	public UnityEvent ReviveCoin;
	public UnityEvent ReviveVideo;
	public int numberCoin;
	float currentTime;
	Vector3 tranCoin;
	Vector3 tranVideo;

	void Awake()
	{
		reviveText.text = LanguageManager.Instance.localizedStringForKey (key: "REVIVE?");
		watchVideoText.text = LanguageManager.Instance.localizedStringForKey (key: "Watch Video?");
		payCoinText.text = LanguageManager.Instance.localizedStringForKey (key: "Pay Coin?");

		tranCoin = coin.gameObject.transform.localPosition;
		tranVideo = video.gameObject.transform.localPosition;
		numberCoin = coinRevive;

	}

	public void SetRevive(ReviveType type)
	{
		currentTime = timeRevive;
		textCoin.text = numberCoin.ToString ();
		if(type == ReviveType.Coin)
		{
			coin.gameObject.SetActive (true);
			video.gameObject.SetActive (false);
			coin.gameObject.transform.localPosition = new Vector3 (0, tranCoin.y, tranCoin.z);
		}
		else if(type == ReviveType.Video)
		{
			video.gameObject.SetActive (true);
			coin.gameObject.SetActive (false);
			video.gameObject.transform.localPosition = new Vector3 (0, tranVideo.y, tranVideo.z);
		}
		else if(type == ReviveType.Coin_Video)
		{
			coin.gameObject.SetActive (true);
			video.gameObject.SetActive (true);
			coin.gameObject.transform.localPosition = tranCoin;
			video.gameObject.transform.localPosition = tranVideo;
		}
		StartCoroutine (TimeRevive());
	}

	public void SetRevive(ReviveType type, int number)
	{
		numberCoin = number;
		SetRevive (type);
	}

	public void SetRevive(ReviveType type, float timeRev, int number)
	{
		numberCoin = number;
		timeRevive = timeRev;
		SetRevive (type);
	}

	IEnumerator TimeRevive()
	{
		while (currentTime > 0)
		{
			currentTime -= Time.deltaTime;
			timeCurrent.localScale = new Vector3 (currentTime / timeRevive, timeCurrent.localScale.y, timeCurrent.localScale.z);
			yield return new WaitForEndOfFrame ();
		}
		ButtonExit ();
	}

	public void ClickCoin()
	{
		if(ReviveCoin != null)
		{
			ReviveCoin.Invoke ();
			numberCoin += coinRevive;
		}
		gameObject.SetActive (false);
	}

	public void ClickVideo()
	{
		if(ReviveVideo != null)
		{
			ReviveVideo.Invoke ();
		}
		gameObject.SetActive (false);
	}

	public void ButtonExit()
	{
		if (NoRevive != null) {
			NoRevive.Invoke ();
		}
		gameObject.SetActive (false);
	}
}
