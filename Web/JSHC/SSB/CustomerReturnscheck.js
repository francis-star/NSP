﻿var Address = "../../";
var gridManager;
var gridManager2;
var count = 0;
var Dataz = "";
var CustCode = GetQueryString("Code");
var State = GetQueryString("State");

$(function () {
    InitControl();
    InitData();
    $("#bodyInfo").css("overflow", "auto");
});

//初始化控件   
function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '关闭', icon: 'logout', click: Close }
        ]
    }); 

}

//初始化数据
function InitData() {
    var arr = new Array();
    arr[0] = CustCode;
    ReturnMethod("BLL_CustomerReturnsDts", "GetSSBDetails", arr, Address, "GetDataSuccess");

    //获取计费数据
    var data = eval(ReturnValue("BLL_CustomerReturnsDts", "GetBillInfo", arr, Address));
    if (data.length == 0) {
        $("#dvMonth").hide();
    }
    else {
        var trHtml = '';
        for (var i = 0; i < data.length; i++) {

            trHtml += ' <tr>';
            trHtml += '       <td>' + data[i].year + '</td>';
            trHtml += '        <td>' + data[i].YTotal + '</td>';
            if (data[i].January == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',1);">' + data[i].January + '</a></td>';


            if (data[i].February == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',2);">' + data[i].February + '</a></td>';
            if (data[i].March == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',3);">' + data[i].March + '</a></td>';
            if (data[i].April == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',4);">' + data[i].April + '</a></td>';
            if (data[i].May == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',5);">' + data[i].May + '</a></td>';
            if (data[i].June == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',6);">' + data[i].June + '</a></td>';
            if (data[i].July == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',7);">' + data[i].July + '</a></td>';
            if (data[i].August == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',8);">' + data[i].August + '</a></td>';
            if (data[i].September == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',9);">' + data[i].September + '</a></td>';
            if (data[i].October == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',10);">' + data[i].October + '</a></td>';
            if (data[i].November == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',11);">' + data[i].November + '</a></td>';
            if (data[i].December == "0")
                trHtml += '       <td></td>';
            else
                trHtml += '        <td><a style="color:blue;cursor:pointer;" onclick="showMonthDts(' + data[i].year + ',12);">' + data[i].December + '</a></td>';

            trHtml += '    </tr > ';
        }
        $("#tbTotal").append(trHtml);
    }
} 


function showMonthDts(year, month) {
    $("#dvMonth").show();
    $("#theadMonth").html(month < 10 ? year + "/0" + month : year + "/" + month);
    var arr = new Array();
    arr[0] = CustCode;
    arr[1] = year;
    arr[2] = month;

    var days = eval(ReturnValue("BLL_CustomerReturnsDts", "GetBillDtsInfo", arr, Address));
    if (days.length == 0) {
        $("#dvMonth").hide();
    }
    else {
        var trOneHtml = '', trTwoHeadHtml = '', trTwoHtml = '';
        for (var i = 0; i < 20; i++) {
            trOneHtml += '<td>' + days[i].Fee + '</td>';
        }
        for (var i = 20; i < 40; i++) {
            if (i < days.length) {
                trTwoHeadHtml += '<td>' + days[i].day + '</td>';
                trTwoHtml += '<td>' + days[i].Fee + '</td>';
            }
            else {
                trTwoHeadHtml += '<td></td>';
                trTwoHtml += '<td></td>';
            }
        }

        $("#trOne").html(trOneHtml);
        $("#trTwoHead").html(trTwoHeadHtml);
        $("#trTwo").html(trTwoHtml);
    }
}
var CustomMethod = {
    GetDataSuccess: function (text) {
        Dataz = eval("(" + text + ")");
        $("#Cust_State1").text(Dataz.Rows[0].Cust_State);
        if (Dataz.Rows[0].Cust_IsView == "1") {
            $("#Cust_IsView").text("显示");
        } else {
            $("#Cust_IsView").text("不显示");
        }

        $("#Cust_Name").text(Dataz.Rows[0].Cust_Name);
        $("#Cust_NameKey").text(Dataz.Rows[0].Cust_NameKey);
        $("#Cust_Phone").text(Dataz.Rows[0].Cust_Phone);
        $("#Cust_Linkman").text(Dataz.Rows[0].Cust_Linkman);
        $("#Cust_LinkPhone").text(Dataz.Rows[0].Cust_LinkPhone);
        $("#Cust_Area").text(Dataz.Rows[0].Cust_ProvinceName + Dataz.Rows[0].Cust_CityName + Dataz.Rows[0].Cust_CountyName + "(" + Dataz.Rows[0].Cust_Address + ")");
        $("#Cust_jf_Area").text(Dataz.Rows[0].Cust_BelongProvinceName + Dataz.Rows[0].Cust_BelongCityName);
        $("#Cust_IsBill").text(Dataz.Rows[0].Cust_IsBill);
        $("#Cust_BillMoney").text(Dataz.Rows[0].Cust_BillMoney);
        $("#Cust_BillNumber").text(Dataz.Rows[0].Cust_BillNumber);
        $("#Cust_Nature").text(Dataz.Rows[0].Cust_Nature);
        $("#Cust_WH_UserName").text(Dataz.Rows[0].Cust_WH_UserName);
        $("#Cust_KFVoice").text(Dataz.Rows[0].Cust_KFVoice);
        $("#Cust_Source").text(Dataz.Rows[0].Cust_Source);
        $("#Cust_WH_Remark").text(Dataz.Rows[0].Cust_WH_Remark);
        if (Dataz.Rows[0].Cust_OperateTime != "" && Dataz.Rows[0].Cust_OperateTime != null) {
            var Cust_OperateTime = (Dataz.Rows[0].Cust_OperateTime).substring(0, 10);
            $("#Cust_OperateTime").text(Cust_OperateTime);
        } else {
            $("#Cust_UnOrder").text("");
        }

        $("#Cust_State").text(Dataz.Rows[0].Cust_State);
        if (Dataz.Rows[0].Cust_UnOrder != "" && Dataz.Rows[0].Cust_UnOrder != null) {
            var Cust_UnOrder = (Dataz.Rows[0].Cust_UnOrder).substring(0, 10);
            $("#Cust_UnOrder").text(Cust_UnOrder);
        } else {
            $("#Cust_UnOrder").text("");
        }

        if (Dataz.Rows[0].Cust_OutDate != "" && Dataz.Rows[0].Cust_OutDate != null) {
            var Cust_OutDate = (Dataz.Rows[0].Cust_OutDate).substring(0, 10);
            $("#Cust_OutDate").text(Cust_OutDate);
        } else {
            $("#Cust_OutDate").text("");
        }
        if (Dataz.Rows[0].Cust_OutMoney != "" && Dataz.Rows[0].Cust_OutMoney != null) {
            $("#Cust_OutMoney").text(Dataz.Rows[0].Cust_OutMoney);
        } else {
            $("#Cust_OutMoney").text("");
        }
        if (Dataz.Rows[0].Cust_ReturnMan != "" && Dataz.Rows[0].Cust_ReturnMan != null) {
            $("#Cust_ReturnMan").text(Dataz.Rows[0].Cust_ReturnMan);
        } else {
            $("#Cust_ReturnMan").text("");
        }
        if (Dataz.Rows[0].Cust_IsLessMoney != "" && Dataz.Rows[0].Cust_IsLessMoney != null) {
            $("#Cust_IsLessMoney").text(Dataz.Rows[0].Cust_IsLessMoney);
        } else {
            $("#Cust_IsLessMoney").text("");
        }
        if (Dataz.Rows[0].Cust_TSNatrue != "" && Dataz.Rows[0].Cust_TSNatrue != null) {
            $("#Cust_TSNatrue").text(Dataz.Rows[0].Cust_TSNatrue);
        } else {
            $("#Cust_TSNatrue").text("");
        }
        if (Dataz.Rows[0].Cust_TSSource != "" && Dataz.Rows[0].Cust_TSSource != null) {
            $("#Cust_TSSource").text(Dataz.Rows[0].Cust_TSSource);
        } else {
            $("#Cust_TSSource").text("");
        }
        if (Dataz.Rows[0].Cust_TSPhone != "" && Dataz.Rows[0].Cust_TSPhone != null) {
            $("#Cust_TSPhone").text(Dataz.Rows[0].Cust_TSPhone);
        } else {
            $("#Cust_TSPhone").text("");
        }
        if (Dataz.Rows[0].Cust_KF_Remark != "" && Dataz.Rows[0].Cust_KF_Remark != null) {
            $("#Cust_KF_Remark").text(Dataz.Rows[0].Cust_KF_Remark);
        } else {
            $("#Cust_KF_Remark").text("");
        }
        if (Dataz.Rows[0].Cust_IsKeep != "" && Dataz.Rows[0].Cust_IsKeep != null) {
            $("#Cust_IsKeep").text(Dataz.Rows[0].Cust_IsKeep);
        } else {
            $("#Cust_IsKeep").text("");
        }
    }
}

//取消
function Close() {
    parent.layer.closeAll('iframe');
}
