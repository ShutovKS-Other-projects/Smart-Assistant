using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Conversational
{
    public class ConversationalRequest : IConversationalService
    {
        private const string SERVER_URL = "http://127.0.0.1:5002/generate_response";

        public async Task<string> GenerateResponseAsync(string userInput)
        {
            var form = new WWWForm();
            form.AddField("user_input", "@@ПЕРВЫЙ@@" + userInput + "@@ВТОРОЙ@@");

            var www = UnityWebRequest.Post(SERVER_URL, form);
            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var jsonResponse = www.downloadHandler.text;
                var response = JsonUtility.FromJson<ResponseData>(jsonResponse);

                if (response.status == "success")
                {
                    var generatedResponse = response.generated_response;
                    return generatedResponse;
                }

                Debug.LogError("Error: " + response.message);
                return null;
            }

            Debug.LogError("Request failed. Error: " + www.error);
            return null;
        }

        [System.Serializable]
        public class ResponseData
        {
            public string status;
            public string generated_response;
            public string message;
        }
    }
}