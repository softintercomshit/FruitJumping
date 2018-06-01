using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour
{
	public DataGame dataGame;
	public Image itemImage;
	public GameObject purchase;
	public Text coin;
	public Text name;
	public Color activeColor;
	public Color deActiveColor;
	[HideInInspector]
	public float deltaSelect;
	public Animator animator;

	float deltaX;

	void Update ()
	{
		CheckSelectItem ();
	}

	public void ActiveItem (bool active)
	{
		if (active) {
			itemImage.color = activeColor;
		} else {
			itemImage.color = deActiveColor;
		}
		purchase.SetActive (!active);
	}

	public void CheckPurchase ()
	{
		if (dataGame.playerPurchase [transform.GetSiblingIndex ()].purchase) {
			ActiveItem (true);
		} else {
			ActiveItem (false);
		}
	}

	void CheckSelectItem ()
	{
		deltaX = transform.localPosition.x + transform.parent.localPosition.x;
		if (deltaX > -deltaSelect && deltaX <= deltaSelect) {                    // select
			SelectItem (true);
			dataGame.playerSelect = transform.GetSiblingIndex ();  
			transform.parent.gameObject.GetComponent<ShopManager> ().CheckPurchase ();             
		} else {                                                                  // not select
			SelectItem (false);
		}
	}

	void SelectItem (bool select)
	{
		if (!animator.GetBool ("select") == select) {
			animator.SetBool ("select", select);
		}
	}
}