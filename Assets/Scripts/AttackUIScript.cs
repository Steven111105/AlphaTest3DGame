using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class AttackUIScript : MonoBehaviour
{
    [SerializeField] TMP_Text ammoText;
    [SerializeField] Image skillCooldownImage;
    [SerializeField] Image ultimateCooldownImage;
    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text healthText;

    bool showSkillDuration = false;
    bool showUltimateDuration = false;

    void Awake()
    {
        PlayerAttack.instance.OnShoot.AddListener(UpdateAmmo);
        PlayerAttack.instance.OnReloadEnd.AddListener(UpdateAmmo);
        PlayerAttack.instance.OnHPChange.AddListener(UpdateHPUI);

        PlayerAttack.instance.ShowSkillDuration.AddListener(()=>showSkillDuration = true);
        PlayerAttack.instance.ShowSkillCooldown.AddListener(()=>showSkillDuration = false);

        PlayerAttack.instance.ShowUltimateDuration.AddListener(()=>showUltimateDuration = true);
        PlayerAttack.instance.ShowUltimateCooldown.AddListener(()=>showUltimateDuration = false);
    }   

    private void Update()
    {
        UpdateSkillCooldown();
        UpdateUltimateCooldown();
    }

    void UpdateAmmo()
    {
        ammoText.text = PlayerAttack.instance.currentAmmo.ToString() + " / " + PlayerAttack.instance.maxAmmo.ToString();
    }

    void UpdateHPUI(int currentHealth)
    {
        healthBar.maxValue = PlayerAttack.instance.maxHealth;
        healthBar.value = currentHealth;
        healthText.text = currentHealth.ToString() + " / " + PlayerAttack.instance.maxHealth.ToString();
    }

    void UpdateSkillCooldown()
    {
        if (showSkillDuration)
        {
            skillCooldownImage.fillAmount = PlayerAttack.instance.skillDurationFloat / PlayerAttack.instance.skillDuration;
        }
        else
        {
            skillCooldownImage.fillAmount = 1 - (PlayerAttack.instance.skillCooldownFloat / PlayerAttack.instance.skillCooldown);
        }
    }

    void UpdateUltimateCooldown()
    {
        if (showUltimateDuration)
        {
            ultimateCooldownImage.fillAmount = PlayerAttack.instance.ultimateDurationFloat / PlayerAttack.instance.ultimateDuration;
        }
        else
        {
            ultimateCooldownImage.fillAmount = 1 - (PlayerAttack.instance.ultimateCooldownFloat / PlayerAttack.instance.ultimateCooldown);
        }
    }
}
