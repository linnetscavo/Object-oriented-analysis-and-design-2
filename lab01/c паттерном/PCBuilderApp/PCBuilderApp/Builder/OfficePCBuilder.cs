using System;
using System.Collections.Generic;
using PCBuilderApp.Data;

namespace PCBuilderApp.Builder
{
    public class OfficePCBuilder : Builder
    {
        public OfficePCBuilder()
        {
            computer.BuildType = "Офисный ПК";
        }

        public override void BuildCPU()
        {
            List<string> options = ComponentData.OfficeCPU;
            computer.Cpu = options[new Random().Next(options.Count)];
        }

        public override void BuildGPU()
        {
            // Офисный ПК использует встроенную графику
            computer.Gpu = ComponentData.GetDefaultComponent("Офисный", "GPU");
        }

        public override void BuildRAM()
        {
            List<string> options = ComponentData.OfficeRAM;
            computer.Ram = options[new Random().Next(options.Count)];
        }

        public override void BuildStorage()
        {
            List<string> options = ComponentData.OfficeStorage;
            computer.Storage = options[new Random().Next(options.Count)];
        }

        public override void BuildMotherboard()
        {
            computer.Motherboard = ComponentData.GetDefaultComponent("Офисный", "Motherboard");
        }

        public override void BuildCase()
        {
            computer.Case = ComponentData.GetDefaultComponent("Офисный", "Case");
        }

        public override void BuildCooling()
        {
            computer.Cooling = ComponentData.GetDefaultComponent("Офисный", "Cooling");
        }
    }
}