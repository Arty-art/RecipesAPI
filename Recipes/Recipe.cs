using Chilkat;
using Newtonsoft.Json;
using System.Data;
using static GetRecipesAPI.RecipeAPI.Recipe;
using GetRecipesAPI.Helpers;

namespace GetRecipesAPI
{
    public class RecipeAPI
    {
        public class Recipe
        {
            //public class ApiResponse
            //{
            //    [JsonProperty("content")]
            //    public required Content Content { get; set; }

            //    [JsonProperty("isSuccess")]
            //    public bool? IsSuccess { get; set; }

            //    [JsonProperty("responseCode")]
            //    public int? ResponseCode { get; set; }

            //    [JsonProperty("errorMessage")]
            //    public string? ErrorMessage { get; set; }
            //}

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

            public static Recipe.Content? ReqGetRecipe(string id)
            {
                HttpRequest req = new()
                {
                    HttpVerb = "GET",
                    Path = $"/nutrition/recipe/get-details/{id}",
                    ContentType = "application/json"
                };
                req.AddHeader("Connection", "Keep-Alive");
                req.AddHeader("accept-encoding", "gzip, deflate, br");
                req.AddHeader("Authorization", $"Bearer {Data.token}");



                Chilkat.Http http = new();
                var resp = http.SynchronousRequest(Data.url, 443, true, req);
                var re = resp.StatusCode.ToString().StartsWith("2");
                var response = http.LastMethodSuccess
                    ? JsonConvert.DeserializeObject<Recipe.Content>(resp.BodyStr ?? throw new Exception("Response body is null."))
                    : throw new ArgumentException(http.LastErrorText);


                return response;
            }

            public static void ReqPutRecipe(Recipe.Content? response)
            {
                HttpRequest req = new()
                {
                    HttpVerb = "PUT",
                    Path = $"/nutrition/recipe/edit",
                    ContentType = "multipart/form-data"
                };
                req.AddHeader("Connection", "Keep-Alive");
                req.AddHeader("accept-encoding", "gzip, deflate, br");
                req.AddHeader("Authorization", $"Bearer {Data.token}");

                var createBool = CreateMultiPartFormBody(ref req, response, "");

                Chilkat.Http http = new();
                if (createBool) {
                    var resp = http.SynchronousRequest(Data.url, 443, true, req);
                    var respons = http.LastMethodSuccess
                        ? resp.BodyStr ?? throw new Exception("Response body is null.")
                        : throw new ArgumentException(http.LastErrorText);
                }
                

            }

            public static void ReqPostRecipe(RecipeAPI.RecipesFromDoc.Recipe recipe, List<AppDbContext.DataBaseRecipes.FoodForRecipes> foodList)
            {
                
                HttpRequest req = new()
                {
                    HttpVerb = "POST",
                    Path = $"/nutrition/recipe/create",
                    ContentType = "multipart/form-data"
                };
                req.AddHeader("Connection", "Keep-Alive");
                req.AddHeader("accept-encoding", "gzip, deflate, br");
                req.AddHeader("Authorization", $"Bearer {Data.token}");

                var createBool = CreateMultiPartFormBody(ref req, recipe, foodList);

                Chilkat.Http http = new();
                if (createBool)
                {
                    var resp = http.SynchronousRequest(Data.url, 443, true, req);
                    var respons = http.LastMethodSuccess
                        ? resp.BodyStr ?? throw new Exception("Response body is null.")
                        : throw new ArgumentException(http.LastErrorText);
                }


            }

