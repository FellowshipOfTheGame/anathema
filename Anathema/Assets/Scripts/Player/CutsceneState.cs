using System.Collections;
using System.Collections.Generic;
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

		public enum UpgradeType { Scythe, DoubleJump, FireAttack }
		private GameObject currentUpgrade;
		private Anathema.Dialogue.Dialogue currentDialogue;

		public override void Enter()
		{
			
		}

		public void Upgrade(UpgradeType upgrade)
		{
			PlayerUpgrades upgrades = GetComponent<PlayerUpgrades>();

			switch(upgrade)
			{
				case UpgradeType.Scythe:
					currentUpgrade = scytheUpgrade;
					currentDialogue = scytheDialogue;
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

		private void UpgradeDestroy()
		{
			Debug.Log(currentUpgrade);
			Debug.Log(currentUpgrade.transform.GetChild(0));
			currentUpgrade.transform.GetChild(0).GetComponent<Animator>().Play("Explode");
			Anathema.Dialogue.DialogueHandler.instance.StartDialogue(currentDialogue);
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

		public void EndCutscene()
		{
			fsm.Transition<Idle>();
		}

		public override void Exit()
		{

		}
		
	}
}