using ConstructionSiteLibrary.Model;
using ConstructionSiteLibrary.Repositories;
using Microsoft.AspNetCore.Components;
using Shared.Defaults;
using Shared.Templates;

namespace ConstructionSiteLibrary.Components.Categories
{
    public partial class SortCategories
    {
        [Parameter]
        public EventCallback OnSaveComplete { get; set; }

        [Parameter]
        public List<TemplateCategoryModel> Categories { get; set; } = [];

        private bool onSaving = false;


        private void OrderList(ChangeObjectIndex indici)
        {

            var items = Categories;
            var itemToMove = items[indici.OldIndex];
            items.RemoveAt(indici.OldIndex);

            if (indici.NewIndex < items.Count)
            {
                items.Insert(indici.NewIndex, itemToMove);
            }
            else
            {
                items.Add(itemToMove);
            }

            OrderElements(items);
            Categories = Categories.OrderBy(x => x.Order).ToList();
            StateHasChanged();
        }

        private static void OrderElements(List<TemplateCategoryModel> lista)
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
