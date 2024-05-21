using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Shared;

namespace ConstructionSiteLibrary.Components.Categories
{
    public partial class SortCategories
    {
        [Parameter]
        public EventCallback OnSaveComplete { get; set; }

        [Parameter]
        public List<CategoryModel> Categories { get; set; } = [];

        private bool onSaving = false;


        private void OrderList((int oldIndex, int newIndex) indici)
        {
            // spezzo la tupla
            var (oldIndex, newIndex) = indici;

            var items = Categories;
            var itemToMove = items[oldIndex];
            items.RemoveAt(oldIndex);

            if (newIndex < items.Count)
            {
                items.Insert(newIndex, itemToMove);
            }
            else
            {
                items.Add(itemToMove);
            }

            OrderElements(items);
            Categories = Categories.OrderBy(x => x.Order).ToList();
            StateHasChanged();
        }

        private static void OrderElements(List<CategoryModel> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                lista[i].Order = i + 1;
            }
        }

        private async Task Save()
        {
            onSaving = true;
            var success = await CategoriesRepository.UpdateCategories(Categories);
            if (success)
            {
                await OnSaveComplete.InvokeAsync();
            }
            onSaving = false;
        }

    }
}
