namespace Sabatex.Core;

/// <summary>
/// Base entity methods for (api,ef)
/// </summary>
public interface IEntityBase<TKey>
{
    /// <summary>
    /// Get primary key as string (int,long,Guid,string)
    /// </summary>
    TKey Id { get; set; }
    /// <summary>
    /// Returns a string representation of the object's key identifier.
    /// </summary>
    /// <returns>A string that represents the key identifier. Returns an empty string if the identifier is null.</returns>
    string ToKeyString()
    {
        return Id?.ToString() ?? "";
    }

}