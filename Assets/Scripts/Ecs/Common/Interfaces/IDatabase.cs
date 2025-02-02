using System.Collections.Generic;

public interface IDatabase<TKey, TValue> {
	IEnumerable<TValue> Values { get; }
	void Add(TKey key, TValue value);
	void Remove(TKey key);
	public TValue Get(TKey key);
	public bool Has(TKey key);
}