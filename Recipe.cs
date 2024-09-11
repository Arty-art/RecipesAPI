using Chilkat;
using Newtonsoft.Json;

namespace GetRecipesAPI
{
    public class RecipeAPI
    {
        public class Recipe
        {
            public class ApiResponse
            {
                [JsonProperty("content")]
                public required Content Content { get; set; }

                [JsonProperty("isSuccess")]
                public bool? IsSuccess { get; set; }

                [JsonProperty("responseCode")]
                public int? ResponseCode { get; set; }

                [JsonProperty("errorMessage")]
                public string? ErrorMessage { get; set; }
            }

            public class Content
            {
                [JsonProperty("id")]
                public string? Id { get; set; }

                [JsonProperty("isFavourite")]
                public bool? IsFavourite { get; set; }

                [JsonProperty("name")]
                public string? Name { get; set; }

                [JsonProperty("imageUrl")]
                public string? ImageUrl { get; set; }

                [JsonProperty("directions")]
                public List<string>? Directions { get; set; }

                [JsonProperty("dietTypes")]
                public List<DietType>? DietTypes { get; set; }

                [JsonProperty("mealTypes")]
                public List<MealType>? MealTypes { get; set; }

                [JsonProperty("tags")]
                public List<Tags>? Tags { get; set; }

                [JsonProperty("foods")]
                public List<Food>? Foods { get; set; }

                [JsonProperty("recipeServing")]
                public RecipeServing? RecipeServing { get; set; }

                [JsonProperty("status")]
                public int? Status { get; set; }

                [JsonProperty("isEditable")]
                public bool? IsEditable { get; set; }
            }

            public class DietType
            {
                [JsonProperty("id")]
                public string? Id { get; set; }

                [JsonProperty("name")]
                public string? Name { get; set; }
            }

            public class MealType
            {
                [JsonProperty("id")]
                public string? Id { get; set; }

                [JsonProperty("name")]
                public string? Name { get; set; }
            }

            public class Tags
            {
                [JsonProperty("id")]
                public string? Id { get; set; }

                [JsonProperty("name")]
                public string? Name { get; set; }
            }

            public class Food
            {
                [JsonProperty("id")]
                public string? Id { get; set; }

                [JsonProperty("name")]
                public string? Name { get; set; }

                [JsonProperty("calories")]
                public double? Calories { get; set; }

                [JsonProperty("protein")]
                public double? Protein { get; set; }

                [JsonProperty("carbs")]
                public double? Carbs { get; set; }

                [JsonProperty("fat")]
                public double? Fat { get; set; }

                [JsonProperty("measurementDescription")]
                public string? MeasurementDescription { get; set; }

                [JsonProperty("servingId")]
                public string? ServingId { get; set; }

                [JsonProperty("servingAmount")]
                public double? ServingAmount { get; set; }
            }

            public class RecipeServing
            {
                [JsonProperty("id")]
                public string? Id { get; set; }

                [JsonProperty("servingDescription")]
                public string? ServingDescription { get; set; }

                [JsonProperty("measurementDescription")]
                public string? MeasurementDescription { get; set; }

                [JsonProperty("numberOfUnits")]
                public double? NumberOfUnits { get; set; }

                [JsonProperty("calories")]
                public double? Calories { get; set; }

                [JsonProperty("carbs")]
                public double? Carbs { get; set; }

                [JsonProperty("protein")]
                public double? Protein { get; set; }

                [JsonProperty("fat")]
                public double? Fat { get; set; }
            }
        }

        public class Request
        {

            public static string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI5ZTIwY2ZmMS00OGUxLTRiMGYtYjVmNy0yZTFmMjAxZWMxM2MiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjdhODRkNDBlLWY4MTEtNDZhNi0yNGI0LTA4ZGM0Y2I2MTdjMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiYXBpX2FjY2VzcyI6ImFsbCIsImV4cCI6MjUzNDAyMzAwODAwLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJNeUF1dGhTZXJ2ZXIifQ.tnATwGeUU5frNLhzLoFNDZPF-8OCs8PLo1z7aQUVWRk";
            public static Recipe.ApiResponse? ReqGetRecipe(string id)
            {
                string url = $"transform-me-gateway-api.azurewebsites.net";
                HttpRequest req = new()
                {
                    HttpVerb = "GET",
                    Path = $"/nutrition/recipe/get-details/{id}",
                    ContentType = "application/json"
                };
                req.AddHeader("Connection", "Keep-Alive");
                req.AddHeader("accept-encoding", "gzip, deflate, br");
                req.AddHeader("Authorization", $"Bearer {token}");



                Chilkat.Http http = new();
                var resp = http.SynchronousRequest(url, 443, true, req);
                var re = resp.StatusCode.ToString().StartsWith("2");
                var response = http.LastMethodSuccess
                    ? JsonConvert.DeserializeObject<Recipe.ApiResponse>(resp.BodyStr ?? throw new Exception("Response body is null."))
                    : throw new ArgumentException(http.LastErrorText);


                return response;
            }

