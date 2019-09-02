using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using HDA.Core.Models.HDACore;

namespace HDA.Core.Controllers
{
    public class HealthFacilitiesController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        
        public IHttpActionResult GetHealthFacilities()
        {
            List<HealthFacilityVM> healthFacilities = new List<HealthFacilityVM>();
            var hfs = db.HealthFacilities;
            foreach(var hf in hfs)
            {
                HealthFacilityVM h = new HealthFacilityVM();
                h.ID = hf.HealthFacilityID;
                h.HealthFacilityName = hf.HealthFacilityNameEn;
                healthFacilities.Add(h);
            }
            return Ok(healthFacilities);
        }
        
        public IHttpActionResult GetHealthFacilities(int id)
        {
            List<HealthFacilityVM> healthFacilities = new List<HealthFacilityVM>();
            var hfs = db.HealthFacilities.Where(i => i.HealthFacilityTypeID == id);
            foreach (var hf in hfs)
            {
                HealthFacilityVM h = new HealthFacilityVM();
                h.ID = hf.HealthFacilityID;
                h.HealthFacilityName = hf.HealthFacilityNameEn;
                healthFacilities.Add(h);
            }
            return Ok(healthFacilities);
        }

        public IHttpActionResult GetFacilityInventory(int id)
        {
            HealthFacilityInventory h = new HealthFacilityInventory();
            if (id > 0)
            {
                //TODO
                /*var a = from b in db.HealthFacilityLocations.Where(c => c.HealthFacilityID == id)
                        group b by b.HealthFacilityID into d
                        select new
                        {
                            HealthFacilityId = d.Key
                            , NumberOfBeds = d.Sum(e => e.NumberOfBeds)
                            , NumberOfClinics = 57 //TODO
                            , NumberOfOperatingRooms = d.Sum(e => e.NumberOfOperatingRooms)
                        };*/
                h.NumberOfBeds = 200;//a.FirstOrDefault().NumberOfBeds;
                h.NumberOfClinics = 10;//a.FirstOrDefault().NumberOfClinics;
                h.NumberOfOperatingRooms = 5;//a.FirstOrDefault().NumberOfOperatingRooms;
                return Ok(h);
            }
            if (id == 0)
            {
                /*var a = from b in db.HealthFacilityLocations.Where(c => c.HealthFacility.HealthFacilityTypeID == 1)
                        group b by b.HealthFacility.HealthFacilityTypeID into d
                        select new
                        {
                            HealthFacilityTypeId = 1
                            ,
                            NumberOfBeds = d.Sum(e => e.NumberOfBeds)
                            ,
                            NumberOfClinics = 100 //TODO
                            ,
                            NumberOfOperatingRooms = d.Sum(e => e.NumberOfOperatingRooms)
                        };*/
                h.NumberOfBeds = 100;//a.FirstOrDefault().NumberOfBeds;
                h.NumberOfClinics = 10; //a.FirstOrDefault().NumberOfClinics;
                h.NumberOfOperatingRooms = 5; //a.FirstOrDefault().NumberOfOperatingRooms;
                return Ok(h);
            }
            return BadRequest();

        }
    }
}
