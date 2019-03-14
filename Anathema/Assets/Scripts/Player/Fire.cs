using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	[Header("This setting only applies when the fire isn't spawned from player's fire attack. Otherwise it will be overwritten by the Player's FireAttack script settings")]
	[Space(10)]
	public int damage;
	
	private SpriteRenderer sRenderer;

	private void Awake()
	{
		sRenderer = GetComponent<SpriteRenderer>();
	}

	public void Despawn()
	{
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Vector2 knockbackDirection = sRenderer.flipX ? Vector2.left : Vector2.right;
		other.GetComponent<Health>()?.Damage(damage, knockbackDirection, Health.DamageType.EnemyAttack);
	}
}
