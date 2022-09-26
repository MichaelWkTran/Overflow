using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public uint score;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    [Header("UI")]
    [SerializeField] TMPro.TMP_Text scoreUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = score.ToString();
    }
}
