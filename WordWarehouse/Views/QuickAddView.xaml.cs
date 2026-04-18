using System.Windows.Controls;

namespace WordWarehouse.Views;

public partial class QuickAddView : UserControl
{
    public QuickAddView()
    {
        InitializeComponent();
        Loaded += (_, _) => ContentTextBox.Focus();
    }
}
