using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SetEvent : MonoBehaviour {
	public EventSystem eventSytstem;
	// Use this for initialization

	void OnEnable () {
		eventSytstem.enabled = false;
		eventSytstem.enabled = true;
		eventSytstem.firstSelectedGameObject = gameObject;
	}
}
