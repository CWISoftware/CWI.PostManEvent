namespace CWI.PostManEvent.Common.Events
{
    public enum ResultEventState
    {
        Waiting = 0,
        Running = 1,
        Stoped = 2,
        Completed = 4,
        Failed = 8
    }
}
