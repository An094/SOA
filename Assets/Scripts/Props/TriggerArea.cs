using Platformer;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private GameEvent _triggerEnterEvent;
    [SerializeField] private GameEvent _triggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController playerController))
        {
            _triggerEnterEvent.Raise();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            _triggerExitEvent.Raise();
        }
    }
}
