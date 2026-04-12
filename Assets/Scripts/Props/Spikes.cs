using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float _damage;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable))
        {
            if(damagable.CanTakeDamage())
            {
                damagable.TakeDamage(_damage);
            }
        }
    }
}
