"use strict";
$(function () {
	var start_time =
//1
"2022.4.8 24:00";
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
			$("#jgsj").text("上海每日新增确诊人数预测：- 人");
		}else{
			var i2 = new Date(i1+" 00:00");
			var days = i2.getTime()-new Date('2022/03/15 00:00').getTime();
			var days2 = new Date('2022/05/01 00:00').getTime()-i2.getTime();
			var x = parseInt(days / (1000 * 60 * 60 * 24));
			if(days>=0 && days2>=0){
				var y=
//3
663.851069849629-969.251686597203*x+347.96175609746*x*x-39.3078106459574*x*x*x+1.89234244765721*x*x*x*x-0.0298798165020042*x*x*x*x*x;
//4
				$("#info").text("完成！");
				$("#jgsj").text("预测单日新增："+parseInt(y)+" 人");
			}else{
				$("#info").text("日期必须大于 2022-03-15 且小于 2022-05-01");
				$("#jgsj").text("预测单日新增：- 人");
			}
		}
    });
});