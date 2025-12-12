using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text;

namespace quan_ly_kho.DataBase.Mongodb
{
    public class DateTimeSerializer : IBsonDocumentSerializer
    {
        public Type ValueType => typeof(DateTime);

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var date = (DateTime)value;
            var milliseconds = new DateTimeOffset(date).ToUnixTimeMilliseconds();
            var bsonDate = new BsonDateTime(milliseconds);
            context.Writer.WriteDateTime(bsonDate.MillisecondsSinceEpoch);
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var ticks = context.Reader.ReadDateTime();
            var date = new DateTimeOffset(ticks, TimeSpan.Zero).UtcDateTime;
            return date;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value, IBsonSerializerRegistry serializerRegistry)
        {
            Serialize(context, args, value);
        }

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args, out bool wasDeserialized)
        {
            wasDeserialized = true;
            return Deserialize(context, args);
        }

        public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomSerializationProvider : IBsonSerializationProvider
    {
        private static readonly DecimalSerializer DecimalSerializer = new DecimalSerializer(BsonType.Decimal128);
        private static readonly NullableSerializer<decimal> NullableSerializer = new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128));

        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(decimal)) return DecimalSerializer;
            if (type == typeof(decimal?)) return NullableSerializer;

            return null; // falls back to Mongo defaults
        }
    }
}
