using UnityEngine;

public interface IProjectile
{
    void Initialize(GameObject owner, Transform firePoint, Vector3 direction, float speed, float lifeTime);
}
