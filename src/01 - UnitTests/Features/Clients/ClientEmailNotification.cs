using MediatR;

namespace Features.Clients
{
    public class ClientEmailNotification : INotification
    {
        public string From { get; private set; }
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }

        public ClientEmailNotification(string from, string to, string subject, string message)
        {
            From = from;
            To = to;
            Subject = subject;
            Message = message;
        }
    }
}