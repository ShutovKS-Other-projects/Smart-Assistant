#region

using System.Threading.Tasks;

#endregion

namespace Services.VoiceRecognition
{
    public interface IVoiceRecognitionService
    {
        Task<string> SendVoiceRequestAsync(string filePath);
    }
}