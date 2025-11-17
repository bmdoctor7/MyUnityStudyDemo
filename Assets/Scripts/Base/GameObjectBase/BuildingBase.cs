using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBase : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth = 200f;
    public float attackRange = 2f;
    public float attackDamage = 15f;
    
    public TextMeshProUGUI hpText;
    public Slider hpSlider;
    
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHpui();
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            GameManager2.Instance.buildingsList.Remove(gameObject);
        }
    }
    
    
    public virtual void UpdateHpui()
    {
        hpSlider.value = currentHealth / maxHealth;
        hpText.text = currentHealth + "/" + maxHealth;
    }
    
}
