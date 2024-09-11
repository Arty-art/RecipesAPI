using Chilkat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GetRecipesAPI.MembershipAPI;

namespace GetRecipesAPI
{
    public class ExerciseAPI
    {
        public static string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyZjZjZGU5NC1lMmVkLTQzN2MtOWU2ZC1hMzk1MTJlYjVjOTYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjdhODRkNDBlLWY4MTEtNDZhNi0yNGI0LTA4ZGM0Y2I2MTdjMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiYXBpX2FjY2VzcyI6ImFsbCIsImV4cCI6MjUzNDAyMzAwODAwLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJNeUF1dGhTZXJ2ZXIifQ.O7FdnFPxX-WDiJDmJ2woAlaIROg16icdQR_JkqSpjx0";
        public static Exercise.ResponseExercise? ReqGetExercise(string id)
        {
            string url = $"transform-me-gateway-api.azurewebsites.net";
            HttpRequest req = new()
            {
                HttpVerb = "GET",
                Path = $"/workout/admin/exercise/get-by-id/{id}",
                ContentType = "application/json"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {token}");



            Chilkat.Http http = new();
            var resp = http.SynchronousRequest(url, 443, true, req);
            var re = resp.StatusCode.ToString().StartsWith("2");
            var response = http.LastMethodSuccess
                ? JsonConvert.DeserializeObject<Exercise.ResponseExercise?>(resp.BodyStr ?? throw new Exception("Response body is null."))
                : throw new ArgumentException(http.LastErrorText);


            return response;
        }

        public static string ReqPutExercise(Exercise.ResponseExercise? response)
        {
            string url = $"mcm-gateway-dev.azurewebsites.net";
            HttpRequest req = new()
            {
                HttpVerb = "PUT",
                Path = $"/workout/admin/exercise/edit",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {token}");



            Http http = new();
            var resp = http.SynchronousRequest(url, 443, true, CreateMultiPartFormBody(req, response, ""));
            var respons = http.LastMethodSuccess
                ? resp.BodyStr ?? throw new Exception("Response body is null.")
                : throw new ArgumentException(http.LastErrorText);

            return respons;
        }

        public static HttpRequest CreateMultiPartFormBody(HttpRequest req, Exercise.ResponseExercise? response, string imagePath)
        {
            req.AddParam("Id", response.content.id);
            req.AddParam("name", response.content.name);
            req.AddParam("tempoBold", response.content.tempoBold.ToString());
            req.AddParam("videoUrl", response.content.videoUrl.ToString());
            AddingRelatedExercises(req, response);
            string pathToFileOnDiskP = $@"D:\NewVimeoVideos\Images\{response.content.videoUrl.Replace("https://player.vimeo.com/video/", "")}.jpg" ?? String.Empty;
            bool success = req.AddFileForUpload("thumbPhoto", pathToFileOnDiskP);
            if (success != true)
            {
                throw new ArgumentException(req.LastErrorText);
            }

            Console.WriteLine($"portraitImage: {pathToFileOnDiskP}");
            Console.WriteLine($"\r\n");

            return req;
        }

        public static HttpRequest AddingRelatedExercises(HttpRequest req, Exercise.ResponseExercise? response)
        {
            if (response.content.relatedExercises.Length > 0)
            {
                for (int i = 0; i < response.content.relatedExercises.Length; i++)
                {
                    req.AddParam($"relatedExercises[{i}].exerciseId", response.content.relatedExercises[i].exerciseId.ToString());
                    req.AddParam($"relatedExercises[{i}].priority", response.content.relatedExercises[i].priority.ToString());
                    req.AddParam($"relatedExercises[{i}].exerciseType", response.content.relatedExercises[i].exerciseType.ToString());
                }
            }
            else
            {
                req.AddParam($"relatedExercises", "[]");
            }



            return req;
        }

        public class Exercise
        {
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
        

    }
}
