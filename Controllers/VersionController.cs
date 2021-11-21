using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace VersionManagement.Controllers
{

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
        public async Task<List<Projet>> GetProjetList()
        {
            var result = await db.Projet.Include(p=>p.Versions).ToListAsync();
            return result;
        }

        [HttpGet]
        public async Task<List<Environment>> GetEnvironmentList()
        {
            var result = await db.Environment.ToListAsync();
            return result;
        }


        [HttpGet]
        public List<dynamic> GetVersionList()
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
                                            select new{
                                                Id = p.Id,
                                                Name = p.Name,
                                                Description = p.Description,
                                                ProjetEnvironment = (from pe in db.ProjetEnvironment
                                                                     join e in db.Environment on pe.EnvironmentId equals e.Id
                                                                     where pe.ProjetId == p.Id
                                                                     select new
                                                                     {
                                                                         EnvironmentId = e.Id,
                                                                         Descriptino = pe.Description,
                                                                         EnvironmentName = e.Name,
                                                                         DeploimentHistory = db.DeploimentHistory.Where(p=>p.ProjetEnvironmentId == pe.Id).OrderByDescending(p=>p.CreatedOn).ToList()

                                                                     }).ToList()
                                            }).ToList(),

                              VersionInfo = db.Version.Where(x => x.ProjetId == pp.Id).OrderByDescending(x => x.CreatedOn).FirstOrDefault()
                          }).ToList<dynamic>();
            return result;
        }


        public class CreateProjetCriteria:Projet
        {
            public List<long> EnvIds { get; set; }
        }

        [HttpPost]
        public async Task<long> CreateProjet([FromBody] CreateProjetCriteria projet)
        {
            if (projet.Id > 0)
            {

            }
            else
            {
                projet.CreatedOn = DateTime.Now;
            }
            db.Update(projet);
            await db.SaveChangesAsync();



            if (projet.ParentId == null)
            {
                var subProjetList = await db.Projet.Where(p => p.ParentId == projet.Id).ToListAsync();

                var projetEnvironmentToRemove = await (from pe in db.ProjetEnvironment
                                                       where subProjetList.Select(p=>p.Id).Contains(pe.ProjetId)
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
            return projet.Id;
        }

        [HttpPost]
        public async Task<long> CreateVersion([FromBody]Version version)
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
            var version = db.Version.Where(p=>p.VersionNumber == VersionNumber).FirstOrDefault();
            var projet = db.Projet.Where(p=>p.Name == ProjetName).FirstOrDefault();
            var env = db.Environment.Where(p=>p.Name == EnvironmentName).FirstOrDefault();

            ProjetEnvironment projetEnv = null;
            if(projet!=null && env!=null){
                projetEnv = db.ProjetEnvironment.Where(p=>p.EnvironmentId == env.Id && p.ProjetId == projet.Id).FirstOrDefault();
            }

            if(version!=null && projetEnv!=null){
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
