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

            LocalDirectoryEndpoint mySpecializedNextBlock = NextBlock as LocalDirectoryEndpoint;

            List<TransferPipe> individualTransferPipes = new List<TransferPipe>();

            foreach (FileInfo fi in DirectoryInfo.GetFiles(searchPattern))
            {
                LocalFileEndpoint originFileEndpoint = new LocalFileEndpoint(fi);
                originFileEndpointsList.Add(originFileEndpoint);


                Transformation[] transformationsClone = null;
                if (this.TransformationList != null)
                {
                    transformationsClone = new Transformation[this.TransformationList.Length];

                    for (int i = 0; i < transformationsClone.Length; i++)
                    {
                        transformationsClone[i] = (Transformation)this.TransformationList[i].Clone();
                    }
                }                
                individualTransferPipes.Add(
                    new TransferPipe(TransferCommand
                        , _fromOrigin: originFileEndpoint
                        , _toDestination: new LocalFileEndpoint(Path.Combine(mySpecializedNextBlock.DirectoryInfo.FullName, fi.Name))
                        , transformationsClone
                    )
                );
            }

            Parallel.ForEach(individualTransferPipes, (pipe) => pipe.Pump());

            if (TransferCommand == TransferCommand.MOVE)
                Destroy();
        }

        internal override void Destroy()
        {
            DirectoryInfo.Delete();
        }

        protected override void ValidateConnectedBlocks()
        {
            base.ValidateConnectedBlocks();
            if (this.TransferEndpointType == TransferEndpointType.ORIGIN && !(this.NextBlock is Transformation || this.NextBlock is CollectionTransferEndpoint))
                throw new Exception("The NextBlock of an ORIGIN circuit block must be a Transformation or an ItemTransferEndpoint!");

            if (this.TransferEndpointType == TransferEndpointType.DESTINATION && !(this.PreviousBlock is Transformation || this.PreviousBlock is CollectionTransferEndpoint))
                throw new Exception("The PreviousBlock of an DESTINATION circuit block must be a Transformation or an CollectionTransferEndpoint!");
        }
    }
}
