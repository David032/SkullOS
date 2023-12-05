using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Deleter : MonoBehaviour
{
    public string idToDelete;

    public void Delete()
    {
        StartCoroutine(DeleteElement());
    }

    IEnumerator DeleteElement()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Delete(URLS.BaseUrl() + "Captures/Delete?filePath=" + idToDelete))
        {
            yield return webRequest.SendWebRequest();
        }
        Destroy(gameObject);
    }
}
