using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        public Board Board { get; private set; }
        private ObservableCollection <TileViewModel> _tiles;
        public BoardViewModel(Board board)
        {
            Board = board;
            _tiles = new ObservableCollection<TileViewModel>(
                board.TileGrid.Select(tile => new TileViewModel(tile))
            );
            DispatcherTimer.Tick += new EventHandler(async(sender, e) => { await AddTilesToDisplay(); });
            DispatcherTimer.Interval = new TimeSpan(10);
        }
        private DispatcherTimer DispatcherTimer { get; } = new DispatcherTimer();
        public ObservableCollection<TileViewModel> Tiles
        {
            get => _tiles;
            set
            {
                if (_tiles != value)
                {
                    _tiles = value;
                    OnPropertyChanged(nameof(Tiles));
                }
            }
        }
        public Collection<TileViewModel> RenderQueue { get; } = new Collection<TileViewModel>();
        public int Score
        {
            get => Score;
            set
            {
                if(Score != value)
                {
                    Score = value;
                    OnPropertyChanged(nameof(Score));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]  string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool CheckCollisions(TileViewModel tvm, Config config)
        {
            return Tiles.Any(r =>Tile.Collides(tvm.Tile,r.Tile)&&config.Tiles.ForegroundGeneration.ContainsKey(r.Type));
        }
        public bool CanSpread(TileViewModel tvm, Config config)
        {
            if(!CheckCollisions(tvm, config))
            {
                if(TilesAt(tvm.X - Tile.Size,tvm.Y).Any(r=>r.Type==tvm.Type))return true; //has to the left
                if (TilesAt(tvm.X + Tile.Size, tvm.Y).Any(r => r.Type == tvm.Type)) return true; //has to the right
                if (TilesAt(tvm.X, tvm.Y - Tile.Size).Any(r => r.Type == tvm.Type)) return true; //has to the top
                if (TilesAt(tvm.X, tvm.Y + Tile.Size).Any(r => r.Type == tvm.Type)) return true; //has to the bottom
            }
            return false;
        }
        public IEnumerable<TileViewModel> TilesAt(double x, double y)
        {
            return Tiles.Where(r=>r.X == x && r.Y == y);
        }
        public void EnableQueue() => DispatcherTimer.Start();
        private async Task AddTilesToDisplay()
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                foreach (var tile in RenderQueue)
                {
                    RenderQueue.Remove(tile);
                    Tiles.Add(tile);
                    await Task.Delay(5);
                    Debug.WriteLine($"Finalized: Added a {tile.Type} at ({tile.X}, {tile.Y})");
                }
                OnPropertyChanged(nameof(Tiles));
                Debug.WriteLine($"Total Tiles After Update: {Tiles.Count}");
            });
        }
    }
}
