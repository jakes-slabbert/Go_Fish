using GoFish.Models;

namespace GoFish.Services
{
    public interface ICurrencyHelper
    {
        List<CurrencyDto> GetAllCurrencies();
    }
}
