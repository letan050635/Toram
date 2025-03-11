using System;
using System.Data;
using System.Windows.Forms;
using CalculatorV2.Data;

namespace ToramFillCalculator.Forms
{
    public partial class OrdersForm : Form
    {
        private DataGridView dgvOrders;
        private OrderRepository orderRepository;

        public OrdersForm(OrderRepository repository)
        {
            InitializeComponent();
            orderRepository = repository;
            SetupControls();
            LoadOrders();
        }

        private void SetupControls()
        {
            this.Text = "Order History";
            this.Size = new System.Drawing.Size(800, 500);
            this.StartPosition = FormStartPosition.CenterParent;

            dgvOrders = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            Button btnRefresh = new Button
            {
                Text = "Refresh",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnRefresh.Click += (sender, e) => LoadOrders();

            this.Controls.Add(dgvOrders);
            this.Controls.Add(btnRefresh);
        }

        private void LoadOrders()
        {
            DataTable orders = orderRepository.GetAllOrders();
            dgvOrders.DataSource = orders;

            // Đặt tên hiển thị cho các cột
            if (dgvOrders.Columns.Count > 0)
            {
                dgvOrders.Columns["OrderID"].HeaderText = "Order ID";
                dgvOrders.Columns["CustomerName"].HeaderText = "Customer Name";
                dgvOrders.Columns["OrderDate"].HeaderText = "Order Date";
                dgvOrders.Columns["ManaPrice"].HeaderText = "Mana Price";
                dgvOrders.Columns["MaterialPrice"].HeaderText = "Material Price";
                dgvOrders.Columns["Mana"].HeaderText = "Mana";
                dgvOrders.Columns["Metal"].HeaderText = "Metal";
                dgvOrders.Columns["Cloth"].HeaderText = "Cloth";
                dgvOrders.Columns["Beast"].HeaderText = "Beast";
                dgvOrders.Columns["Wood"].HeaderText = "Wood";
                dgvOrders.Columns["Medicine"].HeaderText = "Medicine";
                dgvOrders.Columns["TotalSpina"].HeaderText = "Total Spina";
            }
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {

        }
    }
}