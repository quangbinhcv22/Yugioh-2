using System;

[Serializable]
public class TurnTimer
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsCounting { get; set; }
    public Action OnStar;
    public Action OnStop;

    public void Start(DateTime start, int time)
    {
        StartTime = start;
        EndTime = start + TimeSpan.FromSeconds(time);

        IsCounting = true;
        OnStar?.Invoke();
    }

    public void Stop()
    {
        IsCounting = false;
        OnStop?.Invoke();
    }

    public TimeSpan Remaining => EndTime - DateTime.Now;
}