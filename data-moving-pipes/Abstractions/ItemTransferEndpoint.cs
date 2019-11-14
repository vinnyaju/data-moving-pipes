using DataMovingPipes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Abstractions
{

    public abstract class ItemTransferEndpoint : TransferEndpoint
    {
        protected override void ValidateConnectedBlocks()
        {
            base.ValidateConnectedBlocks();
        }

        internal override void Collect(byte[] dataBuffer, int dataLengh)
        {
            if (TransferEndpointType != TransferEndpointType.DESTINATION)
                throw new Exception("This circuit block is neither a DESTINATION nor a TRANSFORMATION, only those kinds of blocks can collect data from theirs previous blocks!");

            if (dataBuffer == null)
                throw new Exception("There is nothing to collect! The data buffer is empty!");
        }
    }
}
