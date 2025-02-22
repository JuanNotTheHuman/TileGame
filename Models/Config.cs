using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
        public Config(int seed, TickConfig tick, TileConfig tiles)
        {
            Seed = seed;
            Tick = tick;
            Tiles = tiles;
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
        public TileConfig()
        {
            int maxcalc = (1920 / Tile.Size) * (1080 / Tile.Size);
            BaseGeneration = new Dictionary<TileType, BaseTileConfig>
            {
                { TileType.GrassA, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassB}, 0.03, double.PositiveInfinity, maxcalc / 2)},
                { TileType.GrassB, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassA}, 0.05, double.PositiveInfinity, maxcalc / 4) },
                { TileType.GrassC, new ForegroundTileConfig(new Collection<TileType>{TileType.GrassB}, 0.01, 1, maxcalc / 5,new Dictionary<ItemType,int>(),new Dictionary<ItemType, int>{{ItemType.Flower,2}}) },
            };
            ForegroundGeneration = new Dictionary<TileType, ForegroundTileConfig>();
            foreach (TileType tileType in Enum.GetValues(typeof(TileType)))
            {
                if (new TileType[] { TileType.GrassA, TileType.GrassB, TileType.GrassC }.Contains(tileType))
                {
                    continue;
                }
                ForegroundGeneration[tileType] = new ForegroundTileConfig(
                    new Collection<TileType>(), 0, 0, 0,
                    new Dictionary<ItemType, int>(),
                    new Dictionary<ItemType, int>()
                );

                foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
                {
                    ForegroundGeneration[tileType].ClickDrops[item] = 0;
                    ForegroundGeneration[tileType].DeathDrops[item] = 0;
                }
            }
            foreach (var x in BaseGeneration)
            {
                x.Value.ClickDrops = new Dictionary<ItemType, int>();
                x.Value.DeathDrops = new Dictionary<ItemType, int>();
                foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
                {
                    x.Value.ClickDrops.Add(item, 0);
                    if (item == ItemType.Flower&&x.Key==TileType.GrassC)
                    {
                        x.Value.DeathDrops.Add(item, 2);
                        continue;
                    }
                    x.Value.DeathDrops.Add(item, 0);
                }
            }
            ForegroundGeneration[TileType.Bush] = new ForegroundTileConfig(
                new Collection<TileType> { TileType.GrassA, TileType.GrassB, TileType.GrassC }, 0.03, 1, 250,
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.Bush].ClickDrops),
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.Bush].DeathDrops)
            );
            ForegroundGeneration[TileType.Mushrooms] = new ForegroundTileConfig(
                new Collection<TileType> { TileType.GrassA, TileType.GrassB, TileType.GrassC },
                SpawnBehavior.Spread, 0.01, 1, 100,
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.Mushrooms].ClickDrops),
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.Mushrooms].DeathDrops)
            );
            ForegroundGeneration[TileType.TreeA] = new ForegroundTileConfig(
                new Collection<TileType> { TileType.GrassA, TileType.GrassB, TileType.GrassC }, 0.02, 2, 200,
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.TreeA].ClickDrops),
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.TreeA].DeathDrops)
            );
            ForegroundGeneration[TileType.TreeB] = new ForegroundTileConfig(
                new Collection<TileType> { TileType.GrassA, TileType.GrassB, TileType.GrassC }, 0.02, 2, 175,
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.TreeB].ClickDrops),
                new Dictionary<ItemType, int>(ForegroundGeneration[TileType.TreeB].DeathDrops)
            );
            BaseGeneration[TileType.GrassC].DeathDrops[ItemType.Flower] = 2;
            ForegroundGeneration[TileType.TreeA].DeathDrops[ItemType.Wood] = 2;
            ForegroundGeneration[TileType.TreeB].DeathDrops[ItemType.Wood] = 2;
            ForegroundGeneration[TileType.Bush].DeathDrops[ItemType.Wood] = 1;
            ForegroundGeneration[TileType.TreeB].ClickDrops[ItemType.Honey] = 1;
            ForegroundGeneration[TileType.Mushrooms].DeathDrops[ItemType.Mushroom] = 2;
        }
        public TileConfig(Dictionary<TileType, BaseTileConfig> baseGeneration, Dictionary<TileType, ForegroundTileConfig> foregroundGeneration)
        {
            BaseGeneration = baseGeneration;
            ForegroundGeneration = foregroundGeneration;
        }
    }
    public class BaseTileConfig
    {
        public double SpawnChance { get; set; }
        public double Health { get; set; }
        public Dictionary<ItemType,int> ClickDrops { get; set; }
        public Dictionary<ItemType, int> DeathDrops { get; set; }
        public BaseTileConfig(double spawnChance, double health,Dictionary<ItemType,int> clickDrops, Dictionary<ItemType, int> deathDrops)
        {
            SpawnChance = spawnChance;
            Health = health;
            DeathDrops = deathDrops;
            ClickDrops = clickDrops;
        }
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
        public ForegroundTileConfig(Collection<TileType> spawnableOn,SpawnBehavior spawnBehavior, double spawnChance, double health, int max, Dictionary<ItemType, int> clickDrops, Dictionary<ItemType, int> deathDrops) : base(spawnChance, health,clickDrops,deathDrops)
        {
            SpawnBehavior = spawnBehavior;
            SpawnableOn = spawnableOn;
            Max = max;
        }
        public ForegroundTileConfig(Collection<TileType> spawnableOn, double spawnChance, double health, int max, Dictionary<ItemType, int> clickDrops, Dictionary<ItemType, int> deathDrops) : base(spawnChance, health, clickDrops, deathDrops)
        {
            SpawnBehavior = SpawnBehavior.Random;
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
}
