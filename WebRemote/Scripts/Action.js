var stat1;
var stat2;
var Pon;
var Pmax;
var ip;
var webAPI = 'http://192.168.100.126:44812/';
var data1 = [];
var Cpu = 0;
var Ram = 0;
var Disk = 0;
var tt = [];

var id = [];
var Name = [];
var IP = [];
var ping = [];

var banname = [];
var banexp = [];
var banreason = [];

function GetStatusData() {

    $('.ConsoleText').animate({
        scrollTop: $('.ConsoleText').get(0).scrollHeight
    }, 500);

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/1',
        dataType: 'json',
        success: function(data) {
            stat1 = data;
            //console.log(stat1);
        },
        error: function(jqXHR, textStatus, errorThrown) {
            console.log(textStatus, errorThrown);
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/2',
        dataType: 'json',
        success: function(data) {
            stat2 = data;
        }
    });



    if (stat1 == "Start" && stat2 == "STOP") {
        $("#status").html("<p>Status<br>Starting</p>");

    }

    if (stat1 == "Start" && stat2 == "START") {
        $("#status").html("<p>Status<br>Online</p>");

    }

    if (stat1 == "Stop" && stat2 == "START") {
        $("#status").html("<p>Status<br>Stopping</p>");

    }

    if (stat1 == "Stop" && stat2 == "STOP") {
        $("#status").html("<p>Status<br>Offline</p>");

    }

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/3',
        dataType: 'json',
        success: function(data) {
            ip = data;
            $("#ip").html("<p>IP:PORT<br>" + data + "</p>");
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/4',
        dataType: 'json',
        success: function(data) {
            $(".ServerName").html("<p>Server Name: " + data + "</p>");
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/5',
        dataType: 'json',
        success: function(data) {
            Pon = data;
            //console.log(data);
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/6',
        dataType: 'json',
        success: function(data) {
            Pmax = data;
            //console.log(data);
        }
    });

    $("#Player").html("<p>Players<br>" + Pon + "/" + Pmax + "</p>");

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/7',
        dataType: 'json',
        success: function(data) {
            //console.log(data);
            $("#OneSync").html("<p>OneSync<br>" + data + "</p>");
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/8',
        dataType: 'json',
        success: function(data) {
            // console.log(data);
            $("#Map").html("<p>Map<br>" + data + "</p>");
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/9',
        dataType: 'json',
        success: function(data) {
            // console.log(data);
            $("#ServerMode").html("<p>Server Mode<br>" + data + "</p>");
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/10',
        dataType: 'json',
        success: function(data) {
            // console.log(data);
            $("#Cpu").html("<p>Cpu Usage " + data + "%</p>");

            Cpu = parseInt(data);

        }
    });
    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/11',
        dataType: 'json',
        success: function(data) {
            // console.log(data);
            $("#Ram").html("<p>Memory Usage " + data + "%</p>");
            Ram = parseInt(data);
        }
    });
    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Dashboard/12',
        dataType: 'json',
        success: function(data) {
            // console.log(data);
            $("#Disk").html("<p>Disk Usage " + data + "%</p>");
            Disk = parseInt(data);
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Console/',
        // dataType: 'json',
        success: function(data) {
            $.each(data, function(index, value) {
                if (index == 0) {
                    $(".ConsoleText").empty();
                }
                // console.log(index);
                // console.log(value);
                $(".ConsoleText").append("<p>" + value + "<br>" + "</p>");
            });

        }
    });

    var action = $('.ActionSelect').find(":selected").text()
        //console.log(action);

    if (action == 'Ban Offline') {
        $(".Sname").hide();
        $(".Sid").show();
        $(".Rbox").show();
        $(".Bantime").hide();
    } else if (action == 'Ban') {
        $(".Sbname").hide();
        $(".Sname").show();
        $(".Sid").hide();
        $(".Rbox").show();
        $(".Bantime").hide();
    } else if (action == "Kick ALL") {
        $(".Sbname").hide();
        $(".Sname").hide();
        $(".Sid").hide();
        $(".Rbox").hide();
        $(".Bantime").hide();
    } else if (action == "UnBan") {
        $(".Sbname").show();
        $(".Sname").hide();
        $(".Sid").hide();
        $(".Rbox").hide();
        $(".Bantime").hide();
    } else {
        $(".Sbname").hide();
        $(".Sid").show();
        $(".Sname").hide();
        $(".Rbox").show();
        $(".Bantime").hide();
    }
    $(".Ptabledata").empty();
    $(".TableON").empty();
    if (parseInt(Pon) == 0) {
        $(".PlayerSelect").empty();
    }
    if (stat1 == "Start" && stat2 == "START" && parseInt(Pon) > 0) {


        $.ajax({
            post: 'GET',
            url: webAPI + '/api/Player/1',
            // dataType: 'json',
            success: function(data) {
                id = data;

            }
        });


        $.ajax({
            post: 'GET',
            url: webAPI + '/api/Player/2',
            // dataType: 'json',
            success: function(data) {
                Name = data;
                $(".TableON").empty();
                $(".PlayerSelect").empty();
                $.each(data, function(index, val) {
                    $(".TableON").append("<p>" + val + "</p>");
                    $(".PlayerSelect").append("<option value=" + val + ">" + val + "</option>");
                    //console.log(name.lenght);

                });
            }
        });

        $.ajax({
            post: 'GET',
            url: webAPI + '/api/Player/3',
            // dataType: 'json',
            success: function(data) {
                IP = data;
            }
        });
        //console.log(IP.lenght);
        $.ajax({
            post: 'GET',
            url: webAPI + '/api/Player/4',
            // dataType: 'json',
            success: function(data) {
                ping = data;
            }
        });

        for (var i = 0; i < id.length; i++) {
            $(".Ptabledata").append("<tr><td>" + id[i] + "</td><td>" + Name[i] + "</td><td>" + IP[i] + "</td><td>" + ping[i] + "</td></tr>");
        }
    }
    $(".Btabledata").empty();
    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Player/5',
        // dataType: 'json',
        success: function(data) {
            banname = data;

        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Player/6',
        // dataType: 'json',
        success: function(data) {
            banexp = data;
        }
    });

    $.ajax({
        post: 'GET',
        url: webAPI + '/api/Player/7',
        // dataType: 'json',
        success: function(data) {
            banreason = data;
        }
    });

    for (var i = 0; i < banname.length; i++) {
        $(".Btabledata").append("<tr><td>" + banname[i] + "</td><td>" + banexp[i] + "</td><td>" + banreason[i] + "</td></tr>");

    }

}

