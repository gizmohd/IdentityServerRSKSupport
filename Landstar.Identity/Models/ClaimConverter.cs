// ***********************************************************************
// Assembly         : Landstar.Identity
// Author           : 
// Created          : 04-16-2024
//
// Last Modified By : 
// Last Modified On : 04-19-2024
// ***********************************************************************
// <copyright file="ClaimConverter.cs" company="Landstar.Identity">
//     Copyright (c) Landstar System, Inc.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace Landstar.Identity.Models;

/// <summary>
/// Class ClaimConverter.
/// Implements the <see cref="JsonConverter" />
/// </summary>
/// <seealso cref="JsonConverter" />
public class ClaimConverter : JsonConverter
{
  /// <inheritdoc />
  public override bool CanConvert(Type objectType)
  {
    return (objectType == typeof(System.Security.Claims.Claim));
  }

  /// <inheritdoc />
  public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
  {
    JObject jo = JObject.Load(reader);
    string type = (string)jo["Type"];
    string value = (string)jo["Value"];
  
    return new System.Security.Claims.Claim(type, value);
  }

  /// <inheritdoc />
  public override bool CanWrite
  {
    get { return false; }
  }

  /// <inheritdoc />
  public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
  {
    throw new NotSupportedException();
  }
}
