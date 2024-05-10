
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Order
    {
        [Key]
        [Required(ErrorMessage = "OrderId is required.")]
        public int OrderId { get; set; }

        [JsonIgnore]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "OrderStatus is required.")]
        public string OrderStatus { get; set; } = null!;


        [Required(ErrorMessage = "Payment method is required.")]
        [JsonConverter(typeof(CustomJsonConverter))] // Apply CustomJsonConverter to the Payment property
        public JsonElement Payment { get; set; }

        public int? UserId { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
    // CustomJsonConverter class for handling JsonElement conversion
    public class CustomJsonConverter : JsonConverter<JsonElement>
    {
        // these write and read methods will invokes automatically
        public override JsonElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement custom deserialization logic if needed
            return JsonSerializer.Deserialize<JsonElement>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, JsonElement value, JsonSerializerOptions options)
        {
            // Implement custom serialization logic if needed
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
