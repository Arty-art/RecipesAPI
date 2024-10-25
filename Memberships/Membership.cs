using Chilkat;
using GetRecipesAPI.Helpers;
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
                    public string id { get; set; }
                    public string sku { get; set; }
                    public string name { get; set; }
                    public string shortDescription { get; set; }
                    public string longDescription { get; set; }
                    public string url { get; set; }
                    public int accessWeekLength { get; set; }
                    public bool isCustom { get; set; }
                    public bool forPurchase { get; set; }
                    public int gender { get; set; }
                    public DateTime? startDate { get; set; }
                    public DateTime? endDate { get; set; }
                    public float price { get; set; }
                    public bool _new { get; set; }
                    public int type { get; set; }
                    public int duration { get; set; }
                    public int level { get; set; }
                    public int trainingsPerWeek { get; set; }
                    public string trainingStyleId { get; set; }
                    public string trainingStrengthId { get; set; }
                    public string coachId { get; set; }
                    public string portraitImageUrl { get; set; }
                    public string landscapeImageUrl { get; set; }
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

        
        public static Membership.ResponseMembership ReqGetMembership(string id)
        {
            HttpRequest req = new()
            {
                HttpVerb = "GET",
                Path = $"/workout/admin/membership/get-by-id/{id}",
                ContentType = "application/json"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {Data.tokenNew}");



            Chilkat.Http http = new();
            var resp = http.SynchronousRequest(Data.url, 443, true, req);
            var re = resp.StatusCode.ToString().StartsWith("2");
            var response = http.LastMethodSuccess
                ? JsonConvert.DeserializeObject<Membership.ResponseMembership>(resp.BodyStr ?? throw new Exception("Response body is null."))
                : throw new ArgumentException(http.LastErrorText);


            return response;
        }

        public static string ReqPutMembership(Membership.ResponseMembership? response)
        {
            HttpRequest req = new()
            {
                HttpVerb = "PUT",
                Path = $"/workout/admin/membership/edit",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            req.AddHeader("Authorization", $"Bearer {Data.tokenOld}");

            Http http = new();
            var resp = http.SynchronousRequest(Data.url, 443, true, CreateMultiPartFormBody(req, response, ""));
            var respons = http.LastMethodSuccess
                ? resp.BodyStr ?? throw new Exception("Response body is null.")
                : throw new ArgumentException(http.LastErrorText);

            return respons;
        }

        public static HttpRequest CreateMultiPartFormBody(HttpRequest req, Membership.ResponseMembership? response, string imagePath)
        {
            req.AddParam("id", response.id);
            req.AddParam("sku ", response.sku);
            req.AddParam("name", response.name);
            req.AddParam("shortDescription", response.shortDescription);
            req.AddParam("longDescription ", response.longDescription);
            req.AddParam("url ", response.url);
            req.AddParam("accessWeekLength", response.accessWeekLength.ToString());
            req.AddParam("isCustom", response.isCustom.ToString());
            req.AddParam("forPurchase ", response.forPurchase.ToString());
            req.AddParam("gender", response.gender.ToString());
            req.AddParam("startDate ", response.startDate.ToString() ?? "");
            req.AddParam("endDate ", response.endDate.ToString() ?? "");
            req.AddParam("price ", response.price.ToString());
            req.AddParam("new ", response._new.ToString());
            req.AddParam("type", response.type.ToString());
            req.AddParam("duration", response.duration.ToString());
            req.AddParam("level ", response.level.ToString());
            req.AddParam("trainingsPerWeek", response.trainingsPerWeek.ToString());
            req.AddParam("trainingStyleId ", response.trainingStyleId);
            req.AddParam("trainingStrengthId", response.trainingStrengthId);
            req.AddParam("coachId ", response.coachId);
            AddingLocations(req, response);
            AddingGoals(req, response);
            AddingRelatedMembershipIds(req, response);
            AddingSubAllMembershipIds(req, response);
            AddingFocusIds(req, response);
            AddingSplitIds(req, response);


            string pathToFileOnDiskP = $@"D:\New Memberships\WhiteLabel\memberships\Male\{response.name}_portreit.jpg" ?? String.Empty;
            string pathToFileOnDiskL = $@"D:\New Memberships\WhiteLabel\memberships\Male\{response.name}_landscape.jpg" ?? String.Empty;
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
            if (response.locations.Length > 0)
            {
                foreach (var location in response.locations)
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
            if (response.goals.Length > 0)
            {
                foreach (var goal in response.goals)
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
            if (response.relatedMembershipIds.Length > 0)
            {
                foreach (var relatedMembershipId in response.relatedMembershipIds)
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
            if (response.subAllMembershipIds.Length > 0)
            {
                foreach (var subAllMembershipId in response.subAllMembershipIds)
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
            if (response.focusIds.Length > 0)
            {
                foreach (var focusId in response.focusIds)
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
            if (response.focusIds.Length > 0)
            {
                foreach (var split in response.splitIds)
                {
                    req.AddParam($"splitIds", split.ToString());
                }
            }
            else
            {
                req.AddParam($"splitIds", "[]"); ;
            }

            return req;
        }

    }
}
