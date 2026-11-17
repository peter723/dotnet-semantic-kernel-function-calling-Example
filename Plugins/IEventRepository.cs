using SemanticKernel.OpenRouter.FunctionCalling.Models;

namespace SemanticKernel.OpenRouter.FunctionCalling.Plugins;

public interface IEventRepository
{
    IReadOnlyCollection<Event> GetUpcomingEvents();
    Event? GetById(int eventId);
    bool TryBookSeat(int eventId, string attendeeName, out string confirmation);
}
