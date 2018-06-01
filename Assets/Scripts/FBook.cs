using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

public class FBook : MonoBehaviour
{
	public string linkAndroid = "https://play.google.com/store/apps/details?id=com.vietbrain.SuperKongSaga";
	public string linkIOS = "https://itunes.apple.com/lt/app/super-kong-saga-magic-monkey/id1065736101";
	public string linkPicture = "https://scontent-hkg3-1.xx.fbcdn.net/hphotos-xtp1/v/t1.0-9/11667440_494793770676666_4943243402128542675_n.jpg?oh=dabd1ec68a80665a348c2d161f98bdbd&oe=578AC7AE";
	public string linkAppFb = "https://www.facebook.com/vgfitcom/";

	string linkShare = "";
	void Awake ()
	{
		if(Application.platform == RuntimePlatform.Android){
			linkShare = linkAndroid;
		}else if(Application.platform == RuntimePlatform.IPhonePlayer){
			linkShare = linkIOS;
		}
	}

	private void InitCallback ()
	{

	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
		} else {
			// Resume the game - we're getting focus again
		}
	}

	[DllImport ("__Internal")]
	private static extern void sendFBShare();

	public void ShareLink ()
	{
		sendFBShare ();
	}
}





