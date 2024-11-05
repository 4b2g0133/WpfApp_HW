using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp_HW
{
    public partial class MainWindow : Window
    {
        // 定義飲品及其價格的字典
        Dictionary<string, int> drinks = new Dictionary<string, int>
        {
            { "紅茶大杯", 60 },
            { "紅茶小杯", 40 },
            { "綠茶大杯", 60 },
            { "綠茶小杯", 40 },
            { "可樂大杯", 50 },
            { "可樂小杯", 30 },
            { "咖啡大杯", 70 },
        };

        // 用於儲存訂單的字典
        Dictionary<string, int> orders = new Dictionary<string, int>();
        string takeout = ""; // 儲存取餐方式

        // MainWindow的建構函數
        public MainWindow()
        {
            InitializeComponent(); // 初始化視窗元件

            // 顯示飲品選單
            DisplayDrinkMenu(drinks);
        }

        // 顯示飲品選單的方法
        private void DisplayDrinkMenu(Dictionary<string, int> drinks)
        {
            stackpanel_DrinkMenu.Children.Clear(); // 清空飲品選單的內容
            stackpanel_DrinkMenu.Height = drinks.Count * 40; // 設定選單高度

            // 遍歷每一種飲品，並創建對應的UI元件
            foreach (var drink in drinks)
            {
                // 創建一個水平排列的StackPanel來顯示每個飲品
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal, // 水平排列
                    Margin = new Thickness(2), // 邊距
                    Height = 35, // 高度
                    VerticalAlignment = VerticalAlignment.Center, // 垂直置中
                    Background = Brushes.AliceBlue // 背景顏色
                };

                // 創建CheckBox顯示飲品名稱和價格
                var cb = new CheckBox
                {
                    Content = $"{drink.Key} {drink.Value}元", // 飲品名稱和價格
                    FontFamily = new FontFamily("微軟正黑體"), // 字體
                    FontSize = 18, // 字體大小
                    Foreground = Brushes.Blue, // 字體顏色
                    Margin = new Thickness(10, 0, 40, 0), // 邊距
                    VerticalContentAlignment = VerticalAlignment.Center // 垂直置中
                };

                // 創建Slider用於選擇數量
                var sl = new Slider
                {
                    Width = 150, // 寬度
                    Value = 0, // 初始值
                    Minimum = 0, // 最小值
                    Maximum = 10, // 最大值
                    IsSnapToTickEnabled = true, // 數值對齊
                    VerticalAlignment = VerticalAlignment.Center, // 垂直置中
                };

                // 創建Label顯示選擇的數量
                var lb = new Label
                {
                    Width = 30, // 寬度
                    Content = "0", // 初始顯示0
                    FontFamily = new FontFamily("微軟正黑體"), // 字體
                    FontSize = 18, // 字體大小
                };

                // 創建數據綁定，使Label顯示Slider的當前值
                Binding myBinding = new Binding("Value")
                {
                    Source = sl, // 綁定來源為Slider
                    Mode = BindingMode.OneWay // 單向綁定
                };
                lb.SetBinding(ContentProperty, myBinding); // 設置綁定

                // 將CheckBox、Slider和Label加入StackPanel
                sp.Children.Add(cb);
                sp.Children.Add(sl);
                sp.Children.Add(lb);

                // 將StackPanel加入飲品選單
                stackpanel_DrinkMenu.Children.Add(sp);
            }
        }

        // 處理取餐方式的RadioButton選中事件
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var rb = sender as RadioButton; // 獲取觸發事件的RadioButton
            if ((rb.IsChecked == true)) // 如果被選中
            {
                takeout = rb.Content.ToString(); // 獲取取餐方式
                //MessageBox.Show($"方式: {takeout}"); // 顯示取餐方式（可選）
            }
        }

        // 處理訂單按鈕的點擊事件
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = ""; // 清空結果文本框
            string discoutMessage = ""; // 儲存折扣訊息

            // 清空現有的訂單
            orders.Clear();
            // 確認所有訂單的品項
            for (int i = 0; i < stackpanel_DrinkMenu.Children.Count; i++)
            {
                var sp = stackpanel_DrinkMenu.Children[i] as StackPanel; // 取得每個飲品的StackPanel
                var cb = sp.Children[0] as CheckBox; // 取得CheckBox
                var sl = sp.Children[1] as Slider; // 取得Slider
                var lb = sp.Children[2] as Label; // 取得Label

                // 如果CheckBox被選中且Slider的值大於0
                if (cb.IsChecked == true && sl.Value > 0)
                {
                    string drinkName = cb.Content.ToString().Split(' ')[0]; // 獲取飲品名稱
                    orders.Add(drinkName, int.Parse(lb.Content.ToString())); // 將飲品和數量加入訂單
                }
            }

            // 顯示訂單細項，並計算總金額
            double total = 0.0; // 總金額
            double sellPrice = 0.0; // 最終應付金額

            ResultTextBlock.Text += $"取餐方式: {takeout}\n"; // 顯示取餐方式

            int num = 1; // 訂單編號
            // 遍歷訂單，計算每項的金額
            foreach (var item in orders)
            {
                string drinkName = item.Key; // 獲取飲品名稱
                int quantity = item.Value; // 獲取數量
                int price = drinks[drinkName]; // 獲取單價

                int subTotal = price * quantity; // 計算小計
                total += subTotal; // 累加總金額
                ResultTextBlock.Text += $"{num}. {drinkName} x {quantity}杯，共{subTotal}元\n"; // 顯示訂單細項
                num++;
            }

            // 根據總金額計算折扣
            if (total >= 500)
            {
                discoutMessage = "滿500元打8折"; // 8折
                sellPrice = total * 0.8; // 計算折後金額
            }
            else if (total >= 300)
            {
                discoutMessage = "滿300元打9折"; // 9折
                sellPrice = total * 0.9; // 計算折後金額
            }
            else
            {
                discoutMessage = "無折扣"; // 無折扣
                sellPrice = total; // 不變
            }

            // 顯示總金額及實付金額
            ResultTextBlock.Text += $"總金額: {total}元\n";
            ResultTextBlock.Text += $"{discoutMessage}，實付金額: {sellPrice}元\n";
        }
    }

}