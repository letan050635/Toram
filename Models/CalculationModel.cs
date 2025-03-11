using System;

namespace ToramFillCalculator.Models
{
    public class CalculationModel
    {
        // Thông tin đơn hàng
        public string CustomerName { get; private set; }
        public DateTime OrderDate { get; private set; }

        // Giá
        public int ManaPrice { get; private set; }
        public int MaterialPrice { get; private set; }

        // Nguyên liệu
        public int Mana { get; private set; }
        public int Metal { get; private set; }
        public int Cloth { get; private set; }
        public int Beast { get; private set; }
        public int Wood { get; private set; }
        public int Medicine { get; private set; }

        public CalculationModel()
        {
            Reset();
        }

        public void SetCustomerInfo(string customerName)
        {
            CustomerName = customerName;
            OrderDate = DateTime.Now;
        }

        public void SetPrices(int manaPrice, int materialPrice)
        {
            ManaPrice = manaPrice;
            MaterialPrice = materialPrice;
        }

        public void SetMaterials(int mana, int metal, int cloth, int beast, int wood, int medicine)
        {
            Mana = mana;
            Metal = metal;
            Cloth = cloth;
            Beast = beast;
            Wood = wood;
            Medicine = medicine;
        }

        public int CalculateTotalSpina()
        {
            int totalMaterialsSpina = ((Metal + Cloth + Beast + Wood + Medicine) * MaterialPrice) / 1000;
            int totalManaSpina = (Mana * ManaPrice) / 1000;
            return totalManaSpina + totalMaterialsSpina;
        }

        public void Reset()
        {
            CustomerName = string.Empty;
            OrderDate = DateTime.Now;
            ManaPrice = 60000;
            MaterialPrice = 10000;
            Mana = 0;
            Metal = 0;
            Cloth = 0;
            Beast = 0;
            Wood = 0;
            Medicine = 0;
        }
    }
}