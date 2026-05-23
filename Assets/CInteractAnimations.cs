using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CInteractAnimations : MonoBehaviour
{
    [System.Serializable]
    public class EventAnim
    {
        public AnimationClip clip;
        public List<EventSequence.TimedEvent> events;
    }
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimatorOverrideController _aoc;
    [SerializeField] private LerpToTransform _cameraLTT;
    [SerializeField] private CompanionRigController _companionRigController;
    [SerializeField] private List<EventAnim> _eventAnims;

    public void StartAnim()
    {
        PlayerManager.SetPlayerEnabled(false);
        _companionRigController.SetLookAt(false);
        _cameraLTT.enabled = false;
        var cam = PlayerLook.Camera.transform;
        _cameraLTT.transform.SetPositionAndRotation(cam.position, cam.rotation);
        _cameraLTT.enabled = true;
        _cameraLTT.gameObject.SetActive(true);
        var eventAnim = _eventAnims[Random.Range(0, _eventAnims.Count)];
        _aoc["Anim_C_DeepKiss"] = eventAnim.clip;
        _animator.SetTrigger("Kiss");
        StopAllCoroutines();
        StartCoroutine(Co_Anim());
        IEnumerator Co_Anim()
        {
            var transitionLength = 0.3f;
            StartCoroutine(EventSequence.Co_Execute(eventAnim.events, true));
            yield return new WaitForSeconds(eventAnim.clip.length - transitionLength);

            UIFade.FadeDurations = Vector3.zero;
            var time = 0f;
            Transform start = _cameraLTT.TargetTransform;
            _cameraLTT.enabled = false;
            _companionRigController.SetLookAt(true);
            while (time < transitionLength)
            {
                time += Time.deltaTime;
                _cameraLTT.transform.Lerp(start, PlayerLook.Camera.transform, time / transitionLength);
                yield return null;
            }
            PlayerManager.SetPlayerEnabled(true);
            _cameraLTT.gameObject.SetActive(false);
            // _aoc["Anim_C_DeepKiss"] = null;
        }

    }
}
