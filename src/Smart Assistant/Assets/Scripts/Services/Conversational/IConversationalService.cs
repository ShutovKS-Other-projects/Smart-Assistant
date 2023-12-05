#region

using System.Threading.Tasks;

#endregion

namespace Services.Conversational
{
    public interface IConversationalService
    {
        Task<string> GenerateResponseAsync(string userInput);
    }
}