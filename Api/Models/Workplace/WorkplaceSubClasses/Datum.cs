using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Datum
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("max")]
        public dynamic Max { get; set; }

        [JsonPropertyName("status")]
        public long Status { get; set; }

        [JsonPropertyName("price")]
        public dynamic Price { get; set; }

        [JsonPropertyName("updated_by")]
        public long UpdatedBy { get; set; }

        [JsonPropertyName("created_by")]
        public long CreatedBy { get; set; }

        [JsonPropertyName("deleted_by")]
        public dynamic DeletedBy { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonPropertyName("deleted_at")]
        public dynamic DeletedAt { get; set; }

        [JsonPropertyName("extra")]
        public Extra Extra { get; set; }

        [JsonPropertyName("type")]
        public dynamic Type { get; set; }

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; }

        [JsonPropertyName("resource_id")]
        public long ResourceId { get; set; }

        [JsonPropertyName("building_id")]
        public long BuildingId { get; set; }

        [JsonPropertyName("user_specific_price")]
        public dynamic UserSpecificPrice { get; set; }

        [JsonPropertyName("user_specific_vat")]
        public dynamic UserSpecificVat { get; set; }

        [JsonPropertyName("canteen_id")]
        public long CanteenId { get; set; }

        [JsonPropertyName("categories")]
        public List<dynamic> Categories { get; set; }

        [JsonPropertyName("food_allergen_labels")]
        public List<dynamic> FoodAllergenLabels { get; set; }

        [JsonPropertyName("vats")]
        public List<dynamic> Vats { get; set; }

        [JsonPropertyName("images")]
        public List<Image> Images { get; set; }
    }
}