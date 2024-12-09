using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadanie9State
{
    public interface IOrderState
    {
        void AddProduct(string product);
        void SubmitOrder();
        void ConfirmPayment();
        void PackProduct(string product);
        void ShipOrder();
        void CancelOrder();
    }
    public class CreatedState : IOrderState
    {
        private Order order;
        public CreatedState(Order order)
        {
            this.order = order;
        }
        public void AddProduct(string product)
        {
            order.Products.Add(product, false);
            Console.WriteLine($"Dodano produkt: {product}");
        }
        public void SubmitOrder()
        {
            if(order.Products.Count == 0)
            {
                Console.WriteLine("Nie można złożyć pustego zamówienia.");
                return;
            }
            order.SetState(new SubmittedState(order));
            Console.WriteLine("Zamówienie zostało złożone i oczekuje na opłatę.");
        }
        public void ConfirmPayment()
        {
            Console.WriteLine("Zanim opłacisz zamówienie, musisz je złożyć.");
        }
        public void PackProduct(string product)
        {
            Console.WriteLine("Zanim spakujesz produkt, musisz złożyć zamówienie.");
        }
        public void ShipOrder()
        {
            Console.WriteLine("Zanim wyślesz zamówienie, musisz je złożyć.");
        }
        public void CancelOrder()
        {
            order.SetState(new CanceledState());
            Console.WriteLine("Zamówienie zostało anulowane.");
        }
    }
    public class SubmittedState : IOrderState
    {
        private Order order;
        public SubmittedState(Order order)
        {
            this.order = order;
        }
        public void AddProduct(string product)
        {
           
            Console.WriteLine($"Zamówienie zostało już złożone nie możesz już dodawać produktów");
        }
        public void SubmitOrder()
        {
           
            Console.WriteLine("Zamówienie zostało złożone i oczekuje na opłatę.");
        }
        public void ConfirmPayment()
        {
            order.SetState(new Paidstate(order));
            order.isPaid = true;
            Console.WriteLine("Płatność została potwierdzona.");
        }
        public void PackProduct(string product)
        {
            Console.WriteLine("Zanim spakujesz produkt, musisz złożyć zamówienie.");
        }
        public void ShipOrder()
        {
            Console.WriteLine("Zanim wyślesz zamówienie, musisz je złożyć.");
        }
        public void CancelOrder()
        {
            order.SetState(new CanceledState());
            Console.WriteLine("Zamówienie zostało anulowane.");
        }
    }
    public class Paidstate : IOrderState
    {
        private Order order;
        public Paidstate(Order order)
        {
            this.order = order;
        }
        public void AddProduct(string product)
        {
            Console.WriteLine("Nie można dodawać produktów do opłaconego zamówienia.");
        }
        public void SubmitOrder()
        {
            Console.WriteLine("Nie można złożyć już złożonego i opłaconego zamówienia.");
        }
        public void ConfirmPayment()
        {
            Console.WriteLine("Płatność jest już potwierdzona nie można tego zrobić ponownie.");
        }
        public void PackProduct(string product)
        {
            if (order.Products.ContainsKey(product))
            {
                if (order.Products[product] == true)
                {
                    Console.WriteLine($"Produkt {product} został już spakowany.");
                    return;
                }
                else
                {
                    order.Products[product] = true;
                    Console.WriteLine($"Produkt {product} został spakowany.");
                }
            }
            else
            {
                Console.WriteLine($"Produkt {product} nie istnieje w zamówieniu.");
            }
        }
        public void ShipOrder()
        {
            foreach(var product in order.Products)
            {
                if (product.Value == false)
                {
                    Console.WriteLine("Nie można wysłać zamówienia, ponieważ nie wszystkie produkty są spakowane.");
                    return;
                }
            }
            order.SetState(new ShippedState());
            Console.WriteLine("Zamówienie zostało wysłane.");
        }
        public void CancelOrder()
        {
            Console.WriteLine("Środki zostały zwrócone klientowi.");
            Console.WriteLine("Zamówienie zostało anulowane.");
            order.Products.Clear();
        }
    }
    public class ShippedState : IOrderState
    {
        public void AddProduct(string product)
        {
            Console.WriteLine("Nie można dodawać produktów do wysłanego zamówienia.");
        }

        public void CancelOrder()
        {
            Console.WriteLine("Nie można anulować wysłanego zamówienia.");
        }

        public void ConfirmPayment()
        {
            
            Console.WriteLine("Nie można potwierdzić płatności dla wysłanego zamówienia.");
        }

        public void PackProduct(string product)
        {
           Console.WriteLine("Nie można spakować produktu dla wysłanego zamówienia.");
        }

        public void ShipOrder()
        {
            Console.WriteLine("Zamówienie zostało już wysłane.");
        }

        public void SubmitOrder()
        {
           Console.WriteLine("Nie można złożyć zamówienia dla wysłanego zamówienia.");
        }
    }
    public class CanceledState : IOrderState
    {
        public void AddProduct(string product)
        {
           Console.WriteLine("Nie można dodawać produktów do anulowanego zamówienia.");
        }

        public void CancelOrder()
        {
            Console.WriteLine("Nie można anulować zamówienia, które zostało już anulowane.");
        }

        public void ConfirmPayment()
        {
            Console.WriteLine("Nie można potwierdzić płatności dla anulowanego zamówienia.");
        }

        public void PackProduct(string product)
        {
            Console.WriteLine("Nie można spakować produktu dla anulowanego zamówienia.");
        }

        public void ShipOrder()
        {
            Console.WriteLine("Nie można wysłać zamówienia, które zostało już anulowane.");
        }

        public void SubmitOrder()
        {
            Console.WriteLine("Nie można złożyć zamówienia dla anulowanego zamówienia.");
        }
    }
}
