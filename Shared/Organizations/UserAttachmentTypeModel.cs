using System;
using System.Text.Json.Serialization;

namespace Shared.Organizations;

public class UserAttachmentTypeModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
}
