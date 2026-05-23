using System.Collections;
using UnityEngine;

public class CGameObjectSelect : MonoBehaviour
{
    [SerializeField] private GameObject highlightObject;
    [SerializeField] private float secondsEnabled;
    public void Off()
    {
        highlightObject.SetActive(false);
    }
    public void On()
    {
        StopAllCoroutines();
        StartCoroutine(Co_On());
    }
    private void OnEnable()
    {
        Commands.OnCommand += OnCommand;
        Off();
    }
    private void OnDisable()
    {
        Commands.OnCommand -= OnCommand;
    }

    private void OnCommand()
    {
        if (CompanionData.CurrentCommand.Transform != gameObject) { Off(); }
    }

    IEnumerator Co_On()
    {
        highlightObject.SetActive(true);
        yield return new WaitForSeconds(secondsEnabled);
        Off();
    }
    private void Start()
    {
        Off();
    }
}
