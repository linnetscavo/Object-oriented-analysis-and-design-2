from abc import ABC, abstractmethod
import matplotlib.pyplot as plt

class Observer(ABC):
    @abstractmethod
    def update(self, symbol: str, price: float):
        pass

class LoggerSubscriber(Observer):
    def __init__(self, text_widget):
        self.text_widget = text_widget
        self.log_count = 0

    def update(self, symbol: str, price: float):
        self.log_count += 1
        timestamp = f"{self.log_count:04d}"
        self.text_widget.insert("end", f"[{timestamp}] {symbol}: ${price:,.2f}\n", "log")
        self.text_widget.see("end")

class AlertSubscriber(Observer):
    def __init__(self, text_widget, thresholds=None):
        self.text_widget = text_widget
        self.thresholds = thresholds or {"BTC": 51000, "ETH": 4100, "DOGE": 0.26}

    def update(self, symbol: str, price: float):
        threshold = self.thresholds.get(symbol, float('inf'))
        if price > threshold:
            self.text_widget.insert("end", 
                f"⚠️ ALERT: {symbol} HIGH! ${price:,.2f} > ${threshold:,.2f}\n", "alert")
            self.text_widget.see("end")
        elif price < threshold * 0.98:
            self.text_widget.insert("end", 
                f"✅ GOOD: {symbol} LOW! ${price:,.2f} < ${threshold:,.2f}\n", "good")
            self.text_widget.see("end")

class MLSubscriber(Observer):
    def __init__(self, text_widget):
        self.text_widget = text_widget
        self.history = {}

    def update(self, symbol: str, price: float):
        if symbol not in self.history:
            self.history[symbol] = []
        self.history[symbol].append(price)
        
        if len(self.history[symbol]) >= 3:
            recent = self.history[symbol][-3:]
            trend = "📈 UP" if recent[-1] > recent[0] else "📉 DOWN"
            prediction = round(price * 1.005, 2)
            self.text_widget.insert("end", 
                f"🤖 ML: {symbol} {trend} | Predict: ${prediction:,.2f}\n", "ml")
            self.text_widget.see("end")
            
            if len(self.history[symbol]) > 10:
                self.history[symbol] = self.history[symbol][-10:]

class GraphSubscriber(Observer):
    def __init__(self, ax, symbol, canvas):
        self.ax = ax
        self.symbol = symbol
        self.canvas = canvas
        self.prices = []
        self.line, = ax.plot([], [], linewidth=2, marker='o', markersize=3)
        self.ax.set_title(f"{symbol} Price Chart", fontsize=10, fontweight='bold')
        self.ax.set_xlabel("Updates", fontsize=8)
        self.ax.set_ylabel("Price ($)", fontsize=8)
        self.ax.grid(True, alpha=0.3)
        self.ax.tick_params(labelsize=8)
        
        colors = {"BTC": "#FF6B35", "ETH": "#627EEA", "DOGE": "#C2A633"}
        self.line.set_color(colors.get(symbol, "#000000"))

    def update(self, symbol: str, price: float):
        if symbol != self.symbol:
            return
        self.prices.append(price)
        
        x_data = list(range(len(self.prices)))
        self.line.set_data(x_data, self.prices)
        
        self.ax.relim()
        self.ax.autoscale_view(scalex=True, scaley=True)
        
        if len(self.prices) > 20:
            self.ax.set_xlim(len(self.prices)-20, len(self.prices))
        
        self.canvas.draw_idle()