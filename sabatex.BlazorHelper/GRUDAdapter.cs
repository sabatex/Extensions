using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Radzen;
using sabatex.BlazorHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace sabatex.BlazorHelper;

public class GRUDAdapter : IGRUDAdapter
{
    private HttpClient Http;
    public GRUDAdapter(HttpClient http)
    {
        this.Http = http;
    }
    public async Task Delete<TItem,TKey>(TItem item) where TItem : BaseReference<TKey>
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        if (Http == null)
            throw new ArgumentNullException(nameof(Http));
        try
        {
            var result = await Http.DeleteAsync($"api/{typeof(TItem).Name}/{item.Id}");
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
    }

    public async Task<RequestData<T>> Get<T>(LoadDataArgs dataArgs) where T : class
    {
        if (Http == null)
            throw new Exception("");    

        var query = $"api/{typeof(T).Name}?top={dataArgs.Top}&skip={dataArgs.Skip}";
        if (!string.IsNullOrEmpty(dataArgs.Filter)) query+= $"&filter={dataArgs.Filter}";
        if (!string.IsNullOrEmpty(dataArgs.OrderBy)) query += $"&orderby={dataArgs.OrderBy}";
        
        try
        {
            var result =  await Http.GetFromJsonAsync<RequestData<T>>(query);
            if (result == null)
                throw new Exception();
            return result;
        }
        catch(Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task Post<T>(T item) where T : class
    {
        await Http.PostAsJsonAsync($"api/{typeof(T).Name}", item);
    }

    public async Task<TItem> Update<TItem>(TItem item) where TItem : class
    {
        try
        {
            var result = await Http.PutAsJsonAsync($"api/{typeof(TItem).Name}", item);
            return await result.Content.ReadFromJsonAsync<TItem>();
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        return null;
    }


}
