public class NoteData
{
    public int Id { get; }
    public float TimeAppear { get; }
    public float Duration { get; }
    public int LaneIndex { get; }

    public bool IsLongNote => Duration > 0.5f;

    public NoteData(int id, float timeAppear, float duration, int laneIndex)
    {
        Id = id;
        TimeAppear = timeAppear;
        Duration = duration;
        LaneIndex = laneIndex;
    }
}