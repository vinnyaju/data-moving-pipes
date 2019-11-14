using DataMovingPipes;
using DataMovingPipes.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataMovingPipes.LocalFiles
{
    public class LocalFileEndpoint : ItemTransferEndpoint
    {
        public FileInfo File { get; set; }

        private FileStream fileStream;
        public LocalFileEndpoint(string _filePath) : this(new FileInfo(_filePath))
        {

        }

        public LocalFileEndpoint(FileInfo _fileInfo)
        {
            File = _fileInfo;
        }

        internal override void Destroy()
        {
            File.Delete();
        }

        internal override void Pump()
        {
            base.Pump();

            using (fileStream = File.OpenRead())
            {
                byte[] b = new byte[4096];
                int dataReaded = 0;
                while ((dataReaded = fileStream.Read(b, 0, b.Length)) > 0)
                {
                    NextBlock.Collect(b, dataReaded);
                }
            }

            if (TransferCommand == TransferCommand.MOVE)
                Destroy();

        }

        internal override void Collect(byte[] dataBuffer, int dataLength)
        {
            base.Collect(dataBuffer, dataLength);

            if (fileStream == null)
                fileStream = File.OpenWrite();

            fileStream.Write(dataBuffer, 0, dataLength);

            if (dataLength < dataBuffer.Length)
                fileStream.Close();

        }

        protected override void ValidateConnectedBlocks()
        {
            base.ValidateConnectedBlocks();
            if (this.TransferEndpointType == TransferEndpointType.ORIGIN && !(this.NextBlock is Transformation || this.NextBlock is ItemTransferEndpoint))
                throw new Exception("The NextBlock of an ORIGIN circuit block must be a Transformation or an ItemTransferEndpoint!");

            if (this.TransferEndpointType == TransferEndpointType.DESTINATION && !(this.PreviousBlock is Transformation || this.PreviousBlock is ItemTransferEndpoint))
                throw new Exception("The NextBlock of an DESTINATION circuit block must be a Transformation or an ItemTransferEndpoint!");
        }
    }
}
