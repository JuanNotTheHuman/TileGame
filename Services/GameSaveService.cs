using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text;
using TileGame.Models;
using System.Text.Json.Serialization;
using System.Linq;
using System.Windows.Markup;
namespace TileGame.Services
{
    public static class GameSaveService
    {
        public static GameSave Current { get; private set; }
        private static readonly string SaveFilePath = "game_saves.json.gz";
        public static void SaveAllSaves(Dictionary<string, GameSave> saves)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
                };
                string json = JsonSerializer.Serialize(saves, options);
                byte[] compressedData = CompressData(json);
                File.WriteAllBytes(SaveFilePath, compressedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game saves: {ex}");
            }
        }
        public static Dictionary<string, GameSave> LoadAllSaves()
        {
            try
            {
                if (!File.Exists(SaveFilePath))
                {
                    return new Dictionary<string, GameSave>();
                }
                byte[] compressedData = File.ReadAllBytes(SaveFilePath);
                string decompressedData = DecompressData(compressedData);
                File.WriteAllText("Decompressed.json", decompressedData);
                if (string.IsNullOrEmpty(decompressedData))
                {
                    return new Dictionary<string, GameSave>();
                }
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
                };
                var allsaves = JsonSerializer.Deserialize<Dictionary<string, GameSave>>(decompressedData,options);
                return allsaves;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game saves: {ex.Message}");
                return new Dictionary<string, GameSave>();
            }
        }
        public static GameSave LoadSave(string saveName)
        {
            try
            {
                var saves = LoadAllSaves();
                var save = saves.ContainsKey(saveName) ? saves[saveName] : null;
                Current = save;
                return save;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
        public static GameSave AddSave(string saveName, GameSave newGameSave)
        {
            try
            {
                Current = newGameSave;
                var saves = LoadAllSaves();
                if (saves.ContainsKey(saveName))
                {
                    Console.WriteLine("Save already exists.");
                    return null;
                }
                saves.Add(saveName, newGameSave);
                string serializedData = JsonSerializer.Serialize(saves, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
                });
                byte[] compressedData = CompressData(serializedData);
                File.WriteAllBytes(SaveFilePath, compressedData);
                Console.WriteLine($"New save added successfully. {newGameSave.Board.TileGrid.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new save: {ex}");
            }
            return newGameSave;
        }
        public static void Save(string saveName, GameSave save)
        {
            try
            {
                var saves = LoadAllSaves();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
                };
                string serializedSave = JsonSerializer.Serialize(save, options);
                if (saves.ContainsKey(saveName))
                {
                    saves[saveName] = save;
                }
                else
                {
                    saves.Add(saveName, save);
                }

                SaveAllSaves(saves);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
        public static void DeleteSave(string saveName)
        {
            try
            {
                var saves = LoadAllSaves();
                if (saves.ContainsKey(saveName))
                {
                    saves.Remove(saveName);
                    SaveAllSaves(saves);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        private static byte[] CompressData(string data)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                    using (var writer = new StreamWriter(gzipStream))
                    {
                        writer.Write(data);
                    }
                    return memoryStream.ToArray();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
        private static string DecompressData(byte[] compressedData)
        {
            try
            {
                using (var memoryStream = new MemoryStream(compressedData))
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipStream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
    [Serializable]
    public class GameSave
    {
        public string Name { get; set; }
        public Player Player { get; set; }
        public Board Board { get; set; }
        public GameSave(string name,Player player,Board board)
        {
            Name = name;
            Player = player;
            Board = board;
        }
        public override string ToString()
        {
            return $"World name: {Name},Player: {Player},Board: {Board}";
        }
    }
}
