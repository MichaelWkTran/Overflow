using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public uint score;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public bool paused { get; private set; } = false;

    [Header("Level")]
    public GameObject[] levelSegments;
    [SerializeField] Transform gameWorld;
    [SerializeField] float loopDistance;

    [Header("Death Wall")]
    [SerializeField] Transform deathWall;
    [SerializeField] float deathWallMoveSpeed;
    [SerializeField] float deathWallAcceleration;

    [Header("UI")]
    [SerializeField] TMPro.TMP_Text scoreUI;
    [SerializeField] GameObject pauseScreen;

    void Start()
    {

    }

    void Update()
    {
        scoreUI.text = score.ToString();

        //Shift objects back if the camera goes further than loopDistance
        if (Camera.main.transform.position.x > loopDistance)
        {
            //Shift all objects in world back by loopDistance
            foreach (Transform worldtransforms in gameWorld)
            {
                worldtransforms.position -= Vector3.right * loopDistance;
            }

            //Shift virtualCamera in world back by loopDistance
            virtualCamera.ForceCameraPosition(virtualCamera.transform.position -= Vector3.right * loopDistance, Quaternion.identity);
        }

        //Move Death Wall
        deathWall.transform.position += Vector3.right * deathWallMoveSpeed * Time.deltaTime;
        deathWallMoveSpeed += deathWallAcceleration;
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
}
