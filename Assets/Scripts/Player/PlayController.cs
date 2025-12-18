using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PlayController : SingletonMonoBase<PlayController>
{
    private PlayController(){}
    
    
    public Animator animator;
    private Camera playerCamera;
    public Camera miniMapCamera;
    
    public Image cdImage;
    public bool isCdFinish = true;
    
    [Header("基本属性")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public int level = 1;
    public float currentMaxExp = 100f;
    public float currentExp = 0f;
    
    public float startSpeed = 5f;
    public float speed = 5f;
    [Header("Dash")]
    public bool isDashing = false;
    public float dashTime;//dash时长
    private float dashTimeLeft;//冲锋剩余时间
    private float lastDash = -10f;//上一次dash时间点
    public float dashCoolDown;
    public float dashSpeed;
    private Vector2 dashDirection;
    
    private bool isInGame1 = false;
    private void Start()
    {
        isCdFinish = true;
        
        if(SceneManager.GetActiveScene().name=="LiveScene")
            isInGame1 = true;
        
        animator = GetComponent<Animator>();
        if (!playerCamera && isInGame1)
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
        
        //主相机逻辑
        if(isInGame1)
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        if(miniMapCamera)
            miniMapCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        
        if(currentExp>= currentMaxExp)
        {
            LevelUp();
        }
        
        
        //斯安威斯坦
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (Time.time >= (lastDash + dashCoolDown)  && isCdFinish)
            {
                //可以执行dash
                ReadyToDash();
            }
        }
        
    }
    
    private void FixedUpdate()
    {
        Dash();
    }
    
    void ReadyToDash()
    {
        isDashing = true;
        
        //ui是否冷却完毕
        isCdFinish = false;
        
        dashTimeLeft = dashTime;
        
        cdImage.fillAmount = 1f;
    }
    void Dash()
    {
        if (isDashing)
        {
            if (!isDashing) return;
            
            if (dashTimeLeft > 0)
            {
                speed = dashSpeed;

                dashTimeLeft -= Time.deltaTime;

                ShadowPool.Instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                speed = startSpeed;
                
                lastDash = Time.time; //冲锋结束后才进入冷却
                cdImage.fillAmount = 1f;
                
                //冷却UI逻辑（冷却完成后isCdFinish为True）
                EventManager.Broadcast(EventType.DashCooldownStart, dashCoolDown);
            }
        }
    }
    
    
    
    #region Game1
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickable"))
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
    
    #region Game2
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //TODO: 回到当前波次开始前
        }
    }

    
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
