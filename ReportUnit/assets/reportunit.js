var menuWidth = 260;

google.load('visualization', '1', { packages: ['corechart'] });

$(document).ready(function () {
    /* init */
    $('select').material_select();
    $('.modal-trigger').leanModal();
    $('.tooltipped').tooltip({ delay: 10 });

    /* for a single report item, hide sidenav */
    if ($('.report-item').length <= 1) {
        $('#slide-out').addClass('hide');

        pinWidth = '56.5%';

        $('.pin').css('width', pinWidth);
        $('.main-wrap, nav').css('padding-left', '20px');
    }

    var passedPercentage = Math.round(((passed / total) * 100)) + '%';
    $('.pass-percentage').text(passedPercentage);
    $('.dashboard .determinate').attr('style', 'width:' + passedPercentage);

    google.setOnLoadCallback(suitesChart);
    google.setOnLoadCallback(testsChart);

    $('ul.doughnut-legend').addClass('right');

    resetFilters();
    $("#nameFilterInput").on("input", filterTestNamesByInput);
    $('.suite:first-child').click();

    $('.details-container').click(function (evt) {
        var t = $(evt.target);

        if (t.is('.showStatusMessage') || t.is('i')) {
            if (t.is('i')) {
                t = t.parent();
            }

            showDynamicModal(t.closest('tr').find('.name').text() + ' StatusMessage', t.next().text());
        }

        if (t.is('.showDescription')) {
            showDynamicModal(t.text() + ' Description', t.next().text());
        }
    });

    /* toggle dashboard on 'Enable Dashboard' click */
    $('#enableDashboard').click(function () {
        $('.suite-list, .suite-details').toggleClass('v-spacer');
        $(this).toggleClass('enabled').children('i').toggleClass('active');
        $('.dashboard').toggleClass('hide');
    });

    /* show suite data on click */
    $('.suite').click(function () {
        var t = $(this);

        $('.suite').removeClass('active');
        $('.suite-name-displayed, .details-container').html('');

        t.toggleClass('active');
        var html = t.find('.suite-content').html();

        $('.suite-name-displayed').text(t.find('.suite-name').text());
        $('.details-container').append(html);
    });

    $('#slide-out .report-item > a').filter(function () {
        return this.href.match(/[^\/]+$/)[0] == document.location.pathname.match(/[^\/]+$/)[0];
    }).parent().addClass('active');

    /* filters -> by suite status */
    $('#suite-toggle li').click(function () {
        var t = $(this);

        if (!t.hasClass('clear')) {
            resetFilters();

            var status = t.text().toLowerCase();

            $('#suites .suite').addClass('hide');
            $('#suites .suite.' + status).removeClass('hide');

            selectVisSuite()
        }
    });

    /* filters -> by test status */
    $('#tests-toggle li').click(function () {
        var t = $(this);

        if (!t.hasClass('clear')) {
            resetFilters();

            var opt = t.text().toLowerCase();

            $('.suite table tr.test-status:not(.' + opt + '), .details-container tr.test-status:not(.' + opt).addClass('hide');
            $('.suite table tr.test-status.' + opt + ', .details-container tr.test-status.' + opt).removeClass('hide');

            hideEmptySuites();
            selectVisSuite();
        }
    });

    /* filters -> by category */
    $('#category-toggle li').click(function () {
        var t = $(this);

        if (!t.hasClass('clear')) {
            resetFilters();

            filterByCategory(t.text());
            selectVisSuite()
        }
    });

    $('.clear').click(function () {
        resetFilters(); selectVisSuite()
    });
});

function showDynamicModal(heading, content) {
    var m = $('#dynamicModal');
    m.find('h4').text(heading);
    m.find('pre').text(content);
    m.openModal({ in_duration: 200 });
}

function filterByCategory(cat) {
    resetFilters();

    $('td.test-features').each(function() {
        if (!($(this).hasClass(cat))) {
            $(this).closest('tr').addClass('hide');
        }
    });

    hideEmptySuites();
}

function hideEmptySuites() {
    $('.suite').each(function() {
        var t = $(this);
        
        if (t.find('tr.test-status').length == t.find('tr.test-status.hide').length) {
            t.addClass('hide');
        }
    });
}

function resetFilters() {
    $('.suite, tr.test-status').removeClass('hide');
    $('.suite-toggle li:first-child, .tests-toggle li:first-child, .feature-toggle li:first-child').click();

    $("#nameFilterInput").val("");
    $("#nameFilterInput").trigger("input");
}

function selectVisSuite() {
    $('.suite:visible').get(0).click();
}

