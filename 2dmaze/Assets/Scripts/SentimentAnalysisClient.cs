using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SentimentAnalysisClient : MonoBehaviour
{
    private string apiUrl = "http://localhost:5000/api/sentiment";  // Modify the URL to match your ASP.NET service URL

    public void AnalyzeSentiment(string text)
    {
        StartCoroutine(AnalyzeSentimentRoutine(text));
    }

    private IEnumerator AnalyzeSentimentRoutine(string text)
    {
        ModelInput input = new ModelInput { Text = text };

        string json = JsonUtility.ToJson(input);
        
        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, json))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                ModelOutput output = JsonUtility.FromJson<ModelOutput>(www.downloadHandler.text);
                Debug.Log(output.Prediction);  // You can handle the result accordingly here
            }
        }
    }
}

[System.Serializable]
public class ModelInput
{
    public string Text;
}

[System.Serializable]
public class ModelOutput
{
    public bool Prediction;
}
