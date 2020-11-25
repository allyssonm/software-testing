using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Sales.Application.Commands
{
    public class UpdateOrderItemCommand : Command
    {
        private Guid _clientId;
        private Guid _id;
        private int _quantidade;

        public UpdateOrderItemCommand(Guid clientId, Guid id, int quantidade)
        {
            _clientId = clientId;
            _id = id;
            _quantidade = quantidade;
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
