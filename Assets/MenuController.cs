using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public static MenuController Instance { get; private set; }

    [SerializeField]
    private Canvas GameOverCanvas;

    [SerializeField]
    private Canvas CongratulationsCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void ShowGameOverMenu()
    {
        this.GameOverCanvas.gameObject.SetActive(true);
    }

    public void ShowCongratulationMenu()
    {
        this.CongratulationsCanvas.gameObject.SetActive(true);
    }

}
