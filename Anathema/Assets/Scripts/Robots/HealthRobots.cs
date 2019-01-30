using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthRobots : Health
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        OnHealthChange += OnHit;
        OnDeath += Die;
    }

    void OnHit(Health health)
    {
        Debug.Log("knockBack");
        animator.Play("DamageFeedback");
    }

    void Die()
    {
        OnHealthChange -= OnHit;
        OnDeath -= Die;
        Destroy(this.gameObject);
    }
}