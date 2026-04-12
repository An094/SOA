using UnityEngine;

public interface IDamagable
{
    bool CanTakeDamage();
    bool TakeDamage(float InDamage);
}
