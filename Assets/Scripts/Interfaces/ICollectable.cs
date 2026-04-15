using UnityEngine;

public interface ICollectable
{
    bool CanCollect();
    void Collect(Transform targetTransform);
}
