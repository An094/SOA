using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private bool _isInRoom;
    [SerializeField] private CollectableRuntimeSet _activeCoins;
    [SerializeField] private CollectableRuntimeSet _coinsInRoom;

    private Tween _selfRotate;
    private Tween _moveTo;
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
        _selfRotate.Kill();
        _moveTo = transform.DOJump(targetTransform.position, jumpPower: 3f, numJumps: 1, duration: 0.5f).OnComplete(() => { _moveTo.Kill(); Destroy(gameObject); });
        
    }

    public bool CanCollect()
    {
        return true;
    }

    private void Start()
    {
        _selfRotate = transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
}
