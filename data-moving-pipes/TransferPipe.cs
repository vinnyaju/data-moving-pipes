using DataMovingPipes.Abstractions;
using System;

namespace DataMovingPipes
{
    public class TransferPipe
    {
        private readonly Transformation[] TransformationList;

        public TransferEndpoint Origin { get; }
        public TransferEndpoint Destination { get; }

        public TransferPipe(TransferCommand _command, TransferEndpoint _fromOrigin, TransferEndpoint _toDestination, Transformation[] _throughTransformations = null)
        {
            this.Origin = _fromOrigin;
            this.Origin.TransferEndpointType = TransferEndpointType.ORIGIN;
            this.Origin.TransferCommand = _command;

            this.Destination = _toDestination;
            this.Destination.TransferEndpointType = TransferEndpointType.DESTINATION;

            this.TransformationList = _throughTransformations;

            if (Origin is CollectionTransferEndpoint && Destination is CollectionTransferEndpoint)
            {
                CollectionTransferEndpoint o = Origin as CollectionTransferEndpoint;
                o.ConnectTo(Destination);
                o.TransformationList = this.TransformationList;
                
            }
            else if (Origin is ItemTransferEndpoint && Destination is ItemTransferEndpoint)
            {
                if (this.TransformationList == null)
                    Origin.ConnectTo(Destination);
                else
                {
                    Origin.ConnectTo(this.TransformationList[0]);
                    this.LinkAllTheTransformations();
                    this.TransformationList[this.TransformationList.Length - 1].ConnectTo(Destination);
                }
            }
        }

        private void LinkAllTheTransformations()
        {
            int pNext = 1;
            for (int i = 0; i < TransformationList.Length-1; i++)
            {
                TransformationList[i].ConnectTo(TransformationList[pNext]);
                pNext++;
            }
        }

        public void Pump()
        {
            Origin.Pump();
        }
    }
}
