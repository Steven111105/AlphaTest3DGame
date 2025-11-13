using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;
    [SerializeField] Animator gunAnimator;
    [SerializeField] Animator meleeAnimator;
    bool isGunOut;
    bool isMeleeOut;
    bool isReloading;
    int currentAmmo;
    public int maxAmmo = 5;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isGunOut = true;
        isMeleeOut = false;
        gunAnimator.SetTrigger("ShowWeapon");
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && isGunOut && !isReloading)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && isGunOut && !isReloading)
        {
            gunAnimator.SetTrigger("Reload");
        }

        // if (Input.GetButtonDown("Fire2"))
        // {
        //     meleeAnimator.SetTrigger("Attack");
        //     Debug.Log("Firing Melee");
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     isGunOut = !isGunOut;
        //     if (isGunOut)
        //     {
        //         gunAnimator.SetTrigger("ShowWeapon");
        //         Debug.Log("Showing Gun");
        //     }
        //     else
        //     {
        //         gunAnimator.SetTrigger("HideWeapon");
        //         Debug.Log("Hiding Gun");
        //     }
        //     if (isMeleeOut)
        //     {
        //         isMeleeOut = false;
        //         meleeAnimator.SetTrigger("HideWeapon");
        //     }
        // }

        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     isMeleeOut = !isMeleeOut;
        //     if (isMeleeOut)
        //     {
        //         meleeAnimator.SetTrigger("ShowWeapon");
        //     }
        //     else
        //     {
        //         meleeAnimator.SetTrigger("HideWeapon");
        //     }
        //     if (isGunOut)
        //     {
        //         isGunOut = false;
        //         gunAnimator.SetTrigger("HideWeapon");
        //     }
        // }
    }

    void Shoot()
    {
        if (currentAmmo > 0)
        {
            gunAnimator.SetTrigger("Attack");
            currentAmmo--;
            Debug.Log("Ammo left: " + currentAmmo);

            // build a ray from the camera center;
            Ray ray;
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("Hit: " + hit.collider.name + " at distance " + hit.distance);

                // notify target (optional). Implement TakeDamage on target if needed.
                if(hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy Hit!");
                }
                else
                {
                    Debug.Log("Non-enemy target hit.");
                }
            }

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
        Debug.Log("Finished Reloading");
    }
}
