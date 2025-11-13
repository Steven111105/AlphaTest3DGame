using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            Die();
        }
    }

    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void Die()
    {
        animator.SetTrigger("Death");
    }
}
