using UnityEngine;
using Utilities;

public class SimpleProjectile : MonoBehaviour, IProjectile
{
    private GameObject _cachedOwner;
    private Vector3 _cachedVelocity;
    private float _cachedLifeTime;

    private bool _isInitialized = false;
    private CountdownTimer _lifeTimeTimer;

    public void Initialize(GameObject owner, Transform firePoint, Vector3 direction, float speed, float lifeTime)
    {
        _isInitialized = true;
        _cachedOwner = owner;
        _cachedVelocity = speed * direction;
        _cachedLifeTime = lifeTime;

        transform.position = firePoint.position;
        transform.rotation = Quaternion.LookRotation(direction);

        _lifeTimeTimer = new(lifeTime);
        _lifeTimeTimer.OnTimerStop += () =>
        {
            Destroy(gameObject);
        };
        _lifeTimeTimer.Start();
    }

    private void Update()
    {
        if (!_isInitialized) return;
        _lifeTimeTimer.Tick(Time.deltaTime);
        transform.position += _cachedVelocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != _cachedOwner)
        {
            Destroy(gameObject);
        }
    }
}
