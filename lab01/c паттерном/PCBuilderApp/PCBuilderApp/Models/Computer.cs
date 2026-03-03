using System;

namespace PCBuilderApp.Models
{
    public class Computer
    {
        public string Cpu { get; set; }
        public string Gpu { get; set; }
        public string Ram { get; set; }
        public string Storage { get; set; }
        public string Motherboard { get; set; }
        public string Case { get; set; }
        public string Cooling { get; set; }
        public string BuildType { get; set; }

        public Computer()
        {
            BuildType = "Стандартный";
        }

        public override string ToString()
        {
            string result = "";
            result += Environment.NewLine;
            result += "=== Конфигурация ПК: " + BuildType + " ===" + Environment.NewLine;
            result += "Процессор: " + Cpu + Environment.NewLine;
            result += "Видеокарта: " + Gpu + Environment.NewLine;
            result += "Оперативная память: " + Ram + Environment.NewLine;
            result += "Накопитель: " + Storage + Environment.NewLine;
            result += "Материнская плата: " + Motherboard + Environment.NewLine;
            result += "Корпус: " + Case + Environment.NewLine;
            result += "Охлаждение: " + Cooling + Environment.NewLine;
            return result;
        }

        public string ToHTML()
        {
            string html = "";
            html += "<!DOCTYPE html>" + Environment.NewLine;
            html += "<html lang='ru'>" + Environment.NewLine;
            html += "<head>" + Environment.NewLine;
            html += "    <meta charset='UTF-8'>" + Environment.NewLine;
            html += "    <title>Конфигурация ПК - " + BuildType + "</title>" + Environment.NewLine;
            html += "    <style>" + Environment.NewLine;
            html += "        body { font-family: Arial, sans-serif; margin: 40px; background: #f5f5f5; }" + Environment.NewLine;
            html += "        .container { max-width: 600px; margin: 0 auto; background: white; padding: 30px; border-radius: 10px; }" + Environment.NewLine;
            html += "        h1 { color: #2c3e50; border-bottom: 3px solid #3498db; padding-bottom: 10px; }" + Environment.NewLine;
            html += "        .component { display: flex; justify-content: space-between; padding: 12px 0; border-bottom: 1px solid #eee; }" + Environment.NewLine;
            html += "        .label { font-weight: bold; color: #555; }" + Environment.NewLine;
            html += "        .value { color: #2c3e50; }" + Environment.NewLine;
            html += "    </style>" + Environment.NewLine;
            html += "</head>" + Environment.NewLine;
            html += "<body>" + Environment.NewLine;
            html += "    <div class='container'>" + Environment.NewLine;
            html += "        <h1>Конфигурация ПК</h1>" + Environment.NewLine;
            html += "        <p><strong>Тип сборки:</strong> " + BuildType + "</p>" + Environment.NewLine;
            html += "        <div class='component'><span class='label'>Процессор:</span><span class='value'>" + Cpu + "</span></div>" + Environment.NewLine;
            html += "        <div class='component'><span class='label'>Видеокарта:</span><span class='value'>" + Gpu + "</span></div>" + Environment.NewLine;
            html += "        <div class='component'><span class='label'>Оперативная память:</span><span class='value'>" + Ram + "</span></div>" + Environment.NewLine;
            html += "        <div class='component'><span class='label'>Накопитель:</span><span class='value'>" + Storage + "</span></div>" + Environment.NewLine;
            html += "        <div class='component'><span class='label'>Материнская плата:</span><span class='value'>" + Motherboard + "</span></div>" + Environment.NewLine;
            html += "        <div class='component'><span class='label'>Корпус:</span><span class='value'>" + Case + "</span></div>" + Environment.NewLine;
            html += "        <div class='component'><span class='label'>Охлаждение:</span><span class='value'>" + Cooling + "</span></div>" + Environment.NewLine;
            html += "    </div>" + Environment.NewLine;
            html += "</body>" + Environment.NewLine;
            html += "</html>";
            return html;
        }
    }
}