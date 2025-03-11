using CalculatorV2.Data;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ToramFillCalculator.Helpers;
using ToramFillCalculator.Models;


namespace ToramFillCalculator.Forms
{
    public partial class MainForm : Form
    {
        private TextBox txtCustomerName;
        private TextBox txtMana, txtMetal, txtCloth, txtBeast, txtWood, txtMedicine;
        private TextBox txtManaPrice, txtMaterialPrice;
        private Button btnCalculate, btnReset, btnSaveOrder, btnViewOrders;
        private Label lblResult;
        private CalculationModel calculationModel;
        private OrderRepository orderRepository;

        public MainForm()
        {
            calculationModel = new CalculationModel();
            orderRepository = new OrderRepository();
            BuildUI();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void BuildUI()
        {
            this.Text = "Toram Fill Calculator";
            this.Size = new System.Drawing.Size(400, 450);

            // Customer Name field
            Label lblCustomerName = new Label { Text = "Customer Name:", Left = 10, Top = 10, Width = 120 };
            txtCustomerName = new TextBox { Left = 180, Top = 10, Width = 150 };

            Label lblManaPrice = new Label { Text = "Mana Price (per 1k):", Left = 10, Top = 40, Width = 120 };
            txtManaPrice = new TextBox { Left = 180, Top = 40, Width = 100, Text = "60000" };
            txtManaPrice.Leave += FormatNumber;
            AddIncrementDecrementButtons(txtManaPrice, 290, 40);

            Label lblMaterialPrice = new Label { Text = "Material Price (per 1k):", Left = 10, Top = 70, Width = 140 };
            txtMaterialPrice = new TextBox { Left = 180, Top = 70, Width = 100, Text = "10000" };
            txtMaterialPrice.Leave += FormatNumber;
            AddIncrementDecrementButtons(txtMaterialPrice, 290, 70);

            Label[] labels = { new Label { Text = "Mana:", Top = 100 }, new Label { Text = "Metal:", Top = 130 },
                               new Label { Text = "Cloth:", Top = 160 }, new Label { Text = "Beast:", Top = 190 },
                               new Label { Text = "Wood:", Top = 220 }, new Label { Text = "Medicine:", Top = 250 } };

            TextBox[] textBoxes = { txtMana = new TextBox(), txtMetal = new TextBox(), txtCloth = new TextBox(),
                                     txtBeast = new TextBox(), txtWood = new TextBox(), txtMedicine = new TextBox() };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Left = 10;
                labels[i].Width = 80;
                textBoxes[i].Left = 180;
                textBoxes[i].Width = 100;
                textBoxes[i].Text = "0";
                textBoxes[i].Leave += FormatNumber;
                labels[i].Top = textBoxes[i].Top = labels[i].Top;
                this.Controls.Add(labels[i]);
                this.Controls.Add(textBoxes[i]);
                AddIncrementDecrementButtons(textBoxes[i], 290, labels[i].Top);
            }

            btnCalculate = new Button { Text = "Calculate", Left = 10, Top = 290, Width = 100 };
            btnCalculate.Click += btnCalculate_Click;

            btnReset = new Button { Text = "Reset", Left = 120, Top = 290, Width = 80 };
            btnReset.Click += btnReset_Click;

            btnSaveOrder = new Button { Text = "Save Order", Left = 210, Top = 290, Width = 100 };
            btnSaveOrder.Click += btnSaveOrder_Click;

            btnViewOrders = new Button { Text = "View Orders", Left = 120, Top = 330, Width = 100 };
            btnViewOrders.Click += btnViewOrders_Click;

            lblResult = new Label { Text = "Total Spina: ", Left = 10, Top = 370, Width = 350 };

            this.Controls.Add(lblCustomerName);
            this.Controls.Add(txtCustomerName);
            this.Controls.Add(lblManaPrice);
            this.Controls.Add(lblMaterialPrice);
            this.Controls.Add(txtManaPrice);
            this.Controls.Add(txtMaterialPrice);
            this.Controls.Add(btnCalculate);
            this.Controls.Add(btnReset);
            this.Controls.Add(btnSaveOrder);
            this.Controls.Add(btnViewOrders);
            this.Controls.Add(lblResult);
        }

        private void AddIncrementDecrementButtons(TextBox textBox, int left, int top)
        {
            Button btnIncrease = new Button { Text = "+", Left = left, Top = top, Width = 25, Height = 25 };
            Button btnDecrease = new Button { Text = "-", Left = left + 30, Top = top, Width = 25, Height = 25 };

            btnIncrease.Click += (sender, e) => AdjustValue(textBox, 1000);
            btnDecrease.Click += (sender, e) => AdjustValue(textBox, -1000);

            this.Controls.Add(btnIncrease);
            this.Controls.Add(btnDecrease);
        }

        private void AdjustValue(TextBox textBox, int amount)
        {
            if (int.TryParse(textBox.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int value))
            {
                value = Math.Max(0, value + amount);
                textBox.Text = value.ToString("N0", CultureInfo.InvariantCulture);
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateModelFromForm();

                // Tính toán và hiển thị kết quả
                int totalSpina = calculationModel.CalculateTotalSpina();
                lblResult.Text = $"Total Spina: {totalSpina:N0}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid input! Please enter only numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra tên khách hàng
                if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
                {
                    MessageBox.Show("Please enter customer name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerName.Focus();
                    return;
                }

                UpdateModelFromForm();

                // Lưu đơn hàng vào database
                if (orderRepository.SaveOrder(calculationModel))
                {
                    MessageBox.Show("Order saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to save order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            OrdersForm ordersForm = new OrdersForm(orderRepository);
            ordersForm.ShowDialog();
        }

        private void UpdateModelFromForm()
        {
            // Lấy giá trị từ các TextBox
            string customerName = txtCustomerName.Text.Trim();
            int manaPrice = NumberFormatter.ParseNumber(txtManaPrice.Text);
            int materialPrice = NumberFormatter.ParseNumber(txtMaterialPrice.Text);
            int mana = NumberFormatter.ParseNumber(txtMana.Text);
            int metal = NumberFormatter.ParseNumber(txtMetal.Text);
            int cloth = NumberFormatter.ParseNumber(txtCloth.Text);
            int beast = NumberFormatter.ParseNumber(txtBeast.Text);
            int wood = NumberFormatter.ParseNumber(txtWood.Text);
            int medicine = NumberFormatter.ParseNumber(txtMedicine.Text);

            // Cập nhật model
            calculationModel.SetCustomerInfo(customerName);
            calculationModel.SetPrices(manaPrice, materialPrice);
            calculationModel.SetMaterials(mana, metal, cloth, beast, wood, medicine);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtCustomerName.Text = "";
            txtMana.Text = "0";
            txtMetal.Text = "0";
            txtCloth.Text = "0";
            txtBeast.Text = "0";
            txtWood.Text = "0";
            txtMedicine.Text = "0";
            txtManaPrice.Text = "60000";
            txtMaterialPrice.Text = "10000";
            lblResult.Text = "Total Spina: ";

            // Reset model
            calculationModel.Reset();
        }

        private void FormatNumber(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && int.TryParse(textBox.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int value))
            {
                textBox.Text = value.ToString("N0", CultureInfo.InvariantCulture);
            }
        }
    }
}