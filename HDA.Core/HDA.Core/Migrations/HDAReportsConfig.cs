using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using HDA.Core.Models.HDAReports;

namespace HDA.Core.Migrations
{
    internal sealed class HDAReportsConfig : DbMigrationsConfiguration<HDAReportsContext>
    {
        public HDAReportsConfig()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\HDAReports";
            MigrationsNamespace = "HDA.Core.Migrations.HDAReports";
        }

        protected override void Seed(HDAReportsContext context)
        {
            SeedCountries(context);
            SeedRegions(context);
            SeedGovernorates(context);
            SeedDomains(context);
            SeedHealthFacilityTypes(context);
            //SeedSectionLookups(context);
            SeedDirectorates(context);
            SeedDiagnosisCodingSystems(context);
            SeedIndicators(context);
            SeedTargets(context);
            SeedReports(context);
            base.Seed(context);
        }

        private void SeedReports(HDAReportsContext context)
        {
            context.Reports.AddOrUpdate(d => d.ReportCode,
                new Report { ReportNameEn = "Provider Workload", ReportNameAr = "" },
                new Report { ReportNameEn = "Disease Management", ReportNameAr = "" },
                new Report { ReportNameEn = "Outpatient Prescriptions", ReportNameAr = "" }
                );

            context.SaveChanges();
        }

        private void SeedTargets(HDAReportsContext context)
        {
            context.Targets.AddOrUpdate(t => new { t.EffectiveDate, t.Value },
                new Target { EffectiveDate = new DateTime(2018,1,1),
                    IndicatorID = context.Indicators.Where(a => a.IndicatorShortName.ToLower() == "Outpatient Encounters".ToLower()).First().IndicatorID,
                    Value = "300"
                },
                new Target
                {
                    EffectiveDate = new DateTime(2018, 1, 1),
                    IndicatorID = context.Indicators.Where(a => a.IndicatorShortName.ToLower() == "Inpatient Transfers".ToLower()).First().IndicatorID,
                    Value = "0.7"
                }
            );
            context.SaveChanges();
        }

        private void SeedIndicators(HDAReportsContext context)
        {
            context.Indicators.AddOrUpdate(i => i.IndicatorShortName,
                new Indicator { IndicatorShortName = "Outpatient Encounters", IndicatorNameEn = "Outpatient Encounters" },
                new Indicator { IndicatorShortName = "Inpatient Transfers", IndicatorNameEn = "Inpatient Transfers" },
                new Indicator { IndicatorShortName = "Surgeries", IndicatorNameEn = "Surgeries" }
                );
            context.SaveChanges();
        }

        private void SeedDirectorates(HDAReportsContext context)
        {
            context.Directorates.AddOrUpdate(d => d.DirectorateNameEn,
                new Directorate { DirectorateNameEn = "Others", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Directorate of Royal Medical Services", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Directorate of Hospital Management", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Irbid", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Balqa", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Ramtha", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Zarqa", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Amman", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Aqaba", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Karak", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Mafraq", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Jarash", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of East Amman", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Ajloun", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Madaba", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Tafila", DirectorateNameAr = "" },
                new Directorate { DirectorateNameEn = "Health Directorate of Maan", DirectorateNameAr = "" }

                );
            context.SaveChanges();
        }

        private void SeedHealthFacilityTypes(HDAReportsContext context)
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

        private void SeedCountries(HDAReportsContext context)
        {
            context.Countries.AddOrUpdate(c => c.CountryCode,
                new Country { CountryNameEn = "Jordan", CountryNameAr = "الأردن", CountryCode = 131 });
            context.SaveChanges();
        }

        private void SeedRegions(HDAReportsContext context)
        {
            context.Regions.AddOrUpdate(r => r.RegionCode,
                new Region { RegionCode = 1, RegionNameEn = "Northern Region", CountryID = context.Countries.Where(a => a.CountryCode == 131).First().CountryID }
                , new Region { RegionCode = 2, RegionNameEn = "Central Region", CountryID = context.Countries.Where(a => a.CountryCode == 131).First().CountryID }
                , new Region { RegionCode = 3, RegionNameEn = "Southern Region", CountryID = context.Countries.Where(a => a.CountryCode == 131).First().CountryID }

                );
            context.SaveChanges();
        }

        private void SeedGovernorates(HDAReportsContext context)
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

        private void SeedDomains(HDAReportsContext context)
        {
            context.Domains.AddOrUpdate(d => d.DomainCode,
                new Domain { DomainCode = "MoH", DomainNameEn = "Ministry of Health" },
                new Domain { DomainCode = "RMS", DomainNameEn = "Royal Medical Services" },
                new Domain { DomainCode = "KHCC", DomainNameEn = "King Hussein Cancer Center" },
                new Domain { DomainCode = "WHCC", DomainNameEn = "Woman Health Comprehensive Center" },
                new Domain { DomainCode = "JUH", DomainNameEn = "Jordan University Hospital" },
                new Domain { DomainCode = "KAUH", DomainNameEn = "King Abdullah University Hospital" }
                );

            context.SaveChanges();
        }
        private void SeedDiagnosisCodingSystems(HDAReportsContext context)
        {
            context.DiagnosisCodingSystems.AddOrUpdate(d => d.CodingSystemName,
                new DiagnosisCodingSystem
                {
                    CodingSystemName = "ICD-09-CM",
                    CodingSystemVersion = "2009",
                    CodingSystemEffectiveDate = new DateTime(2009, 1, 1)
                },
                new DiagnosisCodingSystem
                {
                    CodingSystemName = "ICD-10-CM",
                    CodingSystemVersion = "2012",
                    CodingSystemEffectiveDate = new DateTime(2020, 1, 1)
                },
                new DiagnosisCodingSystem
                {
                    CodingSystemName = "SNOMED CT",
                    CodingSystemVersion = "20190301",
                    CodingSystemEffectiveDate = new DateTime(2019, 5, 27)
                }
                );
            context.SaveChanges();
        }
    }
}