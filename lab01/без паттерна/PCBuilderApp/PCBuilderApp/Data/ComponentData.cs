using System.Collections.Generic;

namespace PCBuilderApp.Data
{
    public static class ComponentData
    {
        // Игровой ПК
        public static List<string> GamingCPU = new List<string>
        { "Intel Core i9-14900K", "Intel Core i7-14700K", "AMD Ryzen 9 7950X", "AMD Ryzen 7 7800X3D" };
        public static List<string> GamingGPU = new List<string>
        { "NVIDIA RTX 4090", "NVIDIA RTX 4080", "NVIDIA RTX 4070 Ti", "AMD RX 7900 XTX" };
        public static List<string> GamingRAM = new List<string>
        { "32GB DDR5-6000", "64GB DDR5-5600", "32GB DDR5-5200" };
        public static List<string> GamingStorage = new List<string>
        { "2TB NVMe SSD", "1TB NVMe SSD + 2TB HDD", "4TB NVMe SSD" };
        public static List<string> GamingMotherboard = new List<string>
        { "ASUS ROG Maximus Z790", "MSI MPG Z790 Carbon", "Gigabyte X670E Aorus" };
        public static List<string> GamingCase = new List<string>
        { "NZXT H9 Flow", "Lian Li O11 Dynamic", "Corsair 5000D Airflow" };
        public static List<string> GamingCooling = new List<string>
        { "NZXT Kraken 360mm", "Arctic Liquid Freezer III 360", "Corsair iCUE H150i" };

        // Офисный ПК
        public static List<string> OfficeCPU = new List<string>
        { "Intel Core i3-14100", "Intel Core i5-14400", "AMD Ryzen 5 7600" };
        public static List<string> OfficeGPU = new List<string>
        { "Intel UHD Graphics (встроенная)", "AMD Radeon Graphics (встроенная)" };
        public static List<string> OfficeRAM = new List<string>
        { "8GB DDR4-3200", "16GB DDR4-3200", "16GB DDR5-4800" };
        public static List<string> OfficeStorage = new List<string>
        { "512GB NVMe SSD", "256GB NVMe SSD", "1TB SATA SSD" };
        public static List<string> OfficeMotherboard = new List<string>
        { "ASUS Prime B760M", "Gigabyte B660M", "MSI PRO B650M" };
        public static List<string> OfficeCase = new List<string>
        { "Deepcool MATREXX 30", "Zalman S2", "Chieftec Scout" };
        public static List<string> OfficeCooling = new List<string>
        { "Boxed кулер", "Deepcool GAMMAXX 300", "Arctic Freezer 34" };

        // Учебный ПК
        public static List<string> StudentCPU = new List<string>
        { "Intel Core i5-13400", "AMD Ryzen 5 5600", "Intel Core i5-12400F" };
        public static List<string> StudentGPU = new List<string>
        { "NVIDIA GTX 1650", "NVIDIA RTX 3050", "AMD RX 6600", "Встроенная графика" };
        public static List<string> StudentRAM = new List<string>
        { "16GB DDR4-3200", "16GB DDR5-4800", "32GB DDR4-3200" };
        public static List<string> StudentStorage = new List<string>
        { "1TB NVMe SSD", "512GB NVMe SSD + 1TB HDD", "1TB SATA SSD" };
        public static List<string> StudentMotherboard = new List<string>
        { "ASUS TUF B760M", "MSI B550M Pro", "Gigabyte B660M Gaming" };
        public static List<string> StudentCase = new List<string>
        { "Deepcool MATREXX 40", "Cooler Master MasterBox Q300L", "NZXT H5 Flow" };
        public static List<string> StudentCooling = new List<string>
        { "Deepcool GAMMAXX 400", "Arctic Freezer 34 eSports", "ID-COOLING SE-214-XT" };

        public static List<string> GetComponents(string buildType, string componentType)
        {
            List<string> result = new List<string>();

            if (buildType == "Игровой")
            {
                if (componentType == "CPU") result = GamingCPU;
                else if (componentType == "GPU") result = GamingGPU;
                else if (componentType == "RAM") result = GamingRAM;
                else if (componentType == "Storage") result = GamingStorage;
                else if (componentType == "Motherboard") result = GamingMotherboard;
                else if (componentType == "Case") result = GamingCase;
                else if (componentType == "Cooling") result = GamingCooling;
            }
            else if (buildType == "Офисный")
            {
                if (componentType == "CPU") result = OfficeCPU;
                else if (componentType == "GPU") result = OfficeGPU;
                else if (componentType == "RAM") result = OfficeRAM;
                else if (componentType == "Storage") result = OfficeStorage;
                else if (componentType == "Motherboard") result = OfficeMotherboard;
                else if (componentType == "Case") result = OfficeCase;
                else if (componentType == "Cooling") result = OfficeCooling;
            }
            else if (buildType == "Учебный")
            {
                if (componentType == "CPU") result = StudentCPU;
                else if (componentType == "GPU") result = StudentGPU;
                else if (componentType == "RAM") result = StudentRAM;
                else if (componentType == "Storage") result = StudentStorage;
                else if (componentType == "Motherboard") result = StudentMotherboard;
                else if (componentType == "Case") result = StudentCase;
                else if (componentType == "Cooling") result = StudentCooling;
            }

            return result;
        }
    }
}