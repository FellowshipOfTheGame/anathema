﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anathema.Player
{
	public class JumpFall : Anathema.Fsm.PlayerState
	{
		[Tooltip("Force that pulls player down. It's recommended to be higher than the base gravity of the ascension state.")]
		[SerializeField] float gravity;

		[Tooltip("Falling speed cap.")]
		[SerializeField] float maxSpeed;

		[Tooltip("The distance of the ray that checks for ground, on fall. Recommended to be a little higher than the distance between the center of the player and the ground.")]
		[SerializeField] float groundCheckDist;

		[Tooltip("Speed in which the player is able to control himself during the descent.")]
		[SerializeField] float horizontalSpeed;

		[Tooltip("Offset in which the raycast win begin, this affects the height and proximity in which the player needs to be to a platform to snap to it. Recomended to be a little lower than the Ground Check Distance.")]
		[SerializeField] float raycastOffset;

		[Tooltip("Whether or not the player has unlocked the skill to double jump.")]

		private PlayerUpgrades playerUpgrades;

		[Tooltip("Whether or not the player can do multiple attacks per jump")]
		public bool isLimitedToOneAttack;

		// Whether or not the player has double jumped
		[HideInInspector] public bool hasDoubleJumped;

		// If isLimitedToOneAttack is true, stores whether or not the player has already attacked
		[HideInInspector] public bool hasAttacked, hasFireAttacked;

		// FIXME: Gambiarra
		private bool jumpCorrection;


		private void Start()
		{
			playerUpgrades = GetComponent<PlayerUpgrades>();
			if (!playerUpgrades) Debug.LogError($"{gameObject.name}: {nameof(Crouch)}: Couldn't find {nameof(PlayerUpgrades)}.");
		}

		public RaycastHit2D CheckIfGrounded()
		{
			// Raycast to check for ground
			RaycastHit2D rayHit = Physics2D.Raycast((Vector2)transform.position + (Vector2.down * raycastOffset), Vector2.down, groundCheckDist - raycastOffset, LayerMask.GetMask("Ground"));
			Debug.DrawRay(transform.position, Vector2.down * groundCheckDist, Color.green);
			return rayHit;
		}

		public override void Enter() {	}

		// FIXME: Gambiarra
		private void Update()
		{
			if(Input.GetButtonDown("Jump"))
				jumpCorrection = true;
		}

		/// <summary>
		/// 	In this class, the FixedUpdate handles the forces responsable for making the player fall and the transitions to other states
		///  such as when the player reaches the ground or double jumps. Also handles air movement.
		/// </summary>
		void FixedUpdate()
		{
			float HorizontalAxis = Input.GetAxisRaw("Horizontal");

			// Handles attacking midair
			if(playerUpgrades.HasScythe && Input.GetButton("NormalAttack") && (!isLimitedToOneAttack || (isLimitedToOneAttack && !hasAttacked)))
			{
				hasAttacked = true;
				rBody.velocity = Vector2.zero;
				animator.SetBool("IsAttacking", true);
				fsm.Transition<AirAttack>();
				return;
			}

			if(playerUpgrades.HasFireAttack && Input.GetButton("FireAttack") && (!isLimitedToOneAttack || (isLimitedToOneAttack && !hasFireAttacked)))
			{
				hasFireAttacked = true;
				rBody.velocity = Vector2.zero;
				animator.SetBool("IsFire", true);
				fsm.Transition<AirFireAttack>();
				return;
			}

			// Handles gravity forces pulling the player to the ground
			if(rBody.velocity.y < maxSpeed)
				rBody.AddForce(Vector2.down * gravity);

			RaycastHit2D rayHit = CheckIfGrounded();

			// Transitions states as soon as the player reaches ground
			if(rayHit)
			{
				this.transform.position += ((Vector3)rayHit.normal * ((groundCheckDist - raycastOffset) - rayHit.distance));
				animator.SetBool("IsWalking", true);
				animator.SetBool("IsFalling", false);
				hasDoubleJumped = false;
				hasAttacked = false;
				hasFireAttacked = false;
				fsm.Transition<Walking>();
			}

			// FIXME: Gambiarra
			if(jumpCorrection && !hasDoubleJumped && playerUpgrades.HasDoubleJump)
			{
				jumpCorrection = false;
				animator.SetBool("IsRising", true);
				animator.SetBool("IsFalling", false);
				hasDoubleJumped = true;
				hasAttacked = false;
				hasFireAttacked = false;
				rBody.velocity = new Vector2(rBody.velocity.x, 0f);
				fsm.Transition<JumpRise>();
				return;
			}

			// Handles air movement
			if(HorizontalAxis == 0f)
				rBody.velocity = new Vector2(0f, rBody.velocity.y);
			else if(HorizontalAxis > 0f)
			{
				sRenderer.flipX = false;
				rBody.velocity = new Vector2(horizontalSpeed, rBody.velocity.y);
			}
			else
			{
				sRenderer.flipX = true;
				rBody.velocity = new Vector2(-horizontalSpeed, rBody.velocity.y);
			}

		}

		public override void Exit()
		{
			// FIXME: Gambiarra
			jumpCorrection = false;
		}
	}
}