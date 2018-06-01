using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[System.Serializable]
public class UrlPlatform{
	public string ios;
	public string android;

	public void OpenURL()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			Application.OpenURL (android);
		}
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Application.OpenURL (ios);
		}
	}
}

public class OpenURL : MonoBehaviour {
	public UrlPlatform moreGame;
	public UrlPlatform rateGame;
	public UrlPlatform likeGame;

	[DllImport ("__Internal")]
	private static extern void sendRateApp();

	public void RateGame()
	{
		sendRateApp ();
//		rateGame.OpenURL ();
	}

	public void MoreGame()
	{
		moreGame.OpenURL ();
	}

	public void LikeGame()
	{
		likeGame.OpenURL ();
	}
}
