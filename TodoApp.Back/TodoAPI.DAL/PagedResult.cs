using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoAPI.DAL
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; private set; }

        public int ItemsCount { get; private set; } = 1;

        public int SelectedPage { get; private set; } = 1;

        public int ItemsPerPage { get; private set; } = 1;

        public PagedResult(IEnumerable<T> items, int itemsCount, int selectedPage, int itemsPerPage)
        {
            Items = items;
            ItemsCount = itemsCount;
            SelectedPage = selectedPage;
            ItemsPerPage = itemsPerPage;
        }
    }
}
