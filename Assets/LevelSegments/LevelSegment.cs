using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] float segmentWidth; //How is the level segment
    [SerializeField] bool createdNextSegment = false; //Whether the next level segment has been created or not
    static float segmentGizmoHeight = 10.0f;  //The height of the level segment box displayed in gizmos
    GameManager gameManager;
    
    void Start()
    {
        //Get the game manager
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        //If the level segment appears on screen, create the next segment
        if (!createdNextSegment && transform.position.x < Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect))
        {
            createdNextSegment = true;
            Instantiate
            (
                gameManager.m_levelSegments[Random.Range(0, gameManager.m_levelSegments.Length)],
                transform.position + (Vector3.right * segmentWidth), Quaternion.identity
            ).transform.parent = transform.parent;
        }

        //Destroy the segment if it goes off screen
        if (transform.position.x + segmentWidth < Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect))
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube
        (
            transform.position + (new Vector3(segmentWidth, segmentGizmoHeight) /2.0f),
            new Vector3(segmentWidth, segmentGizmoHeight, 1.0f)
        );
    }
}
