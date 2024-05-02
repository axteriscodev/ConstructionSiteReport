using System.Reflection.Metadata;

namespace Shared;

public class DocumentFactory
{
    public QuestionRepository QuestionRepository { get; set; } = new QuestionRepository();

    public List<QuestionModel> SelectedQuestion = [];

    public static DocumentModel CreateDocument()
    {
        return new DocumentModel();
    }
}
