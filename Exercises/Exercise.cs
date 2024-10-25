using Chilkat;
using GetRecipesAPI.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static GetRecipesAPI.ExerciseAPI;
using static GetRecipesAPI.Helpers.AppDbContext.DataBaseExercises;
using static GetRecipesAPI.MembershipAPI;

namespace GetRecipesAPI
{
    public class ExerciseAPI
    {
        public static Exercise.ResponseExercise? ReqGetExercise(string id)
        {
            HttpRequest req = new()
            {
                HttpVerb = "GET",
                Path = $"/workout/admin/exercise/get-by-id/{id}",
                ContentType = "application/json"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {Data.token}");



            Chilkat.Http http = new();
            var resp = http.SynchronousRequest(Data.url, 443, true, req);
            var re = resp.StatusCode.ToString().StartsWith("2");
            var response = http.LastMethodSuccess
                ? JsonConvert.DeserializeObject<Exercise.ResponseExercise?>(resp.BodyStr ?? throw new Exception("Response body is null."))
                : throw new ArgumentException(http.LastErrorText);


            return response;
        }

        public static string ReqPutExercise(Exercise.ResponseExerciseWl? response)
        {
            HttpRequest req = new()
            {
                HttpVerb = "PUT",
                Path = $"/workout/admin/exercise/edit",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {Data.token}");



            Http http = new();
            var resp = http.SynchronousRequest(Data.url, 443, true, CreateMultiPartFormBody(req, response, ""));
            var respons = http.LastMethodSuccess
                ? resp.BodyStr ?? throw new Exception("Response body is null.")
                : throw new ArgumentException(http.LastErrorText);

            return respons;
        }

        public static string ReqPutExercise(ExerciseCombinedModel? response)
        {
            HttpRequest req = new()
            {
                HttpVerb = "PUT",
                Path = $"/workout/admin/exercise/edit",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {Data.token}");

            var requestBody = CreateMultiPartFormBody(req, response, "");

            Http http = new();
            var resp = http.SynchronousRequest(Data.url, 443, true, requestBody);
            var respons = http.LastMethodSuccess
                ? resp.BodyStr ?? throw new Exception("Response body is null.")
                : throw new ArgumentException(http.LastErrorText);

            return respons;
        }

        public static HttpRequest CreateMultiPartFormBody(HttpRequest req, ExerciseAPI.Exercise.ResponseExerciseWl? response, string imagePath)
        {
            req.AddParam("Id", response.id);
            req.AddParam("name", response.name);
            req.AddParam("tempoBold", response.tempoBold.ToString());
            req.AddParam("videoUrl", response.videoUrl);
            AddingRelatedExercises(req, response);
            //var isImageDownloaded = DownloadImagesHelper.DownloadExerciseImages(response);
            //if (isImageDownloaded.Result.Success)
            //{
            //    bool success = req.AddFileForUpload("thumbPhoto", isImageDownloaded.Result.SavePath);
            //    if (success != true)
            //    {
            //        throw new ArgumentException(req.LastErrorText);
            //    }

            //    Console.WriteLine($"portraitImage: {isImageDownloaded.Result.SavePath}");
            //    Console.WriteLine($"\r\n");
            //}



            return req;
        }
        public static HttpRequest CreateMultiPartFormBody(HttpRequest req, ExerciseCombinedModel? response, string imagePath)
        {
            req.AddParam("Id", response.id);
            req.AddParam("name", response.ExerciseName);
            req.AddParam("tempoBold", response.TempoStart);
            req.AddParam("videoUrl", response.VideoURL);
            AddingRelatedExercisesToExistedOnes(req, response);
            var isImageDownloaded = DownloadImagesHelper.UpdateExerciseImages(response);
            if (isImageDownloaded.Result.Success) 
            {
                bool success = req.AddFileForUpload("thumbPhoto", isImageDownloaded.Result.SavePath);
                if (success != true)
                {
                    throw new ArgumentException(req.LastErrorText);
                }

                Console.WriteLine($"portraitImage: {isImageDownloaded.Result.SavePath}");
                Console.WriteLine($"\r\n");
            }

            

            return req;
        }


        public static HttpRequest AddingRelatedExercises(HttpRequest req, Exercise.ResponseExerciseWl? response)
        {
            if (response.relatedExercises.Count > 0)
            {
                for (int i = 0; i < response.relatedExercises.Count; i++)
                {
                    req.AddParam($"relatedExercises[{i}].id", response.relatedExercises[i].id.ToString());
                    req.AddParam($"relatedExercises[{i}].priority", response.relatedExercises[i].priority.ToString());
                    req.AddParam($"relatedExercises[{i}].name", response.relatedExercises[i].name.ToString());
                }
            }
            else
            {
                req.AddParam($"relatedExercises", "[]");
            }



            return req;
        }

        public static HttpRequest AddingRelatedExercisesToExistedOnes(HttpRequest req, ExerciseCombinedModel? response)
        {
            
            if (!string.IsNullOrEmpty(response.Swap1))
            {
                req.AddParam($"relatedExercises[0].id", response.Swap1.ToString());
                req.AddParam($"relatedExercises[0].priority", "1");
            }
            if (!string.IsNullOrEmpty(response.Swap2))
            {
                req.AddParam($"relatedExercises[1].id", response.Swap2.ToString());
                req.AddParam($"relatedExercises[1].priority", "2");
            }
            if (!string.IsNullOrEmpty(response.Swap3))
            {
                req.AddParam($"relatedExercises[2].id", response.Swap3.ToString());
                req.AddParam($"relatedExercises[2].priority", "3");
            }
            else if(!string.IsNullOrEmpty(response.Swap1) && !string.IsNullOrEmpty(response.Swap2) && !string.IsNullOrEmpty(response.Swap3))
            {
                req.AddParam($"relatedExercises", "[]");
            }



            return req;
        }

        public class Exercise
        {

            public class ResponseExerciseWl
            {
                public string id { get; set; }
                public string name { get; set; }
                public string videoUrl { get; set; }
                public bool gym { get; set; }
                public bool home { get; set; }
                public bool homeGym { get; set; }
                public bool unilateral { get; set; }
                public int tempoBold { get; set; }
                public int gender { get; set; }
                public int? muscleGroup1 { get; set; }
                public int? muscleGroup2 { get; set; }
                public string thumbPhotoUrl { get; set; }
                public List<RelatedexercisNew>? relatedExercises { get; set; }
            }

            public class RelatedexercisNew
            {
                public string id { get; set; }
                public string name { get; set; }
                public int priority { get; set; }
            }


            public class ResponseExercise
            {
                public Content content { get; set; }
                public bool isSuccess { get; set; }
                public int responseCode { get; set; }
                public object errorMessage { get; set; }
            }

            public class Content
            {
                public string id { get; set; }
                public string name { get; set; }
                public string videoUrl { get; set; }
                public int tempoBold { get; set; }
                public Thumbphoto thumbPhoto { get; set; }
                public Relatedexercis[] relatedExercises { get; set; }
            }

            public class Thumbphoto
            {
                public string fileName { get; set; }
                public string fileType { get; set; }
                public string fileUrl { get; set; }
            }

            public class Relatedexercis
            {
                public string id { get; set; }
                public string exerciseId { get; set; }
                public string name { get; set; }
                public int priority { get; set; }
                public int exerciseType { get; set; }
            }
        }

        public class ExerciseRequestModel
        {
            public string? ExerciseName { get; set; }
            public string? Gym { get; set; }
            public string? HomeGym { get; set; }
            public string? Home { get; set; }
            public string? TempoStart { get; set; }
            public string? Unilateral { get; set; }
            public string? Gender { get; set; }
            public string? MuscleGroup1 { get; set; }
            public string? MuscleGroup2 { get; set; }
            public string? URLToScreenhot { get; set; }
            public string? VideoURL { get; set; }
            public string? Swap1 { get; set; }
            public string? Swap2 { get; set; }
            public string? Swap3 { get; set; }
        }

        public class ExerciseCombinedModel
        {
            public string? id { get; set; }
            public string? ExerciseName { get; set; }
            public string? Gym { get; set; }
            public string? HomeGym { get; set; }
            public string? Home { get; set; }
            public string? TempoStart { get; set; }
            public string? Unilateral { get; set; }
            public string? Gender { get; set; }
            public string? MuscleGroup1 { get; set; }
            public string? MuscleGroup2 { get; set; }
            public string? URLToScreenhot { get; set; }
            public string? VideoURL { get; set; }
            public string? Swap1 { get; set; }
            public string? Swap2 { get; set; }
            public string? Swap3 { get; set; }
        }


        public static void CreateExercise(ExerciseRequestModel exercise)
        {
            List<string> list = new List<string>();
            HttpRequest req = new()
            {
                HttpVerb = "POST",
                Path = $"/workout/admin/exercise/add",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {Data.token}");
            req.AddParam("Name", exercise.ExerciseName);
            req.AddParam("VideoUrl", exercise.VideoURL);
            req.AddParam("Gym", (exercise.Gym.ToLower() == "y" ? true : false).ToString());
            req.AddParam("Home", (exercise.Home.ToLower() == "y" ? true : false).ToString());
            req.AddParam("HomeGym", (exercise.HomeGym.ToLower() == "y" ? true : false).ToString());
            req.AddParam("Unilateral", (exercise.Unilateral.ToLower() == "y" ? true : false).ToString());
            req.AddParam("TempoBold", ParseTempoStart(exercise.TempoStart).ToString());
            req.AddParam("Gender", (exercise.Gender.ToLower() == "m" ? 1 : 0).ToString());
            //req.AddParam("MuscleGroup1", string.IsNullOrWhiteSpace(exercise.MuscleGroup1) ? "" : ParseMuscleGroup(exercise.MuscleGroup1).ToString());
            //req.AddParam("MuscleGroup2", string.IsNullOrWhiteSpace(exercise.MuscleGroup2) ? "" : ParseMuscleGroup(exercise.MuscleGroup2).ToString());
            var isImageDownloaded = DownloadImagesHelper.DownloadExerciseImages(exercise);
            if (isImageDownloaded.Result.Success)
            {
                bool success = req.AddFileForUpload("ThumbPhoto", isImageDownloaded.Result.SavePath);

                //if (success)
                //{
                //    Http http = new();
                //    var resp = http.SynchronousRequest(Data.url, 443, true, req);
                //    var response = http.LastMethodSuccess
                //        ? resp.BodyStr ?? throw new Exception("Response body is null.")
                //        : throw new ArgumentException(http.LastErrorText);
                //}
            }


        }

        public static int ParseMuscleGroup(string muscleGroup)
        {

            // Try to parse MuscleGroup1 and MuscleGroup2 strings into MuscleGroup enums
            if (!Enum.TryParse<Data.MuscleGroup>(muscleGroup, out var muscleGroup1Enum))
            {
                throw new ArgumentException($"Invalid muscle group: {muscleGroup}");
            }
            return (int)muscleGroup1Enum;
        }

        public static int ParseTempoStart(string tempoStart)
        {
            if (tempoStart.ToLower() == "concentric" || tempoStart.ToLower() == "eccentric" || tempoStart.ToLower() == "none")
            {
                // Try to parse MuscleGroup1 and MuscleGroup2 strings into MuscleGroup enums
                if (!Enum.TryParse<Data.TempoStart>(tempoStart, out var tempoStartEnum))
                {
                    throw new ArgumentException($"Invalid TempoStart: {tempoStart}");
                }
                return (int)tempoStartEnum;
            }
            else
            {
                return 0;
            }

        }





    }
}
