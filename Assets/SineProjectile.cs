using System.Collections;
using UnityEngine;

public class SineProjectile : MonoBehaviour
{
    [SerializeField] private Vector2 speedRange = new(4, 8);
    [SerializeField] private float frequency = 1f; // Frequency of the sine wave
    [SerializeField] private PlayerDie _playerDie;
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private SineProjectileLauncher _sineProjectileLauncher;
    private float speed; // Speed of the projectile
    public void Launch(Transform tr)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        transform.position = tr.position;
        transform.rotation = tr.rotation;
        StartCoroutine(MoveForward());
        IEnumerator MoveForward()
        {
            var time = 0f;
            while (time < lifetime)
            {
                //update speed
                // oscillate speed between speedRange.x and speedRange.y using sine wave
                speed = Mathf.Lerp(speedRange.x, speedRange.y, (Mathf.Sin(Time.time * frequency) + 1f) / 2f);
                // Move the projectile forward based on the current speed
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                time += Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
            _sineProjectileLauncher.AddProjectile(this);
        }
    }

}
