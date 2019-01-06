namespace DocumentationGenerator.Services
{
    public interface IProcessorService
    {
        void SetData(string data);

        Processor ProcessData();
    }
}
