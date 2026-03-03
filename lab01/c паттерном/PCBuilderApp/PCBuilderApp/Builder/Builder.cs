using PCBuilderApp.Models;

namespace PCBuilderApp.Builder
{
    public abstract class Builder : IBuilder
    {
        protected Computer computer;

        public Builder()
        {
            computer = new Computer();
        }

        public abstract void BuildCPU();
        public abstract void BuildGPU();
        public abstract void BuildRAM();
        public abstract void BuildStorage();
        public abstract void BuildMotherboard();
        public abstract void BuildCase();
        public abstract void BuildCooling();

        public Computer GetResult()
        {
            return computer;
        }
    }
}