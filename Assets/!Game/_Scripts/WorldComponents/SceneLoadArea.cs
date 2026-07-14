
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SceneLoader))]
public class SceneLoadArea : MonoBehaviour
{
    [SerializeField] private SFX _enterLoadAreaSFX;
    private SceneLoader sceneLoader;
    private void Awake()
    {
        sceneLoader = GetComponent<SceneLoader>();
    }
    private List<Collider> _players = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerManager.Tag) && !_players.Contains(other))
        {
            Debug.Log("Entered Scene Load Area: " + other.gameObject.name, other);
            PlayerManager.SetMovementEnabled(false);
            PlayerManager.SetCameraMovementEnabled(false);
            PlayerManager.SetGravityEnabled(false);
            _players.Add(other);
            Debug.Log(_players.Count);
            foreach (var player in _players)
            {
                Debug.Log("Player: " + player == null);
            }
            // if (_enterLoadAreaSFX != null)
            //     DontDestroyOnLoad(_enterLoadAreaSFX.PlayAtPoint(transform.position));
            sceneLoader.LoadScene();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var b = GetComponent<BoxCollider>();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(b.center, new(b.size.x, b.size.y, b.size.z));
        Gizmos.color = new(0, 1, 0, 0.05f);
        Gizmos.DrawCube(b.center, new(b.size.x, b.size.y, b.size.z));
    }

}
