using NerdStore.BDD.Tests.Config;

namespace NerdStore.BDD.Tests.Order
{
    public class OrderPage : PageObjectModel
    {
        public OrderPage(SeleniumHelper helper) : base(helper) { }

        public void AccessProductsShowcase() =>
            Helper.GoToUrl(Helper.Configuration.ShowcaseUrl);

        public void GetProductDetails(int position = 1) =>
            Helper.ClickByXPath($"html/body/div/main/div/div/div[{position}]/span/a");

        public bool CheckIfProductIsAvailable() =>
            Helper.ValidateUrlContent(Helper.Configuration.ProductUrl);
    }
}
