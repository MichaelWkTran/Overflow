using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    uint m_score;
    uint m_carrots;
    public Cinemachine.CinemachineVirtualCamera m_virtualCamera;
    public bool m_gameStarted { get; private set; } = false;
    public bool m_paused { get; private set; } = false;

    [Header("Level")]
    public GameObject[] m_levelSegments;
    [SerializeField] Transform m_gameWorld;
    [SerializeField] float m_loopDistance;

    [Header("Death Wall")]
    [SerializeField] Transform m_deathWall;
    [SerializeField] float m_deathWallMoveSpeed;
    [SerializeField] float m_deathWallAcceleration;
    [SerializeField] float m_deathWallMaxMoveSpeed;
    [SerializeField] float m_furthestDistance;

    [Header("UI")]
    [SerializeField] Canvas m_canvas;
    [SerializeField] TMPro.TMP_Text m_scoreUI;
    [SerializeField] GameObject m_pauseScreen;
    [SerializeField] GameObject m_gameOverScreen;
    [SerializeField] TMPro.TMP_Text m_gameOverScoreUI;
    [SerializeField] TMPro.TMP_Text m_gameOverHighScoreUI;

    void Update()
    {
        //Dont update game if it has not started
        if (!m_gameStarted) return;

        //Shift objects back if the camera goes further than loopDistance
        if (Camera.main.transform.position.x > m_loopDistance)
        {
            //Shift all objects in world back by loopDistance
            foreach (Transform worldtransforms in m_gameWorld)
            {
                worldtransforms.position -= Vector3.right * m_loopDistance;
            }

            //Shift all particles in the world back by loopDistance
            foreach (ParticleSystem particleSystem in m_gameWorld.GetComponentsInChildren<ParticleSystem>())
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
                particleSystem.GetParticles(particles);

                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].position -= Vector3.right * m_loopDistance;
                }

                particleSystem.SetParticles(particles);
            }


            //Shift virtualCamera in world back by loopDistance
            m_virtualCamera.ForceCameraPosition(m_virtualCamera.transform.position -= Vector3.right * m_loopDistance, Quaternion.identity);
            FindObjectOfType<ParallaxBackground>().ClearLastCameraPosition();
        }

        //Move Death Wall
        m_deathWall.transform.position += Vector3.right * m_deathWallMoveSpeed * Time.deltaTime;
        m_deathWallMoveSpeed += m_deathWallAcceleration * Time.deltaTime * Time.deltaTime;
        if (m_deathWallMoveSpeed > m_deathWallMaxMoveSpeed) m_deathWallMoveSpeed = m_deathWallMaxMoveSpeed;

        //Prevent the death wall from getting too far off screen
        float cameraLeftWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x;
        if (cameraLeftWorldPosition - m_deathWall.position.x - m_furthestDistance > 0)
        {
            m_deathWall.position = new Vector3(cameraLeftWorldPosition - m_furthestDistance, m_deathWall.position.y, m_deathWall.position.z);
        }

        //Set the sprite mask of the death wall
        {
            SpriteRenderer[] deathWallSprites = m_deathWall.GetComponentsInChildren<SpriteRenderer>();
            SpriteMask[] deathWallMask = m_deathWall.GetComponentsInChildren<SpriteMask>();

            for (int i = 0; i < deathWallSprites.Length; i++) deathWallMask[i].sprite = deathWallSprites[i].sprite;
        }
    }

    public void Pause()
    {
        m_paused = true;
        m_pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        m_paused = false;
        m_pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void triggerGameOver()
    {
        //Update High Score
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (m_score > highScore)
        {
            highScore = (int)m_score; PlayerPrefs.SetInt("HighScore", highScore);
        }
        else
        {
            //Update Score UI
            m_gameOverScoreUI.text = "Score: " + m_score.ToString();
            m_gameOverHighScoreUI.text = "High Score: " + highScore.ToString();
        }

        //Update Carrots
        

        //Open GameOver Screen
        m_gameOverScreen.SetActive(true);
    }

    public void StartGame()
    {
        m_canvas.gameObject.SetActive(true);
        m_gameStarted = true;
    }

    public void GoToTitle()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        GoToTitle();
        Menus.m_playOnAwake = true;
    }

    public void AddScore(uint _score)
    {
        m_score += _score;
        m_scoreUI.text = m_score.ToString();
    }

    public void AddCarrot(uint _carrots)
    {
        m_carrots += _carrots;
    }
}
