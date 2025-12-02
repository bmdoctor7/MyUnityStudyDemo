using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayController : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;
    public Camera playerCamera;
    
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public int level = 1;
    public float currentMaxExp = 100f;
    public float currentExp = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (!playerCamera)
        {
            playerCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(moveX, moveY);

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("Horizontal", moveX);
            animator.SetFloat("Vertical", moveY);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
        transform.Translate(direction * speed * Time.deltaTime);
        
        //playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        
        if(currentExp>= currentMaxExp)
        {
            LevelUp();
        }
    }

    #region Game1
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pickable")
        {
            InventoryManager.Instance.AddToBackpack(collision.GetComponent<Pickable>().type);
            Destroy(collision.gameObject);
        }
    }
    
    public void ThrowItem(GameObject itemPrefab,int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject go =  GameObject.Instantiate(itemPrefab);
            Vector2 direction = Random.insideUnitCircle.normalized * 1.2f;
            go.transform.position = transform.position + new Vector3(direction.x,direction.y,0);
            go.GetComponent<Rigidbody2D>().AddForce(direction*3);
        }
    }
    #endregion
    
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //TODO: 回到当前波次开始前
        }
    }

    #region Game2
    public float GetNextLevelExp()
    {
        return (currentMaxExp*level*1.2f) + 77f ;
    }

    public void LevelUp()
    {
        currentExp = currentExp - currentMaxExp;
        currentMaxExp = GetNextLevelExp();
        level += 1;
    }
    
    #endregion
    
}
