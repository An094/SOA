using UnityEngine;

public class GameController : MonoBehaviour
{
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
}
