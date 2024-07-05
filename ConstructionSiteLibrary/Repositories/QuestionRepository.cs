using ConstructionSiteLibrary.Managers;
using Shared.ApiRouting;
using Shared.Defaults;
using Shared.Templates;
using System.Text.Json;

namespace ConstructionSiteLibrary.Repositories;

public class QuestionRepository(HttpManager httpManager)
{
    public List<TemplateQuestionModel> Questions { get; set; } = [];
    public List<TemplateChoiceModel> Choices { get; set; } = [];

    private HttpManager _httpManager = httpManager;


    #region Questions

    public async Task<List<TemplateQuestionModel>> GetQuestions()
    {
        if(Questions.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.QuestionsList, "");
            if (response.Code.Equals("0"))
            {
                Questions = JsonSerializer.Deserialize<List<TemplateQuestionModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }
        return Questions;
    }

    public async Task<bool> SaveQuestion(QuestionModel question)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.SaveQuestion, question);

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
        var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateQuestions, questions);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Questions.Clear();
            return true;
        }

        return false;
    }


    public async Task<bool> HideQuestions(List<QuestionModel> questions)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.HideQuestions, questions);
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

    public async Task<List<TemplateChoiceModel>> GetChoices()
    {
        if (Choices.Count == 0)
        {
            var response = await _httpManager.SendHttpRequest(ApiRouting.ChoicesList, "");
            if (response.Code.Equals("0"))
            {
                Choices = JsonSerializer.Deserialize<List<TemplateChoiceModel>>(response.Content.ToString() ?? "") ?? [];
            }
        }
        return Choices;
    }

    public async Task<bool> SaveChoice(TemplateChoiceModel choice)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.SaveChoice, choice);

        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Choices.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> UpdateChoices(List<TemplateChoiceModel> choices)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.UpdateChoices, choices);
        //NotificationService.Notify(response);
        if (response.Code.Equals("0"))
        {
            Choices.Clear();
            return true;
        }

        return false;
    }

    public async Task<bool> HideChoices(List<TemplateChoiceModel> choices)
    {
        var response = await _httpManager.SendHttpRequest(ApiRouting.HideChoices, choices);
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
