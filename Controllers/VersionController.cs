using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace VersionManagement.Controllers
{
    [Route("[controller]/{action}/{id?}")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly VersionManagementContext db;
        private readonly ILogger<VersionController> _logger;

        public VersionController(ILogger<VersionController> logger, VersionManagementContext context)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        public async Task<List<Version>> GetVersionByProjetId(long? ProjetId, long? ParentId)
        {
            var result = await (from v in db.Version
                                join p in db.Projet on v.ProjetId equals p.Id
                                where (ProjetId == null || p.Id== ProjetId) && (ParentId == null || p.ParentId == ParentId)
                                orderby v.CreatedOn descending
                                select v).ToListAsync();
            return result;
        }

        [HttpGet]
        public async Task<List<dynamic>> GetProjetList()
        {
            var result =  await (from p in db.Projet
                            select new
                            {
                                p.Id,
                                p.CreatedBy,
                                p.CreatedOn,
                                p.Description,
                                p.ParentId,
                                p.Name,
                                Versions = p.Versions.OrderByDescending(p => p.CreatedOn).ToList(),
                                EnvironmentList = (from e in db.Environment
                                                   join pe in db.ProjetEnvironment on e.Id equals pe.EnvironmentId
                                                   join pr in db.Projet on pe.ProjetId equals pr.Id
                                                   orderby e.Order
                                                   where (p.ParentId ==null && pr.ParentId == p.Id) || (p.ParentId!=null && pr.Id == p.Id )
                                                   select e).ToList()
                            }).ToListAsync<dynamic>();

            return result;
        }

        [HttpGet]
        public async Task<List<Environment>> GetEnvironmentList()
        {
            var result = await db.Environment.OrderBy(p=>p.Order).ToListAsync();
            return result;
        }

        [HttpGet]
        public async Task<List<dynamic>> GetDeploimentHistory(long? ProjetId, long? ParentId)
        {
            return await (from d in db.DeploimentHistory
                          join v in db.Version on d.VersionId equals v.Id
                          join pe in db.ProjetEnvironment on d.ProjetEnvironmentId equals pe.Id
                          join e in db.Environment on pe.EnvironmentId equals e.Id
                          join p in db.Projet on pe.ProjetId equals p.Id
                          where (ProjetId == null || p.Id == ProjetId) && (ParentId == null || p.ParentId == ParentId)
                          orderby p.Id, v.Id, d.CreatedOn, e.Order
                          select new
                          {
                              Id = d.Id,
                              ProjetId = p.Id,
                              ProjetName = p.Name,
                              VersionNumber = v.VersionNumber,
                              VersionId = v.Id,
                              CreatedOn = d.CreatedOn,
                              EnvironmentId = e.Id,
                              EnvironmentName = e.Name
                          }).ToListAsync<dynamic>();
        }

        [HttpGet]
        public List<dynamic> GetVersionList()
        {
            try
            {
                var result = (from pp in db.Projet
                              where pp.ParentId == null || pp.ParentId == -1
                              select new
                              {
                                  Id = pp.Id,
                                  Name = pp.Name,
                                  Description = pp.Description,
                                  SubProjet = (from p in db.Projet
                                               where p.ParentId == pp.Id
                                               select new
                                               {
                                                   Id = p.Id,
                                                   Name = p.Name,
                                                   Description = p.Description,
                                                   ProjetEnvironment = (from pe in db.ProjetEnvironment
                                                                        join e in db.Environment on pe.EnvironmentId equals e.Id
                                                                        orderby pe.Id, e.Order
                                                                        where pe.ProjetId == p.Id
                                                                        select new
                                                                        {
                                                                            EnvironmentId = e.Id,
                                                                            Descriptino = pe.Description,
                                                                            EnvironmentName = e.Name,
                                                                            DeploimentHistory = db.DeploimentHistory.Where(p => p.ProjetEnvironmentId == pe.Id).Select(p => new {
                                                                                Id = p.Id,
                                                                                EnvironmentId = p.ProjetEnvironment.EnvironmentId,
                                                                                CreatedOn = p.CreatedOn,
                                                                                VersionNumber = p.Version.VersionNumber,
                                                                                VersionId = p.VersionId
                                                                            }).OrderByDescending(p => p.CreatedOn).ToList()

                                                                        }).ToList()
                                               }).ToList(),

                                  VersionInfo = db.Version.Where(x => x.ProjetId == pp.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault()
                              }).ToList<dynamic>();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
           
        }


        public class CreateProjetCriteria : Projet
        {
            public List<long> EnvIds { get; set; }
        }

        [HttpPost]
        public async Task<long> CreateProjet([FromBody] CreateProjetCriteria projet)
        {
            var projetToSave = new Projet();
            if (projet.Id > 0)
            {
                projetToSave.UpdatedOn = DateTime.Now;
                projetToSave.Id = projet.Id;
            }
            else
            {
                projetToSave.Id = 0;
                projetToSave.CreatedOn = DateTime.Now;
            }
            projetToSave.Name = projet.Name;
            projetToSave.Description = projet.Description;
            projetToSave.ParentId = projet.ParentId;



            db.Update(projetToSave);
            await db.SaveChangesAsync();

            if (projetToSave.ParentId == null && projet.EnvIds.Count() > 0)
            {
                var subProjetList = await db.Projet.Where(p => p.ParentId == projetToSave.Id).ToListAsync();

                var projetEnvironmentToRemove = await (from pe in db.ProjetEnvironment
                                                       where subProjetList.Select(p => p.Id).Contains(pe.ProjetId)
                                                       select pe).ToListAsync();
                db.RemoveRange(projetEnvironmentToRemove);

                if (projet.EnvIds.Count() > 0)
                {
                    foreach (var sub in subProjetList)
                    {
                        foreach (var envId in projet.EnvIds)
                        {
                            var projetEnv = new ProjetEnvironment();

                            projetEnv.ProjetId = sub.Id;
                            projetEnv.EnvironmentId = envId;

                            db.ProjetEnvironment.Add(projetEnv);
                        }
                    }

                    await db.SaveChangesAsync();
                }
            }
            return projetToSave.Id;
        }

        [HttpPost]
        public async Task<long> CreateVersion([FromBody] Version version)
        {

            if (version.Id > 0)
            {

            }
            else
            {
                version.CreatedOn = DateTime.Now;
            }

            db.Update(version);
            await db.SaveChangesAsync();
            return version.Id;
        }

        [HttpPost]
        public async Task<long> CreateProjetEnvironment(ProjetEnvironment projetEnvironment)
        {

            if (projetEnvironment.Id > 0)
            {

            }
            else
            {
                projetEnvironment.CreatedOn = DateTime.Now;
            }

            db.Update(projetEnvironment);
            await db.SaveChangesAsync();
            return projetEnvironment.Id;
        }


        [HttpGet]
        public async Task<long> InsertDeploimentHistory(string VersionNumber, string ProjetName, string EnvironmentName)
        {
            // Add a token to prevent anoynome request 
            var version = db.Version.Where(p => p.VersionNumber == VersionNumber).FirstOrDefault();
            var projet = db.Projet.Where(p => p.Name == ProjetName).FirstOrDefault();
            var env = db.Environment.Where(p => p.Name == EnvironmentName).FirstOrDefault();

            ProjetEnvironment projetEnv = null;
            if (projet != null && env != null)
            {
                projetEnv = db.ProjetEnvironment.Where(p => p.EnvironmentId == env.Id && p.ProjetId == projet.Id).FirstOrDefault();
            }

            if (version != null && projetEnv != null)
            {
                var history = new DeploimentHistory();

                history.CreatedOn = DateTime.Now;

                history.VersionId = version.Id;
                history.ProjetEnvironmentId = projetEnv.Id;

                await db.DeploimentHistory.AddAsync(history);
                await db.SaveChangesAsync();

                return history.Id;
            }
            return -1;
        }
    }
}
