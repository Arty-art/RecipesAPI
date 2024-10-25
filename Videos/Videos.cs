using Chilkat;
using GetRecipesAPI.Helpers;
using Newtonsoft.Json;

namespace GetRecipesAPI
{
    public class VideoApi
    {
        
        public static void ReqAddVideo(Videos video)
        {
            HttpRequest req = new()
            {
                HttpVerb = "POST",
                Path = $"/workout/admin/video/add",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            if (video.Educator.Trim() == "Lauren Simpson")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenLauren}");
            }
            else if (video.Educator.Trim() == "Lara Gya")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenLara}");
            }
            else if (video.Educator.Trim() == "Mark Carroll")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenMark}");
            }
            else
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenDefault}");
            }

            var createBool = CreateMultiPartFormBody(ref req, video);

            Chilkat.Http http = new();
            if (createBool)
            {
                var resp = http.SynchronousRequest(Data.url, 443, true, req);
                var respons = http.LastStatus.ToString().StartsWith("2")
                    ? resp.BodyStr ?? throw new Exception("Response body is null.")
                    : throw new ArgumentException(resp.BodyStr);
            }


        }

        public static void ReqEditVideo(List<Videos> video, GetVideoModel.Video videoResp)
        {
            HttpRequest req = new()
            {
                HttpVerb = "PUT",
                Path = $"/workout/admin/video/edit",
                ContentType = "multipart/form-data"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            var educator = video.FirstOrDefault(x => x.Video_Title.Equals(videoResp.name, StringComparison.OrdinalIgnoreCase))
                                ?.Educator?.Trim();

            // Check educator and add the appropriate authorization header
            if (educator == "Lauren Simpson")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenLauren}");
            }
            else if (educator == "Lara Gya")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenLara}");
            }
            else if (educator == "Mark Carroll")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenMark}");
            }
            else
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenDefault}");
            }

            var createBool = CreateMultiPartFormBody(ref req, videoResp, video);

            Chilkat.Http http = new();
            if (createBool)
            {
                var resp = http.SynchronousRequest(Data.url, 443, true, req);
                var respons = http.LastStatus.ToString().StartsWith("2")
                    ? resp.BodyStr ?? throw new Exception("Response body is null.")
                    : throw new ArgumentException(resp.LastErrorText);
            }


        }

        public static GetVideoModel.ResponseGetVideo ReqPostGetVideos(Videos video)
        {
            HttpRequest req = new()
            {
                HttpVerb = "POST",
                Path = $"/workout/admin/video/get-by-filter",
                ContentType = "application/json"
            };
            req.AddHeader("Connection", "Keep-Alive");
            req.AddHeader("accept-encoding", "gzip, deflate, br");
            if (video.Educator.Trim() == "Lauren Simpson")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenLauren}");
            }
            else if (video.Educator.Trim() == "Lara Gya")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenLara}");
            }
            else if (video.Educator.Trim() == "Mark Carroll")
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenMark}");
            }
            else
            {
                req.AddHeader("Authorization", $"Bearer {Data.tokenDefault}");
            }
            req.LoadBodyFromString(Body(), "utf-8");

            Chilkat.Http http = new();

            var resp = http.SynchronousRequest(Data.url, 443, true, req);
            var respons = http.LastMethodSuccess
                ? JsonConvert.DeserializeObject<GetVideoModel.ResponseGetVideo>(resp.BodyStr ?? throw new Exception("Response body is null."))
                : throw new ArgumentException(http.LastErrorText);


            return respons;
        }

        public static bool CreateMultiPartFormBody(ref HttpRequest req, Videos video)
        {
            req.AddParam("name", video.Video_Title);
            req.AddParam("description", video.Video_Title);
            req.AddParam("Url", video.Vimeo_Link_support_team_to_add);
            req.AddParam("IsForAllMemberships", (video.Program.ToLower() == "all" ? true : false).ToString());
            req.AddParam("IsDefault", "false");
            AddingCategories(ref req, video);
            AddingMemberships(ref req, video);

            (bool, string) pathToFileOnDisk = DownloadImagesHelper.DownloadImageForVideos(video).Result;
            if (pathToFileOnDisk.Item1)
            {
                req.AddFileForUpload("ThumbPhoto", pathToFileOnDisk.Item2);
            }

            return true;
        }

        public static bool CreateMultiPartFormBody(ref HttpRequest req, GetVideoModel.Video videoResp, List<Videos> videos)
        {
            List<string> listBody = new();
            req.AddParam("id", videoResp.id);
            req.AddParam("name", videoResp.name);
            req.AddParam("description", videoResp.description);
            req.AddParam("Url", videoResp.url);
            req.AddParam("IsForAllMemberships", videoResp.isForAllMemberships.ToString().ToLower());
            req.AddParam("IsDefault", videoResp.isDefault.ToString());

            listBody.Add(string.Join(':', "id", videoResp.id));
            listBody.Add(string.Join(':', "name", videoResp.name));
            listBody.Add(string.Join(':', "description", videoResp.description));
            listBody.Add(string.Join(':', "Url", videoResp.url));
            listBody.Add(string.Join(':', "IsForAllMemberships", videoResp.isForAllMemberships.ToString().ToLower()));
            listBody.Add(string.Join(':', "IsDefault", videoResp.isDefault.ToString()));


            AddingCategories(ref req, videoResp, videos, listBody);
            AddingMemberships(ref req, videoResp, listBody);

            string name = SanitizeFileName(videoResp.name)+".jpg";
            string saveDir = @"D:\New Memberships\WhiteLabel\Videos\Images";
            string savePath = Path.Combine(saveDir, name);
            if (File.Exists(savePath))
            {
                req.AddFileForUpload("ThumbPhoto", savePath);
                listBody.Add(string.Join(':', "ThumbPhoto", savePath));
            }
            _ = listBody;
            return true;
        }

        public static HttpRequest AddingCategories(ref HttpRequest req, Videos video)
        {
            if (video.Category_Navigation_Label.Split(',').Length > 0)
            {
                List<string> categoriesList = video.Category_Navigation_Label.Split(',').ToList();
                List<string> categoryIds = new();
                foreach (string category in categoriesList)
                {
                    CategoriesMappings.MapCategories(category, CategoriesMappings.Categories, categoryIds);
                }
                for (int i = 0; i < categoryIds.Count; i++)
                {
                    req.AddParam($"CategoryIds[{i}]", categoryIds[i]);
                }
            }
            else
            {
                req.AddParam("CategoryIds", "[]");
            }


            return req;
        }
        public static HttpRequest AddingMemberships(ref HttpRequest req, Videos video)
        {
            if (video.Program.Split(',').Length > 0 && video.Program.ToLower() != "all")
            {
                List<string> membershipList = video.Program.Split(',').ToList();

                for (int i = 0; i < membershipList.Count; i++)
                {
                    req.AddParam($"MembershipIds[{i}]", membershipList[i]);
                }
            }
            req.AddParam("MembershipIds", "[]");

            return req;
        }

        public static HttpRequest AddingCategories(ref HttpRequest req, GetVideoModel.Video video, List<Videos> videos, List<string> list)
        {
            var categoryList = videos.FirstOrDefault(x => x.Video_Title.Equals(video.name, StringComparison.OrdinalIgnoreCase))
                                ?.Category_Navigation_Label;
            if (video.categoryIds.Length > 0)
            {
                //for (int i = 0; i < video.categoryIds.Length; i++)
                //{
                //    req.AddParam($"CategoryIds[{i}]", video.categoryIds[i]);
                //    list.Add(string.Join(':', $"CategoryIds[{i}]", video.categoryIds[i]));
                //}

                List<string> categoriesList = categoryList.Split(',').ToList();
                List<string> categoryIds = new();
                foreach (string category in categoriesList)
                {
                    CategoriesMappings.MapCategories(category, CategoriesMappings.GetCatgories(video, videos), categoryIds, video, videos);
                }
                for (int i = 0; i < categoryIds.Count; i++)
                {
                    req.AddParam($"CategoryIds[{i}]", categoryIds[i]);
                    list.Add(string.Join(':', $"CategoryIds[{i}]", categoryIds[i]));
                }
            }
            else if (categoryList.Split(',').Length > 0)
            {
                List<string> categoriesList = categoryList.Split(',').ToList();
                List<string> categoryIds = new();
                foreach (string category in categoriesList)
                {
                    CategoriesMappings.MapCategories(category, CategoriesMappings.Categories, categoryIds, video, videos);
                }
                for (int i = 0; i < categoryIds.Count; i++)
                {
                    req.AddParam($"CategoryIds[{i}]", categoryIds[i]);
                    list.Add(string.Join(':', $"CategoryIds[{i}]", categoryIds[i]));
                }
            }
            else
            {
                req.AddParam("CategoryIds", "[]");
                list.Add(string.Join(':', $"CategoryIds", "[]"));
            }


            return req;
        }
        public static HttpRequest AddingMemberships(ref HttpRequest req, GetVideoModel.Video video, List<string> list)
        {
            if (video.membershipIds.Length > 0)
            {
                for (int i = 0; i < video.membershipIds.Length; i++)
                {
                    req.AddParam($"MembershipIds[{i}]", video.membershipIds[i]);
                    list.Add(string.Join(':', $"MembershipIds[{i}]", video.membershipIds[i]));
                }
            }
            else
            {
                req.AddParam("MembershipIds", "[]");
                list.Add(string.Join(':', $"MembershipIds", "[]"));
            }
            return req;
        }



        public static string Body()
        {
            GetVideoModel.RequestGetVideo body = new()
            {
                skip = 0,
                take = 999,
                query = "",
                categoryIds = [],
                tagIds = [],
                membershipIds = []
            };

            return JsonConvert.SerializeObject(body);
        }

        private static string SanitizeFileName(string fileName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '-'); // Replace invalid characters with hyphen or any other character
            }
            return fileName;
        }
    }

    public class CategoriesMappings
    {
        public static void MapCategories(string category, Dictionary<string, string> mappings, List<string> categoryIds)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                var trimmedTag = category.Trim();
                mappings.TryGetValue(trimmedTag, out var key);
                bool isTag = Categories.ContainsValue(key);
                // Check if the trimmedTag exists in the mappings and add corresponding tag ID
                if (isTag)
                {
                    categoryIds.Add(key);
                }
            }
        }

        public static void MapCategories(string category, Dictionary<string, string> mappings, List<string> categoryIds, GetVideoModel.Video video, List<Videos> videos)
        {
            if (!string.IsNullOrWhiteSpace(category))
            {
                var trimmedTag = category.Trim();
                mappings.TryGetValue(trimmedTag, out var key);
                bool isTag = GetCatgories(video, videos).ContainsValue(key);
                // Check if the trimmedTag exists in the mappings and add corresponding tag ID
                if (isTag)
                {
                    categoryIds.Add(key);
                }
            }
        }

        public static Dictionary<string, string> GetCatgories(GetVideoModel.Video video, List<Videos> videos)
        {
            var educator = videos.FirstOrDefault(x => x.Video_Title.Equals(video.name, StringComparison.OrdinalIgnoreCase))
                                ?.Educator;

            if (educator == "Lauren Simpson ")
            {
                return new Dictionary<string, string>
                {
                    {"Building", "83BCACA9-33A1-4203-8F50-4A8518EECA30"},
                    {"Fat Loss", "06E27FB3-FCCC-4176-978D-22A93DB76C79"},
                    {"Meal Prep", "332AE9D7-CCCB-4D4C-9FA3-F772C5E827EB"},
                    {"Mindset", "D384E986-BB81-4AC9-987C-8877E16E002B"},
                    {"Nutrition 101", "C55C06C9-0D90-4400-ABA6-8CD5ED9F9134"},
                    {"Training", "749DBC7A-514D-408B-AD9C-272CE7A56F36"},
                    {"Training Methods", "8EA8761F-525F-454B-AC91-91BAA9E88B6B"},
                    {"Training Technique", "C1F54F1E-2030-4F05-9C7C-6A8360A67766"},
                    {"Warming Up", "4A8A1E16-F56B-4853-B631-969254B89060"},
                    {"Women's Health", "84111B9F-2274-4A6A-96E9-7C49DB94445A"}
                };
            }
            else if(educator == "Lara Gya") 
            {
                return new Dictionary<string, string>
                {
                    {"Building", "0B41C7EC-6252-4300-B033-2CE44EB5B362"},
                    {"Fat Loss", "D6472A3A-F02F-4A31-B15D-0E4A201A431E"},
                    {"Meal Prep", "93991E44-1AF3-496D-BA44-B2C758644F9A"},
                    {"Mindset", "28B736EA-50C6-4922-AB27-C1A77CFAF1D8"},
                    {"Nutrition 101", "6C6AAC12-B3CA-4EF1-8E3A-0801619F9C67"},
                    {"Training", "DF12681A-2AAE-45A3-A2A7-FA1574A03F18"},
                    {"Training Methods", "808D865A-BDE2-4BFF-998A-0662C594749C"},
                    {"Training Technique", "2C27FAAE-65E2-4ADC-98E2-3B3B4114278C"},
                    {"Warming Up", "D0346A48-1E59-40B2-A318-08F88798E617"},
                    {"Women's Health", "5C521556-1B36-408F-AE40-89BC4B8C2859"}
                };
            }
            else if (educator == "Mark Carroll")
            {
                return new Dictionary<string, string>
                {
                    {"Building", "88AF6E6A-33FD-4DDD-AE71-500B6DD2470C"},
                    {"Fat Loss", "D1C1C536-AAC7-4240-AE0D-EA9C9E9AA0D4"},
                    {"Meal Prep", "6AE53E89-ECA2-4937-B505-8917A64FCC06"},
                    {"Mindset", "4DF05347-305C-4142-AB54-1D595DAB034B"},
                    {"Nutrition 101", "DBF58428-1F96-40FD-B330-1BE4E8E95083"},
                    {"Training", "FE568A8C-4C45-498B-A836-25B3A97B801C"},
                    {"Training Methods", "CE293FF8-27A2-4731-A31B-0FCB057BD118"},
                    {"Training Technique", "F1EC59AD-F217-4889-9CFD-D142F54E353E"},
                    {"Warming Up", "A01751CA-11F5-4AAC-9B71-58EFC9E5CB2A"},
                    {"Women's Health", "14D6AD62-1D92-4616-897A-2E1677643903"}
                };
            }
            else
            {
                return new Dictionary<string, string>
                {
                    {"Building", "91B40433-F9DB-463C-9E69-DD2601CD2521"},
                    {"Fat Loss", "BD53E0FC-383A-4E10-B415-C6B50D9F8F48"},
                    {"Meal Prep", "CEFE9643-9CC8-49CA-AEE3-A973EF855606"},
                    {"Mindset", "6D414BAB-6229-4A85-A183-B845F690AFCD"},
                    {"Nutrition 101", "9B5C4DB9-4CBC-40EF-ABDF-B7783CF7BE6C"},
                    {"Training", "61CA1923-0517-40F9-9ED2-DC0409410B39"},
                    {"Training Methods", "5318370A-A21C-4B35-9A92-FE1E458D4903"},
                    {"Training Technique", "19A68109-CF73-4C4A-82D3-52A3E4F81BF6"},
                    {"Warming Up", "2EF6E970-68A9-4F43-8D8B-51D568CADB3D"},
                    {"Women's Health", "C805578B-F617-4D5D-8713-CF9B33F50BEA"}
                };
            }
        }

        public static Dictionary<string, string> Categories = new Dictionary<string, string>
        {
            { "Warming Up", "2EF6E970-68A9-4F43-8D8B-51D568CADB3D" },
            { "Training Technique", "19A68109-CF73-4C4A-82D3-52A3E4F81BF6" },
            { "Meal Prep", "CEFE9643-9CC8-49CA-AEE3-A973EF855606" },
            { "Nutrition 101", "9B5C4DB9-4CBC-40EF-ABDF-B7783CF7BE6C" },
            { "Mindset", "6D414BAB-6229-4A85-A183-B845F690AFCD" },
            { "Fat Loss", "BD53E0FC-383A-4E10-B415-C6B50D9F8F48" },
            { "Women's Health", "C805578B-F617-4D5D-8713-CF9B33F50BEA" },
            { "Training", "61CA1923-0517-40F9-9ED2-DC0409410B39" },
            { "Building", "91B40433-F9DB-463C-9E69-DD2601CD2521" },
            { "Training Methods","5318370a-a21c-4b35-9a92-fe1e458d4903" }
        };
    }

    public class Videos
    {
        public string? Video_Title { get; set; }
        public string? Program { get; set; }
        public string? Category_Navigation_Label { get; set; }
        public string? Drip_Feed_Week { get; set; }
        public string? Educator { get; set; }
        public string? Video_Screenshot_raja { get; set; }
        public string? Vimeo_Link_support_team_to_add { get; set; }
        public string? Video_link_dropbox__QA_ignore { get; set; }
    }

    public class GetVideoModel
    {

        public class RequestGetVideo
        {
            public int skip { get; set; }
            public int take { get; set; }
            public string query { get; set; }
            public string[] categoryIds { get; set; }
            public string[] tagIds { get; set; }
            public string[] membershipIds { get; set; }
        }

        public class ResponseGetVideo
        {
            public int totalAmount { get; set; }
            public Video[] videos { get; set; }
        }

        public class Video
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public string thumbPhotoUrl { get; set; }
            public bool isForAllMemberships { get; set; }
            public string[] categoryIds { get; set; }
            public string[] membershipIds { get; set; }
            public bool isDefault { get; set; }
        }

    }
}
