using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using HDA.Core.Models.HDAReports;
using LinqKit;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class HealthFacilitiesController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();
        
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
            //var hfs = db.HealthFacilities.Where(i => i.HealthFacilityTypeID == id);
            var hfs = db.HealthFacilities;
            foreach (var hf in hfs)
            {
                HealthFacilityVM h = new HealthFacilityVM();
                h.ID = hf.HealthFacilityID;
                h.HealthFacilityName = hf.HealthFacilityNameEn;
                healthFacilities.Add(h);
            }
            return Ok(healthFacilities);
        }
       

        
        [HttpPost]
        public IHttpActionResult PostHealthFacilitiesByType([FromBody] List<SelectedFacilityType> payload)
        {
            
            List<HealthFacilityVM> healthFacilities = new List<HealthFacilityVM>();


            var searchPredicate = PredicateBuilder.New<HealthFacility>();

            foreach (SelectedFacilityType str in payload)
            {
                searchPredicate =
                  searchPredicate.Or(a => a.HealthFacilityTypeID == str.HealthFacilityTypeId);
            }

           
            var hfs = db.HealthFacilities.Where(searchPredicate);

            /*foreach (SelectedFacilityType facilityType in payload)
            {
                hfs.Where(t => t.HealthFacilityTypeID == facilityType.HealthFacilityTypeId);
            }*/
            foreach (var hf in hfs)
            {
                HealthFacilityVM h = new HealthFacilityVM();
                h.ID = hf.HealthFacilityID;
                h.HealthFacilityName = hf.HealthFacilityNameEn;
                healthFacilities.Add(h);
            }
            return Ok(healthFacilities);
        }

        [HttpPost]
        public IHttpActionResult GetPharmacies([FromBody] List<int> payload)
        {
            List<PharmacyVM> pharmacies = new List<PharmacyVM>();

            var selectedHealthFacilitiesSP = PredicateBuilder.New<Pharmacy>();
            foreach(int id in payload)
            {
                selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
            }

            var hfs = db.Pharmacies.Where(selectedHealthFacilitiesSP);
            foreach (var hf in hfs)
            {
                PharmacyVM h = new PharmacyVM
                {
                    ID = hf.PharmacyID,
                    PharmacyName = hf.PharmacyName,
                    HealthFacilityID = hf.HealthFacilityID
                };
                pharmacies.Add(h);
            }
            return Ok(pharmacies);
        }

        public IHttpActionResult GetDrugClasses()
        {
            List<DrugClassVM> drugClasses = new List<DrugClassVM>();
            var hfs = db.DrugClasses;
            foreach (var hf in hfs)
            {
                DrugClassVM h = new DrugClassVM
                {
                    ID = hf.DrugClassID,
                    DrugClassName = hf.DrugClassNameEn,
                };
                drugClasses.Add(h);
            }
            return Ok(drugClasses);
        }

        public IHttpActionResult GetDrugs(int id)
        {
            List<DrugVM> drugs = new List<DrugVM>();
            var hfs = db.Drugs.Where(d=> d.DrugClassID == id);
            foreach (var hf in hfs)
            {
                DrugVM h = new DrugVM
                {
                    ID = hf.DrugID,
                    DrugName = hf.DrugGenericName,
                };
                drugs.Add(h);
            }
            return Ok(drugs);
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
