using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Enums;

namespace TileGame.Models
{
    public class Config
    {
        public TickConfig Tick { get; }
        public TileConfig Tiles { get; }
        public Config() {
            Tick = new TickConfig();
            Tiles = new TileConfig();
        }
    }
    public class TickConfig
    {
        public int Interval { get; }
        public int MaxTransform { get; }
        public TickConfig(int interval, int maxTransform)
        {
            Interval = interval;
            MaxTransform = maxTransform;
        }
        public TickConfig() {
            Interval = 50;
            MaxTransform = 5;
        }
    }
    public class TileConfig
    {
        public Dictionary<TileType, BaseTileConfig> BaseGeneration { get; }
        public Dictionary<TileType, ForegroundTileConfig> ForegroundGeneration { get; }
        public Dictionary<TileType, IEnumerable<TileDrop>> ClickDrops { get; }
        public Dictionary<TileType, IEnumerable<TileDrop>> DeathDrops { get; }
        public TileConfig()
        {
            int maxcalc = (1920 / Tile.Size)*(1080 / Tile.Size);
            BaseGeneration = new Dictionary<TileType, BaseTileConfig>
            {
                {TileType.GrassA, new ForegroundTileConfig(new List<TileType>{TileType.GrassB},0.05,double.PositiveInfinity,maxcalc/2) },
                {TileType.GrassB, new ForegroundTileConfig(new List<TileType>{TileType.GrassA},0.03,double.PositiveInfinity,maxcalc/3)},
                {TileType.GrassC, new ForegroundTileConfig(new List<TileType>{TileType.GrassB},0.01,1,maxcalc/5) },
            };
            ForegroundGeneration = new Dictionary<TileType, ForegroundTileConfig>
            {
                {TileType.Bush, new ForegroundTileConfig(new List<TileType>{TileType.GrassA,TileType.GrassB, TileType.GrassC},0.03, 2, 250) }
            };
            DeathDrops = new Dictionary<TileType, IEnumerable<TileDrop>>
            {
                {TileType.GrassC, new List<TileDrop>{new TileDrop(ItemType.Coin,1)} },
                {TileType.Bush, new List<TileDrop>{new TileDrop(ItemType.Wood,1)} },
                {TileType.TreeTopA, new List<TileDrop>{new TileDrop(ItemType.Wood,2)}},
                {TileType.TreeBottomA, new List<TileDrop>{new TileDrop(ItemType.Wood,2)}},
            };
            ClickDrops = new Dictionary<TileType, IEnumerable<TileDrop>>();
            {

            };
        }
    }
    public class BaseTileConfig
    {
        public double SpawnChance { get; }
        public double Health { get; }
        public BaseTileConfig(double spawnChance, double health)
        {
            SpawnChance = spawnChance;
            Health = health;
        }
    }
    public class ForegroundTileConfig : BaseTileConfig
    {
        public int Max { get; }
        public IEnumerable<TileType> SpawnableOn { get; }
        public ForegroundTileConfig(IEnumerable<TileType> spawnableOn, double spawnChance, double health, int max) : base(spawnChance, health)
        {
            SpawnableOn = spawnableOn;
            Max = max;
        }
    }
    public class TileDrop
    {
        public ItemType Type { get; }
        public int Count { get; }
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
