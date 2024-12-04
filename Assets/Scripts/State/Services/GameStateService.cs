﻿using System;
using Game.State.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Services
{
    public class GameStateDataService
    {
        private const string Key = "quicksave";
        private readonly JsonSerializerSettings _settings;

        public GameStateDataService()
        {
            _settings = new JsonSerializerSettings();
            _settings.Converters.Add(new Vector3Converter());
            _settings.Converters.Add(new Vector2Converter());
            _settings.Converters.Add(new QuaternionConverter());
        }

        public void Save(StateData data)
        {
            var jsonString = JsonConvert.SerializeObject(data, _settings);
            PlayerPrefs.SetString(Key, jsonString);
            PlayerPrefs.Save();
        }

        public StateData Load()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                var jsonString = PlayerPrefs.GetString(Key);
                return JsonConvert.DeserializeObject<StateData>(jsonString, _settings);
            }

            return null;
        }
    }

    public class Vector3Converter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteValue(value.z);
            writer.WriteEndArray();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var x = array[0].Value<float>();
            var y = array[1].Value<float>();
            var z = array[2].Value<float>();
            return new Vector3(x, y, z);
        }
    }

    public class Vector2Converter : JsonConverter<Vector2>
    {
        public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteEndArray();
        }

        public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var x = array[0].Value<float>();
            var y = array[1].Value<float>();
            return new Vector2(x, y);
        }
    }

    public class QuaternionConverter : JsonConverter<Quaternion>
    {
        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.x);
            writer.WriteValue(value.y);
            writer.WriteValue(value.z);
            writer.WriteValue(value.w);
            writer.WriteEndArray();
        }

        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var x = array[0].Value<float>();
            var y = array[1].Value<float>();
            var z = array[1].Value<float>();
            var w = array[1].Value<float>();
            return new Quaternion(x, y, z, w);
        }
    }
}