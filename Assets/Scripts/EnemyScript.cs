using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] Slider healthBar;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCanvasToCamera();
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     Shoot();
        // }
        // if(Input.GetKeyDown(KeyCode.D))
        // {
        //     Die();
        // }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
        if(currentHealth <= 0)
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
        gameObject.GetComponent<Collider>().enabled = false;
    }

    void RotateCanvasToCamera()
    {
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - Camera.main.transform.position);
    }
}
