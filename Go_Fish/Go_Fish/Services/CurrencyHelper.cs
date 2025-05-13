using GoFish.Models;
using System.Globalization;

namespace GoFish.Services
{
    public class CurrencyHelper : ICurrencyHelper
    {
        public List<CurrencyDto> GetAllCurrencies()
        {
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            var currencies = new Dictionary<string, CurrencyDto>();

            foreach (var culture in cultureInfos)
            {
                try
                {
                    var region = new RegionInfo(culture.Name);
                    if (!currencies.ContainsKey(region.ISOCurrencySymbol))
                    {
                        currencies[region.ISOCurrencySymbol] = new CurrencyDto
                        {
                            ISOCode = region.ISOCurrencySymbol,
                            Symbol = region.CurrencySymbol,
                            RemixIconClass = GetRemixIcon(region.ISOCurrencySymbol)
                        };
                    }
                }
                catch
                {
                    // Ignore cultures without valid region info
                }
            }

            // Ensure USD, EUR, GBP, and ZAR are on top, then sort the rest alphabetically
            var prioritizedCurrencies = new List<string> { "USD", "EUR", "GBP", "ZAR" };
            return currencies.Values
                .OrderBy(c => prioritizedCurrencies.Contains(c.ISOCode) ? 0 : 1)
                .ThenBy(c => prioritizedCurrencies.IndexOf(c.ISOCode))
                .ThenBy(c => c.ISOCode)
                .ToList();
        }

        private string GetRemixIcon(string isoCode)
        {
            // Mapping ISO currency codes to Remix Icon classes
            return isoCode switch
            {
                "USD" => "ri-money-dollar-circle-fill",
                "EUR" => "ri-money-euro-circle-fill",
                "GBP" => "ri-money-pound-circle-fill",
                "ZAR" => "ri-money-cny-circle-fill", // No specific ZAR icon, using CNY as placeholder
                _ => "ri-money-circle-fill" // Default icon
            };
        }
    }
}
