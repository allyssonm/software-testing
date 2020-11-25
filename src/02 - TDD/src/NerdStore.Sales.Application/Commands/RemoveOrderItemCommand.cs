using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Sales.Application.Commands
{
    public class RemoveOrderItemCommand : Command
    {
        private Guid _clientId;
        private Guid _id;

        public RemoveOrderItemCommand(Guid clientId, Guid id)
        {
            _clientId = clientId;
            _id = id;
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
