using HDA.Core.Models;
using HDA.Core.Models.HDACore;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace HDA.Core.Migrations
{
    internal sealed class HDACoreConfig : DbMigrationsConfiguration<HDACoreContext>
    {
        public HDACoreConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HDACoreContext context)
        {
            SeedCountries(context);
            SeedRegions(context);
            SeedGovernorates(context);
            SeedDomains(context);
            SeedHealthFacilityTypes(context);
            SeedSectionLookups(context);
            SeedSpecialityLookups(context);
            //SeedServices(context);
            SeedGenderLookups(context);
            SeedMaritalStatusLookups(context);
            SeedSurgerySeverityLookups(context);
            //SeedLocations(context);
            //SeedHealthFacilityLocations(context);
            SeedDirectorates(context);
            SeedHealthFacilities(context);
            SeedDepartments(context);
            SeedMovementLookups(context);
            base.Seed(context);
        }

        private void SeedSpecialityLookups(HDACoreContext context)
        {
            context.SpecialityLookups.AddOrUpdate(h => h.SpecialityNameEn,
                new SpecialityLookup
                {
                    SectionLookupID = context.SectionLookups.Where(g => g.SectionNameEn.ToLower() == "default").First().SectionLookupID,
                    SpecialityNameEn = "Default",
                }

                );
            context.SaveChanges();
        }

        private void SeedSectionLookups(HDACoreContext context)
        {
            context.SectionLookups.AddOrUpdate(m => m.SectionNameEn,
                new SectionLookup { SectionNameEn = "Default" }
                );
            context.SaveChanges();
        }

        private void SeedMovementLookups(HDACoreContext context)
        {
            context.MovementLookups.AddOrUpdate(m => m.MovementNameEn,
                new MovementLookup { MovementNameEn="Regular", MovementNameAr=""},
                new MovementLookup { MovementNameEn = "Irregular", MovementNameAr = "" },
                new MovementLookup { MovementNameEn = "Against Medical Advice", MovementNameAr = "" },
                new MovementLookup { MovementNameEn = "Cancelled Admission", MovementNameAr = "" },
                new MovementLookup { MovementNameEn = "Transfer Out", MovementNameAr = "" },
                new MovementLookup { MovementNameEn = "Death", MovementNameAr = "" },
                new MovementLookup { MovementNameEn = "Death with Autopsy", MovementNameAr = "" },
                new MovementLookup { MovementNameEn = "Undefined", MovementNameAr = "" }
                );
            context.SaveChanges();
        }

        private void SeedSurgerySeverityLookups(HDACoreContext context)
        {
            context.SurgerySeverityLookups.AddOrUpdate(s => s.SeverityEn,
                new SurgerySeverityLookup { SeverityEn = "Major" },
                new SurgerySeverityLookup { SeverityEn = "Minor" }//,
                //new SurgerySeverityLookup { SeverityEn = "Undefined" }
                );
        }

        private void SeedMaritalStatusLookups(HDACoreContext context)
        {
            context.MaritalStatusLookups.AddOrUpdate(m => m.MaritalStatusEn,
                new MaritalStatusLookup { MaritalStatusEn = "Single", MaritalStatusAr = "أعزب" },
                new MaritalStatusLookup { MaritalStatusEn = "Married", MaritalStatusAr = "متزوج" },
                new MaritalStatusLookup { MaritalStatusEn = "Divorced", MaritalStatusAr = "خالع" }
                );
            context.SaveChanges();
        }

        private void SeedGenderLookups(HDACoreContext context)
        {
            context.GenderLookups.AddOrUpdate(g => g.GenderEn,
                new GenderLookup { GenderEn = "Female", GenderAr = "أنثى" },
                new GenderLookup { GenderEn = "Male", GenderAr = "ذكر" });
            context.SaveChanges();
        }

        private void SeedCountries(HDACoreContext context)
        {
            context.Countries.AddOrUpdate(c => c.CountryCode,
                new Country { CountryNameEn = "Jordan", CountryNameAr= "الأردن", CountryCode = 131 });
            context.SaveChanges();
        }

        private void SeedRegions(HDACoreContext context)
        {
            context.Regions.AddOrUpdate(r => r.RegionCode,
                new Region { RegionCode = 1, RegionNameEn = "Northern Region", CountryID = context.Countries.Where(a => a.CountryCode == 131).First().CountryID }
                , new Region { RegionCode = 2, RegionNameEn = "Central Region", CountryID = context.Countries.Where(a => a.CountryCode == 131).First().CountryID }
                , new Region { RegionCode = 3, RegionNameEn = "Southern Region", CountryID = context.Countries.Where(a => a.CountryCode == 131).First().CountryID }

                );
            context.SaveChanges();
        }

        private void SeedGovernorates(HDACoreContext context)
        {
            context.Governorates.AddOrUpdate(g => g.GovernorateNameEn,
                new Governorate { GovernorateCode = 1, GovernorateNameEn = "Irbid", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "northern region").First().RegionID }
                , new Governorate { GovernorateCode = 2, GovernorateNameEn = "Aljoun", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "northern region").First().RegionID }
                , new Governorate { GovernorateCode = 3, GovernorateNameEn = "Jerash", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "northern region").First().RegionID }
                , new Governorate { GovernorateCode = 4, GovernorateNameEn = "Mafraq", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "northern region").First().RegionID }
            
                , new Governorate { GovernorateCode = 5, GovernorateNameEn = "Balqa", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "central region").First().RegionID }
                , new Governorate { GovernorateCode = 6, GovernorateNameEn = "Madaba", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "central region").First().RegionID }
                , new Governorate { GovernorateCode = 7, GovernorateNameEn = "Amman", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "central region").First().RegionID }
                , new Governorate { GovernorateCode = 8, GovernorateNameEn = "Zarqa", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "central region").First().RegionID }
            
                , new Governorate { GovernorateCode = 9, GovernorateNameEn = "Karak", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "southern region").First().RegionID }
                , new Governorate { GovernorateCode = 10, GovernorateNameEn = "Tafila", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "southern region").First().RegionID }
                , new Governorate { GovernorateCode = 11, GovernorateNameEn = "Ma'an", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "southern region").First().RegionID }
                , new Governorate { GovernorateCode = 12, GovernorateNameEn = "Aqaba", RegionID = context.Regions.Where(r => r.RegionNameEn.ToLower() == "southern region").First().RegionID }

                );
            context.SaveChanges();
        }

        private void SeedDomains(HDACoreContext context)
        {
            context.DomainLookups.AddOrUpdate(d => d.DomainCode,
                new DomainLookup { DomainCode = "MoH", DomainNameEn = "Ministry of Health" },
                new DomainLookup { DomainCode = "RMS", DomainNameEn = "Royal Medical Services" },
                new DomainLookup { DomainCode = "KHCC", DomainNameEn = "King Hussein Cancer Center" },
                new DomainLookup { DomainCode = "WHCC", DomainNameEn = "Woman Health Comprehensive Center" },
                new DomainLookup { DomainCode = "JUH", DomainNameEn = "Jordan University Hospital" },
                new DomainLookup { DomainCode = "KAUH", DomainNameEn = "King Abdullah University Hospital" }
                );

            context.SaveChanges();
        }

        private void SeedHealthFacilityTypes(HDACoreContext context)
        {
            context.HealthFacilityTypes.AddOrUpdate(h => h.HealthFacilityTypeCode,
                new HealthFacilityType { HealthFacilityTypeCode = "H", HealthFacilityTypeNameEn = "Hospital", HealthFacilityTypeNameAr = "" },
                new HealthFacilityType { HealthFacilityTypeCode = "CC", HealthFacilityTypeNameEn = "Comprehensive Center", HealthFacilityTypeNameAr = "" },
                new HealthFacilityType { HealthFacilityTypeCode = "SPC", HealthFacilityTypeNameEn = "Specialized Nedical Center", HealthFacilityTypeNameAr = "" },
                new HealthFacilityType { HealthFacilityTypeCode = "PREPH", HealthFacilityTypeNameEn = "Peripheral Center", HealthFacilityTypeNameAr = "" },
                new HealthFacilityType { HealthFacilityTypeCode = "PC", HealthFacilityTypeNameEn = "Primary Center", HealthFacilityTypeNameAr = "" },
                new HealthFacilityType { HealthFacilityTypeCode = "EDU", HealthFacilityTypeNameEn = "Educational Hospital", HealthFacilityTypeNameAr = "" }
                );
            context.SaveChanges();
        }

        private void SeedHealthFacilities(HDACoreContext context)
        {
            context.HealthFacilities.AddOrUpdate(h => h.SourceID,
                new HealthFacility
                {
                    HealthFacilityNameEn = "AL KARAK HOSPITAL",
                    HealthFacilityNameAr = "مستشفى الكرك الحكومي",
                    SourceID = 266,
                    GovernorateID = context.Governorates.Where(g => g.GovernorateNameEn.ToLower() == "karak").First().GovernorateID,
                    DomainLookupID = context.DomainLookups.Where(d => d.DomainCode.ToLower() == "moh").First().DomainLookupID,
                    HealthFacilityTypeID = context.HealthFacilityTypes.Where(h => h.HealthFacilityTypeCode.ToLower() == "H").First().HealthFacilityTypeID,
                    DirectorateLookupID = context.DirectorateLookups.Where(d => d.DirectorateNameEn.ToLower() == "Health Directorate of Karak".ToLower()).First().DirectorateLookupID
                },
                new HealthFacility
                {
                    HealthFacilityNameEn = "AL BASHIR HOSPITAL",
                    HealthFacilityNameAr = "مستشفى البشير",
                    SourceID = 210,
                    GovernorateID = context.Governorates.Where(g => g.GovernorateNameEn.ToLower() == "amman").First().GovernorateID,
                    DomainLookupID = context.DomainLookups.Where(d => d.DomainCode.ToLower() == "moh").First().DomainLookupID,
                    HealthFacilityTypeID = context.HealthFacilityTypes.Where(h => h.HealthFacilityTypeCode.ToLower() == "H").First().HealthFacilityTypeID,
                    DirectorateLookupID = context.DirectorateLookups.Where(d => d.DirectorateNameEn.ToLower() == "Health Directorate of Amman".ToLower()).First().DirectorateLookupID
                }

                );
            context.SaveChanges();
        }

        /*private void SeedServices(HDACoreContext context)
        {
            context.Services.AddOrUpdate(s => s.ServiceNameEn,
                new Service { ServiceNameEn = "LABORATORY" },
                new Service { ServiceNameEn = "GENERAL EMERGENCY" },
                new Service { ServiceNameEn = "GENERAL PEDIATRICS" },
                new Service { ServiceNameEn = "ER OBSTETRICS AND GYNECOLOGY" },
                new Service { ServiceNameEn = "ER PEDIATRICS" },
                new Service { ServiceNameEn = "OBSTETRICS AND GYNECOLOGY" },
                new Service { ServiceNameEn = "ANTENATAL CARE" },
                new Service { ServiceNameEn = "HEMODIALYSIS" },
                new Service { ServiceNameEn = "GENERAL MEDICINE" },
                new Service { ServiceNameEn = "ER OPHTHALMOLOGY" },
                new Service { ServiceNameEn = "ER GENERAL SURGERY" },
                new Service { ServiceNameEn = "PSYCHIATRY" },
                new Service { ServiceNameEn = "ER ORTHOPEDICS" },
                new Service { ServiceNameEn = "MAGNETIC RESONANCE IMAGING" },
                new Service { ServiceNameEn = "ER INTERNAL MEDICINE" },
                new Service { ServiceNameEn = "EMPLOYEE HEALTH" },
                new Service { ServiceNameEn = "NEONATOLOGY" },
                new Service { ServiceNameEn = "OTOLARYNGOLOGY (ENT)" },
                new Service { ServiceNameEn = "INTERNAL MEDICINE" },
                new Service { ServiceNameEn = "GENERAL SURGERY" },
                new Service { ServiceNameEn = "FAMILY PLANNING" },
                new Service { ServiceNameEn = "HEMATOLOGY" },
                new Service { ServiceNameEn = "IMMUNIZATION" },
                new Service { ServiceNameEn = "AUDIOLOGY" },
                new Service { ServiceNameEn = "FAMILY MEDICINE" },
                new Service { ServiceNameEn = "REHABILITATION-PHYSIOTHERAPY" },
                new Service { ServiceNameEn = "PEDIATRIC SURGERY" },
                new Service { ServiceNameEn = "ORTHOPEDICS" },
                new Service { ServiceNameEn = "GENERAL DENTISTRY" },
                new Service { ServiceNameEn = "X-RAY" },
                new Service { ServiceNameEn = "DERMATOLOGY" },
                new Service { ServiceNameEn = "ELECTROENCEPHALOGRAPHY (EEG)" },
                new Service { ServiceNameEn = "OPHTHALMOLOGY" },
                new Service { ServiceNameEn = "CHILDREN DEVELOPMENT" },
                new Service { ServiceNameEn = "NEPHROLOGY" },
                new Service { ServiceNameEn = "NUTRITION" },
                new Service { ServiceNameEn = "RHEUMATOLOGY" },
                new Service { ServiceNameEn = "ENDOCRINOLOGY" },
                new Service { ServiceNameEn = "CARDIOLOGY" },
                new Service { ServiceNameEn = "NEUROLOGY" },
                new Service { ServiceNameEn = "ULTRASOUND" },
                new Service { ServiceNameEn = "PROSTHODONTICS" },
                new Service { ServiceNameEn = "PEDIATRIC PULMONOLOGY" },
                new Service { ServiceNameEn = "COMPUTERIZED TOMOGRAPHY" },
                new Service { ServiceNameEn = "NEUROSURGERY" },
                new Service { ServiceNameEn = "PEDIATRIC NEPHROLOGY" },
                new Service { ServiceNameEn = "ORAL AND MAXILLOFACIAL SURGERY" },
                new Service { ServiceNameEn = "DENTAL CONSERVATIVE TREATMENT" },
                new Service { ServiceNameEn = "UROLOGY" },
                new Service { ServiceNameEn = "ELECTROMYOGRAPHY (EMG)" },
                new Service { ServiceNameEn = "ENDOSCOPY" },
                new Service { ServiceNameEn = "ORTHODONTICS" },
                new Service { ServiceNameEn = "PULMONOLOGY" },
                new Service { ServiceNameEn = "ONCOLOGY" },
                new Service { ServiceNameEn = "THORACIC SURGERY" },
                new Service { ServiceNameEn = "PEDODONTICS" },
                new Service { ServiceNameEn = "PERIODONTICS" },
                new Service { ServiceNameEn = "ENDODONTIC" },
                new Service { ServiceNameEn = "GYNECOLOGIC ONCOLOGY" },
                new Service { ServiceNameEn = "GASTROENTEROLOGY-HEPATOLOGY" },
                new Service { ServiceNameEn = "ANESTHESIA" },
                new Service { ServiceNameEn = "ECHOCARDIOGRAPHY (ECHO)" },
                new Service { ServiceNameEn = "ADOLESCENT AND YOUTH" },
                new Service { ServiceNameEn = "POSTNATAL CARE" },
                new Service { ServiceNameEn = "LITHOTRIPSY" },
                new Service { ServiceNameEn = "OPTOMETRY" },
                new Service { ServiceNameEn = "BRONCHOSCOPY" },
                new Service { ServiceNameEn = "VASCULAR SURGERY" },
                new Service { ServiceNameEn = "CARDIAC SURGERY" },
                new Service { ServiceNameEn = "RADIOTHERAPY" },
                new Service { ServiceNameEn = "INFECTIOUS DISEASE" },
                new Service { ServiceNameEn = "ER ENT" },
                new Service { ServiceNameEn = "PEDIATRIC ENDOCRINOLOGY" },
                new Service { ServiceNameEn = "ORAL MEDICINE" },
                new Service { ServiceNameEn = "PEDIATRIC GASTROENTEROLOGY" },
                new Service { ServiceNameEn = "PEDIATRIC NEUROLOGY" },
                new Service { ServiceNameEn = "PEDIATRIC CARDIOLOGY" },
                new Service { ServiceNameEn = "PRE-SURG EVAL BY MD" },
                new Service { ServiceNameEn = "GENERAL ADMINISTRATION" },
                new Service { ServiceNameEn = "WOMEN HEALTH" },
                new Service { ServiceNameEn = "MENOPAUSE AGE" },
                new Service { ServiceNameEn = "REFERRAL TO CLINIC" },
                new Service { ServiceNameEn = "PLASTIC-RECONSTRUCTIVE SURGERY" },
                new Service { ServiceNameEn = "PUBLIC SERVICES" },
                new Service { ServiceNameEn = "COMMUNITY MEDICINE" },
                new Service { ServiceNameEn = "ELECTROCHOCARDIOGRAPHY (ECG)" }
                );
            context.SaveChanges();
        }*/

        /*private void SeedLocations(HDACoreContext context)
        {
            context.Locations.AddOrUpdate(l => l.LocationCode,
                new Location
                {
                    SourceID = 100,
                    LocationCode = "LOC 1",
                    LocationNameEn = "Location 1"
                });
            context.SaveChanges();
        }*/

        /*private void SeedHealthFacilityLocations(HDACoreContext context)
        {
            context.HealthFacilityLocations.AddOrUpdate(h => new {h.HealthFacilityID, h.LocationID},
                new HealthFacilityLocation
                {
                    HealthFacilityID = context.HealthFacilities.Where(a => a.HealthFacilityNameEn.ToLower() == "AL KARAK HOSPITAL".ToLower()).First().HealthFacilityID,
                    LocationID = context.Locations.Where(b => b.LocationCode.ToLower() == "LOC 1".ToLower()).First().LocationID,
                    ServiceID = context.Services.Where(c => c.ServiceNameEn.ToLower() == "GENERAL EMERGENCY".ToLower()).First().ServiceID,
                    NumberOfBeds = 178,
                    NumberOfOperatingRooms = 6
                },
                new HealthFacilityLocation
                {
                    HealthFacilityID = context.HealthFacilities.Where(a => a.HealthFacilityNameEn.ToLower() == "AL BASHIR HOSPITAL".ToLower()).First().HealthFacilityID,
                    LocationID = context.Locations.Where(b => b.LocationCode.ToLower() == "LOC 1".ToLower()).First().LocationID,
                    ServiceID = context.Services.Where(c => c.ServiceNameEn.ToLower() == "GENERAL EMERGENCY".ToLower()).First().ServiceID,
                    NumberOfBeds = 231,
                    NumberOfOperatingRooms = 12
                }

                );

            context.SaveChanges();
        }*/

        private void SeedDepartments(HDACoreContext context)
        {
            context.DepartmentLookups.AddOrUpdate(d => d.DepartmentCode,
                new DepartmentLookup { DepartmentCode = "IN", DepartmentNameEn= "Inpatient Department", DepartmentNameAr="" },
                new DepartmentLookup { DepartmentCode = "OUT", DepartmentNameEn = "Outpatient Department", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "OR", DepartmentNameEn = "Operations Department", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "ED", DepartmentNameEn = "Emergency Department", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "IC", DepartmentNameEn = "Intensive Care Department", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "PHAR", DepartmentNameEn = "Pharmacy", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "LAB", DepartmentNameEn = "Laboratory", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "RAD", DepartmentNameEn = "Radiology", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "DAY", DepartmentNameEn = "Day Case", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "TELE", DepartmentNameEn = "Telemedicine", DepartmentNameAr = "" },
                new DepartmentLookup { DepartmentCode = "HOME", DepartmentNameEn = "Home Medicine", DepartmentNameAr = "" }

                );
            context.SaveChanges();
        }

        private void SeedDirectorates(HDACoreContext context)
        {
            context.DirectorateLookups.AddOrUpdate(d => d.DirectorateNameEn,
                new DirectorateLookup { DirectorateNameEn="Others", DirectorateNameAr=""},
                new DirectorateLookup { DirectorateNameEn = "Directorate of Royal Medical Services", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Directorate of Hospital Management", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Irbid", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Balqa", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Ramtha", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Zarqa", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Amman", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Aqaba", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Karak", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Mafraq", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Jarash", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of East Amman", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Ajloun", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Madaba", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Tafila", DirectorateNameAr = "" },
                new DirectorateLookup { DirectorateNameEn = "Health Directorate of Maan", DirectorateNameAr = "" }

                );
            context.SaveChanges();
        }

    }
}