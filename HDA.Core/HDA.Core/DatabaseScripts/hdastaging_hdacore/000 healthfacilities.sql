insert into hdacore.healthfacilities(HealthFacilityID, SourceID, FacilityShortName, HealthFacilityNameEn
, HealthFacilityNameAr
, EstimatedClinics
, EstimatedBeds
, DirectorateLookupID
, GovernorateID
, HealthFacilityTypeID
, DomainLookupID
, EHRActivationYear)

select HealthFacilityID
, SourceID
, FacilityShort
, HealthFacilityNameEn
, HealthFacilityNameAr
, EstimatedClinics 
, EstimatedBeds
, DirectorateID
, GovernorateID
, HealthFacilityTypeID
, DomainID
, EHRActivationYear
from hdastaging.healthfacilities