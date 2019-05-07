using System;

namespace BRM.Blackboards.Interfaces
{
    /// <summary>
    /// Allows CRUD operations (create, read, update, delete) for global variables mapped with a key,value pairing
    /// Also provides notification for when values are updated
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
        /// Stores a callback "onUpdate", which will fire with the updated value when the "post" method is called
        /// DO NOT pass anonymous methods for "onUpdate", as these will likely causes persistent references to objects
        /// captured in the anonymous method closure and create memory leaks in your application
        /// If the type T changes for a given key, event processors subscribed to value updates will remain subscribed,
        /// but they will not receive the update events, as the types no longer match.
        /// Changing types for a given key is not advised, as this will likely cause unexpected behavior
        /// Null or empty keys will be ignored
        /// </summary>
        void Subscribe<T>(string key, Action<T> onUpdate);
        
        /// <summary>
        /// Clears the callback paired with the given key
        /// Null or empty keys will be ignored
        /// </summary>
        void Unsubscribe<T>(string key, Action<T> onUpdate);
    }
}