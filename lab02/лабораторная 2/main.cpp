#include "httplib.h"
#include <iostream>
#include <string>
#include <vector>
#include <sstream>
#include <fstream>

using namespace httplib;

class OpenWeatherMock {
public:
    std::string FetchData(const std::string& city) {
        return readFile("data/openweather.json", city);
    }
private:
    std::string readFile(const std::string& path, const std::string& city) {
        std::ifstream file(path);
        if (!file.is_open()) return "{}";
        std::stringstream buffer;
        buffer << file.rdbuf();
        std::string content = buffer.str();
        
        size_t cityPos = content.find("\"name\": \"" + city + "\"");
        if (cityPos == std::string::npos) return "{}";
        
        size_t objStart = content.rfind("{", cityPos);
        size_t objEnd = content.find("}", cityPos);
        if (objStart == std::string::npos || objEnd == std::string::npos) return "{}";
        
        return content.substr(objStart, objEnd - objStart + 1);
    }
};

class AccuWeatherMock {
public:
    std::string GetForecast(const std::string& city) {
        return readFile("data/accuweather.xml", city);
    }
private:
    std::string readFile(const std::string& path, const std::string& city) {
        std::ifstream file(path);
        if (!file.is_open()) return "<weather></weather>";
        std::stringstream buffer;
        buffer << file.rdbuf();
        std::string content = buffer.str();
        
        std::string search = "<city name=\"" + city + "\">";
        size_t start = content.find(search);
        if (start == std::string::npos) return "<weather></weather>";
        
        size_t end = content.find("</city>", start);
        if (end == std::string::npos) return "<weather></weather>";
        
        return content.substr(start, end - start + 7);
    }
};

class LocalService {
public:
    float GetCurrentTemp(const std::string& city) {
        std::ifstream file("data/local.txt");
        if (!file.is_open()) return 0.0f;
        
        std::string line;
        while (std::getline(file, line)) {
            size_t pos = line.find(":");
            if (pos != std::string::npos) {
                std::string fileCity = line.substr(0, pos);
                if (fileCity == city) {
                    return std::stof(line.substr(pos + 1));
                }
            }
        }
        return 0.0f;
    }
};

struct WeatherData {
    std::string source;
    double temperature;
    std::string condition;
    int humidity;
};

class WeatherWidget {
private:
    Server svr;
    OpenWeatherMock openWeather;
    AccuWeatherMock accuWeather;
    LocalService localSensor;
    std::vector<std::string> cities = {"Miami", "Sydney", "Tomsk"};

    // Грязные методы парсинга внутри виджета!
    WeatherData processOpenWeather(const std::string& city) {
        std::string json = openWeather.FetchData(city);
        double tempF = extractJsonValue(json, "temp_f");
        double tempC = (tempF - 32.0) * 5.0 / 9.0;
        std::string condition = extractJsonString(json, "condition");
        int humidity = (int)extractJsonValue(json, "humidity");
        return { "OpenWeather", tempC, condition, humidity };
    }

    WeatherData processAccuWeather(const std::string& city) {
        std::string xml = accuWeather.GetForecast(city);
        double tempC = extractXmlDouble(xml, "temp_c");
        std::string condition = extractXmlString(xml, "condition");
        int humidity = (int)extractXmlDouble(xml, "humidity");
        return { "AccuWeather", tempC, condition, humidity };
    }

    WeatherData processLocalSensor(const std::string& city) {
        float temp = localSensor.GetCurrentTemp(city);
        return { "LocalService", temp, u8"датчик", 50 };
    }
    double extractJsonValue(const std::string& json, const std::string& key) {
        std::string search = "\"" + key + "\": ";
        size_t pos = json.find(search);
        if (pos == std::string::npos) return 0.0;
        pos += search.length();
        return std::stod(json.substr(pos, 5));
    }
    
    std::string extractJsonString(const std::string& json, const std::string& key) {
        std::string search = "\"" + key + "\": \"";
        size_t pos = json.find(search);
        if (pos == std::string::npos) return "unknown";
        pos += search.length();
        size_t end = json.find("\"", pos);
        return json.substr(pos, end - pos);
    }
    
    std::string extractXmlString(const std::string& xml, const std::string& tag) {
        std::string openTag = "<" + tag + ">";
        std::string closeTag = "</" + tag + ">";
        size_t start = xml.find(openTag);
        if (start == std::string::npos) return "0";
        start += openTag.length();
        size_t end = xml.find(closeTag, start);
        return xml.substr(start, end - start);
    }
    
    double extractXmlDouble(const std::string& xml, const std::string& tag) {
        return std::stod(extractXmlString(xml, tag));
    }

public:
    WeatherWidget() {
        setupRoutes();
    }

