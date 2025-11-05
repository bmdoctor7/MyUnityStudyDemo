using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayController : MonoBehaviour
{
    public float speed = 5f;
    public Animator animator;
    public Camera playerCamera;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (playerCamera == null)
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
        
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
    
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
    
    
    
    
    
    
    
    
    
    
}
