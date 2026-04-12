using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _freeLookCamera;
    [SerializeField] private CinemachineCamera _miniGameCamera;
    [SerializeField] private GameEvent _onEnterMiniGame;
    [SerializeField] private GameEvent _onExitMiniGame;

    private void OnEnable()
    {
        _onEnterMiniGame.Register(EnterMiniGame);
        _onExitMiniGame.Register(ExitMiniGame);  
    }

    private void OnDisable()
    {
        _onEnterMiniGame.Unregister(EnterMiniGame);
        _onExitMiniGame.Unregister(ExitMiniGame);
    }

    private void EnterMiniGame(Unit _)
    {
        _freeLookCamera.Priority = 0;
        _miniGameCamera.Priority = 1;
    }

    private void ExitMiniGame(Unit _)
    {
        _freeLookCamera.Priority = 1;
        _miniGameCamera.Priority = 0;
    }
}
