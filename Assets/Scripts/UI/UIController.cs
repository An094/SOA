using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _healthBarOverlay;
    [SerializeField] private GameObject _gameOverOverlay;
    [SerializeField] private GameEvent _onPlayerDied;
    [SerializeField] private GameEvent _onEnterMiniGame;
    [SerializeField] private GameEvent _onExitMiniGame;
    private void Start()
    {
        SetGameOverOverlayVisibility(false);
        _healthBarOverlay.SetActive(true);
    }

    private void OnEnable()
    {
        _onPlayerDied.Register(OnPlayerDied);
        _onEnterMiniGame.Register(EnterMiniGame);
        _onExitMiniGame.Register(ExitMiniGame);
    }
    private void OnDisable()
    {
        _onPlayerDied.Unregister(OnPlayerDied);
        _onEnterMiniGame.Unregister(EnterMiniGame);
        _onExitMiniGame.Unregister(ExitMiniGame);
    }

    private void OnPlayerDied()
    {
        SetGameOverOverlayVisibility(true);
    }

    public void SetGameOverOverlayVisibility(bool bVisible)
    {
        _gameOverOverlay.SetActive(bVisible);
    }

    private void EnterMiniGame()
    {
        _healthBarOverlay.SetActive(false);
    }

    private void ExitMiniGame()
    {
        _healthBarOverlay.SetActive(true);
    }
}
