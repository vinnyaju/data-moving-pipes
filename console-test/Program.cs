using DataMovingPipes;
using DataMovingPipes.Abstractions;
using DataMovingPipes.LocalFiles;
using DataMovingPipes.Transformations;
using System;

namespace ConsoleTeste_STA
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestarTransferenciaDeArquivoLocal();
            TestarTransferenciaDeDiretorioLocal();

            //ForcarOrigemEDestinosIncompativeis();

            Console.WriteLine("Terminado!");

            Console.ReadKey();
        }

        private static void ForcarOrigemEDestinosIncompativeis()
        {
            string filePah = @"C:\Publico\clonezilla-live-2.6.2-15-amd64{0}.iso";
            var origem = new LocalFileEndpoint(String.Format(filePah, string.Empty));
            var destino = new LocalDirectoryEndpoint(@"C:\ftp-server");
            var pipe = new TransferPipe(TransferCommand.COPY, _fromOrigin: origem, _toDestination: destino);
            pipe.Pump();
        }

        private static void TestarTransferenciaDeDiretorioLocal()
        {
            Transformation[] transformacoes = new Transformation[] {
                new DummyTransformation("transformacao1")
                ,new DummyTransformation("transformacao2")
                ,new DummyTransformation("transformacao3")
                ,new DummyTransformation("transformacao4")
                ,new DummyTransformation("transformacao5")
            };

            var origem = new LocalDirectoryEndpoint(@"C:\Publico", "**.zip");            
            var destino = new LocalDirectoryEndpoint(@"C:\ftp-server");
            var pipe = new TransferPipe(TransferCommand.COPY, _fromOrigin: origem, _toDestination: destino, _throughTransformations: transformacoes);
            pipe.Pump();
        }

        private static void TestarTransferenciaDeArquivoLocal()
        {

            Transformation[] transformacoes = new Transformation[] {
                new DummyTransformation("transformacao1")
            };

            string filePah = @"C:\Publico\clonezilla-live-2.6.2-15-amd64{0}.iso";
            var origem = new LocalFileEndpoint(String.Format(filePah, string.Empty));
            var destino = new LocalFileEndpoint(String.Format(filePah, DateTime.Now.ToString("_yyyy-MM-dd_HHmmss")));
            var pipe = new TransferPipe(TransferCommand.COPY, _fromOrigin: origem, _toDestination: destino, _throughTransformations: transformacoes);
            pipe.Pump();
        }

    }
}
