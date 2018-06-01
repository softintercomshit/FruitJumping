using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;


public class LanguageManager
{
	private static LanguageManager instance;

	private LanguageManager() {}

	public static LanguageManager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new LanguageManager();
			}
			return instance;
		}
	}
		
	public void changeTextForElement(string key, string elementName)
	{
		try{
			GameObject item = GameObject.Find(elementName);
//			Debug.Log(item);
			Graphic element = item.GetComponent<Graphic>();
			element.GetComponentInChildren<Text>().text = localizedStringForKey(key);
		}catch(Exception e){
			Debug.Log (e);
		}
	}

	public void changeImageForElement(string key, string elementName){
		try{
			GameObject item = GameObject.Find(elementName);
			Image image = item.GetComponent<Image>();
//			if(image == null)
//				image = item.GetComponent<Graphic>().GetComponentInChildren<Image>();

			Sprite newSprite = Resources.Load<Sprite>(localizedStringForKey(key));
			if (newSprite == null)
				image.sprite = image.sprite;
			else
				image.sprite = newSprite;
		}catch(Exception e){
			Debug.Log (e);
		}
	}

	public String localizedStringForKey(string key){
		// get device language
		string deviceLanguage = PlayerPrefs.GetString ("deviceLanguage");
		if(deviceLanguage.Length == 0)
			deviceLanguage = "en";
		deviceLanguage = "ru";

		string jsonPath = "Languages/" + deviceLanguage + ".json";

		if (!File.Exists(jsonPath))
			jsonPath = "Languages/en.json";
			
		String jsonString = File.ReadAllText (jsonPath);

		Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>> (jsonString);

		string newName = key;
		try{
			newName = dict [key];
		}catch(Exception e){
			Debug.Log (e);
		}

//		Debug.Log (newName);

		return newName;
	}
}