using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioData
{
	public AudioClip click;
	public AudioClip tick;
	public AudioClip jump;
	public AudioClip eat;
	public AudioClip keng;
	public AudioClip die;
}


[System.Serializable]
public class PlatformData
{
	public Sprite sprite;
	public float scale = 1;
	public float speed;
}

[System.Serializable]
public class PlayerPurchase
{
	public Sprite playerSprite;
	public Color color;
	public string namePlay;
	public bool buy = false;
	public bool purchase
	{
		get{ return PlayerPrefs.HasKey (namePlay);}
		set{ if (value) {
				PlayerPrefs.SetInt (namePlay, 1);
			} else {
				PlayerPrefs.DeleteKey (namePlay);
			}
			buy = value;
			}
	}
	public int coin;

	public void SetPurchase()
	{
		purchase = buy;
	}
}

[System.Serializable]
public class DataGame : ScriptableObject
{
	public bool testData = false;
	public bool deleteData = false;
	public int score;
	public int coins;
	public int coin
	{
		get{return (PlayerPrefs.HasKey ("coin"))?PlayerPrefs.GetInt ("coin"):0;}
		set{ PlayerPrefs.SetInt ("coin", value);
			coins = value;
		}
	}
	public int playerActiv;
	public int playerActive
	{
		get{ return (PlayerPrefs.HasKey ("playerActive"))?PlayerPrefs.GetInt ("playerActive"):0;}
		set{ PlayerPrefs.SetInt ("playerActive", value);
			playerActiv = value;
		}
	}
	public int playerSelect;
	public int platformActive;
	public PlayerPurchase[] playerPurchase;
	public bool randomPlatform = false;
	public bool fixRadiusPlatform = true;
	public float radiusFix = 0.9f;
	public PlatformData[] platform;
	public AudioData audioData;
}
