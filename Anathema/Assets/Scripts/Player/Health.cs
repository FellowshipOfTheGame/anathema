using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Health : MonoBehaviour {
    [SerializeField][Range(0, 100)] private int hp;
    [SerializeField][Range(0, 100)] private int maxHP;

    public delegate void HealthChangedHandler(Health health);
    public event HealthChangedHandler OnHealthChange;
    public delegate void DeathHandler();
    public event DeathHandler OnDeath;

    // Whenever player's hp is changed, event is called
    public int Hp {
        get{return hp;}
        set{
            if(hp != value){
                Debug.Log("knockBackHealth");
                if(value > maxHP) {
                    hp = maxHP;
                } else if(value < 0) {
                    hp = 0;
                    OnDeath();
                } else {
                    hp = value;
                }
                Debug.Log("knockBackHealth");
                OnHealthChange(this);
            }
        }
    }

    // Returns HP percentage    
    public float Percentage{
        get{return 100f * ((float)Hp / (float)MaxHP);}
    }

    public int MaxHP{
        get{return maxHP;}
        set{
            maxHP = value;
            if(maxHP < 0)
                Debug.LogWarning("Warning: setting maxHealth < 0");
        }
    }
    
    public virtual void TakeDamage(int damage) {}
}