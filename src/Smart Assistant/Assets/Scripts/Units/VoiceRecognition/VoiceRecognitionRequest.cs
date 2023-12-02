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

        public static Response SendVoiceRequest(string filePath)
        {
            var form = new WWWForm();
            var audioData = File.ReadAllBytes(filePath);

            form.AddBinaryData("audio", audioData, "audio.mp3", "audio/mp3");

            var www = UnityWebRequest.Post(SERVER_URL, form);
            www.SendWebRequest();

            while (!www.isDone)
            {
                Thread.Sleep(100);
            }
            
            if (www.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = www.downloadHandler.text;
                
                return JsonUtility.FromJson<Response>(jsonResponse);
            }

            Debug.LogError("Request failed. Error: " + www.error);
            return default;
        }


        public static async Task<Response> SendVoiceRequestAsync(string filePath)
        {
            var form = new WWWForm();
            var audioData = File.ReadAllBytes(filePath);

            form.AddBinaryData("audio", audioData, "audio.mp3", "audio/mp3");

            var www = UnityWebRequest.Post(SERVER_URL, form);
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = www.downloadHandler.text;
                
                return JsonUtility.FromJson<Response>(jsonResponse);
            }

            Debug.LogError("Request failed. Error: " + www.error);
            return default;
        }
        
        public struct Response
        {
            public string status;
            public string transcriptions;
        }
    }
}