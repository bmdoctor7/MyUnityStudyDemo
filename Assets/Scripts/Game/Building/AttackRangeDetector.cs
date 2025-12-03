using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    
    public string targetTag = "Enemy";
    
    public  List<GameObject> targets = new List<GameObject>();


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            targets.Remove(other.gameObject);
        }
    }
    
    
    public Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject target in targets)
        {
            if (!target) continue;

            Vector3 directionToTarget = target.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestTarget = target.transform;
            }
        }

        return closestTarget;
    }
    
    
    
    
    
    
    
    
    
    
    
    
}
