using System;
using System.Collections.Generic;
using PCBuilderApp.Data;

namespace PCBuilderApp.Builder
{
    public class StudentPCBuilder : Builder
    {
        public StudentPCBuilder()
        {
            computer.BuildType = "Учебный ПК";
        }

        public override void BuildCPU()
        {
            List<string> options = ComponentData.StudentCPU;
            computer.Cpu = options[new Random().Next(options.Count)];
        }

        public override void BuildGPU()
        {
            List<string> options = ComponentData.StudentGPU;
            computer.Gpu = options[new Random().Next(options.Count)];
        }

        public override void BuildRAM()
        {
            List<string> options = ComponentData.StudentRAM;
            computer.Ram = options[new Random().Next(options.Count)];
        }

        public override void BuildStorage()
        {
            List<string> options = ComponentData.StudentStorage;
            computer.Storage = options[new Random().Next(options.Count)];
        }

        public override void BuildMotherboard()
        {
            computer.Motherboard = ComponentData.GetDefaultComponent("Учебный", "Motherboard");
        }

        public override void BuildCase()
        {
            List<string> options = ComponentData.StudentCase;
            computer.Case = options[new Random().Next(options.Count)];
        }

        public override void BuildCooling()
        {
            computer.Cooling = ComponentData.GetDefaultComponent("Учебный", "Cooling");
        }
    }
}