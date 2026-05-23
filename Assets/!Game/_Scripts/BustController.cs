using System.Collections;
using UnityEngine;
public class BustController : MonoBehaviour, ISprocketPushable
{
    [SerializeField] private Transform _bustTransform;
    [SerializeField] private SoxAtkJiggleBone _jiggleBone;
    [SerializeField] private float _shakeDuration = 0.1f;
    [SerializeField] private float _shakeDistance = 0.13f;
    [SerializeField] private float _cooldownDuration = 0.5f;
    [SerializeField] private CompanionData _companionData;

    private float _defaultInercia;
    private float _defaultPosition;
    public void Jiggle(float shakeDuration, float shakeDistance, float cooldownDuration)
    {
        StopAllCoroutines();
        StartCoroutine(Co_Jiggle());
        IEnumerator Co_Jiggle()
        {
            if (!_bustTransform.gameObject.activeInHierarchy) yield break;
            if (_jiggleBone != null) _jiggleBone.m_inercia = 1f;

            yield return ExtensionMethods.Co_FadeFloat(shakeDuration, new(_bustTransform.localPosition.y, _defaultPosition + shakeDistance / 100), fl =>
            {
                var newPos = _bustTransform.localPosition;
                newPos.y = fl;
                _bustTransform.localPosition = newPos;
            });
            yield return ExtensionMethods.Co_FadeFloat(shakeDuration, new(_bustTransform.localPosition.y, _defaultPosition), fl =>
            {
                var newPos = _bustTransform.localPosition;
                newPos.y = fl;
                _bustTransform.localPosition = newPos;
            });
            if (_jiggleBone == null) yield break;
            yield return ExtensionMethods.Co_FadeFloat(cooldownDuration, new(_jiggleBone.m_inercia, _defaultInercia), fl =>
            {
                _jiggleBone.m_inercia = fl;
            });
        }
    }

    public bool Push(Vector3 force)
    {
        if (!_companionData.BustPhysics) return false;
        Jiggle(_shakeDuration, _shakeDistance * force.magnitude, _cooldownDuration);
        return false;
    }

    private void Awake()
    {
        if (_jiggleBone != null)
        {
            _defaultInercia = _jiggleBone.m_inercia;
        }
        _defaultPosition = _bustTransform.localPosition.y;
    }
}
