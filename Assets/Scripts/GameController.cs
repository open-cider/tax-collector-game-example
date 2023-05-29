using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
    [HideInInspector] public int CoinsCollected = 0;

    private bool _isGameOver = false;

    private static string _url  = "https://api.opencider.com/v1/user/query/summary-data";
    private static string _token = "SERVICE_USER_TOKEN";

    
    #region Events
    public void IncrementCoinsCollected() {
        if (!_isGameOver)
        CoinsCollected += 1;
    }

    public void SayGameOver() {
        _isGameOver = true;
        Debug.LogWarning("Posting Score to Open Cider. Score: " + CoinsCollected);
        StartCoroutine(UploadScore());
    }
    #endregion


    /* POST Summary Data */
    [Serializable]
    public class SummaryDataPostRequest{
        public string token;
        public string metric0;
        public int metric1;
        public float metric2;
        public int metric3;
        public float metric4;
    }

    [Serializable]
    public class SummaryDataPostResponse{
        public string status;
        public string message;
    }

    private IEnumerator UploadScore() {
        var request = new SummaryDataPostRequest();
        
        request.token   = _token;
        request.metric0 = "Some player config value here maybe";
        request.metric1 = CoinsCollected;
        /*We don't care about the rest */
        request.metric2 = 0;
        request.metric3 = 0;
        request.metric4 = 0;

        var json = JsonUtility.ToJson(request);

        var uwr = new UnityWebRequest(_url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            var response = JsonUtility.FromJson<SummaryDataPostResponse>(uwr.downloadHandler.text);
            Debug.Log("Received: " + response.message);
        }
    }
}
