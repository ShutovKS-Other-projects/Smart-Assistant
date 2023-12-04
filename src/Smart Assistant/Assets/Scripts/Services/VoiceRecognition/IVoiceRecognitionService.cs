using System.Threading.Tasks;

namespace Services.VoiceRecognition
{
    public interface IVoiceRecognitionService
    {
        Task<string> SendVoiceRequestAsync(string filePath);
    }
}