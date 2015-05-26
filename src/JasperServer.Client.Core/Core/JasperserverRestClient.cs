using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JasperServer.Client.Core
{
    public class JasperserverRestClient
    {
        private string baseurl;
        private AuthenticationHeaderValue authentication;

        /// <summary>
        /// Util to retrieve a report from JasperServer.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password for Username</param>
        /// <param name="baseurl">JasperReports</param>
        /// <example>
        /// JasperserverRestClient jasperserverRestClient = new JasperserverRestClient("username", "password", "http://localhost:8080/jasperserver/rest_v2/reports");
        /// Stream stream = jasperserverRestClient.Get("/reports/SGV/Total.pdf");
        /// 
        /// // or
        /// 
        /// JasperserverRestClient jasperserverRestClient = new JasperserverRestClient("username", "password", "http://localhost:8080/jasperserver/rest_v2/reports");
        /// 
        /// Dictionary<string, string> parameters = new Dictionary<string, string>();
        /// parameters.Add("PARAM1", "VALUE1");
        /// parameters.Add("PARAM2", "VALUE2");
        /// 
        /// Stream stream = jasperserverRestClient.Get("/reports/SGV/Total.pdf", parameters);
        /// </example>
        public JasperserverRestClient(string username, string password, string baseurl)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new NullReferenceException("Username could not be empty.");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new NullReferenceException("Password could not be empty.");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new NullReferenceException("Base URL could not be empty.");
            }

            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            var encoded = Convert.ToBase64String(byteArray);
            this.authentication = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encoded);

            this.baseurl = baseurl;
        }

        /// <summary>
        /// Retrieve the stream of report asked.
        /// </summary>
        /// <param name="report">Report path at server</param>
        /// <returns>Stream of report asked</returns>
        public Stream Get(string report)
        {
            return this.Get(report, null);
        }

        /// <summary>
        /// Retrieve the stream of report asked using parameters.
        /// </summary>
        /// <param name="report">Report path at server</param>
        /// <param name="parameters">Parameters of report</param>
        /// <returns>Stream of report asked using parameters</returns>
        public Stream Get(string report, Dictionary<string, string> parameters)
        {
            if (String.IsNullOrEmpty(report))
            {
                throw new NullReferenceException("Report could not be empty.");
            }

            string url = this.baseurl + report;

            if ((parameters != null) && (parameters.Count > 0))
            {
                List<String> items = new List<String>();

                foreach (var pair in parameters)
                {
                    items.Add(String.Concat(pair.Key, "=", HttpUtility.UrlEncode(pair.Value)));
                }

                url += "?" + String.Join("&", items.ToArray()); ;
            }

            MemoryStream ms = new MemoryStream();
            this.GetTask(url, ms);

            return ms;
        }

        /// <summary>
        /// Retrieve a task of url.
        /// </summary>
        /// <param name="url">Url to retrieve content</param>
        /// <param name="stream">Stream to save the url content</param>
        private async void GetTask(string url, Stream stream)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = authentication;

            var task = client.GetStreamAsync(url);
            Task.WaitAll(task);

            task.Result.CopyTo(stream);
        }

        /// <summary>
        /// Save report to a file path.
        /// </summary>
        /// <param name="report">Report path at server</param>
        /// <param name="filename">File full path to output stream</param>
        public void SaveToFile(string report, string filename)
        {
            this.SaveToFile(report, null, filename);
        }

        /// <summary>
        /// Save report to a file path using parameters.
        /// </summary>
        /// <param name="report">Report path at server</param>
        /// <param name="parameters">Parameters of report</param>
        /// <param name="filename">File full path to output stream</param>
        public void SaveToFile(string report, Dictionary<string, string> parameters, string filename)
        {
            var stream = this.Get(report, parameters);

            stream.Seek(0, SeekOrigin.Begin);

            Stream output = File.OpenWrite(filename);
            stream.CopyTo(output);
            output.Close();
        }
    }
}
