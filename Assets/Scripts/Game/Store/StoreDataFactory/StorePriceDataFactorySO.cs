using UnityEngine;

[CreateAssetMenu(fileName = "StorePriceFactory", menuName = "StoreFactory/StorePriceFactory")]
public class StorePriceDataFactorySO : ScriptableObject
{
    public StorePriceData Create(RawStorePrice rawStorePrice)
    {
        var price = new StorePriceData
        {
            storePrice = new RangeValue(rawStorePrice.priceMiddle, rawStorePrice.priceRange)
        };

        return price;
    }
}
