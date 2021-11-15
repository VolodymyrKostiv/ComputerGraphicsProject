using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Studying_app_kg.Views
{
    /// <summary>
    /// Interaction logic for Instructions.xaml
    /// </summary>
    public partial class Instructions : Page
    {
        public Instructions(int senderId=1)
        {
            InitializeComponent();
           switch (senderId)
                {
                    case 1: Button_OnClick(Button1, new RoutedEventArgs()); break;
                    case 2: Button_OnClick(Button2, new RoutedEventArgs()); break;
                    case 3: Button_OnClick(Button3, new RoutedEventArgs()); break;
                    case 4: Button_OnClick(Button4, new RoutedEventArgs()); break;
            }
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MainPage());
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonName = ((Button) sender).Name;
            SetButtonStyle(sender);
            if (buttonName == "Button1")
            {
                OutputData.Text = "\tРобота в програмі відбувається за допомогою мишки та клавіатури. " +
                                  "На головній сторінці є можливість перейти на сторінки трьох навчальних розділів, а також  розділу з навчальними " +
                                  "матеріалами, натиснувши на відповідну кнопку. На сторінках навчальних матеріалів можна управляти функціоналом розділу" +
                                  "а також перейти на відповідні сторінки в навчальних матеріалах або інструкції або ж повернутись на головну сторінку\n";
                Hyperlink h = new Hyperlink();
                h.Inlines.Add("Відео про цей розділ \n");
                h.NavigateUri = new Uri("https://youtu.be/7x8oV2V2PQA");
                h.RequestNavigate += HandleLinkClick;
                OutputData.Inlines.Add(h);
            }
            else if (buttonName == "Button2")
            {
                OutputData.Text =
                    "\tВ цьому розділі доступна робота з двома типами фракталів: \"Плазма\" та \"Броунівський рух\" переключання між ними " +
                    "доступне за допомогою натиску на переключателі в правій частині екрану.\n" +
                    "     Для фракталу плазма можна ввести кількість ітерацій. вона впливає на те, наскільки буде промальоване вихідне зображення. чим це" +
                    "число більше, тим більша частина зображення буде зафарбована.\n" +
                    "     Для фракталу броунівський рух потрібно вказати початкову точку симуляції (координати x та y, які  рахуються починаючи з лівого " +
                    "верхнього кута), швидкість руху (математичне сподівання для нормального розподілу, яке відповідає за відстань " +
                    "зміщення точки під час руху) та кількість ітерацій - скільки разів буде симулюватись рух для точки\n";
                Hyperlink h = new Hyperlink();
                h.Inlines.Add("Фрактал \"Плазма\" \n");
                h.NavigateUri = new Uri("https://youtu.be/-b9YN70LK38.html");
                h.RequestNavigate += HandleLinkClick;
                OutputData.Inlines.Add(h);
                
                h = new Hyperlink();
                h.Inlines.Add("Фрактал \"Броунівський рух\"  \n");
                h.NavigateUri = new Uri("https://youtu.be/-b9YN70LK38.html");
                h.RequestNavigate += HandleLinkClick;
                OutputData.Inlines.Add(h);
            }
            else if (buttonName == "Button3")
            {
                OutputData.Text =
                    "\tВ цьому розділі доступна робота з двома типами колірних моделей: HSL та CMYK. Спочатку потрібно натиснути на кнопку" +
                    "\"Upload\" та вибрати зображення, з яким можна потім працювати." +
                    " В правій частині екрану відобразяться 2 зображення: завантажене (Before), а також зображення (After) в вибраній колірній моделі (вибір відбувається" +
                    "в лівій честині екрану перемикачем). Нижче пермикача розташовано полосу для зміни яскравості жовтого кольору в зображенні (Yellow saturation)." +
                    "Результат відображається в правій частині екрану. Для перегляду координат кольору потрібно навести курсором на потрібну позицію в правому зображенню. " +
                    "В лівій частині екрану відобразяться координати кольору в вибраній колірній схемі. " +
                    "Якщо навести курсор на позицію в лівому зображенні, то відобразяться координати кольору в колірній моделі RGB" +
                    "Щоб зберегти перетворене треба натиснути на кнопку \"Download\" та вибрати назву файлу та його розсташування.\n";
            }
            else if (buttonName == "Button4")
            {
                OutputData.Text =
                    "\tВ цьому розділі доступний рух трикутника, що заданий певним афінним перетворенням. Для роботи потрібно задати координати вершин трикутника," +
                    " зміщення по траекторії y=x під час руху а також кількість ітерацій - повторів руху трикутника. Для масштабування зображення необхідно користуватися " +
                    "колесом мишки над ділянкою з зображенням (системою координат на виведеним на ній трикутником). Для скидання масштабу до початкового занчення " +
                    "потрібно натиснути на кнопку \"скинути масштабування\". Для збереження результату в вигляді зображення слід натиснути на кнопку \"Зберегти зображення\".\n";
                Hyperlink h = new Hyperlink();
                h.Inlines.Add("Рух трикутника з використанням афінних перетворень \n");
                h.NavigateUri = new Uri("https://youtu.be/flJuYcatKIs");
                h.RequestNavigate += HandleLinkClick;
                OutputData.Inlines.Add(h);
            }
        }
        public void HandleLinkClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;
            string navigateUri = hl.NavigateUri.ToString();
            System.Diagnostics.Process.Start(new ProcessStartInfo(navigateUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void SetButtonStyle(object sender)
        {
            var buttonName = ((Button)sender).Name;
            var bc = new BrushConverter();
            var brush = "#DBCA9A";
            ((Button)sender).Background = (Brush)bc.ConvertFrom("#A89A85");
            if (buttonName == "Button1")
            {
                Button2.Background = (Brush)bc.ConvertFrom(brush);
                Button3.Background = (Brush)bc.ConvertFrom(brush);
                Button4.Background = (Brush)bc.ConvertFrom(brush);
            }
            else if (buttonName == "Button2")
            {
                Button1.Background = (Brush)bc.ConvertFrom(brush);
                Button3.Background = (Brush)bc.ConvertFrom(brush);
                Button4.Background = (Brush)bc.ConvertFrom(brush);
            }
            else if (buttonName == "Button3")
            {
                Button1.Background = (Brush)bc.ConvertFrom(brush);
                Button2.Background = (Brush)bc.ConvertFrom(brush);
                Button4.Background = (Brush)bc.ConvertFrom(brush);
            }
            else if (buttonName == "Button4")
            {
                Button1.Background = (Brush)bc.ConvertFrom(brush);
                Button2.Background = (Brush)bc.ConvertFrom(brush);
                Button3.Background = (Brush)bc.ConvertFrom(brush);
            }
        }
        private void UserGuideButton_OnClick(object sender, RoutedEventArgs e)
        {

            NavigationService?.Navigate(new UserGuidePage());
        }
    }
}
