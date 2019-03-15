using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHealthBar : MonoBehaviour {
	[Tooltip("Player's Health script")]
	[SerializeField] private Health health;

	[SerializeField] private Image img;
	[SerializeField] private float speed;
	private float newHp;

	// Use this for initialization
	void Start () {
		health.OnHealthChange += HandleHealthChanges;
		img = GetComponent<Image>();
	}
	
	/// <sumarry>
	/// 	Called when player's hp is changed. Moves health bar gradually until it reaches its new position.
	/// </sumarry>
	private void HandleHealthChanges(int newHealth){
		newHp = (float)newHealth/100f;
		CancelInvoke();
		InvokeRepeating("MoveHealthBar", 0f, Time.deltaTime);
	}

	private void MoveHealthBar() {
		img.fillAmount = Mathf.Lerp(img.fillAmount, newHp, speed);
		if (Mathf.Abs(img.fillAmount - newHp) < 0.01) {
			CancelInvoke();
		}
	}
}