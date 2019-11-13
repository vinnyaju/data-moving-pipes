using DataMovingPipes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Abstractions
{

    public abstract class ItemTransferEndpoint : TransferEndpoint
    {
        internal virtual void Collect(byte[] dataBuffer, int dataLengh)
        {
            if (TransferEndpointType != TransferEndpointType.DESTINATION)
                throw new Exception("This endpoint is not an DESTINATION, only DESTINATION endpoints can collect data from theirs counterparties!");

            if (dataBuffer == null)
                throw new Exception("There is nothing to collect! The data buffer is empty!");
        }
    }
}