            public static bool CreateMultiPartFormBody(ref HttpRequest req, Recipe.Content? response, string imagePath)
            {
                req.AddParam("id", response.Id);
                req.AddParam("name", response.Name);
                AddingMealTypeId(req, response);
                AddingDietTypeId(req, response);
                AddingTagId(req, response);
                AddingDirections(req, response);
                req.AddParam("serving.measurementDescription", response.RecipeServing.MeasurementDescription);
                req.AddParam("serving.numberOfUnits", response.RecipeServing.NumberOfUnits.ToString());
                req.AddParam("serving.calories", response.RecipeServing.Calories.ToString());
                req.AddParam("serving.carbs", response.RecipeServing.Carbs.ToString());
                req.AddParam("serving.protein", response.RecipeServing.Protein.ToString());
                req.AddParam("serving.fat", response.RecipeServing.Fat.ToString());
                AddingFoods(req, response);
                string pathToFileOnDisk = string.Empty;
                if (response.Name == "Picadillo" || response.Name == "Beef Burrito")
                {
                    pathToFileOnDisk = $@"D:\New Memberships\WhiteLabel\Meals\Images\{response.Name.Replace("&", "and").Replace(",", "_")}.jpg";
                }
                else
                {
                    pathToFileOnDisk = $@"D:\New Memberships\WhiteLabel\Meals\Images\{response.Name.Replace("&", "and").Replace(",", "_")}.jpg";
                }
                if (!File.Exists(pathToFileOnDisk))
                {
                    return false;
                }
                bool success = req.AddFileForUpload("photo", pathToFileOnDisk);
                if (success != true)
                {
                    throw new ArgumentException(req.LastErrorText);
                }

                return true;
            }

            public static bool CreateMultiPartFormBody(ref HttpRequest req, RecipeAPI.RecipesFromDoc.Recipe recipe, List<AppDbContext.DataBaseRecipes.FoodForRecipes> foodList)
            {
                var list = new List<string>();
                req.AddParam("name", recipe.Meal_Name.Trim());
                list.Add(string.Join(':', "name", recipe.Meal_Name));
                AddingMealTypeId(req, recipe, list);
                AddingDietTypeId(req, recipe, list);
                AddingTagId(req, recipe, list);
                AddingDirections(req, recipe, list);
                req.AddParam("serving.measurementDescription", "serving");
                req.AddParam("serving.numberOfUnits", "1");
                req.AddParam("serving.calories", recipe.Calories.ToString());
                req.AddParam("serving.carbs", recipe.Carbs.ToString());
                req.AddParam("serving.protein", recipe.Protein.ToString());
                req.AddParam("serving.fat", recipe.Fats.ToString());
                list.Add(string.Join(':', "serving.measurementDescription", "serving"));
                list.Add(string.Join(':', "serving.numberOfUnits", "1"));
                list.Add(string.Join(':', "serving.calories", recipe.Calories.ToString()));
                list.Add(string.Join(':', "serving.carbs", recipe.Carbs.ToString()));
                list.Add(string.Join(':', "serving.protein", recipe.Protein.ToString()));
                list.Add(string.Join(':', "serving.fat", recipe.Fats.ToString()));
                AddingFoods(req, recipe, foodList, list);
                req.AddParam("Serving.SaturatedFat", "0");
                req.AddParam("Serving.PolyunsaturatedFat", "0");
                req.AddParam("Serving.MonounsaturatedFat", "0");
                req.AddParam("Serving.TransFat", "0");
                req.AddParam("Serving.Cholesterol", "0");
                req.AddParam("Serving.Sodium", "0");
                req.AddParam("Serving.Potassium", "0");
                req.AddParam("Serving.Fiber", "0");
                req.AddParam("Serving.Sugar", "0");
                req.AddParam("Serving.AddedSugars", "0");
                req.AddParam("Serving.VitaminA", "0");
                req.AddParam("Serving.VitaminD", "0");
                req.AddParam("Serving.VitaminC", "0");
                req.AddParam("Serving.Calcium", "0");
                req.AddParam("Serving.Iron", "0");
                list.Add(string.Join(':', "Serving.SaturatedFat", "0"));
                list.Add(string.Join(':', "Serving.PolyunsaturatedFat", "0"));
                list.Add(string.Join(':', "Serving.MonounsaturatedFat", "0"));
                list.Add(string.Join(':', "Serving.TransFat", "0"));
                list.Add(string.Join(':', "Serving.Cholesterol", "0"));
                list.Add(string.Join(':', "Serving.Sodium", "0"));
                list.Add(string.Join(':', "Serving.Potassium", "0"));
                list.Add(string.Join(':', "Serving.Fiber", "0"));
                list.Add(string.Join(':', "Serving.Sugar", "0"));
                list.Add(string.Join(':', "Serving.AddedSugars", "0"));
                list.Add(string.Join(':', "Serving.VitaminA", "0"));
                list.Add(string.Join(':', "Serving.VitaminD", "0"));
                list.Add(string.Join(':', "Serving.VitaminC", "0"));
                list.Add(string.Join(':', "Serving.Calcium", "0"));
                list.Add(string.Join(':', "Serving.Iron", "0"));
                (bool, string) pathToFileOnDisk = DownloadImagesHelper.DownloadImages(recipe).Result;
                if (pathToFileOnDisk.Item1)
                {
                    req.AddFileForUpload("photo", pathToFileOnDisk.Item2);
                }

                _ = list;
                return true;
            }


