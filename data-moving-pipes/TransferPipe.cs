using DataMovingPipes.Abstractions;
using System;

namespace DataMovingPipes
{
    public class TransferPipe
    {
        public TransferEndpoint Origin { get; }
        public TransferEndpoint Destination { get; }

        public TransferPipe(TransferCommand _command, TransferEndpoint _fromOrigin, TransferEndpoint _toDestination)
        {
            Origin = _fromOrigin;
            Origin.TransferEndpointType = TransferEndpointType.ORIGIN;
            Origin.TransferCommand = _command;

            Destination = _toDestination;
            Destination.TransferEndpointType = TransferEndpointType.DESTINATION;

            Origin.ConnectTo(Destination);
        }


        public void Pump()
        {
            Origin.Pump();
        }
    }
}
