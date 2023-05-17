using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttack : MonoBehaviour
{
    [SerializeField]
    private int damage;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private bool canAttack = true;

    private void OnEnable()
    {
        canAttack = true;
    }


    public virtual void Attack(Transform target)
    {
        //Debug.Log("SWIPE AT TARGET!");

        if (canAttack)
        {
            target.GetComponent<Health>().ModifyHealth(damage);
            StartCoroutine(AttackCooldown());
        }
            
    }

    IEnumerator AttackCooldown()
    {
        //attackStarted = true;
        canAttack = false;
        Debug.Log("SWIPE AT TARGET!");

        yield return new WaitForSeconds(cooldown);

        //attackStarted = false;
        canAttack = true;
    }
}
