using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetAlarmPage : ContentPage
    {
        public SetAlarmPage()
        {
            InitializeComponent();
            GetContent();
        }

        public DateTime AlarmTime { get; set; }
        public DateTime AlarmDate { get; set; }    
        /// <summary>
        /// Метод для установки будильника
        /// </summary>
        /// 
        public void GetContent()
        {
            // Создаем виджет выбора даты
            var datePicker = new DatePicker
            {
                Format = "D",
                // Диапазон дат: +/- неделя
                MaximumDate = DateTime.Now.AddDays(7),
                MinimumDate = DateTime.Now.AddDays(-7),
            };


            var datePickerText = new Label { Text = "Дата сигнала ", Margin = new Thickness(0, 20, 0, 0) };

            // Добавляем всё на страницу
            //stackLayout.Children.Add(new Label { Text = "Устройство" });
            //stackLayout.Children.Add(new Entry { BackgroundColor = Color.AliceBlue, Text = "Холодильник" });
            stackLayout.Children.Add(datePickerText);
            stackLayout.Children.Add(datePicker);

            // Виджет выбора времени.
            var timePickerText = new Label { Text = "Время сигнала ", Margin = new Thickness(0, 20, 0, 0) };
            var timePicker = new TimePicker
            {
                Time = new TimeSpan(13, 0, 0)
            };

            stackLayout.Children.Add(timePickerText);
            stackLayout.Children.Add(timePicker);

            //
            Slider slider = new Slider
            {
                Minimum = 0,
                Maximum = 100,
                Value = 60,
                ThumbColor = Color.DodgerBlue,
                MinimumTrackColor = Color.DodgerBlue,
                MaximumTrackColor = Color.Gray
            };
            var sliderText = new Label { Text = $"Громкость: {slider.Value} %", HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 30, 0, 0) };
            stackLayout.Children.Add(sliderText);
            stackLayout.Children.Add(slider);            


            // Создаем заголовок для переключателя
            var switchHeader = new Label { Text = "Не повторять", HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 5, 0, 0) };
            stackLayout.Children.Add(switchHeader);

            // Создаем переключатель
            Switch switchControl = new Switch
            {
                IsToggled = false,
                HorizontalOptions = LayoutOptions.Center,
                ThumbColor = Color.DodgerBlue,
                OnColor = Color.LightSteelBlue,
            };
            stackLayout.Children.Add(switchControl);

            // Регистрируем обработчик события переключения
            switchControl.Toggled += (sender, e) => SwitchHandler(sender, e, switchHeader);


            Button saveAlarmBtn = new Button
            {
                Text = "Сохранить",
                //FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button)),
                //BorderWidth = 1,
                //HorizontalOptions = LayoutOptions.Center,
                //VerticalOptions = LayoutOptions.CenterAndExpand
                BackgroundColor = Color.Silver,
                Margin = new Thickness(0, 5, 0, 0)
            };
            saveAlarmBtn.Clicked += SaveAlarm;
            stackLayout.Children.Add(saveAlarmBtn);

            ////
            //AlarmModel alarm = new AlarmModel();
            //alarm.Date = datePicker.DateSelected;



            //stackLayout.Children.Add(new Button { Text = "Сохранить", BackgroundColor = Color.Silver, Margin = new Thickness(0, 5, 0, 0) }.Clicked += (sender, e) => SaveAlarm(sender, e, datePickerText));

            // Регистрируем обработчик события выбора даты
            datePicker.DateSelected += (sender, e) => DateSelectedHandler(sender, e, datePickerText);
            // Регистрируем обработчик события выбора времени
            timePicker.PropertyChanged += (sender, e) => TimeChangedHandler(sender, e, timePickerText, timePicker);
            // stepper.ValueChanged += (sender, e) => TempChangedHandler(sender, e, stepperText);
            slider.ValueChanged += (sender, e) => VolumeChangedHandler(sender, e, sliderText);

        }

        public void DateSelectedHandler(object sender, DateChangedEventArgs e, Label datePickerText)
        {
            // При срабатывании выбора - будет меняться информационное сообщение.
            datePickerText.Text = "Сигнал сработает " + e.NewDate.ToString("dd/MM/yyyy");
            AlarmDate = e.NewDate;


        }

        public void TimeChangedHandler(object sender, PropertyChangedEventArgs e, Label timePickerText, TimePicker timePicker)
        {
            // Обновляем текст сообщения, когда появляется новое значение времени
            if (e.PropertyName == "Time")
                timePickerText.Text = "В " + timePicker.Time;

            AlarmDate = AlarmDate.Date + timePicker.Time;
        }

        

        /// <summary>
        /// Обработчик изменения громкости
        /// </summary>
        private void VolumeChangedHandler(object sender, ValueChangedEventArgs e, Label header)
        {
            header.Text = String.Format("Громкость: {0:F1}%", e.NewValue);
        }

        /// <summary>
        /// Обработка переключателя
        /// </summary>
        public void SwitchHandler(object sender, ToggledEventArgs e, Label header)
        {
            if (!e.Value)
            {
                header.Text = "Не повторять";
                return;
            }

            header.Text = "Повторять";
        }

        //
        /// <summary>
        /// По хорошему надо было сделать привязку к модели и сохранять настройки в модель. а потом в бд
        /// может быть позже.
        /// </summary>
        public void SaveAlarm(object sender, EventArgs e)
        {
            // Создаем новую табличную разметку
            var layout = new StackLayout();

            layout.VerticalOptions = LayoutOptions.Center;
            // Добавляем всё на страницу
            layout.Children.Add(new Label { Text = "Будильник сработает: " + AlarmDate.ToShortDateString() + " " + AlarmDate.ToShortTimeString(), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center });

            // Инициализация свойства Content созданным табличным лейаутом идентична тому, как если бы мы создавали его в XAML и разместили внутри ContentPage.
            this.Content = layout;
        }
    }
}