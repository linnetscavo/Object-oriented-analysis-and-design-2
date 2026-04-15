import tkinter as tk
from tkinter import ttk, messagebox
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import matplotlib.pyplot as plt
import random
import time

def get_random_price(symbol):
    base = {"BTC": 50000, "ETH": 4000, "DOGE": 0.25}.get(symbol, 1)
    return round(base + random.uniform(-base*0.02, base*0.02), 4)

class MonolithicMarketApp(tk.Tk):
    def __init__(self):
        super().__init__()
        
        self.title("❌ Bad Implementation (No Pattern)")
        self.geometry("1400x900")
        self.configure(bg='#f0f0f0')
        
        self.symbols = ["BTC", "ETH", "DOGE"]
        self.prices = {s: 0 for s in self.symbols}
        self.history = {s: [] for s in self.symbols}
        self.running = True
        self.active_symbols = set()  
        
        self._setup_styles()
        self._create_layout()
        self._create_buttons()
        
        self.after(1000, self.main_loop)
        self.protocol("WM_DELETE_WINDOW", self.quit)

    def _setup_styles(self):
        style = ttk.Style()
        style.theme_use('clam')
        style.configure('TButton', padding=6, font=('Arial', 10))
        style.configure('Header.TLabel', font=('Arial', 14, 'bold'))

    def _create_layout(self):
        main_frame = ttk.Frame(self, padding="10")
        main_frame.pack(fill=tk.BOTH, expand=True)
        
        left_frame = ttk.Frame(main_frame)
        left_frame.pack(side=tk.LEFT, fill=tk.BOTH, expand=True, padx=(0, 10))
        
        right_frame = ttk.Frame(main_frame, width=400)
        right_frame.pack(side=tk.RIGHT, fill=tk.BOTH, expand=False)
        right_frame.pack_propagate(False)
        
        ttk.Label(left_frame, text="📈 Real-time Charts", 
                 style='Header.TLabel').pack(pady=(0, 10))
        
        self.fig, self.axs = plt.subplots(3, 1, figsize=(8, 8), dpi=100)
        plt.tight_layout(pad=3.0)
        self.canvas = FigureCanvasTkAgg(self.fig, master=left_frame)
        self.canvas.get_tk_widget().pack(fill=tk.BOTH, expand=True)
        
        ttk.Label(right_frame, text="📋 Event Log", 
                 style='Header.TLabel').pack(pady=(0, 10))
        
        self.log_text = tk.Text(right_frame, width=45, height=40, 
                               font=('Consolas', 9), bg='white')
        self.log_text.pack(fill=tk.BOTH, expand=True, pady=(0, 10))
        
        self.log_text.tag_config("log", foreground="#333333")
        self.log_text.tag_config("alert", foreground="#d32f2f", font=('Consolas', 9, 'bold'))
        self.log_text.tag_config("good", foreground="#388e3c", font=('Consolas', 9, 'bold'))
        self.log_text.tag_config("ml", foreground="#1976d2", font=('Consolas', 9, 'italic'))
        
        scrollbar = ttk.Scrollbar(self.log_text, command=self.log_text.yview)
        scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        self.log_text.config(yscrollcommand=scrollbar.set)

    def _create_buttons(self):
        button_frame = ttk.Frame(self, padding="10")
        button_frame.pack(side=tk.BOTTOM, fill=tk.X)
        
        colors = {"BTC": "#FF6B35", "ETH": "#627EEA", "DOGE": "#C2A633"}
        
        for idx, symbol in enumerate(self.symbols):
            btn_frame = ttk.Frame(button_frame)
            btn_frame.grid(row=0, column=idx, padx=10)
            
            btn_add = ttk.Button(btn_frame, text=f"➕ Add {symbol}", 
                                command=lambda s=symbol: self.add_symbol(s))
            btn_add.pack(side=tk.LEFT, padx=2)
            
            btn_remove = ttk.Button(btn_frame, text=f"➖ Remove {symbol}", 
                                   command=lambda s=symbol: self.remove_symbol(s))
            btn_remove.pack(side=tk.LEFT, padx=2)
        
        ttk.Button(button_frame, text="🗑️ Clear Logs", 
                  command=self.clear_logs).grid(row=0, column=3, padx=10)
        
        self.toggle_btn = ttk.Button(button_frame, text="⏸️ Pause", 
                                     command=self.toggle_running)
        self.toggle_btn.grid(row=0, column=4, padx=10)
        
        ttk.Button(button_frame, text="❌ Exit", 
                  command=self.quit).grid(row=0, column=5, padx=10)

    def add_symbol(self, symbol):
        if symbol in self.active_symbols:
            messagebox.showinfo("Info", f"{symbol} already active!")
            return
        
        idx = self.symbols.index(symbol)
        ax = self.axs[idx]
        ax.set_title(f"{symbol} Price Chart", fontsize=10, fontweight='bold')
        ax.set_xlabel("Updates", fontsize=8)
        ax.set_ylabel("Price ($)", fontsize=8)
        ax.grid(True, alpha=0.3)
        ax.tick_params(labelsize=8)
        
        colors = {"BTC": "#FF6B35", "ETH": "#627EEA", "DOGE": "#C2A633"}
        self.line, = ax.plot([], [], linewidth=2, marker='o', markersize=3, color=colors.get(symbol, "#000000"))
        
        self.active_symbols.add(symbol)
        self.log_text.insert("end", f"✅ Started monitoring {symbol}\n", "good")
        self.log_text.see("end")

    def remove_symbol(self, symbol):
        if symbol not in self.active_symbols:
            messagebox.showinfo("Info", f"{symbol} is not active!")
            return
        
        idx = self.symbols.index(symbol)
        self.axs[idx].clear()
        self.axs[idx].set_title(f"{symbol} (inactive)", fontsize=10, fontweight='bold')
        self.axs[idx].grid(True, alpha=0.3)
        self.canvas.draw()
        
        self.active_symbols.remove(symbol)
        self.log_text.insert("end", f"⏹️ Stopped monitoring {symbol}\n", "alert")
        self.log_text.see("end")

    def clear_logs(self):
        self.log_text.delete("1.0", tk.END)
        self.log_text.insert("end", "🗑️ Logs cleared\n", "log")

    def toggle_running(self):
        self.running = not self.running
        self.toggle_btn.config(text="▶️ Resume" if not self.running else "⏸️ Pause")

    def main_loop(self):
        if not self.running:
            self.after(1000, self.main_loop)
            return

        for symbol in list(self.active_symbols):
            new_price = get_random_price(symbol)
            old_price = self.prices[symbol]
            self.prices[symbol] = new_price
            self.history[symbol].append(new_price)
            
            self.update_graph(symbol, new_price)
            self.check_alerts(symbol, new_price)
            self.do_ml_analysis(symbol, new_price)
            self.log_event(symbol, new_price)
            
        self.after(1000, self.main_loop)

    def update_graph(self, symbol, price):
        if symbol not in self.active_symbols:
            return
        idx = self.symbols.index(symbol)
        ax = self.axs[idx]
        line = self.line  # Упрощение: используем одну линию на графике
        
        x_data = list(range(len(self.history[symbol])))
        line.set_data(x_data, self.history[symbol])
        
        ax.relim()
        ax.autoscale_view(scalex=True, scaley=True)
        
        if len(self.history[symbol]) > 20:
            ax.set_xlim(len(self.history[symbol])-20, len(self.history[symbol]))
        
        self.canvas.draw_idle()

    def check_alerts(self, symbol, price):
        thresholds = {"BTC": 51000, "ETH": 4100, "DOGE": 0.26}
        threshold = thresholds.get(symbol, float('inf'))
        if price > threshold:
            self.log_text.insert("end", 
                f"⚠️ ALERT: {symbol} HIGH! ${price:,.2f} > ${threshold:,.2f}\n", "alert")
            self.log_text.see("end")
        elif price < threshold * 0.98:
            self.log_text.insert("end", 
                f"✅ GOOD: {symbol} LOW! ${price:,.2f} < ${threshold:,.2f}\n", "good")
            self.log_text.see("end")

    def do_ml_analysis(self, symbol, price):
        if len(self.history[symbol]) >= 3:
            recent = self.history[symbol][-3:]
            trend = "📈 UP" if recent[-1] > recent[0] else "📉 DOWN"
            prediction = round(price * 1.005, 2)
            self.log_text.insert("end", 
                f"🤖 ML: {symbol} {trend} | Predict: ${prediction:,.2f}\n", "ml")
            self.log_text.see("end")
            
            if len(self.history[symbol]) > 10:
                self.history[symbol] = self.history[symbol][-10:]

    def log_event(self, symbol, price):
        count = len(self.history[symbol])
        self.log_text.insert("end", f"[{count:04d}] {symbol}: ${price:,.2f}\n", "log")
        self.log_text.see("end")

if __name__ == "__main__":
    app = MonolithicMarketApp()
    app.mainloop()