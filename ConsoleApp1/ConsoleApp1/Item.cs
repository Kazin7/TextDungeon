namespace ConsoleApp1
{
    public enum ItemType
    {
        Weapon,
        Armor,
        HpBoost
    }

    [Serializable]
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public int StatValue { get; set; }
        public int Price { get; set; }
        public bool IsEquipped { get; set; } = false;
        public bool IsPurchased { get; set; } = false;

        public Item() {}    
        public Item(string name, string description, ItemType type, int value, int price)
        {
            Name = name;
            Description = description;
            Type = type;
            StatValue = value;
            Price = price;
            IsEquipped = false;
            IsPurchased = false;
        }
        public void ShowMyitems(bool withIndex = false, int index = 0)
        {
            string equipMark = IsEquipped ? "[E]" : "   ";
            string typeStat;

            switch (Type)
            {
                case ItemType.Weapon:
                    typeStat = $"공격력 +{StatValue}";
                    break;
                case ItemType.Armor:
                    typeStat = $"방어력 +{StatValue}";
                    break;
                case ItemType.HpBoost:
                    typeStat = $"체력 +{StatValue}";
                    break;
                default:
                    typeStat = $"능력치 +{StatValue}";
                    break;
            }

            if (withIndex)
            {
                Console.WriteLine($"- {index + 1} {equipMark}{Name} | {typeStat} | {Description}");
            }
            else
            {
                Console.WriteLine($"- {equipMark}{Name} | {typeStat} | {Description}");
            }
        }
        public void ShowSellItem(bool withIndex = false, int index = 0)
        {
            string typeStat = Type switch
            {
                ItemType.Weapon => $"공격력 +{StatValue}",
                ItemType.Armor => $"방어력 +{StatValue}",
                ItemType.HpBoost => $"체력 +{StatValue}",
                _ => $"능력치 +{StatValue}"
            };

            if (withIndex)
            {
                Console.WriteLine($"- {index + 1} {Name} | {typeStat} | {Description} | {Price * 0.85f} G");
            }
            else
            {
                Console.WriteLine($"- {Name} | {typeStat} | {Description} | {Price * 0.85f} G");
            }
        }
        public void ShowShopItems(bool withIndex = false, int index = 0)
        {
            string priceDisplay = IsPurchased ? "구매완료" : $"{Price}G";
            string typeStat;
            switch (Type)
            {
                case ItemType.Weapon:
                    typeStat = $"공격력 +{StatValue}";
                    break;
                case ItemType.Armor:
                    typeStat = $"방어력 +{StatValue}";
                    break;
                case ItemType.HpBoost:
                    typeStat = $"체력 +{StatValue}";
                    break;
                default:
                    typeStat = $"능력치 +{StatValue}";
                    break;
            }

            if (withIndex)
            {
                Console.WriteLine($"- {index + 1} {Name} | {typeStat} | {Description} | {priceDisplay}");
            }
            else
            {
                Console.WriteLine($"- {Name} | {typeStat} | {Description} | {priceDisplay}");
            }
        }
    }
}
