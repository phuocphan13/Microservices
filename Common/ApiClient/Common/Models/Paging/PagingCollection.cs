using System.Collections;

namespace ApiClient.Common.Models.Paging;

public class PagingCollection<T> : IEnumerable<T>
{
    public IEnumerable<T> Items { get; }

    public PagingCollection(IEnumerable<T> items)
    {
        this.Items = items;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.Items.GetEnumerator();
}