var optionsCH1 = {
    chart: {
        height: 200,
        type: "radialBar"
    },

    series: [Cpu],

    plotOptions: {
        radialBar: {
            hollow: {

                margin: 10,
                size: "60%"
            },

            dataLabels: {
                showOn: "always",
                name: {
                    offsetY: -5,
                    show: true,
                    color: "#888",
                    fontSize: "13px"
                },
                value: {
                    color: "#111",
                    fontSize: "25px",
                    show: true
                }
            }
        }
    },

    stroke: {
        lineCap: "round",
    },
    labels: ["Cpu Usage"]
};

var optionsCH2 = {
    chart: {
        height: 200,
        type: "radialBar"
    },

    series: [Ram],

    plotOptions: {
        radialBar: {
            hollow: {

                margin: 10,
                size: "60%"
            },

            dataLabels: {
                showOn: "always",
                name: {
                    offsetY: -5,
                    show: true,
                    color: "#888",
                    fontSize: "13px"
                },
                value: {
                    color: "#111",
                    fontSize: "25px",
                    show: true
                }
            }
        }
    },

    stroke: {
        lineCap: "round",
    },
    labels: ["Ram Usage"]
};

var optionsCH3 = {
    chart: {
        height: 200,
        type: "radialBar"
    },

    series: [Cpu],

    plotOptions: {
        radialBar: {
            hollow: {

                margin: 10,
                size: "60%"
            },

            dataLabels: {
                showOn: "always",
                name: {
                    offsetY: -5,
                    show: true,
                    color: "#888",
                    fontSize: "13px"
                },
                value: {
                    color: "#111",
                    fontSize: "25px",
                    show: true
                }
            }
        }
    },

    stroke: {
        lineCap: "round",
    },
    labels: ["Disk Activity"]
};

