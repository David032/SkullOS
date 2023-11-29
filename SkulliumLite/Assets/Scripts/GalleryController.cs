using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    public GameObject displayElement;
    List<Capture> Captures = new List<Capture>();
    List<GameObject> DisplayElements = new List<GameObject>();

    GameObject objectBeingCreated;
    // Start is called before the first frame update
    void Start()
    {
        PopulateDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PopulateDisplay()
    {
        StartCoroutine(GetCaptures());
        foreach (Capture capture in Captures)
        {
            var go = Instantiate(displayElement);
            DisplayElements.Add(go);
            objectBeingCreated = go;
            var fileparts = capture.filename.Split('/');

            StartCoroutine(SetupElement(fileparts[^1]));
        }
    }

    IEnumerator GetCaptures()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://servoskull.local:5000/Captures/All"))
        {
            yield return webRequest.SendWebRequest();
            var result = webRequest.downloadHandler.text;
            result = fixJson(result);
            print(result);
            Captures = JsonUtility.FromJson<List<Capture>>(result);
            print(Captures);
        }
    }

    //id is filename+extension
    IEnumerator SetupElement(string id)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("http://servoskull.local:5000/Captures/Image" + id))
        {
            yield return webRequest.SendWebRequest();
            print(webRequest.url);
            print(webRequest.result);
            var texture = DownloadHandlerTexture.GetContent(webRequest);
            objectBeingCreated.GetComponent<RawImage>().texture = texture;
        }
    }

    string fixJson(string value)
    {
        value = "{\"Captures\":" + value + "}";
        return value;
    }
}


[Serializable]
public class Capture
{
    public string filename;
    public string date;
    public string time;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
