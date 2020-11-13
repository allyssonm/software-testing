using Features.Core;

namespace Features.Clients
{
    public interface IClientRepository : IRepository<Client>
    {
        Client GetByEmail(string email);
    }
}