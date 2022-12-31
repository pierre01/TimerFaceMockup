using Plugin.Maui.Audio;

namespace TimerFaceMockup.Views;

public partial class MainPage : ContentPage
{
    public TimerFace TimerFace1 { get; set; }

    private IAudioPlayer _playerBeep;
    private IAudioPlayer _playerStart;

    PeriodicTimer _clock;
    public MainPage(IAudioManager audioManager)
	{
		InitializeComponent();
        ((TimerFace)myGraphicsView.Drawable).InitializeValues(maxTime, maxExercise); 
        myGraphicsView.Invalidate();

        this.audioManager = audioManager;
        

    }

    int time = 0;
    int exercise = 0;
    int maxExercise = 5;
    int maxTime = 10;
    private IAudioManager audioManager;

    private async void StartButtonClicked(object sender, EventArgs e)
	{
        if (_playerBeep == null)
        {
            _playerBeep = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("beep.wav"));
            _playerStart = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("start.wav"));
        }

        if ( _clock == null)
        {
            _clock = new PeriodicTimer(TimeSpan.FromSeconds(1));
        }
        else
        {
            _clock.Dispose();
            _clock = null;
            time = 0;
            exercise = 0;
            return;
        }
        _playerStart.Play();
        while (await _clock.WaitForNextTickAsync())
        {
            OnTick();
        }

        _playerBeep.Dispose();
        _playerBeep = null;
        _playerStart.Dispose();
        _playerStart = null;
    }

    public async void OnTick()
    {
        time++;

        if (time > maxTime)
        {
            exercise++;
            if (exercise > maxExercise)
            {
                exercise = 0;
            }
            time = 0;
        }
        ((TimerFace)myGraphicsView.Drawable).SetValues(time,exercise);
        myGraphicsView.Invalidate();
        var timeLeft = (maxTime - time).ToString();
        TimeLabel.Text = timeLeft;
        if (maxTime - time <= 5)
        {
            TimeLabel1.Opacity = 1;

            _playerBeep.Play();
            await Task.WhenAll
            (
                
                //TextToSpeech.Default.SpeakAsync(timeLeft),
                TimeLabel1.FadeTo(0, 600),
                TimeLabel1.ScaleTo(3, 600, Easing.CubicOut)
            ); 







            TimeLabel1.ScaleTo(1, 10);

        }
    }
}