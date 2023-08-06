using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityAction<GameObject,int> onTakeDamage;
    public UnityAction onTakeDamageNoParam;

    public UnityAction onChangeHealth;
    public UnityAction onDeath;
    public UnityEvent onDeathEvent;
    public UnityEvent onTakeDamageEvent;
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] GameObject destructionParticles;
    [SerializeField]protected GameObject damageParticles;
    [SerializeField]protected float FXLifetime =1.5f;

    [SerializeField] damageType damageImunity= damageType.none;
   [SerializeField] bool ignoreDamage = false;
    protected int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    virtual public bool  TakeDamage(GameObject attacker, int damage, damageType damageType = damageType.normal)
    {
        if(!ignoreDamage&& damageImunity!=damageType)
        {
            if (damageParticles) Destroy(Instantiate(damageParticles, transform), FXLifetime);

            // damage 
            currentHealth -= damage;
            PlayOnDamageEvents(attacker, damage);
            

            // death management
            if (currentHealth < 1)
                OnDeath();
                if(destructionParticles)
                Instantiate(destructionParticles, transform.position, Quaternion.identity);

            return true;
        }
        return false;
    }

    private void PlayOnDamageEvents(GameObject attacker, int damage)
    {
        onTakeDamage?.Invoke(attacker, damage);
        onTakeDamageNoParam?.Invoke();
        onTakeDamageEvent?.Invoke();
        onChangeHealth?.Invoke();
    }

    virtual public bool  ForceTakeDamage(GameObject attacker, int damage)
    {

        if(damageParticles) Destroy(Instantiate(damageParticles, transform.position,  damageParticles.transform.rotation),FXLifetime);

        // damage 
        currentHealth -=damage;
        onTakeDamage?.Invoke(attacker,damage);
        onTakeDamageNoParam?.Invoke();

        // death management
        if(currentHealth>0) return false;
        OnDeath();
        return true;

    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int IncreaseMaxHealth(int amount = 1)
    {
        maxHealth+=amount;
        onChangeHealth?.Invoke();
        return maxHealth;
    }
    public void OnDeath()
    {
        onDeath?.Invoke();
        onDeathEvent?.Invoke();
    }

    public void SetIgnoreDamage(bool ignoreDamage)
    {
        Debug.Log("changed ignore damage to: "+ ignoreDamage);
        this.ignoreDamage = ignoreDamage;
    }

    public void Heal(int amount)
    {
        currentHealth = currentHealth+amount>maxHealth ? maxHealth: currentHealth+amount;
        onChangeHealth?.Invoke();
    }
}

public enum damageType
{
    none,
    normal,
    bomb
}