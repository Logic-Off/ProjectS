using System;
using System.Collections.Generic;
using System.Diagnostics;

public abstract class ADatabase<TKey, TValue> : IDisposable {
	private readonly Dictionary<TKey, TValue> _dictionary = new();

	public IEnumerable<TValue> Values => _dictionary.Values;

	public void Add(TKey key, TValue value) => _dictionary.Add(key, value);
	public void Remove(TKey key) => _dictionary.Remove(key);

	public TValue Get(TKey key) {
		if (_dictionary.TryGetValue(key, out var value))
			return value;

		KeyNotFound(key);
		return default;
	}

	public bool Has(TKey key) => _dictionary.ContainsKey(key);

	public void Dispose() => _dictionary.Clear();

	[Conditional("DEBUG")]
	private void KeyNotFound(TKey key) => D.Error("[ADatabase]", $"Key '{key}' not found, database '{GetType().Name}'");
}