$(document).ready(function() {
    setInterval(function() { GetStatusData() }, 1000);
    var chart1 = new ApexCharts(document.querySelector(".CH1"), optionsCH1);
    var chart2 = new ApexCharts(document.querySelector(".CH2"), optionsCH2);
    var chart3 = new ApexCharts(document.querySelector(".CH3"), optionsCH3);

    chart1.render();
    chart2.render();
    chart3.render();

    setInterval(function() {
        chart1.updateSeries([
            Cpu
        ]), chart2.updateSeries([
            Ram
        ]), chart3.updateSeries([
            Disk
        ])
    }, 1000)


    var objectArr = [{
            "eventID": 1,
            "time": "2017-08-23 10:01:34",
            "level": "INFO",
            "message": "[loadDB]"
        },
        {
            "eventID": 2,
            "time": "2017-08-23 10:01:35",
            "level": "INFO",
            "message": "[chargeDB]"
        }
    ]

    var data = [{
            "name": "Tiger Nixon",
            "position": "System Architect",
            "salary": "$3,120",
            "start_date": "2011/04/25",
            "office": "Edinburgh",
            "extn": "5421"
        },
        {
            "name": "Garrett Winters",
            "position": "Director",
            "salary": "$5,300",
            "start_date": "2011/07/25",
            "office": "Edinburgh",
            "extn": "8422"
        }
    ]


    $.each(objectArr, function(index, val) {
        console.log(index);
        console.log(val.message);
        // $(".ConsoleText").append(val.message + "<br>");

    });

    $(".StartBT").click(function() {
        $("#status").text("Start");
        var data = {
            id: "1",
            Status1: "Start"

        };
        $.ajax({
            type: 'POST',
            url: webAPI + '/api/Dashboard/',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json"

        });

        console.log("Start complete");
    });

    $(".StopBT").click(function() {
        $(".Pdata").remove();
        //$("#status").text("Stop");
        var data = {
            ID: "1",
            Status1: "Stop"

        };
        $.ajax({
            type: 'POST',
            url: webAPI + '/api/Dashboard/',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });

        console.log("Stop complete");
    });

    $(".ExecuteAction").click(function() {

        var Action = $('.ActionSelect').find(":selected").text();
        var Playername = $('.PlayerSelect').find(":selected").text();
        var BanPlayername = $('.BanPlayerSelect').find(":selected").text();
        var ID = $('.IDSelect').find(":selected").text();
        var Reason = $("#Reason").val();
        var ExpireDate = $('#ExpireDate').val();
        var PermanentBanStat = $('#PermanentCheck').is(":checked");

        if (Action == "Kick") {
            //alert("Kick ID " + ID + " With Reason " + Reason);

            var data = {
                ID: "2",
                ConsoleOUT: "clientkick " + ID + " " + Reason

            };
            $.ajax({
                type: 'POST',
                url: webAPI + '/api/Console/',
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function(jqXHR, textStatus, errorThrown) {
                    console.log("FAIL: " + errorThrown);
                },
                success: function(data, textStatus, jqXHR) {
                    console.log("SUCCESS! " + textStatus);
                    $.each(data, function(index, val) {
                        console.log(val);
                    });
                }
            });

        }

        if (Action == "Kick ALL") {
            // alert("Kick ALL Online Player")


            var data = {
                ID: "1",
                PlayerAction: "Kick all"

            };
            //console.log(data);
            $.ajax({
                type: 'POST',
                url: webAPI + '/api/Player/',
                data: JSON.stringify(data),
                contentType: "application/json",
                // dataType: "json",
                error: function(jqXHR, textStatus, errorThrown) {
                    console.log("FAIL: " + errorThrown);
                },
                success: function(data, textStatus, jqXHR) {
                    console.log("SUCCESS! " + jqXHR);
                    $.each(data, function(index, val) {
                        console.log(index);
                    });
                }
            });
        }

        if (Action == "Ban") {
            // alert("Kick ALL Online Player")
            console.log(ExpireDate);
            if (PermanentBanStat) {
                var data = {
                    ID: "1",
                    PlayerAction: "Ban Online PlayerName=" + Playername + " Reason=" + Reason + " Exp=0"

                };
                //console.log(data);
                $.ajax({
                    type: 'POST',
                    url: webAPI + '/api/Player/',
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    // dataType: "json",
                    error: function(jqXHR, textStatus, errorThrown) {
                        console.log("FAIL: " + errorThrown);
                    },
                    success: function(data, textStatus, jqXHR) {
                        console.log("SUCCESS! " + jqXHR);
                        $.each(data, function(index, val) {
                            console.log(index);
                        });
                    }
                });
            } else {
                var data = {
                    ID: "1",
                    PlayerAction: "Ban PlayerName=" + Playername + "Reason=" + Reason

                };
                //console.log(data);
                $.ajax({
                    type: 'POST',
                    url: webAPI + '/api/Player/',
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    // dataType: "json",
                    error: function(jqXHR, textStatus, errorThrown) {
                        console.log("FAIL: " + errorThrown);
                    },
                    success: function(data, textStatus, jqXHR) {
                        console.log("SUCCESS! " + jqXHR);
                        $.each(data, function(index, val) {
                            console.log(index);
                        });
                    }
                });
            }
        }

        if (Action == "UnBan") {
            var data = {
                ID: "1",
                PlayerAction: "UB PlayerName=" + BanPlayername

            };
            //console.log(data);
            $.ajax({
                type: 'POST',
                url: webAPI + '/api/Player/',
                data: JSON.stringify(data),
                contentType: "application/json",
                // dataType: "json",
                error: function(jqXHR, textStatus, errorThrown) {
                    console.log("FAIL: " + errorThrown);
                },
                success: function(data, textStatus, jqXHR) {
                    console.log("SUCCESS! " + jqXHR);
                    $.each(data, function(index, val) {
                        console.log(index);
                    });
                }
            });
        }
    })



    $(".SendConsole").click(function() {
        //alert("This Function not Available");
        var input = $("#InputConsole").val();
        console.log(input);
        var data = {
            ID: "2",
            ConsoleOUT: input

        };
        $.ajax({
            type: 'POST',
            url: webAPI + '/api/Console/',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });

    })

    $(".PlayerSelect").click(function() {
        $(".PlayerSelect").empty();
        $.each(Name, function(index, val) {
            $(".PlayerSelect").append("<option value=" + val + ">" + val + "</option>");
        })
    })

    $(".BanPlayerSelect").click(function() {
        $(".BanPlayerSelect").empty();
        $.each(banname, function(index, val) {
            $(".BanPlayerSelect").append("<option value=" + val + ">" + val + "</option>");
        })
    })

    $(".IDSelect").click(function() {
        $(".IDSelect").empty();
        $.each(id, function(index, val) {
            $(".IDSelect").append("<option value=" + val + ">" + val + "</option>");
        })
    })

    $(".dashboard").click(function() {
        $("#StatusPanel").show();
        $("#PlayerList").hide();
        $("#Console").hide();
        $("#ResourceManager").hide();
        $("#UserManager").hide();

    });

    $(".playerlist").click(function() {
        $("#StatusPanel").hide();
        $("#PlayerList").show();
        $("#Console").hide();
        $("#ResourceManager").hide();
        $("#UserManager").hide();
    });

    $(".console").click(function() {
        $("#StatusPanel").hide();
        $("#PlayerList").hide();
        $("#Console").show();
        $("#ResourceManager").hide();
        $("#UserManager").hide();
    });

});