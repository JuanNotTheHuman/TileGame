﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TileGame.Enums;
using TileGame.Models;
using TileGame.Services;

namespace TileGame.ViewModels
{
    public class BoardViewModel : ObservableObject
    {
        public int Seed;
        private readonly Random _random;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        [ObservableProperty]
        private int _score;
        [ObservableProperty]
        private double _daytime;
        public double DayTime
        {
            get => _daytime;
            set=> SetProperty(ref _daytime, value);
        }
        public Board Board { get; }
        public ObservableCollection<TileViewModel> Tiles { get; }
        public Collection<TileViewModel> RenderQueue { get; } = new Collection<TileViewModel>();
        public int Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }
        public BoardViewModel(Board board)
        {
            Board = board;
            Tiles = new ObservableCollection<TileViewModel>(
                board.TileGrid.Select(tile => new TileViewModel(tile))
            );
            DayTime = 0;
            Seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            _random = new Random(Seed);
            _timer.Interval = TimeSpan.FromMilliseconds(Board.Config.Tick.Interval);
            _timer.Tick += async (sender, e) => await Tick();
            _timer.Start();
        }
        public bool CanSpread(TileViewModel tvm)
        {
            if (TilesAt(tvm.X,tvm.Y).Count>1) return false;
            return TilesAt(tvm.X - Tile.Size, tvm.Y).Any(r => r.Type == tvm.Type) ||
                   TilesAt(tvm.X + Tile.Size, tvm.Y).Any(r => r.Type == tvm.Type) ||
                   TilesAt(tvm.X, tvm.Y - Tile.Size).Any(r => r.Type == tvm.Type) ||
                   TilesAt(tvm.X, tvm.Y + Tile.Size).Any(r => r.Type == tvm.Type);
        }
        public Collection<TileViewModel> TilesAt(double x, double y) => new Collection<TileViewModel>(Tiles.Where(r => r.X == x && r.Y == y).ToList());
        private async Task AddTilesToDisplay()
        {
            var queueSnapshot = RenderQueue.ToList();
            RenderQueue.Clear();
            foreach (var tile in queueSnapshot)
            {
                Tiles.Add(tile);
                await Task.Delay(5);
            }
        }
        private async Task Tick()
        {
            try
            {
                var entries = Board.Config.Tiles.ForegroundGeneration.Select(r =>{
                        int currentCount = Tiles.Count(t => t.Type == r.Key);
                        double adjustedChance = r.Value.SpawnChance * (1 - (double)currentCount / r.Value.Max);
                        return new WeightedRandomItem<KeyValuePair<TileType, ForegroundTileConfig>>(r, Math.Max(adjustedChance,0));
                    }).ToList();
                for (int i = 0; i < Board.Config.Tick.MaxTransform; i++)
                {
                    if (entries.Count == 0) break;
                    var entry = WeightedRandom.GetRandom(entries, _random);
                    var type = entry.Key;
                    var config = entry.Value;
                    int currentCount = Tiles.Count(t => t.Type == entry.Key);
                    double adjustedChance = config.SpawnChance * (1 - (double)currentCount / config.Max);
                    if (adjustedChance < 0) continue;
                    if(adjustedChance<_random.NextDouble())continue;
                    var spawnableTiles = Tiles.Where(t => config.SpawnableOn.Contains(t.Type) && TilesAt(t.X, t.Y).Count == 1).ToList();
                    if (spawnableTiles.Count == 0) continue;
                    var tile = spawnableTiles[_random.Next(spawnableTiles.Count)];
                    if (config.SpawnBehavior == SpawnBehavior.Random ||
                        (config.SpawnBehavior == SpawnBehavior.Spread && CanSpread(tile)))
                    {
                        RenderQueue.Add(new TileViewModel(new Tile(type, config.Health, tile.X, tile.Y)));
                    }
                }
                if (RenderQueue.Count > 0) await AddTilesToDisplay();
                DayTime += 0.001;
                if(DayTime >= 1)
                {
                    //Trigger trading view
                    while (DayTime >= 0)
                    {
                        DayTime -= 0.001;
                        await Task.Delay(5);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Tick: {ex}");
            }
        }
        public Board ToBoard()
        {
            return new Board(Board.Config, new ObservableCollection<Tile>(Tiles.Select(tvm => tvm.ToTile())));
        }
    }
}