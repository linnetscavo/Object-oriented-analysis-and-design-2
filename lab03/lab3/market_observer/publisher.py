from threading import Thread
import time
from utils import get_random_price

class MarketDataProvider:
    def __init__(self):
        self.subscribers = {}
        self.data = {}
        self._running = True

    def subscribe(self, symbol: str, subscriber):
        if symbol not in self.subscribers:
            self.subscribers[symbol] = []
            self.data[symbol] = 0
        if subscriber not in self.subscribers[symbol]:
            self.subscribers[symbol].append(subscriber)
            print(f"✓ {subscriber.__class__.__name__} subscribed to {symbol}")

    def unsubscribe(self, symbol: str, subscriber):
        if symbol in self.subscribers and subscriber in self.subscribers[symbol]:
            self.subscribers[symbol].remove(subscriber)
            print(f"✗ {subscriber.__class__.__name__} unsubscribed from {symbol}")
            if not self.subscribers[symbol]:
                del self.subscribers[symbol]

    def notify(self, symbol: str, price: float):
        if symbol in self.subscribers:
            for sub in self.subscribers[symbol]:
                try:
                    sub.update(symbol, price)
                except Exception as e:
                    print(f"Error notifying {sub.__class__.__name__}: {e}")

    def update_data(self):
        for symbol in list(self.subscribers.keys()):
            price = get_random_price(symbol)
            self.data[symbol] = price
            self.notify(symbol, price)
    
    def stop(self):
        self._running = False