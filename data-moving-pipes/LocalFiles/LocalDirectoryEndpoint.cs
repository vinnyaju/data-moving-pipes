using DataMovingPipes;
using DataMovingPipes.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataMovingPipes.LocalFiles
{
    public class LocalDirectoryEndpoint : CollectionTransferEndpoint
    {
        public DirectoryInfo DirectoryInfo { get; private set; }

        private List<LocalFileEndpoint> originFileEndpointsList = new List<LocalFileEndpoint>();
        private readonly string searchPattern;

        public LocalDirectoryEndpoint(string _directoryPath, string _searchPattern = "*")
        {
            DirectoryInfo = new DirectoryInfo(_directoryPath);
            searchPattern = _searchPattern;
        }

        internal override void Pump()
        {
            base.Pump();

            LocalDirectoryEndpoint mySpecializedCounterparty = CounterParty as LocalDirectoryEndpoint;

            List<TransferPipe> individualTransferPipes = new List<TransferPipe>();

            foreach (FileInfo fi in DirectoryInfo.GetFiles(searchPattern))
            {
                LocalFileEndpoint originFileEndpoint = new LocalFileEndpoint(fi);

                originFileEndpointsList.Add(originFileEndpoint);
                individualTransferPipes.Add(new TransferPipe(TransferCommand
                    , _fromOrigin: originFileEndpoint
                    , _toDestination: new LocalFileEndpoint(Path.Combine(mySpecializedCounterparty.DirectoryInfo.FullName, fi.Name))));
            }

            Parallel.ForEach(individualTransferPipes, (pipe) => pipe.Pump());

            if (TransferCommand == TransferCommand.MOVE)
                Destroy();
        }

        internal override void Destroy()
        {
            DirectoryInfo.Delete();
        }

        protected override void ValidateCounterparty()
        {
            if (!(CounterParty is CollectionTransferEndpoint))
                throw new Exception("The DESTINATION endpoint shoud be a DirectoryTransferEndpoint");
        }
    }
}
