using DataMovingPipes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Abstractions
{
    public abstract class TransferEndpoint
    {
        public TransferEndpointType TransferEndpointType { get; internal set; }
        public TransferCommand TransferCommand { get; internal set; }
        public TransferEndpoint CounterParty { get; protected set; }
        internal abstract void Destroy();

        internal void ConnectTo(TransferEndpoint _counterparty)
        {
            CounterParty = _counterparty;

            if (_counterparty.CounterParty == null || !_counterparty.CounterParty.Equals(this))
                _counterparty.ConnectTo(this);

            ValidateCounterparty();
        }

        protected abstract void ValidateCounterparty();

        internal virtual void Pump()
        {
            if (TransferEndpointType != TransferEndpointType.ORIGIN)
                throw new Exception("This endpoint is not an ORIGIN, only ORIGIN endpoints can pump data to theirs counterparties!");
        }
    }
}
