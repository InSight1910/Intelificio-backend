﻿using Backend.Common.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Backend.Features.Buildings.Commands.Delete
{
    public class DeleteBuildingCommand : IRequest<Result>
    {
        [JsonIgnore]
        public required int Id { get; set; }

    }
}