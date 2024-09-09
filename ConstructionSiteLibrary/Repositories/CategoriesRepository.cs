using System.Text.Json;
using ConstructionSiteLibrary.Managers;
using Shared;
using Shared.ApiRouting;
using Shared.Defaults;
using Shared.Templates;


namespace ConstructionSiteLibrary.Repositories;

public class CategoriesRepository(HttpManager httpManager)
{
    List<TemplateCategoryModel> Categories = [];

    List<SubjectModel> Subjects = [];

    private HttpManager _httpManager = httpManager;

    #region  Categories

    public async Task<List<TemplateCategoryModel>> GetCategories()
    {

        try
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.CategoriesList, "");
            if (response.Code.Equals("0"))
            {
                Categories = JsonSerializer.Deserialize<List<TemplateCategoryModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }
        catch (Exception e)
        {
            var msg = e.Message;
        }


        return Categories;
    }

    public async Task<bool> SaveCategory(TemplateCategoryModel category)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.SaveCategory, category);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Categories.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateCategories(List<TemplateCategoryModel> categories)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateCategories, categories);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Categories.Clear();
            return true;
        }

        return false;
    }


    public async Task<bool> HideCategories(List<TemplateCategoryModel> categories)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.HideCategories, categories);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Categories.Clear();
            return true;
        }

        return false;
    }

    #endregion

}
