using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using sabatex.BlazorHelper.Models;
using System;
using System.Net.Http.Json;


namespace sabatex.BlazorHelper;
public abstract class BaseGridPage<TItem,TKey>:ComponentBase where TItem: BaseReference<TKey>, new() 
{
    [Inject] protected DialogService dialogService{get;set;}
    [Inject] protected HttpClient Http { get; set; }
    [Inject] protected IGRUDAdapter GRUDAdapter { get; set; }

    protected RadzenDataGrid<TItem> DataGrid;
    protected TItem? ItemToInsertInGrid;
    protected IEnumerable<TItem> DataGridItems;
    protected IList<TItem> DataGridSelectedItems;

    protected async Task InsertRow()
    {
        ItemToInsertInGrid = new TItem();
        await DataGrid.InsertRow(ItemToInsertInGrid);
    }
    protected virtual async Task OnCreateRow(TItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        try
        {
            await GRUDAdapter.Post(item);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

    }
    protected async Task OnUpdateRow(TItem item)
    {
        resetItemToInsertInGrid(item);
        await GRUDAdapter.Update(item);
    }
    protected async Task EditRow(TItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        await DataGrid.EditRow(item);
    }
    protected async Task SaveRow(TItem item)
    {
        resetItemToInsertInGrid(item);
        await DataGrid.UpdateRow(item);
    }
    protected void CancelEdit(TItem item)
    {
        resetItemToInsertInGrid(item);
        DataGrid.CancelEditRow(item);

    }
    protected async Task DeleteRow(TItem item)
    {
        var dialogResult = await dialogService.Confirm("Ви впевнені?",
                                        "Видалення запису",
                                        new ConfirmOptions() { OkButtonText = "Так", CancelButtonText = "Ні" });
        if (dialogResult == false) return;


        resetItemToInsertInGrid(item);
        if (DataGridItems.Contains(item))
        {
            try
            {
                await Http.DeleteAsync($"api/{typeof(TItem).Name}/{item.Id}");
                await DataGrid.Reload();
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
        }
        else
        {
            DataGrid.CancelEditRow(item);
        }
    }
    void resetItemToInsertInGrid(TItem item)
    {
        if (item == ItemToInsertInGrid)
        {
            ItemToInsertInGrid = null;
        }
    }
}

