using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VelocityTest : MonoBehaviour
{
    [SerializeField] private float metersPerSecond;
    [SerializeField] private float accelPerSecond;
    private float time;
    private CharacterController characterController;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        //StartTest();
    }
    [ContextMenu("Start Test")]
    private void StartTest() { StopAllCoroutines(); StartCoroutine(Co_StartTest()); }
    private IEnumerator Co_StartTest()
    {
        yield return null;
        while (true)
        {
            time += Time.deltaTime;
            characterController.Move(MoveThisFrame());
            yield return null;
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
        characterController.Move(MoveThisFrame());
    }
    private Vector3 MoveThisFrame()
    {
        metersPerSecond += accelPerSecond * Time.deltaTime;
        metersPerSecond = Mathf.Clamp(metersPerSecond, 0, 2);
        var distance = metersPerSecond * Time.deltaTime;
        return  distance * Vector3.forward;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name+" "+time);
        other.enabled = false;
    }
}
