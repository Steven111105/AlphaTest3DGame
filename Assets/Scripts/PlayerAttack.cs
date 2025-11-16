using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] CharacterStats playerStats;
    public static PlayerAttack instance;
    public int maxHealth = 100;
    int _currentHealth;
    public int currentHealth
    {
        get { return _currentHealth; }
        set { 
                if(value <= 0) 
                {
                    _currentHealth = 0;
                    OnPlayerDeath.Invoke();
                }
                else
                {
                    _currentHealth = value; 
                    OnHPChange.Invoke(_currentHealth); 
                }
            }
    }
    public int meleeDamage = 50;
    public int bulletDamage = 20;
    #region Events
    [HideInInspector] public UnityEvent OnShoot;
    // for calculating hit accuracy (shots that hit / shots fired)
    [HideInInspector] public UnityEvent OnShootHit;
    [HideInInspector] public UnityEvent OnReloadEnd;
    
    [HideInInspector] public UnityEvent ShowSkillDuration;
    [HideInInspector] public UnityEvent ShowSkillCooldown;
    [HideInInspector] public UnityEvent ShowUltimateDuration;
    [HideInInspector] public UnityEvent ShowUltimateCooldown;
    [HideInInspector] public UnityEvent<int> OnHPChange;
    [HideInInspector] public UnityEvent OnPlayerTakeDamage;
    [HideInInspector] public UnityEvent OnPlayerDeath;
    #endregion
    [SerializeField] Animator gunAnimator;
    [SerializeField] Animator meleeAnimator;
    bool isGunOut;
    bool canShoot;
    bool isMeleeOut;
    bool canStab;
    bool isReloading;
    public int currentAmmo;
    public int maxAmmo = 5;
    bool isSkillReady = false;
    bool isUltimateReady = false;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isGunOut = true;
        isMeleeOut = false;
        isSkillReady = false;
        isUltimateReady = false;
        canShoot = true;
        canStab = true;
        currentAmmo = maxAmmo;
        maxHealth = playerStats.maxHealth;
        meleeDamage = playerStats.damage;
        bulletDamage = playerStats.damage;
        gunAnimator.gameObject.SetActive(true);
        meleeAnimator.gameObject.SetActive(false);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(SwitchToGun());
        StartCoroutine(SkillCooldown());
        ShowSkillCooldown.Invoke();
        StartCoroutine(UltimateCooldown());
        ShowUltimateCooldown.Invoke();

        OnPlayerDeath.AddListener(() => canShoot = false);
        OnPlayerDeath.AddListener(() => canStab = false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.timeScale > 0f && !TransitionManager.instance.fadeInProgress)
        {
            if (isGunOut && !isReloading && canShoot)
            {
                StartCoroutine(Shoot());
            }
            else if (isMeleeOut && canStab)
            {
                StartCoroutine(AttackMelee());
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && isGunOut && !isReloading)
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.E) && isSkillReady)
        {
            isSkillReady = false;
            Debug.Log("Skill used");
            StartCoroutine(StartSkill());
        }

        if (Input.GetKeyDown(KeyCode.Q) && isUltimateReady)
        {
            isUltimateReady = false;
            Debug.Log("Ultimate used");
            StartCoroutine(StartUltimate());
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentHealth -= 10;
        }
    }

    IEnumerator Shoot()
    {
        if (currentAmmo > 0)
        {
            canShoot = false;
            gunAnimator.SetTrigger("Attack");
            currentAmmo--;
            OnShoot.Invoke();
            AudioManager.instance.PlaySFX("Shoot");
            Debug.Log("Ammo left: " + currentAmmo);

            // build a ray from the camera center;
            Ray ray;
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("Hit: " + hit.collider.name + " at distance " + hit.distance);

                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy Hit!");
                    hit.collider.gameObject.GetComponent<EnemyScript>().TakeDamage(bulletDamage);
                    OnShootHit.Invoke();
                }
                else
                {
                    Debug.Log("Non-enemy target hit.");
                }
            }
            yield return new WaitForSeconds(0.3f); // fire rate
            canShoot = true;
        }
        else
        {
            Debug.Log("Out of ammo, reloading");
            Reload();
        }
    }

    void Reload()
    {
        gunAnimator.SetTrigger("Reload");
        isReloading = true;
    }

    public void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        OnReloadEnd.Invoke();
        Debug.Log("Finished Reloading");
    }

    IEnumerator SwitchToGun()
    {
        if (isMeleeOut)
        {
            isMeleeOut = false;
            meleeAnimator.SetTrigger("HideWeapon");
            yield return new WaitForSeconds(0.5f);
            meleeAnimator.gameObject.SetActive(false);
        }
        isGunOut = true;
        gunAnimator.gameObject.SetActive(true);
        gunAnimator.SetTrigger("ShowWeapon");
    }
    IEnumerator SwitchToMelee()
    {
        if (isGunOut)
        {
            isGunOut = false;
            gunAnimator.SetTrigger("HideWeapon");
            yield return new WaitForSeconds(0.5f);
            gunAnimator.gameObject.SetActive(false);
        }
        isMeleeOut = true;
        meleeAnimator.gameObject.SetActive(true);
        meleeAnimator.SetTrigger("ShowWeapon");
    }

    IEnumerator AttackMelee()
    {
        canStab = false;
        meleeAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.25f); // delay to match the animation hit frame
        // build a ray from the camera center;
        Ray ray;
        ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
                                        // 5 range for melee
        if (Physics.Raycast(ray, out hit, 5f))
        {
            Debug.Log("Hit: " + hit.collider.name + " at distance " + hit.distance);

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Hit!");
                hit.collider.gameObject.GetComponent<EnemyScript>().TakeDamage(meleeDamage);
            }
            else
            {
                Debug.Log("Non-enemy target hit.");
            }
        }
        yield return new WaitForSeconds(0.25f); // attack rate
        canStab = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnPlayerTakeDamage.Invoke();
    }

    #region Skill and Ultimate Cooldown
    [Header("Skill and Ultimate Settings")]
    public float skillDuration;
    public float skillCooldown;
    public float ultimateDuration;
    public float ultimateCooldown;

    [HideInInspector] public float skillDurationFloat;
    IEnumerator StartSkill()
    {
        ShowSkillDuration.Invoke();
        StartCoroutine(SkillCooldown());
        // heal 5 hp per second for 5 seconds
        skillDurationFloat = 5f;
        while (skillDurationFloat > 0)
        {
            currentHealth += 5;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            skillDurationFloat -= 1f;
            yield return new WaitForSeconds(1f);
        }
        skillDurationFloat = 0;
        ShowSkillCooldown.Invoke();
        Debug.Log("Skill has ended");
    }
    
    [HideInInspector] public float skillCooldownFloat;
    IEnumerator SkillCooldown()
    {
        skillCooldownFloat = skillCooldown;
        while (skillCooldownFloat > 0)
        {
            skillCooldownFloat -= Time.deltaTime;
            yield return null;
        }
        skillCooldownFloat = 0;
        isSkillReady = true;
        Debug.Log("Skill is ready");
    }
    [HideInInspector] public float ultimateDurationFloat;
    IEnumerator StartUltimate()
    {
        //wait till the melee is out
        yield return StartCoroutine(SwitchToMelee());

        ShowUltimateDuration.Invoke();
        StartCoroutine(UltimateCooldown());

        maxHealth = 200;
        currentHealth = maxHealth;

        ultimateDurationFloat = ultimateDuration;
        while (ultimateDurationFloat > 0)
        {
            ultimateDurationFloat -= Time.deltaTime;
            yield return null;
        }
        ultimateDurationFloat = 0;

        maxHealth = 100;
        if (currentHealth >= maxHealth)
        {
            // Cap hp at 100
            currentHealth = maxHealth;
        }
        else
        {
            // hp below 100, just update the max health
            OnHPChange.Invoke(currentHealth);
        }

        Debug.Log("Melee has ended");
        ShowUltimateCooldown.Invoke();
        StartCoroutine(SwitchToGun());
    }

    // current time left for ultimate cooldown
    [HideInInspector] public float ultimateCooldownFloat;
    IEnumerator UltimateCooldown()
    {
        ultimateCooldownFloat = ultimateCooldown;
        while (ultimateCooldownFloat > 0)
        {
            ultimateCooldownFloat -= Time.deltaTime;
            yield return null;
        }
        ultimateCooldownFloat = 0;
        Debug.Log("Ultimate is ready");
        isUltimateReady = true;
    }

    #endregion
}