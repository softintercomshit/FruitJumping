
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;


public class UnityAdsExample : MonoBehaviour
{
	[HideInInspector]
	public UnityEvent Finish;
	static UnityAdsExample _instan;

	void OnEnable()
	{
		DontDestroyOnLoad (gameObject);
	}

	public static UnityAdsExample Instance
	{
		get{
			if(_instan == null)
			{
				_instan = GameObject.FindObjectOfType <UnityAdsExample> ();
				if (_instan == null) {
					_instan = (new GameObject ()).AddComponent <UnityAdsExample> ();
				}
			}
			return _instan;
		}
	}

	public bool IsReady()
	{
		return Advertisement.IsReady ("rewardedVideo");
	}

	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	public void ShowAd()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			if(Finish != null)
			{
				Finish.Invoke ();
			}
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

}
