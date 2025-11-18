using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 5f;
    public float rotateLerp = 0.2f; 
    public float maxLifeTime = 5f;
    
    public Vector3 targetPosition;
    public bool isGimlet = false;

}
