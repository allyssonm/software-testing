using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Features.Clients
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMediator _mediator;

        public ClientService(IClientRepository clientRepository, 
                              IMediator mediator)
        {
            _clientRepository = clientRepository;
            _mediator = mediator;
        }

        public IEnumerable<Client> GetAllActive()
        {
            return _clientRepository.GetAll().Where(c => c.Active);
        }

        public void Add(Client client)
        {
            if (!client.IsValid())
                return;

            _clientRepository.Add(client);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "Hi", "Welcome!"));
        }

        public void Update(Client client)
        {
            if (!client.IsValid())
                return;

            _clientRepository.Update(client);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "News", "Have a look!"));
        }

        public void Inactivate(Client client)
        {
            if (!client.IsValid())
                return;

            client.Inactivate();
            _clientRepository.Update(client);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "See you", "See you later!"));
        }

        public void Remove(Client client)
        {
            _clientRepository.Remove(client.Id);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "Bye", "Have a good journey!"));
        }

        public void Dispose()
        {
            _clientRepository.Dispose();
        }
    }
}