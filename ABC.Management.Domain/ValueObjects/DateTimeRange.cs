using FluentValidation.Results;

namespace ABC.Management.Domain.ValueObjects;

public record DateTimeRange
{
    public DateTime StartedAt { get; init; }
    public DateTime? EndedAt { get; init; }

    public DateTimeRange()
        : this(DateTime.UtcNow, null)
    {
        
    }

    public DateTimeRange(DateTime startedAt, DateTime? endedAt)
    {
        ValidateRange(startedAt, endedAt);
        StartedAt = startedAt;
        EndedAt = endedAt;
    }

    public TimeSpan Duration
    {
        get
        {
            if (EndedAt == null)
            {
                //return "Ongoing";
                return TimeSpan.Zero;
            }

            TimeSpan elapsedTime = EndedAt.Value - StartedAt;
            return elapsedTime;
            //return $"Duration: {elapsedTime.Days} days, {elapsedTime.Hours} hours, {elapsedTime.Minutes} minutes, {elapsedTime.Seconds} seconds.";
        }
    }


    public static implicit operator DateTimeRange(DateTime startedAt)
    {
        return new DateTimeRange(startedAt);
    }

    private void ValidateRange(DateTime startedAt, DateTime? endedAt)
    {
        if (endedAt == null)
        {
            return;
        }

        if (endedAt < startedAt)
        {
            throw new ValidationException(
                "Invalid date and time range", 
                [
                    new ValidationFailure(
                        nameof(EndedAt), "Ending cannot be greater than started date")
                ]);
        }
    }
}
