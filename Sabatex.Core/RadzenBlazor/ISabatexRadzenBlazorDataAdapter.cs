using Sabatex.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabatex.Core.RadzenBlazor;

namespace Sabatex.Core.RadzenBlazor;
/// <summary>
/// Defines an abstraction for performing asynchronous CRUD and query operations on entities using OData-compatible
/// parameters in a Radzen Blazor application.
/// </summary>
/// <remarks>This interface provides methods for retrieving, creating, updating, and deleting entities, as well as
/// querying collections with support for filtering, sorting, paging, and expansion of related data. Implementations are
/// expected to handle OData query expressions and return results in a format suitable for data-bound UI components. All
/// operations are asynchronous and return tasks that complete when the underlying data operation finishes.</remarks>
public interface ISabatexRadzenBlazorDataAdapter
{
    /// <summary>
    /// Asynchronously retrieves a collection of items that match the specified query parameters.
    /// </summary>
    /// <remarks>This method supports OData query options for flexible data retrieval. The actual support for
    /// specific query options may depend on the underlying data source or implementation.</remarks>
    /// <typeparam name="TItem">The type of the items to retrieve. Must implement IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the key for the items.</typeparam>
    /// <param name="filter">An OData filter expression to restrict the results. Can be null to return all items.</param>
    /// <param name="orderby">An OData orderby expression that determines the sort order of the results. Can be null for default ordering.</param>
    /// <param name="expand">A comma-separated list of related entities to include in the results. Can be null to exclude related entities.</param>
    /// <param name="top">The maximum number of items to return. If null, the default or server-defined limit is used. Must be
    /// non-negative if specified.</param>
    /// <param name="skip">The number of items to skip before returning results. If null, no items are skipped. Must be non-negative if
    /// specified.</param>
    /// <param name="count">A value indicating whether to include the total count of matching items in the result. If <see
    /// langword="true"/>, the count is included; otherwise, it is omitted.</param>
    /// <param name="format">The media type to use for the response format. Can be null to use the default format.</param>
    /// <param name="select">A comma-separated list of properties to include in the results. Can be null to include all properties.</param>
    /// <param name="apply">An OData apply expression to perform server-side transformations such as grouping or aggregation. Can be null if
    /// no transformation is needed.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a QueryResult&lt;TItem&gt; with the
    /// retrieved items and, if requested, the total count.</returns>
    Task<QueryResult<TItem>> GetAsync<TItem,TKey>(string? filter, string? orderby, string? expand, int? top, int? skip, bool? count, string? format=null, string? select=null, string? apply = null) where TItem : class,IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously retrieves a collection of items that match the specified query parameters.
    /// </summary>
    /// <typeparam name="TItem">The type of the items to retrieve. Must implement the IEntityBase&lt;TKey&gt; interface.</typeparam>
    /// <typeparam name="TKey">The type of the key for the items.</typeparam>
    /// <param name="queryParams">The parameters that define filtering, sorting, and paging options for the query. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a QueryResult&lt;TItem&gt; with the items
    /// that match the query and related metadata.</returns>
    Task<QueryResult<TItem>> GetAsync<TItem, TKey>(QueryParams queryParams) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <typeparam name="TItem">The type of the entity to retrieve. Must implement IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <param name="expand">An optional comma-separated list of related entities to include in the result. If null, only the main entity is
    /// retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise,
    /// null.</returns>
    Task<TItem?> GetByIdAsync<TItem, TKey>(TKey id, string? expand=null) where TItem: class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <typeparam name="TItem">The type of the entity to retrieve. Must implement IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the unique identifier for the entity.</typeparam>
    /// <param name="id">The unique identifier of the entity to retrieve. Cannot be null or empty.</param>
    /// <param name="expand">An optional comma-separated list of related entities to include in the result. If null, only the main entity is
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise,
    /// null.</returns>
    Task<TItem?> GetByIdAsync<TItem, TKey>(string id, string? expand = null) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously posts the specified item and returns the result of the validation operation.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to post. Must implement the IEntityBase&lt;TKey&gt; interface.</typeparam>
    /// <typeparam name="TKey">The type of the key for the item.</typeparam>
    /// <param name="item">The item to be posted. Can be null if the operation supports null values.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SabatexValidationModel&lt;TItem&gt; with
    /// the validation outcome and the posted item.</returns>
    Task<SabatexValidationModel<TItem>> PostAsync<TItem, TKey>(TItem? item) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously updates the specified item in the data store and returns the result of the validation operation.
    /// </summary>
    /// <typeparam name="TItem">The type of the item to update. Must be a reference type that implements IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the key for the item.</typeparam>
    /// <param name="item">The item to update. Cannot be null. The item's identifier is used to locate the existing record in the data
    /// store.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a SabatexValidationModel&lt;TItem&gt; with
    /// the updated item and validation results.</returns>
    Task<SabatexValidationModel<TItem>> UpdateAsync<TItem, TKey>(TItem item) where TItem : class, IEntityBase<TKey>;
    /// <summary>
    /// Asynchronously deletes the entity of the specified type with the given identifier.
    /// </summary>
    /// <typeparam name="TItem">The type of the entity to delete. Must implement IEntityBase&lt;TKey&gt;.</typeparam>
    /// <typeparam name="TKey">The type of the identifier for the entity.</typeparam>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync<TItem, TKey>(TKey id) where TItem : class, IEntityBase<TKey>;
    
}
/// <summary>
/// Represents a descriptor for a field, including its name, type, operation, priority, and optional value. 
/// </summary>
/// <param name="Name">The name of the field being described. Cannot be null.</param>
/// <param name="FieldType">The data type of the field. Cannot be null.</param>
/// <param name="operation">The logical operation associated with the field. Defaults to " or ".</param>
/// <param name="priority">The priority of the field in processing or evaluation. Higher values indicate higher priority. Defaults to 0.</param>
/// <param name="value">An optional value associated with the field. Can be null.</param>
public record struct  FieldDescriptor(string Name,Type FieldType,string operation=" or ",int priority = 0,string? value=null);
/// <summary>
/// Represents the result of a validation operation, including the validated item and any associated validation errors.
/// </summary>
/// <typeparam name="TItem">The type of the item being validated.</typeparam>
/// <param name="Result">The validated item, or null if validation failed or no result is available.</param>
/// <param name="Errors">A dictionary containing validation errors, where each key is a property name and the value is a list of error
/// messages for that property. Can be null if there are no errors.</param>
public record SabatexValidationModel<TItem>(TItem? Result, Dictionary<string, List<string>>? Errors=null);
/// <summary>
/// Represents errors that occur during object deserialization.
/// </summary>
/// <remarks>Use this exception to indicate that an object could not be deserialized due to invalid data, format
/// issues, or other deserialization failures. This exception is typically thrown by custom deserialization routines
/// when the input cannot be converted to the expected object type.</remarks>
public class DeserializeException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DeserializeException class with a default error message indicating a
    /// deserialization failure.
    /// </summary>
    public DeserializeException() : base("Deserialize object error.") { }
}