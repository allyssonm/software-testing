using NerdStore.BDD.Tests.Config;
using System;

namespace NerdStore.BDD.Tests.Order
{
    public class OrderPage : PageObjectModel
    {
        public OrderPage(SeleniumHelper helper) : base(helper) { }

        public void AccessProductsShowcase() =>
            Helper.GoToUrl(Helper.Configuration.ShowcaseUrl);

        public void GetProductDetails(int position = 1) =>
            Helper.ClickByXPath($"html/body/div/main/div/div/div[{position}]/span/a");

        public bool IsProductAvailable() =>
            Helper.ValidateUrlContent(Helper.Configuration.ProductUrl);

        public int GetProductQuantity()
        {
            var element = Helper.GetElementByXPath("/html/body/div/main/div/div/div[2]/p[1]");
            var quantity = element.Text.OnlyNumbers();

            if (char.IsNumber(quantity.ToString(), 0)) return quantity;

            return 0;
        }

        public void ClickOnPurchaseNow() =>
            Helper.ClickByXPath("/html/body/div/main/div/div/div[2]/form/div[2]/button");

        public bool IsProductOnCart() =>
            Helper.ValidateUrlContent(Helper.Configuration.CartUrl);

        public decimal GetProductUnitPrice() =>
            Convert.ToDecimal(Helper.GetTextElementById("unitValue").Replace('$', ' ').Trim());

        public decimal GetTotalPrice() =>
            Convert.ToDecimal(Helper.GetTextElementById("totalPrice").Replace('$', ' ').Trim());
    }
}
