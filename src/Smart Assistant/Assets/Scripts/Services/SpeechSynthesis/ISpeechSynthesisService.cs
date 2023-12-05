#region

using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace Services.SpeechSynthesis
{
    public interface ISpeechSynthesisService
    {
        Task<AudioClip> SendTextToSpeechRequestAsync(string text, string language = "ru", int rate = 150,
            float volume = 1.0f);
    }
}