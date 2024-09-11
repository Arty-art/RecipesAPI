using Chilkat;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using static GetRecipesAPI.RecipeAPI;
using static System.Net.Mime.MediaTypeNames;

namespace GetRecipesAPI
{
    public class MembershipAPI
    {
        public class Membership
        {

            public class ResponseMembership
            {
                public Content content { get; set; }
                public bool isSuccess { get; set; }
                public int responseCode { get; set; }
                public object errorMessage { get; set; }
            }

            public class Content
            {
                public string id { get; set; }
                public string? sku { get; set; }
                public string? name { get; set; }
                public string? description { get; set; }
                public string url { get; set; }
                public int? accessWeekLength { get; set; }
                public bool isCustom { get; set; }
                public bool forPurchase { get; set; }
                public int gender { get; set; }
                public DateTime? startDate { get; set; }
                public DateTime? endDate { get; set; }
                public float price { get; set; }
                public bool _new { get; set; }
                public int type { get; set; }
                public int equipment { get; set; }
                public int duration { get; set; }
                public int level { get; set; }
                public string trainingStyleId { get; set; }
                public string trainingStrengthId { get; set; }
                public string coachId { get; set; }
                public Portraitimage portraitImage { get; set; }
                public Landscapeimage landscapeImage { get; set; }
                public string[] relatedMembershipIds { get; set; }
                public string[] subAllMembershipIds { get; set; }
                public string[] focusIds { get; set; }
                public string[] splitIds { get; set; }
                public int[] locations { get; set; }
                public int[] goals { get; set; }
            }

            public class Portraitimage
            {
                public string fileName { get; set; }
                public string fileType { get; set; }
                public string fileUrl { get; set; }
            }

            public class Landscapeimage
            {
                public string fileName { get; set; }
                public string fileType { get; set; }
                public string fileUrl { get; set; }
            }

        }

        public static string tokenNew = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxYjZiYWU4MC1kZjgwLTRiYmItYmFiOS1iMjc2ZWZjZjc3OTYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjdhODRkNDBlLWY4MTEtNDZhNi0yNGI0LTA4ZGM0Y2I2MTdjMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiYXBpX2FjY2VzcyI6ImFsbCIsImV4cCI6MjUzNDAyMzAwODAwLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJNeUF1dGhTZXJ2ZXIifQ.-zS2UFlnwagZb7hvyz_nHGERa5wlClWACg8zfQoU6rM";
        public static string tokenOld = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIyZjZjZGU5NC1lMmVkLTQzN2MtOWU2ZC1hMzk1MTJlYjVjOTYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjdhODRkNDBlLWY4MTEtNDZhNi0yNGI0LTA4ZGM0Y2I2MTdjMiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiYXBpX2FjY2VzcyI6ImFsbCIsImV4cCI6MjUzNDAyMzAwODAwLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJNeUF1dGhTZXJ2ZXIifQ.O7FdnFPxX-WDiJDmJ2woAlaIROg16icdQR_JkqSpjx0";

