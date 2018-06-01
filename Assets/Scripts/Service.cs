using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine.SocialPlatforms;

public class Service : MonoBehaviour {
	public static Service instance;
	public const string idLeadboardIos = "com.appmoss.fruitjumping.scores";
	public const string idLeadboardAndroid = "CgkI7t2-1NsUEAIQBQ";
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
			
		DontDestroyOnLoad (gameObject);
		#if UNITY_ANDROID
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			// enables saving game progress.
			//.EnableSavedGames()
			// registers a callback to handle game invitations received while the game is not running.
			//.WithInvitationDelegate(Match)
			// registers a callback for turn based match notifications received while the
			// game is not running.
			//.WithMatchDelegate(Invita)
			// require access to a player's Google+ social graph to sign in
			.RequireGooglePlus()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
		#endif

	}

	// Use this for initialization
	void Start () {
		Social.localUser.Authenticate (ProcessAuthentication);
		//		#if UNITY_ANDROID
		//		((PlayGamesLocalUser)Social.localUser).GetStats((rc, stats) =>
		//			{
		//				// -1 means cached stats, 0 is succeess
		//				// see  CommonStatusCodes for all values.
		//				if (rc <= 0 && stats.HasDaysSinceLastPlayed()) {
		//					Debug.Log("It has been " + stats.DaysSinceLastPlayed + " days");
		//				}
		//			});
		//		#endif
	}

	void ProcessAuthentication (bool success) {
		if (success) {
//			Debug.Log ("Authenticated, checking achievements");
		} else {
//			Debug.Log ("Failed to authenticate");
		}     
	}

	public void ReportScore (long score, string leaderboardID = 
		#if UNITY_ANDROID
		idLeadboardAndroid
		#endif
		#if UNITY_IOS
		idLeadboardIos
		#endif
	) {
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}

	public void ShowLeadboard()
	{
		Social.ShowLeaderboardUI ();
//		PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIxcv6waICEAIQBg");
	}
}
