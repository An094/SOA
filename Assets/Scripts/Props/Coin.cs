using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private bool _isInRoom;
    [SerializeField] private CollectableRuntimeSet _activeCoins;
    [SerializeField] private CollectableRuntimeSet _coinsInRoom;
    private void OnEnable()
    {
        _activeCoins.Add(this);
        if(_isInRoom ) _coinsInRoom.Add(this);
    }

    private void OnDisable()
    {
        _activeCoins.Remove(this);
        if(_coinsInRoom) _coinsInRoom.Remove(this);
    }

    public void Collect(Transform targetTransform)
    {
        Destroy(gameObject);
    }

    public bool CanCollect()
    {
        return true;
    }
}
