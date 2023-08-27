//------------------------------------------------------------------------
namespace FKGame.InventorySystem
{
    [System.Serializable]
    public class Currency : Item
    {
        public override int MaxStack
        {
            get{return int.MaxValue;}
        }

        public CurrencyConversion[] currencyConversions;
    }
}