using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Anathema.Player;
using Anathema.Rooms;

public class HordeManager : MonoBehaviour {
	public UnityEvent OnHordeDeath;
	[SerializeField] private int enemyQuantity;
	[SerializeField] private GameObject keyPrefab;
	[SerializeField] private UniqueID uniqueID; 
	public Vector3 lastEnemyPosition;

	/// <summary>
	/// Calls event when there is no enemy reamining in the horde
	/// </summary>
	/// <returns></returns>
	public int EnemyQuantity {
		get { return enemyQuantity; }
		set {
			enemyQuantity = value;
			if (enemyQuantity == 0) {
				OnHordeDeath.Invoke();
			}
		}
	}

	public void SpawnKey() {
		keyPrefab.GetComponent<KeyPickup>().key = uniqueID;
		Instantiate(keyPrefab, lastEnemyPosition, Quaternion.identity);
	}

	public void ObtainFireAttack() {
		GameObject player = PlayerFinder.Find("Player");
		player.GetComponent<CutsceneState>().StartCutscene(CutsceneState.UpgradeType.FireAttack , true);
	}
}