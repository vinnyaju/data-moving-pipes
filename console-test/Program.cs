using DataMovingPipes;
using DataMovingPipes.LocalFiles;
using System;

namespace ConsoleTeste_STA
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestarTransferenciaDeArquivoLocal();
            TestarTransferenciaDeDiretorioLocal();


            Console.WriteLine("Terminado!");

            Console.ReadKey();
        }

        private static void TestarTransferenciaDeDiretorioLocal()
        {
            var origem = new LocalDirectoryEndpoint(@"C:\Publico", "**.iso");            
            var destino = new LocalDirectoryEndpoint(@"C:\ftp-server");
            var pipe = new TransferPipe(TransferCommand.COPY, _fromOrigin: origem, _toDestination: destino);
            pipe.Pump();
        }

        private static void TestarTransferenciaDeArquivoLocal()
        {
            string filePah = @"C:\Publico\clonezilla-live-2.6.2-15-amd64{0}.iso";
            var origem = new LocalFileEndpoint(String.Format(filePah, string.Empty));
            var destino = new LocalFileEndpoint(String.Format(filePah, DateTime.Now.ToString("_yyyy-MM-dd_HHmmss")));
            var pipe = new TransferPipe(TransferCommand.COPY, _fromOrigin: origem, _toDestination: destino);
            pipe.Pump();
        }
    }
}
