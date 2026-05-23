using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HeaderUI : UIComponent
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    public override void SetComponentProfile(ComponentProfile profile)
    {
        
    }

    protected override void Awake()
    {
        base.Awake();
        _textMeshPro.alpha = 0;
    }

    private void OnEnable()
    {
        UIManager.OnShowHeader += ShowText;
        UIManager.OnHideHeader += HideText;
    }
    private void OnDisable()
    {
        UIManager.OnShowHeader -= ShowText;
        UIManager.OnHideHeader -= HideText;
    }

    private void ShowText() 
    { 
        _textMeshPro.text = UIManager.HeaderText;
        _textMeshPro.alpha = 0;
        _textMeshPro.alignment = UIManager.HeaderAlignment;
        StopAllCoroutines();
        StartCoroutine(Co_TextFade(2, new(0, 1)));
    }
    private IEnumerator Co_TextFade(float seconds, Vector2 span)
    {
        var time = 0f;
        while(time < seconds)
        {
            _textMeshPro.alpha = Mathf.Lerp(span.x, span.y, time/seconds);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void HideText() 
    {
        StopAllCoroutines();
        StartCoroutine(Co_TextFade(2, new(_textMeshPro.alpha, 0)));
    }

   
}
