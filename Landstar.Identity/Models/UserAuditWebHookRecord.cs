// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 10-04-2024
//
// Last Modified By : 
// Last Modified On : 10-04-2024
// ***********************************************************************
// <copyright file="UserAuditWebHookRecord.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Landstar.Identity.Models
{
  /// <summary>
  /// Class UserAuditWebHookRecord.
  /// </summary>
  public class UserAuditWebHookRecord
  {
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the change data.
    /// </summary>
    /// <value>The change data.</value>
    public DateTime ChangeDate { get; set; }

    /// <summary>
    /// Gets or sets the user data.
    /// </summary>
    /// <value>The user data.</value>
    [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
    public string UserData { get; set; }

    /// <summary>
    /// Gets or sets the previous user data.
    /// </summary>
    /// <value>The previous user data.</value>
    [JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
    public string PreviousUserData { get; set; }

    /// <summary>
    /// Gets the delta.
    /// </summary>
    /// <value>The delta.</value>
    public Dictionary<string, (JToken OldValue, JToken NewValue)> Delta => CalculateDelta(UserData, PreviousUserData);

    /// <summary>
    /// Calculates the delta.
    /// </summary>
    /// <param name="oldJson">The old json.</param>
    /// <param name="newJson">The new json.</param>
    /// <returns>Dictionary&lt;System.String, System.ValueTuple&lt;JToken, JToken&gt;&gt;.</returns>
    static Dictionary<string, (JToken OldValue, JToken NewValue)> CalculateDelta(string oldJson, string newJson)
    {
      var oldObj = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(oldJson);
      var newObj = JsonConvert.DeserializeObject<Dictionary<string, JToken>>(newJson);

      var delta = new Dictionary<string, (JToken OldValue, JToken NewValue)>();

      // Find keys that are only in the old object or modified
      foreach (var oldKey in oldObj.Keys)
      {
        if (!newObj.TryGetValue(oldKey, out JToken value))
        {
          delta[oldKey] = (oldObj[oldKey], null);  // Key removed
        }
        else if (!JToken.DeepEquals(oldObj[oldKey], value))
        {
          delta[oldKey] = (oldObj[oldKey], value);  // Value changed
        }
      }

      // Find keys that are only in the new object
      foreach (var newKey in newObj.Keys)
      {
        if (!oldObj.ContainsKey(newKey))
        {
          delta[newKey] = (null, newObj[newKey]);  // Key added
        }
      }

      return delta;
    }
  }
}
