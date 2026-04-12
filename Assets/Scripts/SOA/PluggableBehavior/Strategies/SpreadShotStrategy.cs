using UnityEngine;

[CreateAssetMenu(fileName = "New SpreadShotStrategy", menuName = "FireStrategies/SpreadShotStrategy")]
public class SpreadShotStrategy : FireStrategy
{
    [SerializeField] private float _spreadAngle = 20f;
    [SerializeField] private int _projectileNumber = 5;
    public override void Fire(GameObject owner, Transform firePoint)
    {
        for (int i = 0; i < _projectileNumber; i++)
        {
            var projectileGO = Instantiate(_projectilePrefab, firePoint.position, firePoint.rotation);
            if (projectileGO.TryGetComponent(out IProjectile projectile))
            {
                Vector3 forward = firePoint.forward;
                Quaternion rotation = Quaternion.AngleAxis(_spreadAngle * (i - _projectileNumber / 2 ), Vector3.up);
                Vector3 rotatedForward = rotation * forward;

                projectile.Initialize(owner, firePoint, rotatedForward, _projectileSpeed, _projectileLifeTime);
            }
        }

    }
}
