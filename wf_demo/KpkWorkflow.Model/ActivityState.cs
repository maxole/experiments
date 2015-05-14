namespace KpkWorkflow.Model
{
    public enum ActivityState
    {
        None,
        Next,
        Prev,
        Abort,
        Error,
        Skip,
        Repeat,
        Cancel
    }
}
