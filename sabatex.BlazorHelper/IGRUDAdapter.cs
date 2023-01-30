using Radzen;
using sabatex.BlazorHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sabatex.BlazorHelper;

public interface IGRUDAdapter
{
    Task<RequestData<T>> Get<T>(LoadDataArgs dataArgs) where T : class;
    Task Post<T>(T item) where T : class;
    Task<T> Update<T>(T item) where T : class;
    Task Delete<TItem,TKey>(TItem item) where TItem : BaseReference<TKey>;
}
