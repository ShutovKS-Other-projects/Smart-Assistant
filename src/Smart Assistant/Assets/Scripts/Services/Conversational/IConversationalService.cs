using System.Threading.Tasks;

namespace Services.Conversational
{
    public interface IConversationalService
    {
        Task<string> GenerateResponseAsync(string userInput);
    }
}