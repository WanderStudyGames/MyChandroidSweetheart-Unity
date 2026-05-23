using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerManager.Tag))
        {
            SceneHandler.LoadScene(SceneManager.GetActiveScene().name, 0);
        }
    }
}
