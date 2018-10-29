using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	[Tooltip("Health Scrollbar reference")]
	[SerializeField] Scrollbar scroll;
	[Tooltip("Player's Health script")]
	[SerializeField] private Health health;
	[Tooltip("Health Scrollbar position")]
	[SerializeField] private int healthBarValue;
	[Tooltip("Frequency in which health bar moves")]
	[SerializeField][Range(1, 1000)] private float barSpeed;
	[Tooltip("Space moved by health bar in each iteration (the smaller the better")]
	[SerializeField][Range(0, 0.01f)] private float barOffset;
	[Tooltip("Indicates if player is being healed")]
	[SerializeField] private bool healing;
	[Tooltip("Indicates if player is taking damage")]
	[SerializeField] private bool takingDamage;

	private void Start(){
		health.OnHealthChange += MoveHealthBar;

		healthBarValue = health.MaxHP;
		scroll.value = 1f;

	}

	/// <sumarry>
	/// 	Called when player's hp is changed. Moves health bar gradually until it reaches its new position.
	/// </sumarry>

	private void MoveHealthBar(Health health){
		if(healthBarValue < health.Hp && !healing){
			CancelInvoke();
			takingDamage = false;
			healing = true;
			InvokeRepeating("Heal", 0, 1f/barSpeed);
		}
		else if(healthBarValue > health.Hp && !takingDamage){
			CancelInvoke();
			healing = false;
			takingDamage = true;
			InvokeRepeating("TakeDamage", 0, 1f/barSpeed);
		}
		healthBarValue = health.Hp;
	}

	// Incresaes health bar until it reaches its new value	
	private void Heal(){
		if(scroll.value >= health.Percentage/100){
			healing = false;
			CancelInvoke();
		}
		scroll.value += barOffset;
	}	

	// Decreases health bar until it reaches its new value	
	private void TakeDamage(){
		if(scroll.value <= health.Percentage/100){
			takingDamage = false;
			CancelInvoke();
		}
		scroll.value -= barOffset;
	}	
}