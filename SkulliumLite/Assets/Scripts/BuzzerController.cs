using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class BuzzerController : MonoBehaviour
{
    string tuneToPlay;
    public TextMeshProUGUI text;
    int tuneInt = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTuneToHappyBirthday() //0
    {
        text.text = "Will play: Happy Birthday";
        tuneInt = 0;
        tuneToPlay = "happyBirthday";
    }

    public void SetTuneToAlphabetSong() //1
    {
        text.text = "Will play: Alphabet Song";
        tuneInt = 1;
        tuneToPlay = "alphabetSong";
    }

    public void PlayTune()
    {
        StartCoroutine(SendTuneRequest());
    }

    IEnumerator SendTuneRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("http://servoskull.local:5000/Buzzer/PlayTune?tune=" + tuneInt))
        {
            yield return webRequest.SendWebRequest();
        }
    }
}
