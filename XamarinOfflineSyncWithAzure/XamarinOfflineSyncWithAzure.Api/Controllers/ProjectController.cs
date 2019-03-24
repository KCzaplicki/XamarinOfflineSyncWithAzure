using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using XamarinOfflineSyncWithAzure.Api.DataObjects;
using XamarinOfflineSyncWithAzure.Api.Models;

namespace XamarinOfflineSyncWithAzure.Api.Controllers
{
    public class ProjectController : TableController<Project>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new XamarinOfflineSyncWithAzureContext();
            DomainManager = new EntityDomainManager<Project>(context, Request);
        }

        // GET tables/Project
        public IQueryable<Project> GetAllProjects()
        {
            return Query();
        }

        // GET tables/Project/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Project> GetProject(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Project/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Project> PatchProject(string id, Delta<Project> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Project
        public async Task<IHttpActionResult> PostTodoItem(Project item)
        {
            var current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Project/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteProject(string id)
        {
            return DeleteAsync(id);
        }
    }
}