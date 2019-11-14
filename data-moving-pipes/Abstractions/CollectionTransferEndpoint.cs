using System;
using System.Collections.Generic;
using System.Text;

namespace DataMovingPipes.Abstractions
{
    public abstract class CollectionTransferEndpoint : TransferEndpoint
    {
        public Transformation[] TransformationList { get; internal set; }
        internal override void Collect(byte[] dataBuffer, int dataLength)
        {
            throw new NotImplementedException("CollectioTransferEndpoints are Cuircuit Blocks who can't collect data directly, use it as a ORIGIN group of ItemTransferEndpoints!");
        }
    }
}
