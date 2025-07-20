using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }
    [SerializeField] private GameObject PanelWin;
    [SerializeField] private GameObject PanelLosing;
    private bool gameEnd = false;
    private void Awake()
    {
        Instance = this;
    }
    public void Losing()
    {
        if (!IsGameEnd())
        {
            gameEnd = true;
            PanelLosing.SetActive(true);
            AudioManager.Instance.PlayOneShot(SoundType.Losing);
            AudioManager.Instance.DestroySoundTypeSource(SoundType.Charge);

        }
    }

    public void Victory()
    {
        if (!IsGameEnd())
        {
            gameEnd = true;
            PanelWin.SetActive(true);
            AudioManager.Instance.PlayOneShot(SoundType.Victory);
            AudioManager.Instance.DestroySoundTypeSource(SoundType.Charge);
        }
      
    }
    public bool IsGameEnd()
    {
        return gameEnd;
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
        AudioManager.Instance.PlayOneShot(SoundType.Button);
    }
}
