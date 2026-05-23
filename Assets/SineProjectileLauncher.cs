using System.Collections.Generic;
using UnityEngine;

public class SineProjectileLauncher : MonoBehaviour
{
    [SerializeField] private List<SineProjectile> _projectiles = new();
    public void AddProjectile(SineProjectile projectile)
    {
        _projectiles.AddUnique(projectile);
    }
    public void Launch()
    {
        if (_projectiles.Count == 0) return;
        _projectiles[0].Launch(transform);
        _projectiles.RemoveAt(0);
    }
}
