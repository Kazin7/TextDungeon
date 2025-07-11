using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp1
{
    public static class SaveSystem
    {
        private static string savePath = "SystemData.json";

        public static void Save(Character player, List<Item> inventoryItemList, List<Item> shopItemList)//상점물품,플레이어정보,인벤토리 저장
        {
            try
            {
                var data = new SystemData
                {
                    Player = player,
                    Inventory = inventoryItemList,
                    Shop = shopItemList
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(savePath, json);
                Console.WriteLine("[저장 완료] SystemData.json 생성됨");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[저장 실패] {ex.Message}");
            }
        }

        public static bool Load(out Character player, out List<Item> inventoryItemList, out List<Item> shopItemList)
        {
            player = null;
            inventoryItemList = new List<Item>();
            shopItemList = new List<Item>();
            if (!File.Exists(savePath))
            {
                return false;
            }

            try
            {
                string json = File.ReadAllText(savePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return false;
                }

                var data = JsonSerializer.Deserialize<SystemData>(json);

                if (data == null)
                {
                    return false;
                }

                player = data.Player;
                inventoryItemList = data.Inventory ?? new List<Item>();
                shopItemList = data.Shop ?? new List<Item>();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[로드 실패] 저장된 데이터를 불러올 수 없습니다.\n{ex.Message}");
                return false;
            }
        }
    }
    public class SystemData
    {
        public Character Player { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Item> Shop { get; set; }
    }
}