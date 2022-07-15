using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerFaceMockup.Views;

public class TimerFace : IDrawable
{

    Color _trackColor = Color.FromArgb("#b4cccccc");
    Color _timeStrokeColor = Colors.Green;
    Color _repsStrokeColor = Colors.Orange;
    Color _exerciseStrokeColor = Colors.Red;
    int _exerciseTotalTime = 60;
    int _exerciseTotalRepetitions =5;
    int _exerciseTime = 0;
    int _exerciseRepetitions =0;
    public TimerFace()
    {

    }

    public TimerFace(int exerciseTotalTime, int exerciseTotalRepetitions, Color trackColor, Color timeStrokeColor, Color repsStrokeColor, Color exerciseStrokeColor)
    {
        _trackColor = trackColor;
        _timeStrokeColor = timeStrokeColor;
        _repsStrokeColor = repsStrokeColor;
        _exerciseStrokeColor = exerciseStrokeColor;
        _exerciseTotalTime = exerciseTotalTime;
        _exerciseTotalRepetitions = exerciseTotalRepetitions;   
    }


    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // Calculate Time to Angle
     
        var timeToAngle = ((_exerciseTime / (float)_exerciseTotalTime) * 360f)-89;
        var timeToAngleRad = (Math.PI*(-timeToAngle+90))/180;
        // Calculate Exercises to Angle
        var exercisesToAngle = ((_exerciseRepetitions / (float)_exerciseTotalRepetitions) * 360f)-89;
        var exercisesToAngleRad = (Math.PI * (-exercisesToAngle + 90)) / 180;


        canvas.SaveState();

        // Translation and scaling (200x200 pixel square with the origin in the center)
        canvas.Translate(dirtyRect.Center.X, dirtyRect.Center.Y);
        float scale = Math.Min(dirtyRect.Width / 200f, dirtyRect.Height / 200f);
        canvas.Scale(scale, scale);

        // Rectangle around the timer
        canvas.StrokeSize = 12;
        canvas.StrokeLineCap = LineCap.Square;

        float x, y;
        

        // Backgtound tracks
        canvas.StrokeColor = _trackColor;
        canvas.DrawArc(-90, -90, 180, 180, 90, 95, true, false);
        canvas.DrawArc(-70, -70, 140, 140, 90, 95, true, false);


        // Track 1
        canvas.StrokeColor = Colors.Green;
        canvas.DrawArc(-90, -90, 180, 180, -timeToAngle, 90, true, false);
        // Track 2
        canvas.StrokeColor = Colors.Orange;
        canvas.DrawArc(-70, -70, 140, 140, -exercisesToAngle, 90,  true, false);


        // Draw tips of graphs
        canvas.StrokeSize = 1;
        canvas.StrokeColor = Colors.White;

        canvas.FillColor = _exerciseStrokeColor;

        x = (float)(Math.Sin(exercisesToAngleRad) * 70);
        y = (float)(Math.Cos(exercisesToAngleRad) * 70);
        canvas.SetShadow(new SizeF(4, 4), 6, Colors.Black); // use sine and cosine  SizeF(-6+Math.Cos(exercisesToAngle)*6, -6+Math.Sin(exercisesToAngle)*6)
        canvas.FillCircle(x, y, 8);
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Black);
        canvas.DrawCircle(x, y, 8);



        canvas.FillColor = _timeStrokeColor; // Fill color cannot be null

        canvas.SetShadow(new SizeF(4,4), 6, Colors.Black);
        x = (float)(Math.Sin(timeToAngleRad) * 90f);
        y = (float)(Math.Cos(timeToAngleRad) * 90f);
        canvas.FillCircle(x, y, 8);
        canvas.SetShadow(new SizeF(0, 0), 0, Colors.Black);
        canvas.DrawCircle(x, y, 8);

        canvas.RestoreState();

    }
    public void DrawClock(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();

        canvas.StrokeLineCap = LineCap.Round;
        canvas.FillColor = Colors.Gray;

        // Translation and scaling
        canvas.Translate(dirtyRect.Center.X, dirtyRect.Center.Y);
        float scale = Math.Min(dirtyRect.Width / 200f, dirtyRect.Height / 200f);
        canvas.Scale(scale, scale);

        // Hour and minute marks
        for (int angle = 0; angle < 360; angle += 6)
        {
            canvas.FillCircle(0, -90, angle % 30 == 0 ? 4 : 2);
            canvas.Rotate(6);
        }

        DateTime now = DateTime.Now;

        canvas.StrokeColor = Colors.White;

        // Hour hand
        canvas.StrokeSize = 20;
        canvas.SaveState();
        canvas.Rotate(30 * now.Hour + now.Minute / 2f);
        canvas.DrawLine(0, 0, 0, -50);
        canvas.RestoreState();

        // Minute hand
        canvas.StrokeSize = 10;
        canvas.SaveState();
        canvas.Rotate(6 * now.Minute + now.Second / 10f);
        canvas.DrawLine(0, 0, 0, -70);
        canvas.RestoreState();

        // Second hand
        canvas.StrokeSize = 2;
        canvas.SaveState();
        canvas.Rotate(6 * now.Second);
        canvas.DrawLine(0, 10, 0, -80);
        canvas.RestoreState();

        canvas.RestoreState();
    }

    /// <summary>
    /// Set the timer Values
    /// </summary>
    /// <param name="exerciseTime">time left in the exercise in seconds (the Arc represent totalTime - exerciseTime)
    /// <seealso cref="_exerciseTotalTime"/>
    /// </param>
    /// <param name="exerciseRepetitions">Number of times the exercise left to repeat</param>
    public void SetValues(int exerciseTime, int exerciseRepetitions)
    {
        _exerciseTime = exerciseTime;
        _exerciseRepetitions = exerciseRepetitions;
    }

    public void InitializeValues(int exerciseTotalTime, int exerciseTotalRepetitions, Color trackColor=null, Color timeStrokeColor=null, Color repsStrokeColor=null, Color exerciseStrokeColor=null)
    {
        _trackColor = trackColor==null?_trackColor:trackColor;
        _timeStrokeColor = timeStrokeColor == null ?_timeStrokeColor:timeStrokeColor;
        _repsStrokeColor = repsStrokeColor == null ?_repsStrokeColor:repsStrokeColor;
        _exerciseStrokeColor = exerciseStrokeColor == null ?_exerciseStrokeColor:exerciseStrokeColor;
        _exerciseTotalTime = exerciseTotalTime;
        _exerciseTotalRepetitions = exerciseTotalRepetitions;
    }
}
