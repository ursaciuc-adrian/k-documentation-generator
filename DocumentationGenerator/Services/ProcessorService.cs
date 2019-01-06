using System;

namespace DocumentationGenerator.Services
{
    public class ProcessorService : IProcessorService
    {
        private string RawData { get; set; }

        public Processor ProcessData()
        {
            var processor = new Processor();
            processor.ProcessData(RawData);

            return processor;
        }

        public void SetData(string data)
        {
            RawData = data;
        }
    }
}
