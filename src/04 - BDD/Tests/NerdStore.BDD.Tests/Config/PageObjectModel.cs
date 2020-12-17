namespace NerdStore.BDD.Tests.Config
{
    public abstract class PageObjectModel
    {
        protected readonly SeleniumHelper Helper;

        public PageObjectModel(SeleniumHelper helper)
        {
            Helper = helper;
        }

        public string GetUrl() => Helper.GetUrl();

        public void NavigateToUrl(string url) => Helper.GoToUrl(url);
    }
}
