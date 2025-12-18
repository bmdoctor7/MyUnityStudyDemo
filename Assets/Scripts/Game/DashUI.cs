using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashUI : MonoBehaviour
{
    public Image dashImage;

    private void Start()
    {
        dashImage = this.GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventManager.AddListener<float>(EventType.DashCooldownStart, OnDashCooldownStart);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<float>(EventType.DashCooldownStart, OnDashCooldownStart);
    }
    
    private Coroutine _cooldownRoutine;
    private void OnDashCooldownStart(float duration)
    {
        
        if (_cooldownRoutine != null) StopCoroutine(_cooldownRoutine);
        _cooldownRoutine = StartCoroutine(FillCooldown(duration));
    }

    private IEnumerator FillCooldown(float duration)
    {
        dashImage.fillAmount = 1f;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            dashImage.fillAmount = 1f - (timer / duration);
            yield return null;
        }
        dashImage.fillAmount = 0f;
        PlayController.Instance.isCdFinish = true;
    }
}
