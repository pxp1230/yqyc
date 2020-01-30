"use strict";
$(function () {
	var start_time =
//1
"2020.01.29 24:00";
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
			$("#jgsj").text("全国确诊人数预测：- 人");
		}else{
			var i2 = new Date(i1+" 00:00");
			var days = i2.getTime()-new Date('2020/01/13 00:00').getTime();
			var days2 = new Date('2020/04/01 00:00').getTime()-i2.getTime();
			var x = parseInt(days / (1000 * 60 * 60 * 24));
			if(days>=0 && days2>=0){
				var y=
//3
95.4468524252003-256.051143129338*x+146.763515122655*x*x-26.9954726323736*x*x*x+2.0024509803922*x*x*x*x-0.044298741763913*x*x*x*x*x;
//4
				$("#info").text("完成！");
				$("#jgsj").text("全国确诊人数预测："+parseInt(y)+" 人");
			}else{
				$("#info").text("日期必须大于 2020-01-13 且小于 2020-04-01");
				$("#jgsj").text("全国确诊人数预测：- 人");
			}
		}
    });
});