    void setupRoutes() {
        svr.Get("/", [this](const Request&, Response& res) {
            std::string cityOptions;
            for (const auto& city : cities) {
                cityOptions += "<option value=\"" + city + "\">" + city + "</option>";
            }
            
            const std::string html = R"HTML(
<!DOCTYPE html>
<html lang="ru">
<head>
    <meta charset="UTF-8">
    <title>Weather Widget - NO ADAPTER</title>
    <style>
        body { font-family: 'Segoe UI', Arial, sans-serif; text-align: center; padding: 50px; background: #f0f2f5; }
        .container { background: white; padding: 40px; border-radius: 12px; max-width: 600px; margin: 0 auto; box-shadow: 0 4px 12px rgba(0,0,0,0.1); }
        h1 { color: #333; }
        .badge { background: #dc3545; color: white; padding: 5px 10px; border-radius: 15px; font-size: 12px; text-transform: uppercase; }
        select { padding: 12px; font-size: 16px; border: 2px solid #ddd; border-radius: 6px; width: 60%; }
        button { padding: 12px 24px; background: #dc3545; color: white; border: none; border-radius: 6px; cursor: pointer; font-size: 16px; margin-left: 10px; }
        button:hover { background: #c82333; }
        #result { margin-top: 30px; padding: 20px; background: #e9ecef; border-radius: 8px; min-height: 200px; text-align: left; }
        .weather-item { padding: 15px; margin: 10px 0; background: white; border-radius: 8px; border-left: 5px solid #dc3545; box-shadow: 0 2px 4px rgba(0,0,0,0.05); }
        small { color: #dc3545; display: block; margin-top: 5px; font-weight: bold; }
    </style>
</head>
<body>
    <div class="container">
        <h1>🌤️ Weather Widget (BEFORE Adapter)</h1>
        <p><span class="badge">No Pattern</span> <strong>Direct Calls & Manual Parsing!</strong></p>
        <select id="city">)HTML" + cityOptions + R"HTML(</select>
        <button onclick="loadWeather()">Load Weather</button>
        <div id="result">Select a city and click Load Weather...</div>
    </div>
    <script>
        async function loadWeather() {
            const city = document.getElementById('city').value;
            const result = document.getElementById('result');
            result.innerHTML = 'Reading files with hardcoded parsing logic...';
            
            try {
                const response = await fetch('/api/weather?city=' + encodeURIComponent(city));
                const data = await response.json();
                
                let html = '<h3>Weather in ' + data.city + '</h3>';
                data.sources.forEach(w => {
                    html += `<div class="weather-item">
                        <strong>${w.source}:</strong> ${w.temperature.toFixed(1)}°C, ${w.condition}, ${w.humidity}%<br>
                        <small>⚠️ Hardcoded parsing logic in Widget!</small>
                    </div>`;
                });
                result.innerHTML = html;
            } catch (e) {
                result.innerHTML = '<p style="color:red">Error: ' + e.message + '</p>';
            }
        }
    </script>
</body>
</html>
)HTML";
            res.set_content(html, "text/html; charset=utf-8");
        });

        svr.Get("/api/weather", [this](const Request& req, Response& res) {
            std::string city = "Tomsk";
            if (req.has_param("city")) {
                city = req.get_param_value("city");
            }

            std::cout << "Processing (No Pattern + Files): " << city << std::endl;

            WeatherData ow = processOpenWeather(city);
            WeatherData aw = processAccuWeather(city);
            WeatherData lw = processLocalSensor(city);

            std::vector<WeatherData> results = {ow, aw, lw};

            std::stringstream json;
            json << "{\"city\":\"" << city << "\", \"sources\":[";
            for (size_t i = 0; i < results.size(); ++i) {
                const auto& w = results[i];
                json << "{\"source\":\"" << w.source 
                     << "\",\"temperature\":" << w.temperature 
                     << ",\"condition\":\"" << w.condition 
                     << "\",\"humidity\":" << w.humidity << "}";
                if (i < results.size() - 1) json << ",";
            }
            json << "]}";

            res.set_content(json.str(), "application/json; charset=utf-8");
        });
    }

    void Run() {
        std::cout << "Weather Widget WITHOUT Adapter Pattern" << std::endl;
        std::cout << "Open: http://localhost:8080" << std::endl;
        std::cout << "Problem: Widget knows about JSON, XML, and file parsing!" << std::endl;
        std::cout << "========================================" << std::endl;
        svr.listen("localhost", 8080);
    }
};

int main() {
    std::cout << "=== LAB 2: PROBLEM DEMONSTRATION (NO ADAPTER) ===" << std::endl;
    std::cout << "Author: Klimova Darya, 932301" << std::endl << std::endl;

    WeatherWidget app;
    app.Run();

    return 0;
}