        public static Membership.ResponseMembership ReqGetMembership(string id)
        {
            string url = $"transform-me-gateway-api.azurewebsites.net";
            HttpRequest req = new()
            {
                HttpVerb = "GET",
                Path = $"/workout/admin/membership/get-by-id/{id}",
                ContentType = "application/json"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {tokenNew}");



            Chilkat.Http http = new();
            var resp = http.SynchronousRequest(url, 443, true, req);
            var re = resp.StatusCode.ToString().StartsWith("2");
            var response = http.LastMethodSuccess
                ? JsonConvert.DeserializeObject<Membership.ResponseMembership>(resp.BodyStr ?? throw new Exception("Response body is null."))
                : throw new ArgumentException(http.LastErrorText);


            return response;
        }

        public static string ReqPutMembership(Membership.ResponseMembership? response)
        {
            string url = $"mcm-gateway-dev.azurewebsites.net";
            HttpRequest req = new()
            {
                HttpVerb = "PUT",
                Path = $"/workout/admin/membership/edit",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {tokenOld}");



            Http http = new();
            var resp = http.SynchronousRequest(url, 443, true, CreateMultiPartFormBody(req, response, ""));
            var respons = http.LastMethodSuccess
                ? resp.BodyStr ?? throw new Exception("Response body is null.")
                : throw new ArgumentException(http.LastErrorText);

            return respons;
        }

        public static HttpRequest CreateMultiPartFormBody(HttpRequest req, Membership.ResponseMembership? response, string imagePath)
        {

            req.AddParam("id", response.content.id);
            req.AddParam("sku", response.content.sku);
            req.AddParam("name", response.content.name);
            req.AddParam("description", response.content.description);
            req.AddParam("price", response.content.price.ToString());
            req.AddParam("url", response.content.url);
            req.AddParam("coachId", response.content.coachId);
            req.AddParam("startDate", response.content.startDate.ToString() ?? "");
            req.AddParam("endDate", response.content.endDate.ToString() ?? "");
            req.AddParam("equipment", response.content.equipment.ToString());
            req.AddParam("duration", response.content.duration.ToString());
            req.AddParam("level", response.content.level.ToString());
            req.AddParam("trainingStyleId", response.content.trainingStyleId);
            req.AddParam("trainingStrengthId", response.content.trainingStrengthId);
            req.AddParam("forPurchase", response.content.forPurchase.ToString());
            req.AddParam("accessWeekLength", response.content.accessWeekLength.ToString());
            req.AddParam("new", response.content._new.ToString());
            req.AddParam("gender", response.content.gender.ToString());
            req.AddParam("type", response.content.type.ToString());
            req.AddParam("membershipType", response.content.type.ToString());
            AddingLocations(req, response);
            AddingGoals(req, response);
            AddingRelatedMembershipIds(req, response);
            AddingSubAllMembershipIds(req, response);
            AddingFocusIds(req, response);
            AddingSplitIds(req, response);
            string pathToFileOnDiskP = $@"D:\New Memberships\MembershipImages/{response.content.name.Replace(' ', '_')}_portrait.png" ?? String.Empty;
            string pathToFileOnDiskL = $@"D:\New Memberships\MembershipImages/{response.content.name.Replace(' ', '_')}_landscape.png" ?? String.Empty;
            bool success = req.AddFileForUpload("portraitImage", pathToFileOnDiskP);
            if (success != true)
            {
                throw new ArgumentException(req.LastErrorText);
            }
            success = req.AddFileForUpload("landscapeImage", pathToFileOnDiskL);
            if (success != true)
            {
                throw new ArgumentException(req.LastErrorText);
            }

            Console.WriteLine($"portraitImage: {pathToFileOnDiskP}");
            Console.WriteLine($"landscapeImage: {pathToFileOnDiskL}");

            Console.WriteLine($"\r\n");

            return req;
        }

        public static HttpRequest AddingLocations(HttpRequest req, Membership.ResponseMembership? response)
        {
            if (response.content.locations.Length > 0)
            {
                foreach (var location in response.content.locations)
                {
                    req.AddParam($"locations", location.ToString());
                }
            }
            else
            {
                req.AddParam($"locations", "[]");
            }

            return req;
        }

        public static HttpRequest AddingGoals(HttpRequest req, Membership.ResponseMembership? response)
        {
            if (response.content.goals.Length >0)
            {
                foreach (var goal in response.content.goals)
                {
                    req.AddParam($"goals", goal.ToString().Trim());
                }
            }
            else
            {
                req.AddParam($"goals", "[]");
            }

            return req;
        }

        public static HttpRequest AddingRelatedMembershipIds(HttpRequest req, Membership.ResponseMembership? response)
        {
            if(response.content.relatedMembershipIds.Length > 0)
            {
                foreach (var relatedMembershipId in response.content.relatedMembershipIds)
                {
                    req.AddParam($"relatedMembershipIds", relatedMembershipId.ToString());
                }
            }
            else
            {
                req.AddParam($"relatedMembershipIds", "[]");
            }

            return req;
        }

        public static HttpRequest AddingSubAllMembershipIds(HttpRequest req, Membership.ResponseMembership? response)
        {
            if(response.content.subAllMembershipIds.Length > 0)
            {
                foreach (var subAllMembershipId in response.content.subAllMembershipIds)
                {
                    req.AddParam($"subAllMembershipIds", subAllMembershipId.ToString());
                }
            }
            else
            {
                req.AddParam($"subAllMembershipIds", "[]");
            }

            return req;
        }

        public static HttpRequest AddingFocusIds(HttpRequest req, Membership.ResponseMembership? response)
        {
            if(response.content.focusIds.Length > 0)
            {
                foreach (var focusId in response.content.focusIds)
                {
                    req.AddParam($"focusIds", focusId.ToString());
                }
            }
            else
            {
                req.AddParam($"focusIds", "[]");
            }

            return req;
        }

        public static HttpRequest AddingSplitIds(HttpRequest req, Membership.ResponseMembership? response)
        {
            if (response.content.focusIds.Length > 0)
            {
                foreach(var split in response.content.splitIds)
                {
                    req.AddParam($"splitIds", split.ToString());
                }
            }
            else
            {
                req.AddParam($"splitIds", "[]");;
            }

            return req;
        }

    }
}
