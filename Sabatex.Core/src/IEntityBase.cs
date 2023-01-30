namespace Sabatex.Core
{
    /// <summary>
    /// Base entity methods for (api,ef)
    /// </summary>
    public interface IEntityBase
    {
        /// <summary>
        /// Get primary key as string (int,long,Guid,string)
        /// </summary>
        string KeyAsString();
    }
}