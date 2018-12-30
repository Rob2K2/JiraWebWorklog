using JiraJWL.Modelos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JiraJWL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Project> projects = new List<Project>();
            projects = GetProjects();
            ViewBag.Projects = projects;

            return View();
        }

        [HttpPost]
        public ActionResult Index(string project)
        {
            List<Project> projects = new List<Project>();
            projects = GetProjects();
            ViewBag.Projects = projects;

            return View(GetIssues(project));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private List<Project> GetProjects()
        {
            string name = Session["name"].ToString();
            string value = Session["value"].ToString();
            var client = new RestClient("https://meng-mod-04.atlassian.net/rest/api/2/project");
            var request = new RestRequest(Method.GET);
            request.AddCookie(name, value);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Project>>(request);

            return response.Data;
        }

        private List<Issue> GetIssues(string project)
        {
            string name = Session["name"].ToString();
            string value = Session["value"].ToString();
            var client = new RestClient("https://meng-mod-04.atlassian.net/rest/api/2/search?jql=project=" + project);
            var request = new RestRequest(Method.GET);
            request.AddCookie(name, value);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<IssueList>>(request);

            return response.Data[0].Issues;
        }

        [HttpPost]
        public ActionResult Issue(string issueKey)
        {
            return View(GetIssue(issueKey));
        }

        [HttpPost]
        public ActionResult AddWorklog(string issueKey, string timeSpent, string timeOriginalEstimate, string seconds)
        {
            string name = Session["name"].ToString();
            string value = Session["value"].ToString();
            bool res = false;

            long lngTimeSpent = 0;
            res = long.TryParse(timeSpent, out lngTimeSpent);
            if (!res)
            {
                lngTimeSpent = 0;
            }

            long lngTimeOriginalEstimate = long.Parse(timeOriginalEstimate);
            long segundos = long.Parse(seconds);

            if (segundos + lngTimeSpent >= lngTimeOriginalEstimate + lngTimeOriginalEstimate * 0.1)
            {
                TempData["alert"] = true;
            }

            string fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff-0400");

            var client = new RestClient("https://meng-mod-04.atlassian.net/rest/api/2/issue/" + issueKey + "/worklog");
            var request = new RestRequest(Method.POST);
            request.AddCookie(name, value);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n\"comment\": {\r\n    \"type\": \"doc\",\r\n    \"version\": 1,\r\n    \"content\": [\r\n      {\r\n        \"type\": \"paragraph\",\r\n        \"content\": [\r\n          {\r\n            \"type\": \"text\",\r\n            \"text\": \"Increasing time\"\r\n          }\r\n        ]\r\n      }\r\n    ]\r\n  },\r\n  \"started\": \"" + fecha + "\",\r\n  \"timeSpentSeconds\": " + segundos + "\r\n}",
                ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode.ToString() == "Created")
            {
                TempData["successful"] = issueKey;
            }

            //return RedirectToAction("Index");
            return View("Issue", GetIssue(issueKey));
        }

        private Issue GetIssue(string issueKey)
        {
            string name = Session["name"].ToString();
            string value = Session["value"].ToString();

            var client = new RestClient("https://meng-mod-04.atlassian.net/rest/api/2/issue/" + issueKey);
            var request = new RestRequest(Method.GET);
            request.AddCookie(name, value);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<Issue>(request);

            return response.Data;
        }
    }
}