using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponType", menuName = "ExtendableEnums/WeaponType")]
public class WeaponType : ScriptableObject
{
    public List<WeaponType> _preys;

    public bool IsWinner(WeaponType other)
    {
        return _preys.Contains(other);
    }
}
