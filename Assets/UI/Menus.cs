using UnityEngine;

//All code relating to the menus the player can access before the game starts including the title scene,
//settings scene, shop scene, and exit scene
public class Menus : MonoBehaviour
{
    public static bool m_playOnAwake = false; //If true when the scene starts, then game starts automatically

    [Header("Title Screen")]
    [SerializeField] RectTransform m_titleScreen;
    [SerializeField] TMPro.TMP_Text m_highScoreText;

    [Header("Settings Screen")]
    [SerializeField] RectTransform m_settingsScreen;
    [SerializeField] UnityEngine.Audio.AudioMixer m_audioMixer;

    [Header("ShopScreen")]
    [SerializeField] RectTransform m_shopScreen;


    void Start()
    {
        //Start the game automatically when playOnAwake starts
        if (m_playOnAwake) StartGame();
        else
        {
            //Set the high score text on the title screen
            int highScore = PlayerPrefs.GetInt("HighScore");
            if (highScore > 0) m_highScoreText.text = "High Score: " + highScore.ToString();
            else m_highScoreText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //Start the game when the player presses the jump button
        if
        (
            InputUtilities.CanUseJumpInput() && //Check whether the player presses the jump input
            m_titleScreen.gameObject.activeSelf //Check whether the title screen is open
        )
        {
            StartGame();
            return;
        }
    }

    void StartGame()
    {
        //Dont automatically start the game the next time the player plays
        m_playOnAwake = false;
        
        //Activate the game manager and start the game
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.gameObject.SetActive(true);
        gameManager.StartGame();

        //Destroy Menus
        Destroy(gameObject);
    }

    #region Settings Menu
    public void SetSFXVolume(float _value)
    {
        m_audioMixer.SetFloat("sfxVolume", _value);
    }

    public void SetMusicVolume(float _value)
    {
        m_audioMixer.SetFloat("musicVolume", _value);
    }

    public void SetQuality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }
    #endregion

    public void ExitGame()
    {
        Application.Quit();
    }
}
