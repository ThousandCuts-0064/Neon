interface Map<K, V> {
    getOrSet(key: K, factory: () => V): V;
}

Map.prototype.getOrSet = function <K, V>(this: Map<K, V>, key: K, factory: () => V): V {
    const value = this.get(key);

    if (value !== undefined)
        return value;

    const newValue = factory();

    this.set(key, newValue);

    return newValue;
};