using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Api.Models.Workplace.WorkplaceSubClasses
{
    public class Datum
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("max")]
        public dynamic Max { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("price")]
        public dynamic Price { get; set; }

        [JsonProperty("updated_by")]
        public long UpdatedBy { get; set; }

        [JsonProperty("created_by")]
        public long CreatedBy { get; set; }

        [JsonProperty("deleted_by")]
        public dynamic DeletedBy { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("deleted_at")]
        public dynamic DeletedAt { get; set; }

        [JsonProperty("extra")]
        public Extra Extra { get; set; }

        [JsonProperty("type")]
        public dynamic Type { get; set; }

        [JsonProperty("resource_type")]
        public string ResourceType { get; set; }

        [JsonProperty("resource_id")]
        public long ResourceId { get; set; }

        [JsonProperty("building_id")]
        public long BuildingId { get; set; }

        [JsonProperty("user_specific_price")]
        public dynamic UserSpecificPrice { get; set; }

        [JsonProperty("user_specific_vat")]
        public dynamic UserSpecificVat { get; set; }

        [JsonProperty("canteen_id")]
        public long CanteenId { get; set; }

        [JsonProperty("categories")]
        public List<dynamic> Categories { get; set; }

        [JsonProperty("food_allergen_labels")]
        public List<dynamic> FoodAllergenLabels { get; set; }

        [JsonProperty("vats")]
        public List<dynamic> Vats { get; set; }

        [JsonProperty("images")]
        public List<Image> Images { get; set; }
    }
}