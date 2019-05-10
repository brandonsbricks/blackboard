﻿namespace BRM.Blackboards.Interfaces
{
    /// <summary>
    /// Allows CRUD operations (create, read, update, delete) for global variables mapped with a key,value pairing
    /// </summary>
    public interface IBlackboard
    {
        /// <summary>
        /// adds or overwrites the given value to the paired key
        /// the value can be any type ie: reference, struct, or primitive
        /// All subscribers will receive an onUpdate callback with the new value
        /// If the type T changes for a given key, event processors subscribed to value updates will remain subscribed,
        /// but they will not receive the update events, as the types no longer match.
        /// Changing types for a given key is not advised, as this will likely cause unexpected behavior
        /// Null or empty keys will be ignored
        /// </summary>
        void Post<T>(string key, T value);
        
        /// <summary>
        /// returns the value paired to the given key
        /// the value can be any type ie: reference, struct, or primitive
        /// Null or empty keys will return a default(T) value
        /// </summary>
        T Get<T>(string key);
        
        /// <summary>
        /// Clears the key/value pair from the blackboard
        /// Null or empty keys will be ignored
        /// </summary>
        void Delete(string key);
        
        /// <summary>
        /// Clears all data in the blackboard
        /// </summary>
        void ClearData();
    }
}