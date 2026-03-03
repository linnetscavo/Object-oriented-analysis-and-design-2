using System;
using System.Collections.Generic;
using PCBuilderApp.Data;

namespace PCBuilderApp.Builder
{
    public class GamingPCBuilder : Builder
    {
        public GamingPCBuilder()
        {
            computer.BuildType = "Игровой ПК";
        }

        public override void BuildCPU()
        {
            List<string> options = ComponentData.GamingCPU;
            computer.Cpu = options[new Random().Next(options.Count)];
        }

        public override void BuildGPU()
        {
            List<string> options = ComponentData.GamingGPU;
            computer.Gpu = options[new Random().Next(options.Count)];
        }

        public override void BuildRAM()
        {
            List<string> options = ComponentData.GamingRAM;
            computer.Ram = options[new Random().Next(options.Count)];
        }

        public override void BuildStorage()
        {
            List<string> options = ComponentData.GamingStorage;
            computer.Storage = options[new Random().Next(options.Count)];
        }

        public override void BuildMotherboard()
        {
            List<string> options = ComponentData.GamingMotherboard;
            computer.Motherboard = options[new Random().Next(options.Count)];
        }

        public override void BuildCase()
        {
            List<string> options = ComponentData.GamingCase;
            computer.Case = options[new Random().Next(options.Count)];
        }

        public override void BuildCooling()
        {
            List<string> options = ComponentData.GamingCooling;
            computer.Cooling = options[new Random().Next(options.Count)];
        }
    }
}