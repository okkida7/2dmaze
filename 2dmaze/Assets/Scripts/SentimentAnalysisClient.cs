using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SentimentAnalysisClient : MonoBehaviour
{
    private string baseUrl = "http://127.0.0.1:7890/api/sentiment";

    private void Start()
    {
        StartCoroutine(PostSentence("I love this product!"));
    }

    IEnumerator PostSentence(string sentence)
    {
        // Create a request with a JSON payload
        UnityWebRequest www = new UnityWebRequest(baseUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes($"{{\"Text\": \"{sentence}\"}}");
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Request failed: {www.error}");
        }
        else
        {
            Debug.Log($"Received: {www.downloadHandler.text}");
            
            // Optionally, parse the JSON response if needed
            // SentimentResponse response = JsonUtility.FromJson<SentimentResponse>(www.downloadHandler.text);
            // Debug.Log($"Sentiment Score: {response.sentiment_score}");
        }
    }

    [System.Serializable]
    public class SentimentResponse
    {
        public float sentiment_score;
    }
}