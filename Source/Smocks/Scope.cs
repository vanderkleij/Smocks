namespace Smocks
{
    /// <summary>
    /// The scope of Smocks: should it rewrite only direct references
    /// or rewrite any loaded assembly.
    /// </summary>
    public enum Scope
    {
        All,
        DirectReferences
    }
}