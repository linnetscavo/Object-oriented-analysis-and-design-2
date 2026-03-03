using PCBuilderApp.Models;

namespace PCBuilderApp.Builder
{
    public class Director
    {
        public Computer Construct(IBuilder builder)
        {
            builder.BuildCPU();
            builder.BuildGPU();
            builder.BuildRAM();
            builder.BuildStorage();
            builder.BuildMotherboard();
            builder.BuildCase();
            builder.BuildCooling();

            return builder.GetResult();
        }

    }
}