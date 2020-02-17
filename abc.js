"use strict";
$(function () {
	var start_time =
//1
"2020.02.16 24:00";
//2
	$("#start").text(start_time);
	
	var now = new Date();
	var day = ("0" + now.getDate()).slice(-2);
	var month = ("0" + (now.getMonth() + 1)).slice(-2);
	var today = now.getFullYear()+"-"+(month)+"-"+(day);
	$('#content').val(today);
	
    $("#content").keydown(function (e) {
        if (e.keyCode == 13) {
            $("#send-btn").click();
        }
    });
    $("#send-btn").click(function () {
		var i1 = $("#content").val();
		if(i1===""){
            $("#info").text("请输入日期！");
			$("#jgsj").text("湖北外全国新增确诊人数预测：- 人");
		}else{
			var i2 = new Date(i1+" 00:00");
			var days = i2.getTime()-new Date('2020/01/20 00:00').getTime();
			var days2 = new Date('2020/04/01 00:00').getTime()-i2.getTime();
			var x = parseInt(days / (1000 * 60 * 60 * 24));
			if(days>=0 && days2>=0){
				var y=
//3
15.489373112994-11.7466033531028*x+25.7933113842689*x*x-2.50136890818448*x*x*x+0.0845181304612384*x*x*x*x-0.000979488026871604*x*x*x*x*x;
//4
				$("#info").text("完成！");
				$("#jgsj").text("湖北外全国新增确诊人数预测："+parseInt(y)+" 人");
			}else{
				$("#info").text("日期必须大于 2020-01-20 且小于 2020-04-01");
				$("#jgsj").text("湖北外全国新增确诊人数预测：- 人");
			}
		}
    });
});