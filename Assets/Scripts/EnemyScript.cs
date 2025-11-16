using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] Slider healthBar;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] int bulletDamage = 10;
    [SerializeField] Transform shootPoint;
    NavMeshAgent agent;
    Animator animator;

    public void LoadStat(CharacterStats stats)
    {
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        bulletDamage = stats.damage;
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
        StartCoroutine(ShootTimer());
        PlayerAttack.instance.OnPlayerDeath.AddListener(() => StopAllCoroutines());
    }

    // Update is called once per frame
    void Update()
    {
        RotateCanvasToCamera();
        if(CanSeePlayer())
        {
            // stay if can see player
            agent.SetDestination(transform.position);
        }
        else
        {
            // chase player if can't see
            agent.SetDestination(PlayerAttack.instance.transform.position);
        }
        transform.LookAt(PlayerAttack.instance.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
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

    IEnumerator Shoot()
    {
        float speed = agent.speed;
        animator.SetTrigger("Shoot");
        AudioManager.instance.PlaySFXAtLocation(transform, "Shoot");
        agent.speed = 0;

        transform.LookAt(PlayerAttack.instance.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        shootPoint.transform.LookAt(PlayerAttack.instance.transform);

        // build a ray from the camera center;
        Ray ray;
        ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.collider.name + " at distance " + hit.distance);

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player Hit!");
                hit.collider.transform.parent.gameObject.GetComponent<PlayerAttack>().TakeDamage(bulletDamage);
            }
            else
            {
                Debug.Log("Non-player target hit.");
            }
        }
        yield return new WaitForSeconds(0.5f);
        agent.speed = speed;
    }

    bool CanSeePlayer()
    {
        shootPoint.LookAt(PlayerAttack.instance.transform);
        // shoot 3 rays so the enemy needs to peek a bit more
        return CheckRayCast(shootPoint.position, PlayerAttack.instance.transform)
                && CheckRayCast(shootPoint.position  + new Vector3(0.12f, 0, 0), PlayerAttack.instance.transform)
                && CheckRayCast(shootPoint.position  + new Vector3(-0.12f, 0, 0), PlayerAttack.instance.transform)
        ;
        
    }

    bool CheckRayCast(Vector3 origin, Transform target)
    {
        Ray ray;
        ray = new Ray(origin, target.position - origin);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public void Die()
    {
        StopAllCoroutines();
        animator.SetTrigger("Death");
        AudioManager.instance.PlaySFXAtLocation(transform, "Death");
        gameObject.GetComponent<Collider>().enabled = false;
        agent.speed = 0;
        EnemyManager.instance.OnEnemyKilled.Invoke();
        Destroy(gameObject, 3f);
    }

    void RotateCanvasToCamera()
    {
        canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - Camera.main.transform.position);
    }

    IEnumerator ShootTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(Shoot());
        }
    }
}
