using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenuDriver : MonoBehaviour
{
    public RawImage LatestImage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetLatestImage());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshImage()
    {
        StartCoroutine(GetLatestImage());
    }

    public void DeleteMostRecent()
    {
        StartCoroutine(DeleteLatestImage());
        StartCoroutine(GetLatestImage());
    }

    IEnumerator GetLatestImage()
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(URLS.BaseUrl() + "Captures/MostRecent"))
        {
            yield return webRequest.SendWebRequest();
            print(webRequest.url);
            print(webRequest.result);
            var texture = DownloadHandlerTexture.GetContent(webRequest);
            LatestImage.texture = texture;
        }
    }

    IEnumerator DeleteLatestImage()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(URLS.BaseUrl() + "Captures/DeleteMostRecent"))
        {
            yield return webRequest.SendWebRequest();
        }
    }

}
