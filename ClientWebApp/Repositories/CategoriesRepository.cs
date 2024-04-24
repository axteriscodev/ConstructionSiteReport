using System.Text.Json;
using ClientWebApp.Managers;
using Shared;


namespace ClientWebApp.Repositories;

public class CategoriesRepository(HttpManager httpManager)
{
    List<CategoryModel> Categories = [];

    List<SubjectModel> Subjects = [];

    private HttpManager _httpManager = httpManager;

    #region  Categories

    public async Task<List<CategoryModel>> GetCategories()
    {
        if (Categories.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest("Category/CategoriesList", "");
            if (response.Code.Equals("0"))
            {
                Categories = JsonSerializer.Deserialize<List<CategoryModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }

        return Categories;
    }

    public async Task<bool> SaveCategory(CategoryModel category)
    {
        var response = await _httpManager.SendHttpRequest("Category/SaveCategory", category);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Categories.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateCategory(List<CategoryModel> categories)
    {
        var response = await _httpManager.SendHttpRequest("Category/UpdateCategories", categories);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Categories.Clear();
            return true;
        }

        return false;
    }

    
    public async Task<bool> HideCategory(List<CategoryModel> categories)
    {
        var response = await _httpManager.SendHttpRequest("Category/HideCategories", categories);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Categories.Clear();
            return true;
        }

        return false;
    }

    #endregion

    #region Subjects

    public async Task<List<SubjectModel>> GetSubjects()
    {
        if (Subjects.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest("Category/SubjectsList", "");
            if (response.Code.Equals("0"))
            {
                Subjects = JsonSerializer.Deserialize<List<SubjectModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }

        return Subjects;
    }

    #endregion 

}
