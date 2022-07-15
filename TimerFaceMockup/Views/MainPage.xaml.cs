namespace TimerFaceMockup.Views;

public partial class MainPage : ContentPage
{
    public TimerFace TimerFace1 { get; set; }

    PeriodicTimer _clock;
    public MainPage()
	{
		InitializeComponent();
        ((TimerFace)myGraphicsView.Drawable).InitializeValues(maxTime, maxExercise); 
        myGraphicsView.Invalidate();
        
        

    }

    int time = 0;
    int exercise = 0;
    int maxExercise = 5;
    int maxTime = 10;
    private async void StartButtonClicked(object sender, EventArgs e)
	{
        if( _clock ==null)
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

        while(await _clock.WaitForNextTickAsync())
        {
            OnTick();
        }
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
            TextToSpeech.Default.SpeakAsync(timeLeft); 
            TimeLabel1.Opacity = 1;
            await Task.WhenAll
            (
                TimeLabel1.FadeTo(0, 600),
                TimeLabel1.ScaleTo(3, 600, Easing.CubicOut)
            );
            TimeLabel1.ScaleTo(1, 10);

        }
    }
}