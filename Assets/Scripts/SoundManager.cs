using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class ButtonSoundSprite
{
	public Button[] soundButton;
	public Sprite muteSprite;
	public Sprite unMuteSprite;

	public void Mute (bool mute)
	{
		foreach (Button but in soundButton) {
			but.transform.GetChild (0).GetComponent<Image> ().sprite = mute ? (muteSprite) : (unMuteSprite);
		}
	}
}

public class SoundManager : MonoBehaviour
{
	public float volum = 1;
	public ButtonSoundSprite sound;
	public AudioSource[] audioGameList;
	public AudioSource clickButton;
	float[] listVolum;

	// Use this for initialization
	void Awake ()
	{
		GetVolum ();
	}

	void Start ()
	{
		CheckSound ();
	}

	void GetVolum ()
	{
		listVolum = new float[audioGameList.Length];
		for (int i = 0; i < listVolum.Length; i++) {
			listVolum [i] = audioGameList [i].volume;
		}
	}

	void SetVolum (float scaleVolum)
	{
		for (int i = 0; i < listVolum.Length; i++) {
			audioGameList [i].volume = listVolum [i] * scaleVolum;
		}
	}

	public void CheckSound ()
	{
		sound.Mute (volum == 0);
	}

	public void ButtonSound ()
	{
		if (volum == 0) {
			volum = 1;
		} else {
			volum = 0;
		}
		SetVolum (volum);
		CheckSound ();
	}

	public void ClickButton()
	{
		if(clickButton.isPlaying)
		{
			clickButton.Stop ();
		}
		clickButton.Play ();
	}
}
