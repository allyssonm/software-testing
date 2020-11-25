using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Sales.Application.Commands
{
    public class ApplyOrderVoucherCommand : Command
    {
        private Guid _clientId;
        private string _voucherCodigo;

        public ApplyOrderVoucherCommand(Guid clientId, string voucherCodigo)
        {
            _clientId = clientId;
            _voucherCodigo = voucherCodigo;
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
