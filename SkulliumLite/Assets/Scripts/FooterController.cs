using UnityEngine;

public class FooterController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Gallery;
    public GameObject BuzzerController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        Gallery.SetActive(false);
        BuzzerController.SetActive(false);
    }
    public void ShowGallery()
    {
        MainMenu.SetActive(false);
        Gallery.SetActive(true);
        BuzzerController.SetActive(false);
    }
    public void ShowBuzzerController()
    {
        MainMenu.SetActive(false);
        Gallery.SetActive(false);
        BuzzerController.SetActive(true);
    }
}
