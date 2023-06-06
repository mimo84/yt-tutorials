using FoodDiary.Core.Messages;
using MediatR;

namespace FoodDiary.Data.Services;

public class PingHandler : IRequestHandler<Ping, string>
{
    public Task<string> Handle(Ping request, CancellationToken cancellationToken)
    {
        return Task.FromResult($"Pong {DateTime.Now}");
    }
}
