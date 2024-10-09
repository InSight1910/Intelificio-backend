﻿using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.CommonSpaces.Commands.Update;

public class UpdateCommonSpaceCommand : IRequest<Result>
{
    [JsonIgnore] public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsInMaintenance { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    [JsonIgnore] public int CommunityId { get; set; }
}