            public static string ReqPutRecipe(Recipe.ApiResponse? response)
            {
                string url = $"transform-me-gateway-api.azurewebsites.net";
                HttpRequest req = new()
                {
                    HttpVerb = "PUT",
                    Path = $"/nutrition/recipe/edit",
                    ContentType = "multipart/form-data"
                };
                req.AddHeader("Connection", "Keep-Alive");
                req.AddHeader("accept-encoding", "gzip, deflate, br");
                req.AddHeader("Authorization", $"Bearer {token}");



                Chilkat.Http http = new();
                var resp = http.SynchronousRequest(url, 443, true, CreateMultiPartFormBody(req, response, ""));
                var respons = http.LastMethodSuccess
                    ? resp.BodyStr ?? throw new Exception("Response body is null.")
                    : throw new ArgumentException(http.LastErrorText);

                return respons;
            }

            public static HttpRequest CreateMultiPartFormBody(HttpRequest req, Recipe.ApiResponse? response, string imagePath)
            {
                req.AddParam("id", response.Content.Id);
                req.AddParam("name", response.Content.Name);
                AddingMealTypeId(req, response);
                AddingDietTypeId(req, response);
                AddingTagId(req, response);
                AddingDirections(req, response);
                req.AddParam("serving.measurementDescription", response.Content.RecipeServing.MeasurementDescription);
                req.AddParam("serving.numberOfUnits", response.Content.RecipeServing.NumberOfUnits.ToString());
                req.AddParam("serving.calories", response.Content.RecipeServing.Calories.ToString());
                req.AddParam("serving.carbs", response.Content.RecipeServing.Carbs.ToString());
                req.AddParam("serving.protein", response.Content.RecipeServing.Protein.ToString());
                req.AddParam("serving.fat", response.Content.RecipeServing.Fat.ToString());
                AddingFoods(req, response);
                string pathToFileOnDisk = string.Empty;
                if (response.Content.Name == "Picadillo" || response.Content.Name == "Beef Burrito")
                {
                    pathToFileOnDisk = $@"C:\Users\sucha\downloaded_images\{response.Content.Name.Replace(' ', '_')}.jpg";
                }
                else
                {
                    pathToFileOnDisk = $@"C:\Users\sucha\downloaded_images\{response.Content.Name.Replace(' ', '_')}.png";
                }
                bool success = req.AddFileForUpload("photo", pathToFileOnDisk);
                if (success != true)
                {
                    throw new ArgumentException(req.LastErrorText);
                }

                return req;
            }


            public static HttpRequest AddingMealTypeId(HttpRequest req, Recipe.ApiResponse response)
            {
                if (response.Content.MealTypes.Count > 0)
                {
                    foreach (var MealType in response.Content.MealTypes)
                    {
                        req.AddParam($"MealTypeIds", MealType.Id);

                    }
                }
                else
                {
                    req.AddParam($"MealTypeIds", "[]");
                }

                return req;
            }

            public static HttpRequest AddingDietTypeId(HttpRequest req, Recipe.ApiResponse response)
            {
                if (response.Content.DietTypes.Count > 0)
                {
                    foreach (var DietType in response.Content.DietTypes)
                    {
                        req.AddParam($"DietTypeIds", DietType.Id);

                    }

                }
                else
                {
                    req.AddParam($"DietTypeIds", "[]");
                }

                return req;
            }

            public static HttpRequest AddingTagId(HttpRequest req, Recipe.ApiResponse response)
            {
                if (response.Content.Tags.Count > 0)
                {
                    foreach (var Tag in response.Content.Tags)
                    {
                        req.AddParam($"TagIds", Tag.Id);

                    }
                }
                else
                {
                    req.AddParam($"TagIds", "[]");
                }

                return req;
            }

            public static HttpRequest AddingDirections(HttpRequest req, Recipe.ApiResponse response)
            {
                if (response.Content.Directions.Count > 0)
                {
                    foreach (var Direction in response.Content.Directions)
                    {
                        req.AddParam($"Directions", Direction);

                    }
                }
                else
                {
                    req.AddParam($"Directions", "[]");
                }

                return req;
            }

            public static HttpRequest AddingFoods(HttpRequest req, Recipe.ApiResponse response)
            {
                for (int i = 0; i < response.Content.Foods.Count; i++)
                {

                    req.AddParam($"foods[{i}].Id", response.Content.Foods[i].Id);
                    req.AddParam($"foods[{i}].ServingAmount", response.Content.Foods[i].ServingAmount.ToString());
                    req.AddParam($"foods[{i}].ServingId", response.Content.Foods[i].ServingId);

                }

                return req;
            }


        }
    }
}
