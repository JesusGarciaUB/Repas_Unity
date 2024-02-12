using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiHelper
{
    public static IEnumerator Get(string url, Dictionary<string, string> parameters, Action<string> onSuccess, Action<Exception> onFailure)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        foreach(KeyValuePair<string, string> parameter in parameters)
        {
            request.SetRequestHeader(parameter.Key, parameter.Value);
        }

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onFailure(new Exception(request.error));
            yield break;
        }

        string text = request.downloadHandler.text;

        onSuccess(text);
    }

    public static IEnumerator Get<T>(string url, Dictionary<string, string> parameters, Action<T> onSuccess, Action<Exception> onFailure)
    {
        return Get(url, parameters, jsonText => 
        {
            try
            {
                T result = JsonUtility.FromJson<T>(jsonText);
                onSuccess(result);
            } 
            catch (Exception e)
            {
                onFailure(e);
            }
        }, onFailure);
    }
}
