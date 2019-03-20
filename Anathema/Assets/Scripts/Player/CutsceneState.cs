using System.Collections;
using System.Collections.Generic;
using Anathema.Rooms;
using UnityEngine;

namespace Anathema.Player
{
	/// <summary>
	/// 	This is a void, empty state to prevent input during cutscenes.
	/// </summary>
	public class CutsceneState : Anathema.Fsm.PlayerState
	{
		[SerializeField] private GameObject scytheUpgrade, doubleJumpUpgrade, fireUpgrade;
		[SerializeField] private Anathema.Dialogue.Dialogue scytheDialogue, doubleJumpDialogue, fireDialogue;
		[SerializeField] private float defaultUpgradeAnimationDuration;
		[SerializeField] private Anathema.Rooms.UniqueID key;

		public enum UpgradeType { Scythe, DoubleJump, FireAttack }
		private GameObject currentUpgrade;
		private Anathema.Dialogue.Dialogue currentDialogue;
		private Anathema.Saving.GameData gameData;

		public override void Enter()
		{
			rBody.velocity = Vector2.zero;
		}

		private void Start()
		{
			Anathema.SceneLoading.SceneLoader.OnLateSceneLoaded += GetSaveData;
		}

		public void GetSaveData(Anathema.Rooms.UniqueID iD, Anathema.Saving.GameData gameData)
		{
			this.gameData = gameData;
		}

		public void CommitData()
		{
			var save = new Saving.SaveProfile(gameData.ProfileName);
			save.Save(gameData);
		}

		public void Upgrade(int upgradeIndex)
		{
			switch(upgradeIndex)
			{
				case 0:
					Upgrade(UpgradeType.Scythe);
					break;
				case 1:
					Upgrade(UpgradeType.DoubleJump);
					break;
				case 2:
					Upgrade(UpgradeType.FireAttack);
					break;
				default:
					break;
			}
		}

		public void Upgrade(UpgradeType upgrade)
		{
			PlayerUpgrades upgrades = GetComponent<PlayerUpgrades>();

			switch(upgrade)
			{
				case UpgradeType.Scythe:
					currentUpgrade = scytheUpgrade;
					currentDialogue = scytheDialogue;
					
					if(!upgrades)
					{
						if(gameData != null)
						{
							gameData.hasScythe = true;
							CommitData();
						}
					}
					else 
						upgrades.HasScythe = true;

					break;
				
				case UpgradeType.DoubleJump:
					currentUpgrade = doubleJumpUpgrade;
					currentDialogue = doubleJumpDialogue;
					upgrades.HasDoubleJump = true;
					
					break;

				case UpgradeType.FireAttack:
					currentUpgrade = fireUpgrade;
					currentDialogue = fireDialogue;
					upgrades.HasFireAttack = true;

					break;

				default:
					break;
			}
			
			currentUpgrade = Instantiate(currentUpgrade, this.transform.position, Quaternion.identity, this.transform);
			Debug.Log(currentUpgrade);
		}

		public void Upgrade(UpgradeType upgrade, float duration)
		{
			Upgrade(upgrade);
			Invoke(nameof(UpgradeDestroy), duration);
		}

		public void UpgradeDestroy(bool endCutscene = true)
		{
			Debug.Log(currentUpgrade);
			Debug.Log(currentUpgrade.transform.GetChild(0));
			currentUpgrade.transform.GetChild(0).GetComponent<Animator>().Play("Explode");
			Anathema.Dialogue.DialogueHandler.instance.StartDialogue(currentDialogue);

			if(endCutscene)
				EndCutscene();
		}

		public void StartCutscene()
		{
			fsm.Transition<CutsceneState>();
		}

		public void StartCutscene(UpgradeType upgrade, bool useDefaultTime)
		{
			StartCutscene();
			if(!useDefaultTime)
				Upgrade(upgrade);
			else
				Upgrade(upgrade, defaultUpgradeAnimationDuration);
		}

		public void StartCutscene(UpgradeType upgrade, float duration)
		{
			StartCutscene();
			Upgrade(upgrade, duration);
		}

		public void AddKey()
		{
			List<UniqueID> keys = new List<UniqueID>(gameData.keys);
			keys.Add(key);
			gameData.keys = keys.ToArray();
		}
		
		public void EndCutscene()
		{
			fsm.Transition<Idle>();
		}

		public void TalkToJudas()
		{
			if(gameData != null)
			{
				gameData.hasTalkedToJudas = true;
				CommitData();
			}
		}

		public override void Exit()
		{
			Anathema.SceneLoading.SceneLoader.OnLateSceneLoaded -= GetSaveData;			
		}
		
	}
}