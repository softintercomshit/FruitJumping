using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
	public DataGame dataGame;
	public PlayerControl player;
	public UiManager uiManager;
	public ItemShop itemShop;
	public float deltaItem;
	public Text buttonBuy;

	void Awake ()
	{
		if (dataGame.testData) {
			SetDataPrefab ();
		} else if(dataGame.deleteData)
		{
			PlayerPrefs.DeleteAll ();
		}
		PlayerPrefs.DeleteAll ();

		dataGame.playerPurchase [0].purchase = true;

		GetComponent <ScrollView> ().scrollX.deltaScroll = new Vector2 (-deltaItem * (dataGame.playerPurchase.Length - 1), 0);
		Create ();
		transform.localPosition = new Vector3 (-transform.GetChild (dataGame.playerActive).localPosition.x, transform.localPosition.y, transform.localPosition.z);
		uiManager.SetCoin ();
	}

	[ContextMenu ("Set Data Buy")]
	public void SetDataPrefab()
	{
		for (int i = 0; i < dataGame.playerPurchase.Length; i++) {
			dataGame.playerPurchase [i].SetPurchase ();
		}
		dataGame.playerActive = dataGame.playerActiv;
		dataGame.coin = dataGame.coins;
	}

	[ContextMenu ("Create Shop")]
	void Create ()
	{
		ItemShop item;
		for (int i = 0; i < dataGame.playerPurchase.Length; i++) {
			
			item = ((GameObject)Instantiate (itemShop.gameObject, transform.position, transform.rotation)).GetComponent<ItemShop> ();
			item.transform.SetParent (transform);
			item.transform.localPosition = new Vector3 (i * deltaItem, 0, 0);
			item.itemImage.sprite = dataGame.playerPurchase [i].playerSprite;
			RectTransform _rectTran = item.itemImage.GetComponent<RectTransform> ();
			_rectTran.sizeDelta = new Vector2 (_rectTran.sizeDelta.y * dataGame.playerPurchase [i].playerSprite.bounds.size.x / dataGame.playerPurchase [i].playerSprite.bounds.size.x, _rectTran.sizeDelta.y);
			item.deltaSelect = deltaItem / 2;
			item.coin.text = dataGame.playerPurchase [i].coin.ToString ();
			item.name.text = dataGame.playerPurchase [i].namePlay;
			item.CheckPurchase ();
		}
	}

	public void OnEnable ()
	{
		transform.localPosition = new Vector3 (-transform.GetChild (dataGame.playerActive).localPosition.x, transform.localPosition.y, transform.localPosition.z);
	}

	public void ButtonPurchase ()
	{
		if (dataGame.playerPurchase [dataGame.playerSelect].purchase) {         // purchased  => select
			dataGame.playerActive = dataGame.playerSelect;
			player.ActivePlayer ();
			uiManager.BackShop ();
		} else {                                       
			if (dataGame.coin < dataGame.playerPurchase [dataGame.playerSelect].coin) {     // not purchase         
				uiManager.Pingcoin ();
			} else {                                                                  // purchase
				PurchaseItem ();
				CheckPurchase ();
			}
		}
	}

	void PurchaseItem ()
	{
		dataGame.coin -= dataGame.playerPurchase [dataGame.playerSelect].coin;
		dataGame.playerPurchase [dataGame.playerSelect].purchase = true;
		uiManager.SetCoin ();
		transform.GetChild (dataGame.playerSelect).gameObject.GetComponent<ItemShop> ().CheckPurchase ();
	}

	public void CheckPurchase ()
	{
		if (dataGame.playerPurchase [dataGame.playerSelect].purchase) {
//			buttonBuy.text = "Select";
			buttonBuy.text = LanguageManager.Instance.localizedStringForKey(key: "Select");
		} else {
			buttonBuy.text = LanguageManager.Instance.localizedStringForKey(key: "Buy");
//			buttonBuy.text = "Buy";
		}
	}

}
