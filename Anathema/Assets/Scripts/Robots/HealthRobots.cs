using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthRobots : Health
{
    public override void TakeDamage(int damage) { }
    void Awake()
    {
        OnHealthChange += OnHit;
        OnDeath += Die;
    }

    void OnHit(Health health)
    {
        gameObject.GetComponent<Animation>().Play("KnockBack");
    }

    void Die()
    {
        OnHealthChange -= OnHit;
        OnDeath -= Die;
        Destroy(this.gameObject);
    }
}