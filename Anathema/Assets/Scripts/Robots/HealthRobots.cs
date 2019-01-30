using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthRobots : Health
{
    void Awake()
    {
        OnHealthChange += OnHit;
        OnDeath += Die;
    }

    void OnHit(int health)
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