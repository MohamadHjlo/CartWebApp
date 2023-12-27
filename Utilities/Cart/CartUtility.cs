namespace Utilities.Cart
{
    public static class CartUtility
    {
        public static decimal CalculateCartItemDiscountByPercent(decimal price,decimal discountPercent)
        {
            return (long)price * ((100 - discountPercent));
        }

        public static decimal CalculateCartItemDiscountByPrice(decimal price, decimal discountPrice)
        {
            return (long)price * ((100 - discountPrice) / 100);
        }
    }
}