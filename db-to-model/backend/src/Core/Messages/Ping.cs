using MediatR;

namespace FoodDiary.Core.Messages;

public class Ping : IRequest<string> { }
