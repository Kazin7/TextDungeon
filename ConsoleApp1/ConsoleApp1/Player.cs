namespace ConsoleApp1
{
    [Serializable]
    public class Character //캐릭터 정보
    {
        public int Level { get; set; } = 1;
        public string Name { get; set; } = "hunterspin";
        public string Job { get; set; } = "전사";
        public float AtkDmg { get; set; } = 10;
        public int Def { get; set; } = 5;
        public int Hp { get; set; } = 100;
        public float Gold { get; set; } = 1500f;
        public int DungeonClear { get; set; } = 0;
        public List<Item> EquippedItems { get; set; } = new List<Item>();
        public void ShowStatus()
        {
            int bonusAtk = EquippedItems
                .Where(i => i.IsEquipped && i.Type == ItemType.Weapon)
                .Sum(i => i.StatValue);

            int bonusDef = EquippedItems
                .Where(i => i.IsEquipped && i.Type == ItemType.Armor)
                .Sum(i => i.StatValue);

            int bonusHp = EquippedItems
                .Where(i => i.IsEquipped && i.Type == ItemType.HpBoost)
                .Sum(i => i.StatValue);

            string atkStr = bonusAtk > 0 ? $" (+{bonusAtk})" : "";
            string defStr = bonusDef > 0 ? $" (+{bonusDef})" : "";
            string hpStr = bonusHp > 0 ? $" (+{bonusHp})" : "";

            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} ( {Job} )");
            Console.WriteLine($"공격력 : {AtkDmg + bonusAtk}{atkStr}");
            Console.WriteLine($"방어력 : {Def + bonusDef}{defStr}");
            Console.WriteLine($"체 력 : {Hp + bonusHp}{hpStr}");
            Console.WriteLine($"Gold : {Gold} G\n");
            Console.WriteLine("0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
        }
        public bool IsItemEquipped(ItemType type)
        {
            foreach (Item item in EquippedItems)
            {
                if (item.Type == type && item.IsEquipped)
                {
                    return true;
                }
            }
            return false;
        }
        public Item GetEquippedItem(ItemType type)
        {
            foreach (Item item in EquippedItems)
            {
                if (item.Type == type && item.IsEquipped)
                {
                    return item;
                }
            }
            return null;
        }
        public void LevelUp()
        {
            int required = 1;

            for (int i = 1; i < Level; i++)
            {
                required += i; // 누적 필요 클리어 수
            }
            Console.WriteLine("클리어 횟수" + DungeonClear);
            Console.WriteLine("필요 횟수" + required);
            if (DungeonClear >= required)
            {
                Level++;
                AtkDmg += 0.5f;
                Def++;
                Console.WriteLine($"\n레벨이 {Level - 1} → {Level}로 상승했습니다!\n");
                Console.WriteLine($"방어력이 {Def - 1} → {Def}로 상승했습니다!\n");
                Console.WriteLine($"공격력이 {AtkDmg - 0.5} → {AtkDmg}로 상승했습니다!\n");
                DungeonClear = 0;
            }
        }
    }
}