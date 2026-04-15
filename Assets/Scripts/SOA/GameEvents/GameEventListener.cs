using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private UnityEvent _response;

    private void OnEnable() => _gameEvent.Register(OnEventRaise);

    private void OnDisable() => _gameEvent.Unregister(OnEventRaise);

    public void OnEventRaise()
    {
        _response.Invoke();
    }
}