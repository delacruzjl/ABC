namespace ABC.SharedKernel;

public record DailyContext
{
    public bool HadBreakfast { get; init; }
    public bool HadLunch { get; init; }
    public bool HadDinner { get; init; }
    public bool HadSnack { get; init; }
    public bool SleptWell { get; init; }
    public int? HoursOfSleep { get; init; }

    public DailyContext() { }

    public DailyContext(
        bool hadBreakfast,
        bool hadLunch,
        bool hadDinner,
        bool hadSnack,
        bool sleptWell,
        int? hoursOfSleep)
    {
        HadBreakfast = hadBreakfast;
        HadLunch = hadLunch;
        HadDinner = hadDinner;
        HadSnack = hadSnack;
        SleptWell = sleptWell;
        HoursOfSleep = hoursOfSleep;
    }
}
