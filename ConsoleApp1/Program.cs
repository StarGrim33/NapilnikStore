using System.Xml.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            warehouse.Show();   //Вывод всех товаров на складе с их остатком

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3); //при такой ситуации возникает ошибка так, как нет нужного количества товара на складе

            cart.Show();    //Вывод всех товаров в корзине

            Console.WriteLine(cart.Order().PayLink);

            cart.Add(iPhone12, 9); //Ошибка, после заказа со склада убираются заказанные товары
        }
    }

    public class Good
    {
        public string Name { get; private set; }

        public Good(string name)
        {
            if (name == "" || name == null)
                throw new NullReferenceException();

            Name = name;
        }
    }

    public class Warehouse
    {
        private Dictionary<Good, int> _goods;

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        public Warehouse()
        {
            _goods = new Dictionary<Good, int>();
        }

        public void Delive(Good good, int amount)
        {
            if(good  == null || amount <= 0) 
                throw new NullReferenceException();

            if (_goods.ContainsKey(good))
                _goods[good] += amount;
            else
                _goods.Add(good, amount);
        }

        public void Show()
        {
            foreach(var good in _goods)
            {
                Console.WriteLine($"Название: {good.Key}, количество: {good.Value}");
            }
        }

        public void RemoveGoods(Good good, int amount)
        {
            if (_goods.ContainsKey(good))
            {
                if (_goods[good] - amount == 0)
                    _goods.Remove(good);
                else
                    _goods[good] -= amount;
            }
            else
                throw new Exception();
        }
    }

    public class Shop
    {
        public Warehouse Warehouse { get; private set; }

        public string PayLink { get; private set; } = "buy.com";

        public Cart Cart() => new Cart(this);

        public Shop(Warehouse warehouse)
        {
            Warehouse = warehouse;
        }
    }

    public class Cart
    {
        public Shop Shop { get; private set; }

        private readonly Dictionary<Good, int> _goods;

        public Cart(Shop shop)
        {
            Shop = shop;
            _goods = new Dictionary<Good, int>();
        }

        public void Show()
        {
            foreach (var good in _goods)
            {
                Console.WriteLine($"Название: {good.Key}, количество{good.Value}");
            }
        }

        public void Add(Good good, int amount)
        {
            if (Shop.Warehouse.Goods.ContainsKey(good))
            {
                if (Shop.Warehouse.Goods[good] >= amount)
                {
                    Shop.Warehouse.RemoveGoods(good, amount);
                    _goods.Add(good, amount);
                }
                else
                    throw new ArgumentOutOfRangeException();
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        public Shop Order() => Shop;
    }
}