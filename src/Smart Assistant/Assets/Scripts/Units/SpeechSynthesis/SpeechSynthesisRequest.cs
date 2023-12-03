using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Units.SpeechSynthesis
{
    public static class SpeechSynthesisRequest
    {
        private const string SERVER_URL = "http://127.0.0.1:5001/text_to_speech";

        public static AudioClip SendTextToSpeechRequest(string text, string language = "ru", int rate = 150, float volume = 1.0f)
        {
            return SendTextToSpeechRequestAsync(text, language, rate, volume).Result;
        }

        public static async Task<AudioClip> SendTextToSpeechRequestAsync(string text, string language = "ru", int rate = 150, float volume = 1.0f)
        {
            var form = new WWWForm();
            form.AddField("text", text);
            form.AddField("language", language);
            form.AddField("rate", rate.ToString());
            form.AddField("volume", volume.ToString());
            
            var www = UnityWebRequest.Post(SERVER_URL, form);
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = www.downloadHandler.text;
                var responseJson = JsonUtility.FromJson<Response>(jsonResponse);
                var audioPath = responseJson.audio_path;

                return await GetAudioAsync(audioPath);
            }

            Debug.LogError("Request failed. Error: " + www.error);
            return null;
        }

        private static AudioClip GetAudio(string audioPath)
        {
            return GetAudioAsync(audioPath).Result;
        }

        private static async Task<AudioClip> GetAudioAsync(string audioPath)
        {
            Debug.Log("Audio path: " + audioPath);
            var audioRequest = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.WAV);
            audioRequest.SetRequestHeader("Accept", "audio/*");

            await audioRequest.SendWebRequest();

            if (audioRequest.result == UnityWebRequest.Result.Success)
            {
                var audioClip = DownloadHandlerAudioClip.GetContent(audioRequest);
                return audioClip;
            }

            Debug.LogError("Failed to load audio. Error: " + audioRequest.error);
            return null;
        }


        public struct Response
        {
            public string status;
            public string audio_path;
        }
    }
}