
namespace VioletClockV._1;

public partial class MainPage : ContentPage
{
    private IDispatcherTimer? _timer;

    public MainPage()
    {
        InitializeComponent();
        ClockView.Drawable = new ClockDrawable();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ClockView.Invalidate();

        if (_timer == null)
        {
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);

            _timer.Tick += (_, _) =>
            {
                ClockView.Invalidate();
            };
        }

        if (!_timer.IsRunning)
        {
            _timer.Start();
        }
    }

    protected override void OnDisappearing()
    {
        if (_timer != null && _timer.IsRunning)
        {
            _timer.Stop();
        }

        base.OnDisappearing();
    }
}
