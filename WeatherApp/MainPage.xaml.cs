using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        public const string BUTTON_TEXT = "Get weather";
        public MainPage()
        {
            InitializeComponent();


        }

        private async void SetAlarmModalPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SetAlarmPage());
        }

        /// <summary>
        /// Метод для отображения погоды по клику
        /// </summary>
        private void LoadWeather(object sender, EventArgs e)
        {
            // Создаем новую табличную разметку
            var layout = new Grid();
            // Задаем чёрный фон
            layout.BackgroundColor = Color.Black;

            // Создаем цветной прямоугольник, который будет фоном для текста
            var upperBox = new BoxView { BackgroundColor = Color.Bisque };
            // Генерим заголовок и выравниваем с помощью свойств.
            var upperHeader = new Label() { Text = $"{Environment.NewLine}Inside", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Start, FontSize = 45, TextColor = Color.FromRgb(48, 48, 48) };
            // Генерим непосредственно текст со значениями температуры и тоже выравниваем.
            var upperText = new Label() { Text = $"{Environment.NewLine}+ 26 °C  ", HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, FontSize = 105, TextColor = Color.FromRgb(48, 48, 48) };
            // Добавляем все элементы в одну ячейку табличной разметки Grid. В результате они будут помещены "один поверх другого", и прямоугольник будет фоном для текста
            layout.Children.Add(upperBox, 0, 0);
            layout.Children.Add(upperHeader, 0, 0);
            layout.Children.Add(upperText, 0, 0);

            // Аналогично заполняем средний блок
            var middleBox = new BoxView { BackgroundColor = Color.LightBlue };
            var middleHeader = new Label() { Text = $"{Environment.NewLine} Outside", HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Start, FontSize = 45, TextColor = Color.FromRgb(48, 48, 48) };
            var middleText = new Label() { Text = $"{Environment.NewLine}- 15 °C  ", HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, FontSize = 105, TextColor = Color.FromRgb(48, 48, 48) };
            layout.Children.Add(middleBox, 0, 1);
            layout.Children.Add(middleHeader, 0, 1);
            layout.Children.Add(middleText, 0, 1);

            // Аналогично заполняем нижний блок
            var bottomBox = new BoxView { BackgroundColor = Color.DarkCyan };
            var bottomHeader = new Label() { Text = $"{Environment.NewLine} Pressure", HorizontalTextAlignment = TextAlignment.Center, FontSize = 45, TextColor = Color.FromRgb(48, 48, 48) };
            var bottomText = new Label() { Text = $"{Environment.NewLine}760 mm ", HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, FontSize = 100, TextColor = Color.FromRgb(48, 48, 48) };
            layout.Children.Add(bottomBox, 0, 2);
            layout.Children.Add(bottomHeader, 0, 2);
            layout.Children.Add(bottomText, 0, 2);

            // Инициализация свойства Content созданным табличным лейаутом идентична тому, как если бы мы создавали его в XAML и разместили внутри ContentPage.
            this.Content = layout;
        }

        /// <summary>
        /// Метод для установки будильника
        /// </summary>
        private void SetAlarm(object sender, EventArgs eventArgs)
        {
            // Основной контейнер компоновки
            var layout = new StackLayout() { Margin = new Thickness(20) };

            // Заголовок
            var header = new Label { Text = "Установить будильник", Margin = new Thickness(0, 20, 0, 0), FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };
            layout.Children.Add(header);

            // Виджет выбора даты с описанием
            var datePickerText = new Label { Text = "Дата запуска", Margin = new Thickness(0, 20, 0, 0) };
            layout.Children.Add(datePickerText);
            var datePicker = new DatePicker
            {
                Format = "D",
                MaximumDate = DateTime.Now.AddDays(7),
                MinimumDate = DateTime.Now.AddDays(-7),
            };
            layout.Children.Add(datePicker);

            // Виджет выбора времени с описанием
            var timePickerText = new Label { Text = "Время запуска ", Margin = new Thickness(0, 20, 0, 0) };
            layout.Children.Add(timePickerText);
            var timePicker = new TimePicker
            {
                Time = new TimeSpan(13, 0, 0)
            };
            layout.Children.Add(timePicker);

            // Переключатель громкости с описанием
            Slider slider = new Slider
            {
                Minimum = 0,
                Maximum = 30,
                Value = 5.0,
                ThumbColor = Color.DodgerBlue,
                MinimumTrackColor = Color.DodgerBlue,
                MaximumTrackColor = Color.Gray
            };
            var sliderText = new Label { Text = $"Громкость: {slider.Value}", HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 30, 0, 0) };
            // Регистрируем обработчик события изменения громкости
            slider.ValueChanged += (s, e) => VolumeHandler(s, e, sliderText);
            layout.Children.Add(sliderText);
            layout.Children.Add(slider);

            // Переключатель и заголовок для него
            var switchHeader = new Label { Text = "Повторять каждый день", HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 5, 0, 0) };
            layout.Children.Add(switchHeader);
            Switch switchControl = new Switch
            {
                IsToggled = false,
                HorizontalOptions = LayoutOptions.Center,
                ThumbColor = Color.DodgerBlue,
                OnColor = Color.LightSteelBlue,
            };
            layout.Children.Add(switchControl);

            // Кнопка сохранения и обработчик для неё
            var saveAlarmButton = new Button { Text = "Сохранить", BackgroundColor = Color.Silver, Margin = new Thickness(0, 5, 0, 0) };
            saveAlarmButton.Clicked += (s, e) => SaveAlarmHandler(s, e, datePicker.Date + timePicker.Time);
            layout.Children.Add(saveAlarmButton);

            // Инициализация леаута
            this.Content = layout;
        }

        /// <summary>
        /// Обработчик события изменения громкости
        /// </summary>
        private void VolumeHandler(object sender, ValueChangedEventArgs e, Label header)
        {
            header.Text = String.Format("Громкость: {0:F1}", e.NewValue);
        }

        /// <summary>
        /// Обработчик сохранения будильника
        /// </summary>
        void SaveAlarmHandler(object sender, EventArgs e, DateTime alarmDate)
        {
            var layout = new StackLayout() { Margin = new Thickness(20), VerticalOptions = LayoutOptions.Center };
            var dateHeader = new Label { Text = $"Будильник сработает:", FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };
            var dateText = new Label { Text = $"{alarmDate.Day}.{alarmDate.Month} в {alarmDate.Hour}:{alarmDate.Minute}", FontSize = 20, HorizontalTextAlignment = TextAlignment.Center };

            layout.Children.Add(dateHeader);
            layout.Children.Add(dateText);
            this.Content = layout;
        }

    }
}
