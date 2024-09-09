using System;
using System.Text.Json.Serialization;

namespace Shared.Organizations;

public class UserAttachmentModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("type")]
    public UserAttachmentTypeModel Type { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; } = "";
}
