using UnityEngine;

[CreateAssetMenu(fileName = "New SingleShotStrategy", menuName = "FireStrategies/SingleShotStrategy")]
public class SingleShotStrategy : FireStrategy
{
    public override void Fire(GameObject owner, Transform firePoint)
    {
        var projectileGO = Instantiate(_projectilePrefab, firePoint.position, firePoint.rotation);
        if(projectileGO.TryGetComponent(out IProjectile projectile))
        {
            projectile.Initialize(owner, firePoint, firePoint.forward, _projectileSpeed, _projectileLifeTime);
        }
    }
}
