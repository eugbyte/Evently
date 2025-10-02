using JetBrains.Annotations;

namespace Evently.Server.Common.Domains.Models;

[UsedImplicitly]
public sealed record AccountDto(string Id, string Email, string Username, string Name);