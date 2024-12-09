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
           // order.SetState(new SubmittedState(order));
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
            Console.WriteLine("Zamówienie zostało anulowane.");
        }
    }

}
