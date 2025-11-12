using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;
    [SerializeField] Animator meleeAnimator;
    bool isGunOut;
    bool isMeleeOut;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            gunAnimator.SetTrigger("Attack");
            Debug.Log("Firing Gun");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            meleeAnimator.SetTrigger("Attack");
            Debug.Log("Firing Melee");
        }
        if (Input.GetKeyDown(KeyCode.R) && isGunOut)
        {
            gunAnimator.SetTrigger("Reload");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isGunOut = !isGunOut;
            if (isGunOut)
            {
                gunAnimator.SetTrigger("ShowWeapon");
                Debug.Log("Showing Gun");
            }
            else
            {
                gunAnimator.SetTrigger("HideWeapon");
                Debug.Log("Hiding Gun");
            }
            if (isMeleeOut)
            {
                isMeleeOut = false;
                meleeAnimator.SetTrigger("HideWeapon");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isMeleeOut = !isMeleeOut;
            if (isMeleeOut)
            {
                meleeAnimator.SetTrigger("ShowWeapon");
            }
            else
            {
                meleeAnimator.SetTrigger("HideWeapon");
            }
            if (isGunOut)
            {
                isGunOut = false;
                gunAnimator.SetTrigger("HideWeapon");
            }
        }
        

    }
}
