using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TileGame.Enums;

namespace TileGame.Models
{
    [Serializable]
    public class Player
    {
        public Inventory Inventory { get; set; }
        public Player() { 
            Inventory = new Inventory();
        }
        [JsonConstructor]
        public Player(Inventory inventory)
        {
            Inventory = inventory;
        }
        public override string ToString()
        {
            return $"Inventory:{Inventory}";
        }
    }
    [Serializable]
    public class Inventory
    {
        [JsonInclude]
        public Dictionary<ItemType, Item> Items { get; set; }

        public Inventory()
        {
            Items = new Dictionary<ItemType, Item>
        {
            {ItemType.Coin, new Item("Used to buy goods.") },
            {ItemType.Wood, new Item("A primary resource.") },
            {ItemType.Mushroom, new Item("Just a mushroom.") },
            {ItemType.Flower, new Item("A really pretty flower.") },
            {ItemType.Honey, new Item("A very sticky, yet yummy substance.") }
        };
        }

        public Inventory(Dictionary<ItemType, Item> items)
        {
            Items = items;
        }
        public override string ToString()
        {
            return $"{string.Join(",",Items.Select(r=>$"({r.Key}:{r.Value.Count})"))}";
        }
    }

    [Serializable]
    public class Item
    {
        [JsonPropertyName("Description"),JsonInclude]
        public string Description { get; set; }

        [JsonPropertyName("Count"),JsonInclude]
        public int Count { get; set; }

        public Item() : this("null") { }
        public Item(string description, int count)
        {
            Description = description;
            Count = count;
        }
        public Item(string description) : this(description, 0) { }
    }
}
