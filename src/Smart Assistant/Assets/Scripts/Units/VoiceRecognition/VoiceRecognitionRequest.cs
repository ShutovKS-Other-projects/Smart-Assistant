using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Units.VoiceRecognition
{
    public static class VoiceRecognitionRequest
    {
        private const string SERVER_URL = "http://127.0.0.1:5000/transcribe_audio";

        public static async Task<string> SendVoiceRequestAsync(string filePath)
        {
            var form = new WWWForm();
            var audioData = File.ReadAllBytes(filePath);

            form.AddBinaryData("audio", audioData, "audio.mp3", "audio/mp3");

            var www = UnityWebRequest.Post(SERVER_URL, form);
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = www.downloadHandler.text;
                var response = JsonUtility.FromJson<Response>(jsonResponse);
                
                if (response.status == "success")
                {
                    var transcriptions = response.transcriptions;
                    return transcriptions;
                }
                
                Debug.LogError("Error: " + response.message);
                return null;
            }

            Debug.LogError("Request failed. Error: " + www.error);
            return null;
        }
        
        [System.Serializable]
        public struct Response
        {
            public string status;
            public string transcriptions;
            public string message;
        }
    }
}