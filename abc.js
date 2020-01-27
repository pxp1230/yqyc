"use strict";
$(function () {
	var start_time = new Date('2020-01-27 16:48');
	$("#start").text(start_time.toLocaleString());
	
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
			$("#jgsj").text("全国确诊人数预测：- 人");
		}else{
			var i2 = new Date(i1);
			var days = i2.getTime()-new Date('2020/01/13').getTime();
			var x = parseInt(days / (1000 * 60 * 60 * 24));
			if(days>=0){
				var y=42.7031733746128-13.3611816846734*x+3.54310302056111*x*x+1.32682091515283*x*x*x-0.172944541268835*x*x*x*x+0.0097905255219494*x*x*x*x*x;
				$("#info").text("完成！");
				$("#jgsj").text("全国确诊人数预测："+parseInt(y)+" 人");
			}else{
				$("#info").text("日期必须大于 2020-01-13");
				$("#jgsj").text("全国确诊人数预测：- 人");
			}
		}
    });
});