function clickListItem(listClass, index) {
    $('#' + listClass).find('li').get(index).click();
}

function suitesChart() {
    if (!$('#suite-analysis').length) {
        return false;
    }

    var passed = $('.suite-result.passed').length;
    var failed = $('.suite-result.failed').length;
    var others = $('.suite-result.error, .suite-result.inconclusive, .suite-result.skipped').length;

    $('.suite-pass-count').text(passed);
    $('.suite-fail-count').text(failed);
    $('.suite-others-count').text(others);

    var data = google.visualization.arrayToDataTable([
	  ['Test Status', 'Count'],
	  ['Pass', passed],
	  ['Fail', failed],
	  ['Inconclusive', $('.suite-result.inconclusive').length],
	  ['Error', $('.suite-result.error').length],
	  ['Skipped', $('.suite-result.skipped').length]
    ]);

    var chart = new google.visualization.PieChart(document.getElementById('suite-analysis'));
    chart.draw(data, getChartOptions());
}

/* report -> tests chart */
function testsChart() {
    if (!$('#test-analysis').length) {
        return false;
    }

    var data = {};

    if ($('body.summary').length > 0) {
        total = parseInt($('#total-tests').text());
        passed = parseInt($('#total-passed').text());
        failed = parseInt($('#total-failed').text());
        others = parseInt($('#total-others').text());

        var data = google.visualization.arrayToDataTable([
          ['Test Status', 'Count'],
          ['Pass', passed],
          ['Fail', failed],
          ['Others', others]
        ]);

        $('.test-others-count').text(others);
    }
    else {
        var data = google.visualization.arrayToDataTable([
          ['Test Status', 'Count'],
          ['Pass', passed],
          ['Fail', failed],
          ['Inconclusive', $('.suite-result.inconclusive').length],
          ['Error', errors],
          ['Skipped', skipped]
        ]);

        $('.test-others-count').text(errors + inconclusive + skipped);
    }

    $('.test-pass-count').text(passed);
    $('.test-fail-count').text(failed);

    var chart = new google.visualization.PieChart(document.getElementById('test-analysis'));
    chart.draw(data, getChartOptions());
}

function getChartOptions() {
    return {
        backgroundColor: { fill: 'transparent' },
        chartArea: { 'width': '92%', 'height': '100%' },
        colors: ['#00af00', 'red', 'orange', 'tomato', 'dodgerblue'],
        fontName: 'Nunito',
        fontSize: '11',
        titleTextStyle: { color: '#1366d7', fontSize: '14' },
        pieHole: 0.2,
        height: 180,
        pieSliceText: 'value',
        width: 300
    };
}

var options = {
	segmentShowStroke : false, 
	percentageInnerCutout : 55, 
	animationSteps : 1,
	legendTemplate : '<ul class=\'<%=name.toLowerCase()%>-legend\'><% for (var i=0; i<segments.length; i++){%><li><span style=\'background-color:<%=segments[i].fillColor%>\'></span><%if(segments[i].label){%><%=segments[i].label%><%}%></li><%}%></ul>'
};

function filterTestNamesByInput() {
    var filterInput = $("#nameFilterInput");
    var filterText = filterInput
        .val()
        .toLowerCase();

    var suiteElements = $("ul#suite-collection > li")
    if (filterText === "") {
        suiteElements.show();
        return;
    }

    jQuery.each(suiteElements,
        function(index, suiteElement) {
            var base = $(suiteElement);
            base.hide();

            var suiteName = base
                .find(".suite-name")
                .text()
                .toLowerCase();
            if (suiteName.includes(filterText)) {
                base.show();
            }
        });
}

/* test case counts */
var total = $('.test-name').length;
var passed = $('td.passed').length;
var failed = $('td.failed').length;
var inconclusive = $('td.inconclusive').length;
var errors = $('td.error').length;
var skipped = $('td.skipped').length;


/* draw legend for test and step charts [DASHBOARD] */
function drawLegend(chart, id) {
	var helpers = Chart.helpers;
	var legendHolder = document.getElementById(id);
	
	legendHolder.innerHTML = chart.generateLegend();
	
	helpers.each(legendHolder.firstChild.childNodes, function(legendNode, index) {
		helpers.addEvent(legendNode, 'mouseover', function() {
			var activeSegment = chart.segments[index];
			activeSegment.save();
			activeSegment.fillColor = activeSegment.highlightColor;
			chart.showTooltip([activeSegment]);
			activeSegment.restore();
		});
	});
	
	Chart.helpers.addEvent(legendHolder.firstChild, 'mouseout', function() {
		chart.draw();
	});
	
	$('#' + id).after(legendHolder.firstChild);
}
