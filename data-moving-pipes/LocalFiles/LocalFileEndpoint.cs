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

            LocalFileEndpoint mySpecializedCounterparty = CounterParty as LocalFileEndpoint;

            using (fileStream = File.OpenRead())
            {
                byte[] b = new byte[4096];
                int dataReaded = 0;
                while ((dataReaded = fileStream.Read(b, 0, b.Length)) > 0)
                {
                    mySpecializedCounterparty.Collect(b, dataReaded);
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

        protected override void ValidateCounterparty()
        {
            if (!(CounterParty is ItemTransferEndpoint))
                throw new Exception("The DESTINATION endpoint shoud be a FileTransferEndpoint");
        }
    }
}
