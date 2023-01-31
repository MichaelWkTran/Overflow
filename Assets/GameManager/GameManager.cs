using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    uint score;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public bool gameStarted { get; private set; } = false;
    public bool paused { get; private set; } = false;

    [Header("Level")]
    public GameObject[] levelSegments;
    [SerializeField] Transform gameWorld;
    [SerializeField] float loopDistance;

    [Header("Death Wall")]
    [SerializeField] Transform deathWall;
    [SerializeField] float deathWallMoveSpeed;
    [SerializeField] float deathWallAcceleration;
    [SerializeField] float deathWallMaxMoveSpeed;
    [SerializeField] float furthestDistance;

    [Header("UI")]
    [SerializeField] Canvas canvas;
    [SerializeField] TMPro.TMP_Text scoreUI;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject gameOverScreen;

    void Update()
    {
        //Dont update game if it has not started
        if (!gameStarted) return;

        //Shift objects back if the camera goes further than loopDistance
        if (Camera.main.transform.position.x > loopDistance)
        {
            //Shift all objects in world back by loopDistance
            foreach (Transform worldtransforms in gameWorld)
            {
                worldtransforms.position -= Vector3.right * loopDistance;
            }

            //Shift all particles in the world back by loopDistance
            foreach (ParticleSystem particleSystem in gameWorld.GetComponentsInChildren<ParticleSystem>())
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
                particleSystem.GetParticles(particles);

                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].position -= Vector3.right * loopDistance;
                }

                particleSystem.SetParticles(particles);
            }

            //Shift virtualCamera in world back by loopDistance
            virtualCamera.ForceCameraPosition(virtualCamera.transform.position -= Vector3.right * loopDistance, Quaternion.identity);
        }

        //Move Death Wall
        deathWall.transform.position += Vector3.right * deathWallMoveSpeed * Time.deltaTime;
        deathWallMoveSpeed += deathWallAcceleration * Time.deltaTime * Time.deltaTime;
        if (deathWallMoveSpeed > deathWallMaxMoveSpeed) deathWallMoveSpeed = deathWallMaxMoveSpeed;

        //Prevent the death wall from getting too far off screen
        float cameraLeftWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x;
        if (cameraLeftWorldPosition - deathWall.position.x - furthestDistance > 0)
        {
            deathWall.position = new Vector3(cameraLeftWorldPosition - furthestDistance, deathWall.position.y, deathWall.position.z);
        }

        //Set the sprite mask of the death wall
        {
            SpriteRenderer[] deathWallSprites = deathWall.GetComponentsInChildren<SpriteRenderer>();
            SpriteMask[] deathWallMask = deathWall.GetComponentsInChildren<SpriteMask>();

            for (int i = 0; i < deathWallSprites.Length; i++) deathWallMask[i].sprite = deathWallSprites[i].sprite;
        }
    }

    public void Pause()
    {
        paused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void UnPause()
    {
        paused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void triggerGameOver()
    {
        //Update High Score
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (score > highScore) { highScore = (int)score; PlayerPrefs.SetInt("HighScore", highScore); }
        
        //Open GameOver Screen
        gameOverScreen.SetActive(true);
    }

    public void StartGame()
    {
        canvas.gameObject.SetActive(true);
        gameStarted = true;
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        GoToTitle();
        Menus.playOnAwake = true;
    }

    public void AddScore(uint _score)
    {
        score += _score;
        scoreUI.text = score.ToString();
    }
}
