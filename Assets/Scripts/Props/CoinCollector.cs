using Platformer;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private CollectableRuntimeSet _coinsInRoom;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController _))
        {
            foreach(var coin  in _coinsInRoom.Items)
            {
                coin.Collect(transform);
            }
        }
    }
}
