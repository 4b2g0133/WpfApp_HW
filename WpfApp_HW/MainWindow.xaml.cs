using System.Windows;
using System.Windows.Controls;

namespace WpfApp_HW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // 建構函式，初始化 MainWindow
        public MainWindow()
        {
            InitializeComponent(); // 初始化 UI 元件
        }

        // 當 TextBox 文字變更時觸發的事件處理函式
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 將事件的發送者轉換成 TextBox 型別
            var targetTextBox = sender as TextBox;

            // 用於存放解析後的數量
            int amount;

            // 嘗試將 TextBox 的文字轉換為整數
            bool success = int.TryParse(targetTextBox.Text, out amount);

            // 如果解析失敗，顯示錯誤訊息
            if (!success)
            {
                MessageBox.Show("請輸入正整數", "輸入錯誤");
            }
            // 如果輸入數量小於等於 0，也顯示錯誤訊息
            else if (amount <= 0)
            {
                MessageBox.Show("請輸入正整數", "輸入錯誤");
            }
            else
            {
                // 取得 TextBox 所在的 StackPanel 容器
                var targetStackPanel = targetTextBox.Parent as StackPanel;

                // 取得 StackPanel 中的第一個子元素（假設是 Label）
                var targetLabel = targetStackPanel.Children[0] as Label;

                // 取得飲料名稱（假設 Label 的內容是飲料名稱）
                var drinkName = targetLabel.Content.ToString();

                // 將訂購結果顯示在 ResultTextBlock 上
                ResultTextBlock.Text += $"您選擇了{drinkName}，數量為{amount}杯\n";
            }
        }
    }
}