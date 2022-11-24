using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menus : MonoBehaviour
{
    public static bool playOnAwake = false;
    [SerializeField] RectTransform titleScreen;

    void Start()
    {
        if (playOnAwake) StartGame();
    }

    void Update()
    {
        if
        (
            Input.GetButton("Jump") && titleScreen.gameObject.activeSelf &&
            !InputUtilities.IsMouseOverUI((RaycastResult _raycastResult) => { return _raycastResult.gameObject.GetComponent<Selectable>() == null; })
        )
        {
            StartGame();
            return;
        }
    }

    public void StartGame()
    {
        playOnAwake = false;
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.gameObject.SetActive(true);
        gameManager.StartGame();
        Destroy(gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
