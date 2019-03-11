using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHealthBar : MonoBehaviour {
	[Tooltip("Player's Health script")]
	[SerializeField] private Health health;

	[SerializeField] private Image img;
	[SerializeField] private float speed;

	// Use this for initialization
	void Start () {
		health.OnHealthChange += MoveHealthBar;
		img = GetComponent<Image>();
	}
	
	/// <sumarry>
	/// 	Called when player's hp is changed. Moves health bar gradually until it reaches its new position.
	/// </sumarry>
	private void MoveHealthBar(int health){
		img.fillAmount = Mathf.Lerp(img.fillAmount, health.Percentage, speed);
	}
}