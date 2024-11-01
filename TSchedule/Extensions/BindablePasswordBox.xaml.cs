using System.Windows;
using System.Windows.Controls;

namespace TSchedule.Extensions;

public partial class BindablePasswordBox : UserControl
{
    public BindablePasswordBox()
    {
        InitializeComponent();
    }

    // Зависимое свойство для двусторонней привязки пароля
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindablePasswordChanged));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    // Свойство для заголовка
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(string), typeof(BindablePasswordBox), new PropertyMetadata(string.Empty));

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    // Свойство для подсказки
    public static readonly DependencyProperty PlaceholderTextProperty =
        DependencyProperty.Register("PlaceholderText", typeof(string), typeof(BindablePasswordBox), new PropertyMetadata(string.Empty));

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    // Свойство для радиуса углов
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(BindablePasswordBox), new PropertyMetadata(new CornerRadius(0)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    // Свойство для режима показа пароля
    public static readonly DependencyProperty PasswordRevealModeProperty =
        DependencyProperty.Register("PasswordRevealMode", typeof(bool), typeof(BindablePasswordBox), new PropertyMetadata(false));

    public bool PasswordRevealMode
    {
        get => (bool)GetValue(PasswordRevealModeProperty);
        set => SetValue(PasswordRevealModeProperty, value);
    }

    // Свойство для контекстного меню
    public static readonly DependencyProperty UsingTextContextMenuProperty =
        DependencyProperty.Register("UsingTextContextMenu", typeof(bool), typeof(BindablePasswordBox), new PropertyMetadata(false));

    public bool UsingTextContextMenu
    {
        get => (bool)GetValue(UsingTextContextMenuProperty);
        set => SetValue(UsingTextContextMenuProperty, value);
    }

    // Обработчик изменения свойства BindablePassword
    private static void OnBindablePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindablePasswordBox passwordBox)
        {
            passwordBox.passwordBox.PasswordChanged -= passwordBox.PasswordBox_PasswordChanged;

            if (e.NewValue is string newPassword && passwordBox.passwordBox.Password != newPassword)
            {
                passwordBox.passwordBox.Password = newPassword;
            }

            passwordBox.passwordBox.PasswordChanged += passwordBox.PasswordBox_PasswordChanged;
        }
    }

    // Синхронизация BindablePassword при изменении пароля
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = passwordBox.Password;
    }
}
