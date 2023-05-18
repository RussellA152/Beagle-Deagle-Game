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
        if (canAttack)
        {
            target.GetComponent<Health>().ModifyHealth(damage);
            StartCoroutine(AttackCooldown());
        }
            
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        Debug.Log("SWIPE AT TARGET!");

        yield return new WaitForSeconds(cooldown);

        canAttack = true;
    }
}
