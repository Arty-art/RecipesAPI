using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetRecipesAPI.Helpers
{
    public class AppDbContext
    {
        public class DataBaseRecipes
        {

            public static string GetFoodId()
            {
                string id = string.Empty;
                string query = "SELECT Top(1) Id \r\n  " +
                               "FROM [dbo].[Foods]\r\n  " +
                               "order by createdAt desc";
                try
                {
                    using SqlConnection db = new(Data.GET_CONNECTION_STRING_NUTRITION);
                    using SqlCommand command = new(query, db);
                    db.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetGuid(0).ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}\r\n{ex.StackTrace}");
                }
                finally
                {
                    SqlConnection.ClearAllPools();
                }
                return id;
            }

            public static List<FoodForRecipes> GetFoods()
            {
                List<FoodForRecipes> listRecipes = new();
                string query = "SELECT f.id as FoodId, fs.id as FoodServingId, f.name\r\n  " +
                    "FROM [dbo].[Foods] f\r\n  " +
                    "join Foodservings fs on fs.foodId = f.id\r\n  " +
                    "--where f.CreatedAt > ('2024-09-23 16:38:50.0000000 +00:00') and f.CreatedAt < ('2024-09-24 00:38:50.0000000 +00:00') \r\n" +
                    "order by f.createdAt desc";
                try
                {
                    using SqlConnection db = new(Data.GET_CONNECTION_STRING_NUTRITION);
                    using SqlCommand command = new(query, db);
                    db.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var row = new FoodForRecipes();
                        row.FoodId = reader.GetGuid(0);
                        row.FoodServingId = reader.GetGuid(1);
                        row.FoodName = reader.GetString(2);
                        listRecipes.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}\r\n{ex.StackTrace}");
                }
                finally
                {
                    SqlConnection.ClearAllPools();
                }
                return listRecipes;
            }


            public static void InsertFoods(Meals meal)
            {


                string query = //"SET IDENTITY_INSERT WorkoutExercises ON" +
                                   "INSERT INTO [dbo].[Foods]\r\n           " +
                                   "([Id], [FatSecretId], [Name], [Brand], [Barcode], [CreatorId], [Status], [CreatedAt], [ModifiedAt], [IsDeleted], [ImageUrl], [Notes])\r\n     " +
                                   "VALUES\r\n           " +
                                   $"('{Guid.NewGuid()}', null, '{meal.Ingredients}', null, null, null, 1, '{DateTime.Now}', null, 0, null, null)";
                //"\r\n\r\nSET IDENTITY_INSERT WorkoutExercises OFF";
                try
                {
                    using SqlConnection db = new(Data.GET_CONNECTION_STRING_NUTRITION);
                    using SqlCommand command = new(query, db);

                    db.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        continue;
                    }

                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}\r\n{ex.StackTrace}");
                }
                finally
                {
                    SqlConnection.ClearAllPools();
                }
            }

            public static void InsertFoodServings(string foodId, Meals meal)
            {


                string query = //"SET IDENTITY_INSERT WorkoutExercises ON" +
                                   "INSERT INTO [dbo].[FoodServings]\r\n           " +
                                   "([Id], [FoodId], [FatSecretId], [ServingDescription], [MeasurementDescription], [NumberOfUnits], [MetricAmount], [MetricUnit], [IsDefault], [Calories], [Carbs], [Protein], [Fat], [SaturatedFat], [PolyunsaturatedFat], [MonounsaturatedFat], [TransFat], [Cholesterol], [Sodium], [Potassium], [Fiber], [Sugar], [AddedSugars], [VitaminA], [VitaminD], [VitaminC], [Calcium], [Iron], [CreatedAt], [ModifiedAt], [IsDeleted])\r\n     " +
                                   "VALUES\r\n           " +
                                   $"('{Guid.NewGuid()}', '{foodId}', null, '{meal.AmountSize + ' ' + meal.IngredientUnitServing.ToLower()}', '{meal.IngredientUnitServing.ToLower()}', {ParseFractionToFloat(meal.AmountSize)}, {(meal.Amount == "--" ? "null" : meal.Amount)}, '{(meal.IngredientUnit.ToLower() == "--" ? "null" : meal.IngredientUnit.ToLower())}', 0, {meal.Calories}, {meal.Carbs}, {meal.Protein}, {meal.Fats}, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '{DateTime.Now}', null, 0)";
                //"\r\n\r\nSET IDENTITY_INSERT WorkoutExercises OFF";
                try
                {
                    using SqlConnection db = new(Data.GET_CONNECTION_STRING_NUTRITION);
                    using SqlCommand command = new(query, db);

                    db.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        continue;
                    }

                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}\r\n{ex.StackTrace}");
                }
                finally
                {
                    SqlConnection.ClearAllPools();
                }

            }

            public static float ParseFractionToFloat(string fraction)
            {
                if (fraction.Contains('/'))
                {
                    if (string.IsNullOrWhiteSpace(fraction))
                        throw new ArgumentException("Fraction cannot be null or empty.", nameof(fraction));

                    string[] parts = fraction.Split('/');
                    if (parts.Length != 2)
                        throw new FormatException("Invalid fraction format. It should be in 'numerator/denominator' format.");

                    if (!float.TryParse(parts[0], out float numerator))
                        throw new FormatException("Invalid numerator in fraction.");

                    if (!float.TryParse(parts[1], out float denominator))
                        throw new FormatException("Invalid denominator in fraction.");

                    if (denominator == 0)
                        throw new DivideByZeroException("Denominator cannot be zero.");

                    return numerator / denominator;
                }
                return float.Parse(fraction);
            }

            public class Meals
            {
                public string? AmountSize { get; set; }
                public string? IngredientUnitServing { get; set; }
                public string? Ingredients { get; set; }
                public string? IsProtein { get; set; }
                public string? IsCarbohydrates { get; set; }
                public string? IsFats { get; set; }
                public string? IsFruitsVegetables { get; set; }
                public string? IsPantryHerbs { get; set; }
                public string? Amount { get; set; }
                public string? IngredientUnit { get; set; }
                public string? Calories { get; set; }
                public string? Protein { get; set; }
                public string? Fats { get; set; }
                public string? Carbs { get; set; }
            }

            public class FoodForRecipes
            {
                public Guid FoodId { get; set; }
                public Guid FoodServingId { get; set; }
                public string FoodName { get; set; }
            }

        }

        public class DataBaseExercises
        {
            private static T GetValueOrDefault<T>(SqlDataReader reader, int index, T defaultValue = default)
            {
                if (!reader.IsDBNull(index))
                {
                    return (T)reader.GetValue(index);
                }
                else
                {
                    return defaultValue;
                }
            }
            public static List<Exercise> GetNewExercises()
            {
                List<Exercise> listRecipes = new();
                string query = "SELECT * FROM [dbo].[Exercises]\r\n  " +
                               "where CreatedAt > '2024-10-24 00:00:00.0000000'\r\n  " +
                               "order by CreatedAt";
                try
                {
                    using SqlConnection db = new(Data.GET_CONNECTION_STRING_WORKOUT);
                    using SqlCommand command = new(query, db);
                    db.Open();

                    using SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var row = new Exercise();
                        row.Id = GetValueOrDefault<Guid>(reader, 0);
                        row.Name = GetValueOrDefault<string>(reader, 1);
                        row.VideoUrl = GetValueOrDefault<string>(reader, 2);
                        row.TempoBold = GetValueOrDefault<int>(reader, 3);
                        row.ThumbPhotoId = GetValueOrDefault<Guid>(reader, 4);
                        row.CreatedAt = GetValueOrDefault<DateTime>(reader, 5);
                        row.ModifiedAt = GetValueOrDefault<DateTime>(reader, 6);
                        row.IsDeleted = GetValueOrDefault<bool>(reader, 7);
                        row.Gender = GetValueOrDefault<int>(reader, 8);
                        row.Gym = GetValueOrDefault<bool>(reader, 9);
                        row.Home = GetValueOrDefault<bool>(reader, 10);
                        row.HomeGym = GetValueOrDefault<bool>(reader, 11);
                        row.MuscleGroup1 = GetValueOrDefault<int>(reader, 12);
                        row.MuscleGroup2 = GetValueOrDefault<int>(reader, 13);
                        row.Unilateral = GetValueOrDefault<bool>(reader, 14);
                        row.CoachId = GetValueOrDefault<Guid>(reader, 15);
                        listRecipes.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}\r\n{ex.StackTrace}");
                }
                finally
                {
                    SqlConnection.ClearAllPools();
                }
                return listRecipes;
            }


            public class Exercise
            {
                public Guid? Id { get; set; }
                public string? Name { get; set; }
                public string? VideoUrl { get; set; }
                public int? TempoBold { get; set; }
                public Guid? ThumbPhotoId { get; set; }
                public DateTime? CreatedAt { get; set; }
                public DateTime? ModifiedAt { get; set; }
                public bool? IsDeleted { get; set; }
                public int? Gender { get; set; }
                public bool? Gym { get; set; }
                public bool? Home { get; set; }
                public bool? HomeGym { get; set; }
                public int? MuscleGroup1 { get; set; }
                public int? MuscleGroup2 { get; set; }
                public bool? Unilateral { get; set; }
                public Guid? CoachId { get; set; }
            }


        }
    }
}
