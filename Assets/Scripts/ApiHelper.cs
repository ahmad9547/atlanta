using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class ApiHelper : MonoBehaviour
{
    public static bool DisableDefaultErrorHandling = true;

    public static IEnumerator GetWithPercentage(string url, Action<string> success, Action<string> error, string payload = "", Dictionary<string, string> headers = null)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(url);

        if (headers != null)
        {
            foreach (KeyValuePair<string, string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key, pair.Value);
            }
        }

        webRequest.SetRequestHeader("Content-Type", "application/json");

        if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }

        Debug.Log($"Accessing Endpoint {url} with GET Request");
        webRequest.SendWebRequest();
        yield return new WaitForSeconds(1.5f);
        while (!webRequest.isDone) 
        {
            yield return new WaitForSeconds(0.01f);
            float progress = webRequest.downloadProgress;
            Debug.Log(progress);
            DataManager.Instance.UpdateLoadingPercentage(progress*100f);
        }

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                if (!DisableDefaultErrorHandling)
                {
                    Debug.Log("Error Occured for Endpoint : " + url);
                    Debug.LogError("Error Body : " + webRequest.error);
                    error(webRequest.error);
                }
                else
                {
                    error(webRequest.downloadHandler.text);
                }
                break;
            case UnityWebRequest.Result.Success:
                //Debug.Log("Response Received for Endpoint : " + url);
                //Debug.Log( "Response Body : " + webRequest.downloadHandler.text );
                success(webRequest.downloadHandler.text);
                break;
        }
    }


    public static IEnumerator Get(string url, Action<string> success, Action<string> error, string payload = "", Dictionary<string,string> headers = null)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get( url );

        if (headers != null)
        {
            foreach (KeyValuePair<string,string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key,pair.Value);
            }
        }
        
        webRequest.SetRequestHeader( "Content-Type", "application/json" );
        
        if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes( payload );
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw( bodyRaw );
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }
        
        Debug.Log($"Accessing Endpoint {url} with GET Request");
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                if (!DisableDefaultErrorHandling)
                {
                    Debug.Log("Error Occured for Endpoint : " + url);
                    Debug.LogError("Error Body : " + webRequest.error );
                    error(webRequest.error);
                }
                else
                {
                    error(webRequest.downloadHandler.text);
                }
                break;
            case UnityWebRequest.Result.Success:
                //Debug.Log("Response Received for Endpoint : " + url);
                //Debug.Log( "Response Body : " + webRequest.downloadHandler.text );
                success(webRequest.downloadHandler.text);
                break;
        }
    }

    public static async Task PostFormAsync(string url, Action<string> success, Action<string> error, Dictionary<string, string> formData, int lastIndex, Dictionary<string, string> headers = null)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Post(url, formData);

        // Set any additional headers
        if (headers != null)
        {
            foreach (KeyValuePair<string, string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key, pair.Value);
            }
        }

        //Print(formData);
        Debug.Log($"Accessing Endpoint {url} with POST Request");

        try
        {
            await webRequest.SendWebRequestAsync();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    if (!DisableDefaultErrorHandling)
                    {
                        Debug.Log("Error Occurred for Endpoint: " + url);
                        Debug.LogError("Error Body: " + webRequest.error);
                        error(webRequest.error);
                    }
                    else
                    {
                        error(webRequest.downloadHandler.text);
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Response Received for Endpoint: " + url);
                    DataManager.Instance.startDictionary = lastIndex+1;
                    Debug.Log("Response Body: " + webRequest.downloadHandler.text);
                    success(webRequest.downloadHandler.text);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception during POST request: " + ex.Message);
            error(ex.Message);
        }
    }

    private static void Print(Dictionary<string, string> formData)
    {
        foreach (string item in formData.Values)
        {
            Debug.Log(item);
        }
    }

    public static IEnumerator PostFormCoroutine(string url, Action<string> success, Action<string> error, Dictionary<string, string> formData, Dictionary<string, string> headers = null)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Post(url, formData);

        // Set any additional headers
        if (headers != null)
        {
            foreach (KeyValuePair<string, string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key, pair.Value);
            }
        }

        Debug.Log($"Accessing Endpoint {url} with POST Request");

        // Send the request and wait for it to complete
        yield return webRequest.SendWebRequest();

        // Handle the response based on the result
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Error Occurred for Endpoint: " + url);
                Debug.LogError("Error Body: " + webRequest.error);
                error(webRequest.error);
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint: " + url);
                Debug.Log("Response Body: " + webRequest.downloadHandler.text);
                success(webRequest.downloadHandler.text);
                break;
        }
    }


    public static async Task PostAsync(string url, Action<string> success, Action<string> error, string payload, Dictionary<string, string> headers = null)
    {
        using UnityWebRequest webRequest = new UnityWebRequest(url, "POST");

        if (headers != null)
        {
            foreach (KeyValuePair<string, string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key, pair.Value);
            }
        }

        webRequest.SetRequestHeader("Content-Type", "application/json");

        if (!string.IsNullOrEmpty(payload))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        webRequest.downloadHandler = new DownloadHandlerBuffer();

        Debug.Log($"Accessing Endpoint {url} with POST Request");

        try
        {
            await webRequest.SendWebRequestAsync();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    if (!DisableDefaultErrorHandling)
                    {
                        Debug.Log("Error Occurred for Endpoint: " + url);
                        Debug.LogError("Error Body: " + webRequest.error);
                        error(webRequest.error);
                    }
                    else
                    {
                        error(webRequest.downloadHandler.text);
                    }
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Response Received for Endpoint: " + url);
                    Debug.Log("Response Body: " + webRequest.downloadHandler.text);
                    success(webRequest.downloadHandler.text);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception during POST request: " + ex.Message);
            error(ex.Message);
        }
    }


    public static IEnumerator Post(string url, Action<string> success, Action<string> error, string payload, Dictionary<string,string> headers = null)
    {
        using UnityWebRequest webRequest = new UnityWebRequest(url, "POST");
        
        if (headers != null)
        {
            foreach (KeyValuePair<string,string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key,pair.Value);
            }
        }
        
        webRequest.SetRequestHeader( "Content-Type", "application/json" );
            
        if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes( payload );
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw( bodyRaw );
        }
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            
        Debug.Log($"Accessing Endpoint {url} with POST Request");
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
               if (!DisableDefaultErrorHandling)
                {
                    Debug.Log("Error Occured for Endpoint : " + url);
                    Debug.LogError("Error Body : " + webRequest.error );
                    error(webRequest.error);
                }
                else
                {
                    error(webRequest.downloadHandler.text);
                }   
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + url);
                Debug.Log( "Response Body : " + webRequest.downloadHandler.text );
                success(webRequest.downloadHandler.text);
                break;
        }
    }

    public static IEnumerator Patch(string url, Action<string> success, Action<string> error, string payload, Dictionary<string, string> headers = null)
    {
        using UnityWebRequest webRequest = new UnityWebRequest(url, "PATCH");

        if (headers != null)
        {
            foreach (KeyValuePair<string, string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key, pair.Value);
            }
        }

        webRequest.SetRequestHeader("Content-Type", "application/json");

        if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(payload);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }

        Debug.Log($"Accessing Endpoint {url} with PATCH Request");
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                if (!DisableDefaultErrorHandling)
                {
                    Debug.Log("Error Occured for Endpoint : " + url);
                    Debug.LogError("Error Body : " + webRequest.error);
                    error(webRequest.error);
                }
                else
                {
                    error(webRequest.downloadHandler.text);
                }
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + url);
                Debug.Log("Response Body : " + webRequest.downloadHandler.text);
                success(webRequest.downloadHandler.text);
                break;
        }
    }

    public static IEnumerator Delete(string url, Action<string> success, Action<string> error, string payload, Dictionary<string,string> headers = null)
    {
        using UnityWebRequest webRequest = new UnityWebRequest(url, "DELETE");
        
        if (headers != null)
        {
            foreach (KeyValuePair<string,string> pair in headers)
            {
                webRequest.SetRequestHeader(pair.Key,pair.Value);
            }
        }
        
        webRequest.SetRequestHeader( "Content-Type", "application/json" );
            
        if (payload != "")
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes( payload );
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw( bodyRaw );
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        }
            
        Debug.Log($"Accessing Endpoint {url} with DELETE Request");
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                if (!DisableDefaultErrorHandling)
                {
                    Debug.Log("Error Occured for Endpoint : " + url);
                    Debug.LogError("Error Body : " + webRequest.error );
                    error(webRequest.error);
                }
                else
                {
                    error(webRequest.downloadHandler.text);
                }
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + url);
                Debug.Log( "Response Body : " + webRequest.downloadHandler.text );
                success(webRequest.downloadHandler.text);
                break;
        }
    }
    
    public static IEnumerator DownloadImage( string mediaUrl, Action<Texture2D> success, Action<string> error )
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture( mediaUrl );
        
        yield return request.SendWebRequest();
        
        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
            case UnityWebRequest.Result.ProtocolError:
                if (!DisableDefaultErrorHandling)
                {
                    Debug.Log("Error Occured for Endpoint : " + mediaUrl);
                    Debug.LogError("Error Body : " + request.error );
                    error(request.error);
                }
                else
                {
                    error(request.downloadHandler.text);
                }
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Response Received for Endpoint : " + mediaUrl);
                Debug.Log( "Response Body : " + request.downloadHandler.text );
                var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                success(texture);
                break;
        }
    }
}
