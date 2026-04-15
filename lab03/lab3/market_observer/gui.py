import tkinter as tk
from tkinter import ttk, messagebox
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import matplotlib.pyplot as plt
from publisher import MarketDataProvider
from subscribers import LoggerSubscriber, AlertSubscriber, MLSubscriber, GraphSubscriber

class MarketGUI(tk.Tk):
    def __init__(self):
        super().__init__()
        
        self.title("📊 Cryptocurrency Market Observer - Lab Work")
        self.geometry("1400x900")
        self.configure(bg='#f0f0f0')
        
        self.provider = MarketDataProvider()
        self.running = True
        self.active_symbols = set()
        self.subscribers_map = {}
        
        self._setup_styles()
        self._create_layout()
        self._create_buttons()
        
        self.after(1000, self._loop)
        self.protocol("WM_DELETE_WINDOW", self._on_closing)

    def _setup_styles(self):
        style = ttk.Style()
        style.theme_use('clam')
        style.configure('TButton', padding=6, font=('Arial', 10))
        style.configure('TLabel', font=('Arial', 10))
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
        
        symbols = ["BTC", "ETH", "DOGE"]
        colors = {"BTC": "#FF6B35", "ETH": "#627EEA", "DOGE": "#C2A633"}
        
        for idx, symbol in enumerate(symbols):
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
        
        idx = ["BTC", "ETH", "DOGE"].index(symbol)
        ax = self.axs[idx]
        
        graph_sub = GraphSubscriber(ax, symbol, self.canvas)
        logger_sub = LoggerSubscriber(self.log_text)
        alert_sub = AlertSubscriber(self.log_text)
        ml_sub = MLSubscriber(self.log_text)
        
        self.subscribers_map[symbol] = {
            'graph': graph_sub,
            'logger': logger_sub,
            'alert': alert_sub,
            'ml': ml_sub
        }
        
        for sub in self.subscribers_map[symbol].values():
            self.provider.subscribe(symbol, sub)
        
        self.active_symbols.add(symbol)
        self.log_text.insert("end", f"✅ Started monitoring {symbol}\n", "good")
        self.log_text.see("end")

    def remove_symbol(self, symbol):
        if symbol not in self.active_symbols:
            messagebox.showinfo("Info", f"{symbol} is not active!")
            return
        
        for sub in self.subscribers_map[symbol].values():
            self.provider.unsubscribe(symbol, sub)
        
        idx = ["BTC", "ETH", "DOGE"].index(symbol)
        self.axs[idx].clear()
        self.axs[idx].set_title(f"{symbol} (inactive)", fontsize=10, fontweight='bold')
        self.axs[idx].grid(True, alpha=0.3)
        self.canvas.draw()
        
        del self.subscribers_map[symbol]
        self.active_symbols.remove(symbol)
        
        self.log_text.insert("end", f"⏹️ Stopped monitoring {symbol}\n", "alert")
        self.log_text.see("end")

    def clear_logs(self):
        self.log_text.delete("1.0", tk.END)
        self.log_text.insert("end", "🗑️ Logs cleared\n", "log")

    def toggle_running(self):
        self.running = not self.running
        self.toggle_btn.config(text="▶️ Resume" if not self.running else "⏸️ Pause")

    def _loop(self):
        if self.running and self.active_symbols:
            self.provider.update_data()
        self.after(1000, self._loop)

    def _on_closing(self):
        self.running = False
        self.provider.stop()
        self.destroy()