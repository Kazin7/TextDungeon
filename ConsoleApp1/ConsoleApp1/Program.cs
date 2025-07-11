using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        static List<Item> inventoryItemList = new List<Item>();
        static List<Item> shopItemList = new List<Item>();
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    return result;
                }
                Console.WriteLine("잘못된 입력입니다. 숫자를 입력해주세요.\n");
            }
        }
        static void StartScene(Character player) //시작 장면
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 종료하기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            int action = ReadInt(">> ");
            Console.WriteLine();

            switch (action)
            {
                case 1:
                    Status(player);
                    break;
                case 2:
                    Inventory(player);
                    break;
                case 3:
                    Shop(player);
                    break;
                case 4:
                    Dungeon(player);
                    break;
                case 5:
                    Rest(player);
                    break;
                case 6:
                    Console.WriteLine("게임을 종료합니다.");
                    SaveSystem.Save(player, inventoryItemList, shopItemList);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.\n");
                    break;
            }
        }
        static void Status(Character player)//플레이어 상태 출력
        {
            player.ShowStatus();
            while (true)
            {
                int action = ReadInt(">> ");
                if (action == 0) break;
                Console.WriteLine("잘못된 입력입니다.\n");
            }
            StartScene(player);
        }
        static void Inventory(Character player) //인벤 출력
        {
            while (true)
            {
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                Console.WriteLine("[아이템 목록]\n");
                foreach (Item item in inventoryItemList)
                {
                    item.ShowMyitems();
                }
                Console.WriteLine("\n1. 장착 관리");
                Console.WriteLine("0. 나가기\n");

                int action = ReadInt(">> ");

                if (action == 0) break;
                else if (action == 1) Equip(player);
                else Console.WriteLine("잘못된 입력입니다.\n");
            }
            StartScene(player);
        }
        static void Equip(Character player) //장비 장착
        {
            bool changed = false;
            while (true)
            {
                int index = 0;
                foreach (Item item in inventoryItemList)
                {
                    item.ShowMyitems(true, index++);
                }
                Console.WriteLine("0. 나가기\n");
                int action = ReadInt(">> ") - 1;

                if (action == -1) break;
                if (action >= 0 && action < inventoryItemList.Count)
                {
                    Item selectedItem = inventoryItemList[action];
                    if (selectedItem.IsEquipped)
                    {
                        selectedItem.IsEquipped = false;
                        player.EquippedItems.Remove(selectedItem);
                        Console.WriteLine($"[{selectedItem.Name}] 장착 해제됨\n");
                        changed = true;
                    }
                    else
                    {
                        if (player.IsItemEquipped(selectedItem.Type))
                        {
                            Item equipped = player.GetEquippedItem(selectedItem.Type);
                            equipped.IsEquipped = false;
                            player.EquippedItems.Remove(equipped);
                            Console.WriteLine($"[{equipped.Name}] 장착 해제됨");
                            changed = true;
                        }
                        selectedItem.IsEquipped = true;
                        player.EquippedItems.Add(selectedItem);
                        Console.WriteLine($"[{selectedItem.Name}] 장착 완료\n");
                        changed = true;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                }
                if (changed)
                    SaveSystem.Save(player, inventoryItemList, shopItemList);
            }
            StartScene(player);
        }
        static void Shop(Character player)//상점
        {
            while (true)
            {
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold:F0} G\n");
                Console.WriteLine("[아이템 목록]");
                foreach (Item item in shopItemList)
                {
                    item.ShowShopItems();
                }
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기\n");

                int action = ReadInt(">> ");
                if (action == 0) break;
                else if (action == 1) BuyItemShop(player);
                else if (action == 2) SellItemShop(player);
                else Console.WriteLine("잘못된 입력입니다.\n");
            }
            StartScene(player);
        }
        static void BuyItemShop(Character player)//아이템 구매
        {
            while (true)
            {
                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold:F0} G\n");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < shopItemList.Count; i++)
                {
                    shopItemList[i].ShowShopItems(true, i);
                }
                Console.WriteLine("\n0. 나가기\n");
                int action = ReadInt(">> ") - 1;
                if (action == -1) break;
                if (action >= 0 && action < shopItemList.Count)
                {
                    Item selectedItem = shopItemList[action];
                    if (selectedItem.IsPurchased)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.\n");
                        continue;
                    }
                    if (selectedItem.Price > player.Gold)
                    {
                        Console.WriteLine("Gold 가 부족합니다.\n");
                        continue;
                    }
                    Console.WriteLine("구매를 완료했습니다.\n");
                    player.Gold -= selectedItem.Price;
                    selectedItem.IsPurchased = true;
                    inventoryItemList.Add(selectedItem);
                    SaveSystem.Save(player, inventoryItemList, shopItemList);
                }
                else Console.WriteLine("잘못된 입력입니다.\n");
            }
            StartScene(player);
        }
        static void SellItemShop(Character player)//아이템 판매
        {
            while (true)
            {
                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold:F0} G\n");
                Console.WriteLine("[아이템 목록]\n");
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    inventoryItemList[i].ShowSellItem(true, i);
                }
                Console.WriteLine("\n0. 나가기\n");
                int action = ReadInt(">> ") - 1;
                if (action == -1) break;
                if (action >= 0 && action < inventoryItemList.Count)
                {
                    Item selectedItem = inventoryItemList[action];
                    Console.WriteLine("판매를 완료했습니다.\n");
                    player.Gold += (float)(selectedItem.Price * 0.85);
                    selectedItem.IsEquipped = false;
                    inventoryItemList.Remove(selectedItem);
                    SaveSystem.Save(player, inventoryItemList, shopItemList);
                }
                else Console.WriteLine("잘못된 입력입니다.\n");
            }
            StartScene(player);
        }
        static void Dungeon(Character player)//던전 입장
        {
            int easyDef = 5;
            int normalDef = 11;
            int hardDef = 17;
            Random rand = new Random();

            float bonusAtk = player.EquippedItems.Where(i => i.IsEquipped && i.Type == ItemType.Weapon).Sum(i => i.StatValue);
            int bonusDef = player.EquippedItems.Where(i => i.IsEquipped && i.Type == ItemType.Armor).Sum(i => i.StatValue);

            int finalDefense = player.Def + bonusDef;
            float finalDmg = player.AtkDmg + bonusAtk;

            while (true)
            {
                Console.WriteLine("던전입장");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
                Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
                Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
                Console.WriteLine("0. 나가기\n");

                int action = ReadInt(">> ");

                if (action == 0)
                {
                    break;
                }
                else if (action == 1 && TryEnterDungeon(player, finalDefense, finalDmg, 1000, easyDef)) return;
                else if (action == 2 && TryEnterDungeon(player, finalDefense, finalDmg, 1700, normalDef)) return;
                else if (action == 3 && TryEnterDungeon(player, finalDefense, finalDmg, 2500, hardDef)) return;
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.\n");
                }
            }
            StartScene(player);
        }
        static bool TryEnterDungeon(Character player, int finalDefense, float finalDmg, int baseGold, int requiredDef)//던전 성공실패 처리
        {
            Random rand = new Random();
            if (finalDefense < requiredDef)
            {
                if (rand.Next(0, 100) < 40)
                {
                    DungeonFail(player);
                    StartScene(player);
                    return true;
                }
            }
            DungeonSuccess(player, finalDefense, finalDmg, baseGold);
            StartScene(player);
            return true;
        }
        static void DungeonFail(Character player)//던전 실패
        {
            Console.WriteLine("던전 클리어에 실패했습니다!");
            player.Hp /= 2;
            SaveSystem.Save(player, inventoryItemList, shopItemList);
        }
        static void DungeonSuccess(Character player, int finalDefense, float finalDmg, int baseGold)//던전 성공
        {
            Random rand = new Random();

            player.DungeonClear++; //클리어 횟수 추가
            int prevHp = player.Hp;
            int prevGold = (int)player.Gold;

            int damageMin = Math.Max(0, 20 - finalDefense);//
            int damageMax = Math.Max(damageMin, 36 - finalDefense);

            int rndHpMinus = rand.Next(damageMin, damageMax);
            player.Hp -= rndHpMinus;
            if (player.Hp < 0) player.Hp = 0;

            float minGold = baseGold + baseGold * finalDmg * 0.01f;
            float maxGold = baseGold + baseGold * finalDmg * 0.02f;
            int bonusGold = (int)((float)(rand.NextDouble() * (maxGold - minGold) + minGold));
            player.Gold += bonusGold;

            string dungeonName = baseGold switch
            {
                1000 => "쉬운 던전",
                1700 => "일반 던전",
                2500 => "어려운 던전",
                _ => "알 수 없는 던전"
            };

            Console.WriteLine("\n던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{dungeonName}을 클리어 하였습니다.\n");

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {prevHp} -> {player.Hp}");
            Console.WriteLine($"Gold {prevGold:f0} G -> {player.Gold:F0} G\n");

            player.LevelUp();
            SaveSystem.Save(player, inventoryItemList, shopItemList);

            Console.WriteLine("0. 나가기\n");
            Console.Write("아무키나 입력해주세요.\n>> ");
            Console.ReadLine();
        }
        static void Rest(Character player)//휴식 창
        {
            int restMoney = 500;
            while (true)
            {
                Console.WriteLine("휴식하기");
                Console.WriteLine($"{restMoney} G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold:F0} G)\n");
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기\n");
                int action = ReadInt(">> ");

                if (action == 0)
                {
                    break;
                }
                else if (action == 1)
                {
                    if (player.Gold >= restMoney)
                    {
                        Console.WriteLine("휴식을 완료했습니다.");
                        player.Hp = 100;
                        player.Gold -= restMoney;
                        SaveSystem.Save(player, inventoryItemList, shopItemList);
                        StartScene(player);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.\n");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.\n");
                }
            }
            StartScene(player);
        }
        static void Main(string[] args)
        {
            Character player = new Character();

            if (!SaveSystem.Load(out player, out inventoryItemList, out shopItemList))
            {
                player = new Character();

                inventoryItemList = new List<Item>
                {
                    new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", ItemType.Weapon, 2, 600),
                    new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", ItemType.Armor, 5, 1000),
                    new Item("낡은 투구", "방랑객이던 투사가 쓰던 투구입니다.", ItemType.HpBoost, 10, 2000),
                    new Item("헐거운 허리띠", "체력을 보강해주는 낡은 허리띠입니다.", ItemType.HpBoost, 5, 800),
                    new Item("훈련용 목검", "초보자들이 쓰는 가벼운 나무검입니다.", ItemType.Weapon, 1, 2000)
                };

                shopItemList = new List<Item>
                {
                    new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", ItemType.Armor, 5, 1000),
                    new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", ItemType.Armor, 9, 1500),
                    new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", ItemType.Armor, 15, 3500),
                    new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", ItemType.Weapon, 2, 600),
                    new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", ItemType.Weapon, 5, 1500),
                    new Item("스파르타의 창", "전설의 창입니다.", ItemType.Weapon, 7, 2000),
                    new Item("생명력 허리띠", "체력을 보강해주는 마법이 깃든 허리띠입니다.", ItemType.HpBoost, 12, 1500),
                    new Item("번개의 단검", "빠른 공격이 가능한 단검입니다.", ItemType.Weapon, 10, 3000)
                };

                SaveSystem.Save(player, inventoryItemList, shopItemList);
            }

            StartScene(player);

        }
    }
}
