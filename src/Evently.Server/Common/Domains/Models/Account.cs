using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Common.Domains.Models;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public sealed record Account(string IdentityUserId, string Email, string UserName);