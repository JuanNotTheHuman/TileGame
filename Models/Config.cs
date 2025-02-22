using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Enums;

namespace TileGame.Models
{
    public class Config
    {
        public int Seed { get; set; }
        public TickConfig Tick { get; set; }
        public TileConfig Tiles { get; set; }
        public Config() {
            Tick = new TickConfig();
            Tiles = new TileConfig();
        }
    }
    public class TickConfig
    {
        public int Interval { get; set; }
        public int MaxTransform { get; set; }
        public TickConfig(int interval, int maxTransform)
        {
            Interval = interval;
            MaxTransform = maxTransform;
        }
        public TickConfig() {
            Interval = 100;
            MaxTransform = 15;
        }
    }
    public class TileConfig
    {
        public Dictionary<TileType, BaseTileConfig> BaseGeneration { get; set; }
        public Dictionary<TileType, ForegroundTileConfig> ForegroundGeneration { get; set; }
        public Dictionary<TileType, ICollection<TileDrop>> ClickDrops { get; set; }
        public Dictionary<TileType, ICollection<TileDrop>> DeathDrops { get; set; }
        public TileConfig()
        {
            int maxcalc = (1920 / Tile.Size)*(1080 / Tile.Size);
            BaseGeneration = new Dictionary<TileType, BaseTileConfig>
            {
                {TileType.GrassA, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassB},0.03,double.PositiveInfinity,maxcalc/2) },
                {TileType.GrassB, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassA},0.05,double.PositiveInfinity,maxcalc/4)},
                {TileType.GrassC, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassB},0.01,1,maxcalc/5) },
            };
            ForegroundGeneration = new Dictionary<TileType, ForegroundTileConfig>
            {
                {TileType.Bush, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassA,TileType.GrassB, TileType.GrassC},0.03, 1, 250) },
                {TileType.Mushrooms, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassA,TileType.GrassB,TileType.GrassC },SpawnBehavior.Spread,0.01,1,100) },
                {TileType.TreeA,new ForegroundTileConfig(new Collection<TileType>{TileType.GrassA,TileType.GrassB,TileType.GrassC },0.02,2,200) },
                {TileType.TreeB,new ForegroundTileConfig(new Collection<TileType>{TileType.GrassA,TileType.GrassB,TileType.GrassC },0.02,2,175) }

            };
            DeathDrops = new Dictionary<TileType, ICollection<TileDrop>>
            {
                {TileType.GrassC, new Collection<TileDrop>{new TileDrop(ItemType.Flower,2)} },
                {TileType.Bush, new Collection<TileDrop>{new TileDrop(ItemType.Wood,1)} },
                {TileType.TreeA, new Collection<TileDrop>{new TileDrop(ItemType.Wood,2)}},
                {TileType.TreeB, new Collection<TileDrop>{new TileDrop(ItemType.Wood,1)}},
                {TileType.Mushrooms, new Collection<TileDrop>{new TileDrop(ItemType.Mushroom,2)} }
            };
            ClickDrops = new Dictionary<TileType, ICollection<TileDrop>>
            {
                { TileType.TreeB,new Collection<TileDrop>{ new TileDrop(ItemType.Honey,1)}}
            };
        }
    }
    public class BaseTileConfig
    {
        public double SpawnChance { get; set; }
        public double Health { get; set; }
        public BaseTileConfig(double spawnChance, double health)
        {
            SpawnChance = spawnChance;
            Health = health;
        }
    }
    public class ForegroundTileConfig : BaseTileConfig
    {
        public int Max { get; set; }
        public Collection<TileType> SpawnableOn { get; set; }
        public SpawnBehavior SpawnBehavior { get; set; }
        public ForegroundTileConfig(Collection<TileType> spawnableOn,SpawnBehavior spawnBehavior, double spawnChance, double health, int max) : base(spawnChance, health)
        {
            SpawnBehavior = spawnBehavior;
            SpawnableOn = spawnableOn;
            Max = max;
        }
        public ForegroundTileConfig(Collection<TileType> spawnableOn, double spawnChance, double health, int max) : base(spawnChance, health)
        {
            SpawnBehavior = SpawnBehavior.Random;
            SpawnableOn = spawnableOn;
            Max = max;
        }
    }
    public class TileDrop
    {
        public ItemType Type { get; set; }
        public int Count { get; set; }
        public TileDrop(ItemType type)
        {
            Type = type;
            Count = 0;
        }
        public TileDrop(ItemType type, int count)
        {
            Type = type;
            Count = count;
        }
        public TileDrop()
        {

        }
    }
}
