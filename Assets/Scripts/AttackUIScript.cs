using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackUIScript : MonoBehaviour
{
    [SerializeField] TMP_Text ammoText;
    [SerializeField] Image skillCooldownImage;
    [SerializeField] Image ultimateCooldownImage;
    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image redPanel;

    bool showSkillDuration = false;
    bool showUltimateDuration = false;

    void Awake()
    {
        PlayerAttack.instance.OnShoot.AddListener(UpdateAmmo);
        PlayerAttack.instance.OnReloadEnd.AddListener(UpdateAmmo);
        PlayerAttack.instance.OnHPChange.AddListener(UpdateHPUI);
        PlayerAttack.instance.OnPlayerTakeDamage.AddListener(() => StartCoroutine(RedFlash()));

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
        ammoText.text = $"Ammo: {PlayerAttack.instance.currentAmmo} / {PlayerAttack.instance.maxAmmo}";
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

    IEnumerator RedFlash()
    {
        Debug.Log("Red Flash");
        float flashDuration = 0.2f;
        float t = 0;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, t / flashDuration);
            redPanel.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }
        redPanel.color = new Color(1f, 0f, 0f, 0f);
    }
}
