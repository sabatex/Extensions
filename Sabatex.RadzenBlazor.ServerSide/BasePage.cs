using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace sabatex.RadzenBlazor.ServerSide;
public abstract class BasePage<T,TIdentityDBContext>:ComponentBase where T:class,new() where TIdentityDBContext: DbContext
{
    [Inject] protected TIdentityDBContext dbContext { get; set; } = default!;
    [Inject] protected DialogService dialogService{get;set;} = default!;


    protected RadzenDataGrid<T>? DataGrid;
    protected T? ItemToInsertInGrid;
    protected IEnumerable<T> DataGridItems;
    protected IList<T> DataGridSelectedItems;

    protected async Task InsertRow()
    {
        ItemToInsertInGrid = new T();
        await DataGrid.InsertRow(ItemToInsertInGrid);
    }
    protected virtual async Task OnCreateRow(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();
    }
    protected async Task OnUpdateRow(T item)
    {
        resetItemToInsertInGrid(item);
        dbContext.Update(item);
        await dbContext.SaveChangesAsync();
    }
    protected async Task EditRow(T item)
    {
        await DataGrid.EditRow(item);
    }
    protected async Task SaveRow(T item)
    {
        resetItemToInsertInGrid(item);
        await DataGrid.UpdateRow(item);
    }
    protected void CancelEdit(T item)
    {
        resetItemToInsertInGrid(item);
        DataGrid.CancelEditRow(item);

        // For production
        var orderEntry = dbContext.Entry(item);
        if (orderEntry.State == EntityState.Modified)
        {
            orderEntry.CurrentValues.SetValues(orderEntry.OriginalValues);
            orderEntry.State = EntityState.Unchanged;
        }
    }
    protected async Task DeleteRow(T item)
    {
        var dialogResult = await dialogService.Confirm("Ви впевнені?",
                                        "Видалення запису",
                                        new ConfirmOptions() { OkButtonText = "Так", CancelButtonText = "Ні" });
        if (dialogResult == false) return;


        resetItemToInsertInGrid(item);
        if (DataGridItems.Contains(item))
        {
            dbContext.Remove(item);
            await dbContext.SaveChangesAsync();
            await DataGrid.Reload();
        }
        else
        {
            DataGrid.CancelEditRow(item);
        }
    }

    void resetItemToInsertInGrid(T item)
    {
        if (item == ItemToInsertInGrid)
        {
            ItemToInsertInGrid = null;
        }
    }

}

