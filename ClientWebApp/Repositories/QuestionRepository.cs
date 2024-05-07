using ClientWebApp.Managers;
using Shared;
using System.Text.Json;
namespace ClientWebApp;

public class QuestionRepository(HttpManager httpManager)
{
    public List<QuestionModel> Questions { get; set; } = [];
    public List<ChoiceModel> Choices { get; set; } = [];

    private HttpManager _httpManager = httpManager;


    #region Questions

    public async Task<List<QuestionModel>> GetQuestions()
    {
        if(Questions.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest("Question/QuestionsList", "");
            if (response.Code.Equals("0"))
            {
                Questions = JsonSerializer.Deserialize<List<QuestionModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }
        return Questions;
    }

    public async Task<bool> SaveQuestion(QuestionModel question)
    {
        var response = await _httpManager.SendHttpRequest("Question/SaveQuestion", question);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Questions.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateQuestions(List<QuestionModel> questions)
    {
        var response = await _httpManager.SendHttpRequest("Question/UpdateQuestions", questions);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Questions.Clear();
            return true;
        }

        return false;
    }


    public async Task<bool> HideCategories(List<QuestionModel> questions)
    {
        var response = await _httpManager.SendHttpRequest("Question/HideQuestions", questions);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Questions.Clear();
            return true;
        }

        return false;
    }

    #endregion

    #region Choices

    public async Task<List<ChoiceModel>> GetChoices()
    {
        if (Choices.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest("Question/ChoicesList", "");
            if (response.Code.Equals("0"))
            {
                Choices = JsonSerializer.Deserialize<List<ChoiceModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }
        return Choices;
    }

    public async Task<bool> SaveChoice(ChoiceModel choice)
    {
        var response = await _httpManager.SendHttpRequest("Question/SaveChoice", choice);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Choices.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateChoices(List<ChoiceModel> choices)
    {
        var response = await _httpManager.SendHttpRequest("Question/UpdateChoices", choices);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Choices.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> HideCategories(List<ChoiceModel> choices)
    {
        var response = await _httpManager.SendHttpRequest("Question/HideChoices", choices);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Choices.Clear();
            return true;
        }

        return false;
    }

    #endregion
}
