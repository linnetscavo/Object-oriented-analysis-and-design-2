import random

def get_random_price(symbol):
    base_prices = {
        "BTC": 50000,
        "ETH": 4000,
        "DOGE": 0.25
    }
    volatility = {"BTC": 0.02, "ETH": 0.02, "DOGE": 0.10}
    
    base = base_prices.get(symbol, 1)
    fluctuation = base * volatility.get(symbol, 0.02)
    return round(base + random.uniform(-fluctuation, fluctuation), 4)