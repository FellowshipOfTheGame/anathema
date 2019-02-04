using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Health : MonoBehaviour {
    [SerializeField][Range(0, 100)] private int hp;
    [SerializeField][Range(0, 100)] private int maxHP;

    public delegate void HealthChangedHandler(int health);
    public event HealthChangedHandler OnHealthChange;

    public delegate void KnockBackHandler(Vector2 hitVector);
    public event KnockBackHandler OnKnockback;

    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

    public enum DamageType {Heal, EnemyAttack, LevelHazard, NormalDamage}

    public bool isInvulnerable = false;

    /// <summary>
    ///     The entity's health can only be changed by other scripts through this method, which ensures the correct aftereffects 
    /// will be applied.
    /// </summary>
    /// <param name="value">    The value to be added to/subtracted from the entity's health </param>
    /// <param name="hitVector">    The direction in which the damage was inflicted in </param>
    /// <param name="damageType">   The damage type or attack type </param>
    /// <returns>   Whether or not it was successful (false when player is invulnerable) </returns>
    public bool Damage(int value, Vector2 hitVector, DamageType damageType)
    {
        switch(damageType)
        {
            case DamageType.EnemyAttack:
                OnKnockback(hitVector);
                goto case DamageType.NormalDamage;

            case DamageType.LevelHazard:
                goto case DamageType.NormalDamage;

            case DamageType.NormalDamage:
                if(isInvulnerable)
                    return false;

                hp -= value;
                //OnHealthChange(hp);

                if(hp < 0)
                {
                    hp = 0;
                    OnDeath();
                }
                break;

            case DamageType.Heal:
                if(hp == maxHP)
                    return false;

                hp += value;
                OnHealthChange(hp);

                if(hp > maxHP)
                    hp = maxHP;
                
                break;           

            default:
                break;
        }
        return true;
    }

    /// <summary>
    ///     Heals the player by calling the damage method in the correct mode
    /// </summary>
    /// <param name="value">    The value to be added to the entity's health</param>
    /// <returns>   Whether or not it was successful (false when the player is already at full health)</returns>
    public bool Heal(int value)
    {
        return Damage(value, Vector2.zero, DamageType.Heal);
    }

    // Returns HP percentage    
    public float Percentage{
        get{return 100f * ((float)hp / (float)MaxHP);}
    }

    public int MaxHP{
        get{return maxHP;}
        set{
            maxHP = value;
            if(maxHP < 0)
                Debug.LogWarning("Warning: setting maxHealth < 0");
        }
    }
}