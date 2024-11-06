using System;
using System.Threading.Tasks;
using UnityEngine.Networking;

public static class UnityWebRequestExtensions
{
    public static Task<UnityWebRequestAsyncOperation> SendWebRequestAsync(this UnityWebRequest request)
    {
        var tcs = new TaskCompletionSource<UnityWebRequestAsyncOperation>();
        var operation = request.SendWebRequest();
        operation.completed += _ => tcs.SetResult(operation);
        return tcs.Task;
    }
}