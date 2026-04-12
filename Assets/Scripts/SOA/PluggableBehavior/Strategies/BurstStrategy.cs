using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New BurstStrategy", menuName = "FireStrategies/BurstStrategy")]
public class BurstStrategy : FireStrategy
{
    [SerializeField] private int _burstCount = 3;
    [SerializeField] private float _burstDelay = 0.1f;

    public override void Fire(GameObject owner, Transform firePoint)
    {
        if (owner.TryGetComponent(out MonoBehaviour monoBehaviour))
        {
            monoBehaviour.StartCoroutine(FireBurst(owner, firePoint));
        }
    }

    private IEnumerator FireBurst(GameObject owner, Transform firePoint)
    {
        for (int i = 0; i < _burstCount; i++)
        {
            var projectileGO = Instantiate(_projectilePrefab, firePoint.position, firePoint.rotation);
            if (projectileGO.TryGetComponent(out IProjectile projectile))
            {
                projectile.Initialize(owner, firePoint, firePoint.forward, _projectileSpeed, _projectileLifeTime);
            }

            if (i < _burstCount - 1)
            {
                yield return new WaitForSeconds(_burstDelay);
            }
        }
    }
}
