using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;


namespace UserManagement
{
    public class UserInfoLogger : MonoBehaviour
    {
        private static UserInfoLogger instance;

        private string apiUrl = "";

        [DllImport("__Internal")]
        private static extern void GetBrowserDetails();

        public void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR

            if(instance != null && instance != this) 
            {
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(instance.gameObject);
            }

            GetBrowserDetails();
#endif
        }

        public void OnReceiveBrowserDetails(string jsonBrowserDetails)
        {
            StartCoroutine(SendUserData(jsonBrowserDetails));
        }

        private IEnumerator SendUserData(string jsonBrowserDetails)
        {
            string osVersion = SystemInfo.operatingSystem;

            BrowserDetails browserDetails = JsonUtility.FromJson<BrowserDetails>(jsonBrowserDetails);

            UserData userData = new UserData()
            {
                OSVersion = osVersion,
                BrowserDetails = browserDetails
            };

            string jsonData = JsonUtility.ToJson(userData);

            UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data sent successfully : "+jsonData);
            }
            else
            {
                Debug.LogError("Error sending data: " + request.error);
            }
        }
    }

    [System.Serializable]
    public class BrowserDetails
    {
        public string ip;
        public string browserType;
        public string browserVersion;
    }

    [System.Serializable]
    public class UserData
    {
        public string OSVersion;
        public BrowserDetails BrowserDetails;
    }
}