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
