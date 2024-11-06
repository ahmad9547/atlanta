using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestScript : MonoBehaviour
{
    string url = "https://amw.kiwicreations.io:3000/paint"; // Replace with your URL
    public ListHitLines hitLines = new ListHitLines();
    void Start()
    {
        StartCoroutine(DownloadFile(url));
    }

    IEnumerator DownloadFile(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();

        while (!request.isDone)
        {
            // Update the UI with the progress
            float progress = request.downloadProgress;
            Debug.Log(progress);

            yield return new WaitForSeconds(0.01f);
        }

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            hitLines = JsonConvert.DeserializeObject<ListHitLines>(request.downloadHandler.text);
            Debug.Log("Download complete!");
            // Handle the downloaded data here, e.g., save it to a file
        }
    }
}
