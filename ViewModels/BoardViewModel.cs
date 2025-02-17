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

namespace TileGame.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        public int Seed;
        private Random _random;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private int _score;

        public Board Board { get; }
        private Config Config { get; }
        public ObservableCollection<TileViewModel> Tiles { get; }
        public Collection<TileViewModel> RenderQueue { get; } = new Collection<TileViewModel>();

        public int Score
        {
            get => _score;
            set
            {
                if (_score != value)
                {
                    _score = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public BoardViewModel(Board board, Config config)
        {
            Config = config;
            Board = board;
            Tiles = new ObservableCollection<TileViewModel>(
                board.TileGrid.Select(tile => new TileViewModel(tile))
            );
            Seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            _random = new Random(Seed);
            _timer.Interval = TimeSpan.FromMilliseconds(Config.Tick.Interval);
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
            OnPropertyChanged(nameof(Tiles));
        }
        private async Task Tick()
        {
            try
            {
                int totalTransforms = 0;
                var spawnableTiles = Tiles.Where(tile => Config.Tiles.ForegroundGeneration.Any(c => c.Value.SpawnableOn.Contains(tile.Type)) && TilesAt(tile.X, tile.Y).Count == 1).ToList();
                foreach (var entry in Config.Tiles.ForegroundGeneration)
                {
                    var type = entry.Key;
                    var config = entry.Value;
                    int currentCount = Tiles.Count(r => r.Type == type);
                    if (currentCount >= config.Max) continue;
                    double adjustedSpawnChance = config.SpawnChance * (1 - (double)currentCount / config.Max);
                    for (int i = 0; i < Config.Tick.MaxTransform; i++)
                    {
                        if (totalTransforms >= Config.Tick.MaxTransform) break;
                        var tile = spawnableTiles[_random.Next(spawnableTiles.Count)];
                        if (adjustedSpawnChance < _random.NextDouble()) continue;
                        if (config.SpawnBehavior == SpawnBehavior.Random || (config.SpawnBehavior == SpawnBehavior.Spread && CanSpread(tile)))
                        {
                            RenderQueue.Add(new TileViewModel(new Tile(type, config.Health, tile.X, tile.Y)));
                            totalTransforms++;
                        }
                    }
                    if (totalTransforms >= Config.Tick.MaxTransform) break;
                }
                if (RenderQueue.Count > 0) await AddTilesToDisplay();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in Tick: {ex}");
            }
        }
    }
}