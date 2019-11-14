using DataMovingPipes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Abstractions
{
    public abstract class TransferEndpoint : CircuitBlock
    {
        public TransferEndpointType TransferEndpointType { get; internal set; }
        public TransferCommand TransferCommand { get; internal set; }

        protected override void ValidateConnectedBlocks()
        {
            switch (this.TransferEndpointType)
            {
                case TransferEndpointType.ORIGIN:
                    if (this.PreviousBlock != null)
                        throw new Exception("The PreviousBlock of an oirigin CircuitBlock has to be null!");
                    break;
                case TransferEndpointType.DESTINATION:
                    if (this.NextBlock != null)
                        throw new Exception("The NextBlock of a destination CircuitBlock has to be null!");
                    break;
                default:
                    break;
            }
        }

        internal abstract void Destroy();

        internal virtual void Pump()
        {
            if (TransferEndpointType != TransferEndpointType.ORIGIN)
                throw new Exception("This endpoint is not an ORIGIN, only ORIGIN endpoints can pump data to theirs counterparties!");
        }
    }
}
