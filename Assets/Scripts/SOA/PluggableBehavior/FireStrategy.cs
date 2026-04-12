using UnityEngine;

public abstract class FireStrategy : ScriptableObject
{
    [SerializeField] protected float _projectileSpeed;
    [SerializeField] protected float _projectileLifeTime;
    [SerializeField] protected GameObject _projectilePrefab;

    public abstract void Fire(GameObject owner, Transform firePoint);
}
