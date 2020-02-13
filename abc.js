"use strict";
$(function () {
	var start_time =
//1
"2020.02.12 24:00";
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
			$("#jgsj").text("湖北外全国确诊人数预测：- 人");
		}else{
			var i2 = new Date(i1+" 00:00");
			var days = i2.getTime()-new Date('2020/01/13 00:00').getTime();
			var days2 = new Date('2020/04/01 00:00').getTime()-i2.getTime();
			var x = parseInt(days / (1000 * 60 * 60 * 24));
			if(days>=0 && days2>=0){
				var y=
//3
-122.788360358724+322.102078464961*x-98.9163179158748*x*x+9.94060479377849*x*x*x-0.315254937206187*x*x*x*x+0.0032116574418476*x*x*x*x*x;
//4
				$("#info").text("完成！");
				$("#jgsj").text("湖北外全国确诊人数预测："+parseInt(y)+" 人");
			}else{
				$("#info").text("日期必须大于 2020-01-13 且小于 2020-04-01");
				$("#jgsj").text("湖北外全国确诊人数预测：- 人");
			}
		}
    });
});