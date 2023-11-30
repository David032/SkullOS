using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    public GameObject displayElement;
    public List<string> captures = new List<string>();
    public CaptureList CaptureList = new CaptureList();
    List<GameObject> DisplayElements = new List<GameObject>();

    GameObject objectBeingCreated;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PopulateGallery()
    {
        CaptureList = new CaptureList();
        StartCoroutine(PopulateDisplay());
    }

    public void ClearGallery()
    {
        foreach (var item in DisplayElements)
        {
            Destroy(item);
        }
        CaptureList = null;
    }

    IEnumerator PopulateDisplay()
    {
        yield return StartCoroutine(GetCaptures());
        foreach (string capture in CaptureList.Captures)
        {
            var go = Instantiate(displayElement);
            go.transform.SetParent(transform, false);
            go.transform.localScale = Vector3.one;
            DisplayElements.Add(go);
            objectBeingCreated = go;

            yield return StartCoroutine(SetupElement(capture));
        }
        yield return null;
    }

    IEnumerator GetCaptures()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://servoskull.local:5000/Captures/AllCaptures"))
        {
            yield return webRequest.SendWebRequest();
            var result = webRequest.downloadHandler.text;
            result = fixJson(result);
            print(result);


            CaptureList.rawData = result;
            JsonUtility.FromJsonOverwrite(result, CaptureList);
            print(CaptureList.Captures.Count);
        }
    }

    //id is filename+extension
    IEnumerator SetupElement(string id)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("http://servoskull.local:5000/Captures/Image/?fileId=" + id))
        {
            yield return webRequest.SendWebRequest();
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
public class CaptureList
{
    public string rawData;
    public List<string> Captures = new List<string>();
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
