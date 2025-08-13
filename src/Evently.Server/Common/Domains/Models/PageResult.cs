namespace Evently.Server.Common.Domains.Models;

public sealed class PageResult<T> {
	public List<T> Items { get; init; } = [];
	public int TotalCount { get; init; }
}