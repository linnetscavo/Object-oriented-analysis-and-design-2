using PCBuilderApp.Models;

namespace PCBuilderApp.Builder
{
    public interface IBuilder
    {
        void BuildCPU();
        void BuildGPU();
        void BuildRAM();
        void BuildStorage();
        void BuildMotherboard();
        void BuildCase();
        void BuildCooling();
        Computer GetResult();
    }
}