            public static HttpRequest AddingMealTypeId(HttpRequest req, Recipe.Content response)
            {
                if (response.MealTypes.Count > 0)
                {
                    foreach (var MealType in response.MealTypes)
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

            public static HttpRequest AddingDietTypeId(HttpRequest req, Recipe.Content response)
            {
                if (response.DietTypes.Count > 0)
                {
                    foreach (var DietType in response.DietTypes)
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

            public static HttpRequest AddingTagId(HttpRequest req, Recipe.Content response)
            {
                if (response.Tags.Count > 0)
                {
                    foreach (var Tag in response.Tags)
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

            public static HttpRequest AddingDirections(HttpRequest req, Recipe.Content response)
            {
                if (response.Directions.Count > 0)
                {
                    foreach (var Direction in response.Directions)
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

            public static HttpRequest AddingFoods(HttpRequest req, Recipe.Content response)
            {
                for (int i = 0; i < response.Foods.Count; i++)
                {

                    req.AddParam($"foods[{i}].Id", response.Foods[i].Id);
                    req.AddParam($"foods[{i}].ServingAmount", response.Foods[i].ServingAmount.ToString());
                    req.AddParam($"foods[{i}].ServingId", response.Foods[i].ServingId);

                }

                return req;
            }

            public static HttpRequest AddingMealTypeId(HttpRequest req, RecipeAPI.RecipesFromDoc.Recipe recipe, List<string> list)
            {
                var mealTypeIds = new List<string>();

                foreach (var mealType in new[] { "Breakfast", "Dinner", "Snack", "Lunch" })
                {
                    switch (mealType)
                    {
                        case "Breakfast" when recipe.Breakfast.ToLower() == "y" && MealType.ContainsKey("Breakfast"):
                            mealTypeIds.Add(MealType["Breakfast"]);
                            break;
                        case "Dinner" when recipe.Dinner.ToLower() == "y" && MealType.ContainsKey("Dinner"):
                            mealTypeIds.Add(MealType["Dinner"]);
                            break;
                        case "Snack" when recipe.Snack.ToLower() == "y" && MealType.ContainsKey("Snack"):
                            mealTypeIds.Add(MealType["Snack"]);
                            break;
                        case "Lunch" when recipe.Lunch.ToLower() == "y" && MealType.ContainsKey("Lunch"):
                            mealTypeIds.Add(MealType["Lunch"]);
                            break;
                    }
                }
                if (mealTypeIds.Count > 0)
                {
                    foreach(var mealTypeId in mealTypeIds)
                    {
                        req.AddParam("MealTypeIds", mealTypeId);
                        list.Add(string.Join(':', "MealTypeIds", mealTypeId));
                    }
                }
                else
                {
                    req.AddParam("MealTypeIds", "[]");
                }
                _ = list;
                return req;
            }

            public static HttpRequest AddingDietTypeId(HttpRequest req, RecipeAPI.RecipesFromDoc.Recipe recipe, List<string> list)
            {
                var dietTypeIds = new List<string>();
                foreach (var dietType in new[] { "No_Restrictions", "Vegan", "Vegetarian", "Lactose_Free", "Nut_Free" })
                {
                    switch (dietType)
                    {
                        case "No_Restrictions" when recipe.No_Restrictions.ToLower() == "y" && DietType.ContainsKey("No_Restrictions"):
                            dietTypeIds.Add(DietType["No_Restrictions"]);
                            break;
                        case "Vegan" when recipe.Vegan.ToLower() == "y" && DietType.ContainsKey("Vegan"):
                            dietTypeIds.Add(DietType["Vegan"]);
                            break;
                        case "Vegetarian" when recipe.Vegetarian.ToLower() == "y" && DietType.ContainsKey("Vegetarian"):
                            dietTypeIds.Add(DietType["Vegetarian"]);
                            break;
                        case "Lactose_Free" when recipe.Lactose_Free.ToLower() == "y" && DietType.ContainsKey("Lactose_Free"):
                            dietTypeIds.Add(DietType["Lactose_Free"]);
                            break;
                        case "Nut_Free" when recipe.Nut_Free.ToLower() == "y" && DietType.ContainsKey("Nut_Free"):
                            dietTypeIds.Add(DietType["Nut_Free"]);
                            break;
                    }
                }
                if (dietTypeIds.Count > 0)
                {
                    foreach (var dietTypeId in dietTypeIds)
                    {
                        req.AddParam("DietTypeIds", dietTypeId);
                        list.Add(string.Join(':', "DietTypeIds", dietTypeId));
                    }
                }
                else
                {
                    req.AddParam("DietTypeIds", "[]");
                }

                return req;
            }

            public static HttpRequest AddingTagId(HttpRequest req, RecipeAPI.RecipesFromDoc.Recipe recipe, List<string> list)
            {
                var tagIds = new List<string>();

                AddTagsFromRecipeField(recipe.tag1, Tag, tagIds);
                AddTagsFromRecipeField(recipe.tag2, Tag, tagIds);
                AddTagsFromRecipeField(recipe.tag3, Tag, tagIds);

                if (tagIds.Count > 0)
                {
                    foreach (var tagId in tagIds)
                    {
                        req.AddParam("TagIds", tagId);
                        list.Add(string.Join(':', "TagIds", tagId));
                    }
                }
                else
                {
                    req.AddParam("TagIds", "[]");
                }
                _ = list;

                return req;
            }

            public static HttpRequest AddingDirections(HttpRequest req, RecipeAPI.RecipesFromDoc.Recipe recipe, List<string> list)
            {
                if (recipe.Directions.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().Count > 0)
                {
                    foreach (var Direction in recipe.Directions.Where(x => !string.IsNullOrWhiteSpace(x)).ToList())
                    {
                        req.AddParam($"Directions", Direction);
                        list.Add(string.Join(':', $"Directions", Direction));
                    }
                }
                else
                {
                    req.AddParam($"Directions", "[]");
                }
                _ = list;
                return req;
            }

            public static HttpRequest AddingFoods(HttpRequest req, RecipeAPI.RecipesFromDoc.Recipe recipe, List<AppDbContext.DataBaseRecipes.FoodForRecipes> foods, List<string> list)
            {
                for (int i = 0; i < recipe.Ingredients.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().Count; i++)
                {

                    list.Add(string.Join(':', $"foods[{i}].Id", foods.Where(x => x.FoodName.Trim().ToLower()
                                                                              .Equals(recipe.Ingredients[i].Trim().ToLower())
                                                                              ).Select(f => f.FoodId).FirstOrDefault().ToString()));
                    if (foods.Where(x => x.FoodName
                    .Trim()
                    .ToLower()
                    .Equals(recipe.Ingredients[i]
                    .Trim()
                    .ToLower()))
                        .Select(f => f.FoodId)
                        .FirstOrDefault()
                        .ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        Console.WriteLine($"Invalid FoodId found for ingredient '{recipe.Ingredients[i]}' at index {i}");
                    }
                    list.Add(string.Join(':', $"foods[{i}].ServingAmount", AppDbContext.DataBaseRecipes.ParseFractionToFloat(recipe.Ingredient_Size[i]).ToString()));
                    list.Add(string.Join(':', $"foods[{i}].ServingId", foods.Where(x => x.FoodName.Trim().ToLower()
                                                                              .Equals(recipe.Ingredients[i].Trim().ToLower())
                                                                              ).Select(f => f.FoodServingId).FirstOrDefault().ToString()));
                    req.AddParam($"foods[{i}].Id", foods.Where(x => x.FoodName.Trim().ToLower()
                                                                              .Equals(recipe.Ingredients[i].Trim().ToLower())
                                                                              ).Select(f => f.FoodId).FirstOrDefault().ToString());
                    req.AddParam($"foods[{i}].ServingAmount", AppDbContext.DataBaseRecipes.ParseFractionToFloat(recipe.Ingredient_Size[i]).ToString());
                    req.AddParam($"foods[{i}].ServingId", foods.Where(x => x.FoodName.Trim().ToLower()
                                                                              .Equals(recipe.Ingredients[i].Trim().ToLower())
                                                                              ).Select(f => f.FoodServingId).FirstOrDefault().ToString());

                }
                _ = list;

                return req;
            }


        }

        private static void AddTagsFromRecipeField(string type, Dictionary<string, string> mappings, List<string> typeIds)
        {
            if (!string.IsNullOrWhiteSpace(type))
            {
                var trimmedTag = type.Trim().ToLower();
                mappings.TryGetValue(trimmedTag, out var key);
                bool isTag =  Tag.ContainsValue(key);
                // Check if the trimmedTag exists in the mappings and add corresponding tag ID
                if (isTag)
                {
                    typeIds.Add(key);
                }
            }
        }

        public static Dictionary<string, string> MealType = new Dictionary<string, string>
        {
            { "Dinner", "867CFFF9-53CE-43E4-9B30-728BBB58EC12" },
            { "Lunch", "88E8B026-056E-4DAE-A632-8791899ED14A" },
            { "Snack", "16B9B011-A2FA-4613-97B5-BF706E56CC2A" },
            { "Breakfast", "83BECF58-E2A5-4B02-B188-E17D02F1374F" }
        };

        public static Dictionary<string, string> DietType = new Dictionary<string, string>
        {
            { "No_Restrictions", "4F757F4F-56E6-46C7-99E5-31E490CE5711" },
            { "Vegan", "6775BCE4-81BC-4BF7-85F6-58F627B04C17" },
            { "Vegetarian", "C584BB7C-8DFB-4334-B4FF-704982F598AC" },
            { "Lactose_Free", "F66C011D-F97F-4CAC-AF8E-800E003A6F79" },
            { "Nut_Free", "07356E62-3203-415A-96B2-EA4F9A011AD0" }
        };

        public static Dictionary<string, string> Tag = new Dictionary<string, string>
        {
            {"meal prep friendly", "27B77528-89C2-4734-F6A3-08DCDD3D5EFA" },
            {"express", "0307AD6A-5F15-4FEB-F6A4-08DCDD3D5EFA" },
            {"meal prep friendly recipe", "1E1DFD91-E880-4010-F6A5-08DCDD3D5EFA" },
            {"vegan friendly", "7FC12FC6-E211-4757-F6A6-08DCDD3D5EFA" },
            {"new recipe", "8789D70A-4585-4361-F6A7-08DCDD3D5EFA" },
            {"gluten free", "F2FF0945-4E5B-4211-F6A8-08DCDD3D5EFA" },
            {"express recipe", "A7AC8B1C-377D-4381-F6A9-08DCDD3D5EFA" },
            {"new", "718C10F4-D3A2-496B-8FD8-1F899CF90F16" },
            {"budget friendly", "403A7810-EC1D-4D7C-95EC-6E2F0F8E6BE2" },
            {"easy prep", "13A7C464-AA0B-4BDD-BCA4-7C95EDB02A87" }
        };

        

        public class RecipesFromDoc
        {
            public class Recipe
            {
                public string ID { get; set; }
                public string Meal_Name { get; set; }
                public List<string> Ingredient_Size { get; set; } = new List<string>();
                public List<string> Ingredient_Unit { get; set; }  = new List<string>();
                public List<string> Ingredients { get; set; }  = new List<string>();
                public List<string> Directions { get; set; }  = new List<string>();
                public string Calories { get; set; }
                public string Protein { get; set; }
                public string Fats { get; set; }
                public string Carbs { get; set; }
                public string Breakfast { get; set; }
                public string Lunch { get; set; }
                public string Dinner { get; set; }
                public string Snack { get; set; }
                public string Vegan { get; set; }
                public string Vegetarian { get; set; }
                public string Gluten_Free { get; set; }
                public string Lactose_Free { get; set; }
                public string Seafood_Free { get; set; }
                public string Nut_Free { get; set; }
                public string No_Restrictions { get; set; }
                public string Image_Link_1280x1024 { get; set; }
                public string Image_link_960x1100 { get; set; }
                public string athlete { get; set; }
                public string tag1 { get; set; }
                public string tag2 { get; set; }
                public string tag3 { get; set; }

            }
        }
    }
}
