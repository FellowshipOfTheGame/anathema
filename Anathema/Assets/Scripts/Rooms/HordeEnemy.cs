using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeEnemy : MonoBehaviour {
	[SerializeField] private HordeManager hordeManager; 
	
	void Awake() {
		hordeManager.EnemyQuantity++;
	}
	void OnDestroy() {
		hordeManager.EnemyQuantity--;
	}
}