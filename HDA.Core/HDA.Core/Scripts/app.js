$('a[href="' + this.location.pathname + '"]').parents('li,ul').addClass('active');
$(document).ajaxError(function () {
    $('#ajaxProgress').attr('hidden', true);
    toastr.warning('An error occured during the request. Please contact support.');
});
$(function () {
    Highcharts.setOptions({
        lang: {
            viewData: '<i class="fa fa-table"></i> <b>Toggle Data Table</b>',
            downloadCSV: '<i class="fa fa-file-excel-o" aria-hidden="true"></i> Download CSV'
            },
        exporting: {
            buttons: {
                contextButton: {
                    menuItems: ['downloadPNG', 'downloadJPEG', 'downloadPDF', 'separator', 'downloadCSV', 'viewData'],
                },
            },
            csv: {
                dateFormat: '%Y-%m-%d'
            },
            chartOptions: {
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: false
                        }
                    }
                }
            },
            fallbackToExportServer: false,
            tableCaption:false,
        },
        chart: {
            backgroundColor: '#f8f9fa',
            plotBackgroundColor: '#ffffff',
            //spacingBottom: 5,
            //spacingTop: 5,
            //spacingLeft: 5,
            //spacingRight: 5,
        }
    });

});

$(function () {
    Highcharts.Chart.prototype.viewData = function () {
        if (!this.insertedTable) {
            var div = document.createElement('div');
            div.className = 'highcharts-data-table';
            // Insert after the chart container
            this.renderTo.parentNode.insertBefore(div, this.renderTo.nextSibling);
            div.innerHTML = this.getTable();
            this.insertedTable = true;
            var date_str = new Date().getTime().toString();
            var rand_str = Math.floor(Math.random() * (1000000)).toString();
            this.insertedTableID = 'div_' + date_str + rand_str
            div.id = this.insertedTableID;
        }
        else {
            $('#' + this.insertedTableID).toggle();
        }
    };
});

var fInput;
var facilityAws;

$(document).ready(function () {

    toastr.options = {
        'positionClass': 'toast-bottom-left',
    }

    var f = document.getElementsByClassName("text_hospital");
    fInput = f[0];
    
    facilityAws = new Awesomplete(fInput, { minChars: 0 });
});

function getAllowedHealthFacilityTypes() {
    $.ajax({
        type: 'GET',
        url: '../api/HealthFacilityTypes/GetAllowedHealthFacilityTypes',
        contentType: "application/json; charset=utf-8"
    }).done(function (res) {
        healthFacilityTypes = res;
        populateHealthFacilityTypeIdsSelects(healthFacilityTypes);
    }).fail(function (xhr, textStatus, errorThrown) {
        Swal.fire('Error', xhr.responseJSON.Message, 'error');
    });
}

function populateHealthFacilityTypeIdsSelects(healthFacilityTypes) {
    $('.hospital_types').each(function () {
        var element = $(this);
        element.empty();
        healthFacilityTypes.forEach(function (healthFacilityType) {
            element.append($("<option></option>")
                .attr("value", healthFacilityType.HealthFacilityTypeID)
                .text(healthFacilityType.HealthFacilityTypeNameEn));
        });
    });

    $('.hospital_types').multiselect({
        enableHTML: true,
        buttonWidth: '100%',
        includeSelectAllOption: true,
        allSelectedText: 'All Selected',
        onSelectAll: function () {
            updateSelectedHospitalTypes();
            updateSelectedHospitalsIds();
        },
        buttonClass: 'btn btn-primary btn-block',
        buttonText: function (options, select) {
            if (options.length === 0) {
                return '<i class="fa fa-hospital-o fa-lg" aria-hidden="true"></i> Select Hospital Type';
            }
            else if (options.length > 3) {
                return options.length + ' selected';
            }
            else {
                var labels = [];
                options.each(function () {
                    if ($(this).attr('label') !== undefined) {
                        labels.push($(this).attr('label'));
                    }
                    else {
                        labels.push($(this).html());
                    }
                });
                return labels.join(', ') + '';
            }
        },
        onChange: function (element, checked) {
            updateSelectedHospitalTypes();
            updateSelectedHospitalsIds();
        }
    });
}

var selectedHospitalTypes = [];
var selectedHospitalTypesJson;

function updateSelectedHospitalTypes() {

    selectedHospitalTypes = [];
    $.each($('.hospital_types option:selected'), function () {
        var a = new Object();
        a.HealthFacilityTypeId = $(this).val();
        selectedHospitalTypes.push(a);
    });
    console.log(selectedHospitalTypes);
    selectedHospitalTypesJson = JSON.stringify(selectedHospitalTypes);
    GetFacilityList(fInput, facilityAws, selectedHospitalTypesJson);

}

function updateSelectedHospitalsIds() {

    selectedHospitalsIds = [];

    selectedHospitalTypes.forEach(function (e) {
        selectedHospitalsIds.push(e.HealthFacilityTypeId);
    });

    //console.log(selectedHospitalsIds);
    //selectedHospitalTypesJson = JSON.stringify(selectedHospitalTypes);
    //GetFacilityList(fInput, facilityAws, selectedHospitalTypesJson);

}



function GetFacilityList(fInput, aws1, selectedFacilityTypes) {
    fInput.addEventListener('awesomplete-selectcomplete', SelectFacility);
    $.ajax({
        url: '../api/HealthFacilities/PostHealthFacilitiesByType',
        type: 'POST',
        data: selectedFacilityTypes,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var serverData = data;
            var hfList = [];
            for (var i = 0; i < serverData.length; i++) {
                hfList.push({ label: serverData[i]['HealthFacilityName'], value: serverData[i]['ID'] + '~' + serverData[i]['HealthFacilityName'] });
            }
            aws1.list = hfList;
            console.log(hfList);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

$('.text_hospital').focus(function () {
    if (facilityAws._list.length < 10) {
        facilityAws.evaluate();
    }
});

$(function () {
    var start = moment().startOf('year');
    var end = moment();

    function cb(start, end) {
        $('.report_range span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        startDate = start.format('YYYY-MM-DD');
        endDate = end.format('YYYY-MM-DD');
    }

    $('.report_range').daterangepicker({
        startDate: start,
        endDate: end,
        alwaysShowCalendars: true,
        ranges: {
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            'Year to Date': [moment().startOf('year'), moment()],
            'Last Year': [moment().startOf('year').subtract(1, 'year'), moment().endOf('year').subtract(1, 'year')]
        }
    }, cb);
    cb(start, end);
});


