using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CompanionHealth : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private UnityEvent _onDie;
    private bool cooldown = false;
    private void Awake()
    {
        UpdateHealthText();
    }
    private void UpdateHealthText()
    {
        if (_healthText != null)
        {
            _healthText.text = "Companion Health: " + _health.ToString();
        }
    }
    public void TakeDamage()
    {
        if (cooldown) return;
        _health--;
        UpdateHealthText();
        if (_health <= 0)
        {
            _onDie?.Invoke();
        }
        cooldown = true;
        StartCoroutine(Co_Cooldown());
        IEnumerator Co_Cooldown()
        {
            yield return new WaitForSeconds(2f);
            cooldown = false;
        }
    }
}
