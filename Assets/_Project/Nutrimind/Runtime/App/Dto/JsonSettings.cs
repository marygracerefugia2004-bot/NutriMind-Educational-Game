using System;
using NutriMind.Runtime.App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NutriMind.Runtime.App.Dto
{
    /// <summary>
    /// Newtonsoft.Json converter that safely handles unknown enum values
    /// by falling back to a designated default value (typically "Unknown" = 0).
    /// This ensures that server responses with new/unrecognized enum members
    /// never crash the client.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to convert.</typeparam>
    public class SafeEnumConverter<TEnum> : JsonConverter where TEnum : struct
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(TEnum) || objectType == typeof(TEnum?);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (objectType == typeof(TEnum?))
                    return null;
                return default(TEnum);
            }

            if (reader.TokenType == JsonToken.Integer)
            {
                int intVal = Convert.ToInt32(reader.Value);
                if (Enum.IsDefined(typeof(TEnum), intVal))
                    return (TEnum)Enum.ToObject(typeof(TEnum), intVal);
                return default(TEnum);
            }

            if (reader.TokenType == JsonToken.String)
            {
                string strVal = reader.Value?.ToString();
                if (Enum.TryParse<TEnum>(strVal, ignoreCase: true, out var parsed))
                    return parsed;
                // Try matching on enum member names with underscore conversion
                // e.g., "true_false" -> TrueFalse, "multiple_choice" -> MultipleChoice
                string normalized = strVal?.Replace("_", "");
                if (normalized != null)
                {
                    foreach (TEnum member in Enum.GetValues(typeof(TEnum)))
                    {
                        if (string.Equals(member.ToString(), normalized, StringComparison.OrdinalIgnoreCase))
                            return member;
                    }
                }
                return default(TEnum);
            }

            return default(TEnum);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            TEnum enumVal = (TEnum)value;
            // Write as snake_case string for JSON contract compatibility
            string name = enumVal.ToString();
            string snakeCase = ToSnakeCase(name);
            writer.WriteValue(snakeCase);
        }

        private static string ToSnakeCase(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            var result = new System.Text.StringBuilder(name.Length + 4);
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsUpper(c))
                {
                    if (i > 0) result.Append('_');
                    result.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }

    /// <summary>
    /// Newtonsoft.Json contract resolver setting for safe deserialization.
    /// Missing required fields produce default values rather than crashes.
    /// Unknown fields are ignored. Unknown enum values fall back to default.
    /// </summary>
    public static class JsonSettings
    {
        /// <summary>
        /// Shared Newtonsoft.Json serializer settings for all provider DTO
        /// deserialization. Configured for contract safety:
        /// <list type="bullet">
        ///   <item>Missing members are ignored (not an error)</item>
        ///   <item>Null values are handled gracefully</item>
        ///   <item>Date parsing is lenient</item>
        ///   <item>Defaults are honored for absent optional fields</item>
        /// </list>
        /// </summary>
        public static JsonSerializerSettings SafeDefaults => new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            FloatParseHandling = FloatParseHandling.Double,
            Converters =
            {
                new SafeEnumConverter<ChallengeAnswerType>(),
                new SafeEnumConverter<ErrorAction>(),
                new SafeEnumConverter<LearningCyclePhase>(),
                new SafeEnumConverter<StationState>(),
                new SafeEnumConverter<SubjectType>(),
                new SafeEnumConverter<DataProviderMode>(),
                new SafeEnumConverter<AppState>()
            }
        };
    }
}