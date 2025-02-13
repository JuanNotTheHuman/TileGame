using System;
using System.Collections.Generic;
using System.Linq;

public static class WeightedRandom
{
    static Random Random = new Random();
    public static T GetRandom<T>(IEnumerable<WeightedRandomItem<T>> items)
    {
        return GetRandom(items, Random);
    }

    public static T GetRandom<T>(IEnumerable<WeightedRandomItem<T>> items, Random random)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("The collection must not be null or empty.");

        double totalWeight = items.Sum(item => item.Weight);
        if (totalWeight <= 0)
            throw new InvalidOperationException("Total weight must be greater than zero.");

        double randomValue = random.NextDouble() * totalWeight;
        double cumulativeWeight = 0;

        foreach (var item in items)
        {
            cumulativeWeight += item.Weight;
            if (randomValue < cumulativeWeight)
                return item.Item;
        }
        throw new InvalidOperationException("Failed to select a weighted random item.");
    }
}

public class WeightedRandomItem<T>
{
    public T Item { get; set; }
    public double Weight { get; set; }

    public WeightedRandomItem(T item, double weight)
    {
        if (weight < 0) throw new ArgumentException("Weight of item cannot be lower than 0.");
        Item = item;
        Weight = weight;
    }
}
