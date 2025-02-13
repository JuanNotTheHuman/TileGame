using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Enums;

namespace TileGame.Models
{
    public class Player
    {
        public Inventory Inventory { get; }
        public Player() { 
            Inventory = new Inventory();
        }
    }
    public class Inventory
    {
        public Dictionary<ItemType,Item> Items { get; }
        public Inventory()
        {
            Items = new Dictionary<ItemType, Item>
            {
                {ItemType.Coin, new Item("Used to buy goods.") },
                {ItemType.Wood, new Item("A primary resource.") }
            };
        }
    }
    public class Item
    {
        public string Description { get; set; }
        public int Count { get; set; }
        public Item() : this("null") { }
        public Item( string description, int count)
        {
            Description = description;
            Count = count;
        }
        public Item( string description ) : this(description, 0){ }
    }
}
