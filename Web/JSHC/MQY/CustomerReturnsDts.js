var Address = "../../";
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
            { text: '提交', icon: 'comment', click: Exam },
            { line: true },
            { text: '取消', icon: 'logout', click: Close }
        ]
    });
    $("#Cust_UnOrder").ligerDateEditor({ format: "yyyy-MM-dd", width: 180, cancelable: false });
    $("#Cust_OutDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 180, cancelable: false });

    var TSSourceData = [{ id: 1, text: "省号百" }, { id: 2, text: "省投" }, { id: 3, text: "区域中心" }, { id: 4, text: "南京号百" }, { id: 5, text: "用户" }, { id: 6, text: "客户经理" }, { id: 7, text: "地市号百" }, { id: 8, text: "地市营业厅" }, { id: 9, text: "公司" }, { id: 10, text: "智恒" }];
    $("#Cust_TSSource").ligerComboBox({
        data: TSSourceData,
        textField: 'text',
        width: 180,
        cancelable: false
    });

    $("#Cust_State").ligerComboBox({
        width: 160,
        selectBoxWidth: 160,
        cancelable: false,
        data: [
            { text: '已审', id: '已审' },
            { text: '退订', id: '退订' },
            { text: '退费', id: '退费' }
        ]
    });

    var TSNatrueData = [{ id: 1, text: "一般投诉" }, { id: 2, text: "重大投诉" }, { id: 3, text: "恶劣投诉" }];
    $("#Cust_TSNatrue").ligerComboBox({
        data: TSNatrueData,
        textField: 'text',
        width: 180,
        cancelable: false,
        onSelected: function (newvalue) {
            if (newvalue == "1" || newvalue == "") {
                $("#Cust_IsAddBlack").prop("checked", false);
            }
            else {
                $("#Cust_IsAddBlack").prop("checked", true);
            }
        }
    });
    var arr = new Array();
    arr[0] = CustCode;
    var Result = eval("(" + ReturnValue("BLL_CustomerReturnsDts", "GetMQYBilllist", arr, Address) + ")");
    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '年度', name: 'BL_Year', minWidth: 60 },
            { display: '总计', name: 'BL_TotalMoney', minWidth: 60 },
            { display: '一月', name: 'BL_One', minWidth: 60 },
            { display: '二月', name: 'BL_Two', minWidth: 60 },
            { display: '三月', name: 'BL_Three', minWidth: 60 },
            { display: '四月', name: 'BL_Four', minWidth: 60 },
            { display: '五月', name: 'BL_Five', minWidth: 60 },
            { display: '六月', name: 'BL_Six', minWidth: 60 },
            { display: '七月', name: 'BL_Seven', minWidth: 60 },
            { display: '八月', name: 'BL_Eight', minWidth: 60 },
            { display: '九月', name: 'BL_Nine', minWidth: 60 },
            { display: '十月', name: 'BL_Ten', minWidth: 60 },
            { display: '十一月', name: 'BL_Eleven', minWidth: 60 },
            { display: '十二月', name: 'BL_Twelve', minWidth: 60 }
        ],
        pageSize: 10, width: '99.8%', height: '150px',
        pageSizeOptions: [10], enabledSort: false,
        userPager: false, isScroll: true,
        enabledSort: false, data: Result

    });

    ///登录记录lcl
    //var ar = new Array();
    //ar[0] = BillNumber;
    //ar[1] = CustPhone1;
    //var Result2 = eval("(" + ReturnValue("BLL_CustomerReturnsDts", "GetLoginLog", ar, Address) + ")");
    gridManager2 = $("#grid2").ligerGrid({
        columns: [
            { display: '登录名', name: 'LL_U_LoginName', minWidth: 90 },
            { display: '登录时间', name: 'LL_LoginTime', minWidth: 90 }
        ],
        pageSize: 20, width: '99.8%', height: '150px',
        pageSizeOptions: [10],
        // enabledSort: false, data: Result2
    });

    $("#Cust_UnOrder").ligerGetDateEditorManager().setDisabled();
    $("#Cust_OutDate").ligerGetDateEditorManager().setDisabled();
    $("#Cust_TSNatrue").ligerTextBox().setEnabled();
    $("#Cust_TSSource").ligerTextBox().setEnabled();
    $("#Cust_ReturnMan").attr("disabled", true);
    $("#Cust_OutMoney").attr("disabled", true);
    $("#Cust_KF_Remark").attr("disabled", false);
    //$("#Cust_IsLessMoney").attr("disabled", true);
}

//初始化数据
function InitData() {
    var arr = new Array();
    arr[0] = CustCode;
    ReturnMethod("BLL_CustomerReturnsDts", "GetMQYDetails", arr, Address, "GetDataSuccess");
    ReturnMethod("BLL_CustomerReturnsDts", "GetLastestCustState", arr, Address, "GetLastestState");
}

function change() {
    if ($("#Cust_State").val() == "退订") {
        var d = new Date();
        var str = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();

        if (Dataz.Rows[0].Cust_UnOrder != "" && Dataz.Rows[0].Cust_UnOrder != null) {
            var Cust_UnOrder = (Dataz.Rows[0].Cust_UnOrder).substring(0, 10);
            $("#Cust_UnOrder").val(Cust_UnOrder);
        } else {
            $("#Cust_UnOrder").val(str);
        }

        $("#Cust_OutDate").val("");
        $("#Cust_OutMoney").val("");
        $("#Cust_ReturnMan").val("");
        //$("#Cust_IsLessMoney").attr("checked", false);

        if (Dataz.Rows[0].Cust_TSNatrue != "" && Dataz.Rows[0].Cust_TSNatrue != null) {
            $("#Cust_TSNatrue").val(Dataz.Rows[0].Cust_TSNatrue);
        }

        if (Dataz.Rows[0].Cust_TSSource != "" && Dataz.Rows[0].Cust_TSSource != null) {
            $("#Cust_TSSource").val(Dataz.Rows[0].Cust_TSSource);
        }
        if (Dataz.Rows[0].Cust_KF_Remark != "" && Dataz.Rows[0].Cust_KF_Remark != null) {
            $("#Cust_KF_Remark").val(Dataz.Rows[0].Cust_KF_Remark);
        }

        $("#Cust_UnOrder").ligerGetDateEditorManager().setEnabled();
        $("#Cust_OutDate").ligerGetDateEditorManager().setDisabled();
        $("#Cust_TSNatrue").ligerTextBox().setEnabled();
        $("#Cust_TSSource").ligerTextBox().setEnabled();
        $("#Cust_ReturnMan").attr("disabled", true);
        $("#Cust_OutMoney").attr("disabled", true);
        $("#Cust_KF_Remark").attr("disabled", false);
        //$("#Cust_IsLessMoney").attr("disabled", true);
    }
    else if ($("#Cust_State").val() == "退费") {
        var d = new Date();
        var str = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();

        if (Dataz.Rows[0].Cust_UnOrder != "" && Dataz.Rows[0].Cust_UnOrder != null) {
            var Cust_UnOrder = (Dataz.Rows[0].Cust_UnOrder).substring(0, 10);
            $("#Cust_UnOrder").val(Cust_UnOrder);
        } else {
            $("#Cust_UnOrder").val(str);
        }
        if (Dataz.Rows[0].Cust_OutDate != "" && Dataz.Rows[0].Cust_OutDate != null) {
            var Cust_OutDate = (Dataz.Rows[0].Cust_OutDate).substring(0, 10);
            $("#Cust_OutDate").val(Cust_OutDate);
        } else {
            $("#Cust_OutDate").val(str);
        }

        if (Dataz.Rows[0].Cust_ReturnMan != "" && Dataz.Rows[0].Cust_ReturnMan != null) {
            $("#Cust_ReturnMan").val(Dataz.Rows[0].Cust_ReturnMan);
        } else {
            var strName = ReturnValue("BLL_Main", "GetLoginMan", "", Address);
            $("#Cust_ReturnMan").val(strName);
        }

        $("#Cust_OutMoney").val(Dataz.Rows[0].Cust_OutMoney);
        if (Dataz.Rows[0].Cust_IsLessMoney == "是") {
            $("#Cust_IsLessMoney").attr("checked", true);
        } else {
            $("#Cust_IsLessMoney").attr("checked", false);
        }
        if (Dataz.Rows[0].Cust_TSNatrue != "" && Dataz.Rows[0].Cust_TSNatrue != null) {
            $("#Cust_TSNatrue").val(Dataz.Rows[0].Cust_TSNatrue);
        }

        if (Dataz.Rows[0].Cust_TSSource != "" && Dataz.Rows[0].Cust_TSSource != null) {
            $("#Cust_TSSource").val(Dataz.Rows[0].Cust_TSSource);
        }
        if (Dataz.Rows[0].Cust_KF_Remark != "" && Dataz.Rows[0].Cust_KF_Remark != null) {
            $("#Cust_KF_Remark").val(Dataz.Rows[0].Cust_KF_Remark);
        }

        $("#Cust_UnOrder").ligerGetDateEditorManager().setEnabled();
        $("#Cust_OutDate").ligerGetDateEditorManager().setEnabled();
        $("#Cust_TSNatrue").ligerTextBox().setEnabled();
        $("#Cust_TSSource").ligerTextBox().setEnabled();
        $("#Cust_ReturnMan").attr("disabled", false);
        $("#Cust_OutMoney").attr("disabled", false);
        $("#Cust_KF_Remark").attr("disabled", false);
        //$("#Cust_IsLessMoney").attr("disabled", false);
    }
    else if ($("#Cust_State").val() == "已审") {
        $("#Cust_UnOrder").val("");
        $("#Cust_OutDate").val("");
        $("#Cust_OutMoney").val("");
        $("#Cust_ReturnMan").val("");
        //$("#Cust_IsLessMoney").attr("checked", false);

        $("#Cust_UnOrder").ligerGetDateEditorManager().setDisabled();
        $("#Cust_OutDate").ligerGetDateEditorManager().setDisabled();
        $("#Cust_TSNatrue").ligerTextBox().setEnabled();
        $("#Cust_TSSource").ligerTextBox().setEnabled();
        $("#Cust_ReturnMan").attr("disabled", true);
        $("#Cust_OutMoney").attr("disabled", true);
        $("#Cust_KF_Remark").attr("disabled", false);
        //$("#Cust_IsLessMoney").attr("disabled", true);
    }
}

//保存信息 
function Exam() {
    if (VaildData()) {
        var arr = new Array();
        arr[0] = CustCode;
        arr[1] = $("#Cust_State").val();
        arr[2] = $("#Cust_UnOrder").val();
        arr[3] = $("#Cust_OutDate").val();
        arr[4] = $("#Cust_OutMoney").val();
        arr[5] = $("#Cust_ReturnMan").val();

        var IsLessMoney = "";
        if ($("#Cust_IsLessMoney").prop("checked")) {
            IsLessMoney = "是";
        } else {
            IsLessMoney = "否";
        }

        arr[6] = IsLessMoney;
        arr[7] = $("#Cust_TSNatrue").val();
        arr[8] = $("#Cust_TSSource").val();
        arr[9] = $("#Cust_KF_Remark").val();
        if ($("#Cust_IsKeep").prop("checked")) {
            arr[10] = "是";
        } else {
            arr[10] = "否";
        }
        arr[11] = $("#Cust_TSPhone").val();
        if ($("#Cust_IsAddBlack").prop("checked")) {
            arr[12] = "1";
        } else {
            arr[12] = "0";
        }

        ReturnMethod("BLL_CustomerReturnsDts", "ModMQYState", arr, Address, "SaveQuitSuccess");
    }
}

function VaildData() {
    if ($("#Cust_State").val() == "") {
        $.ligerDialog.warn('客户状态不能为空！');
        $("#Cust_State").focus();
        return false;
    }
    if ($("#Cust_TSNatrue").val() == "") {
        $.ligerDialog.warn('投诉性质不能为空！');
        $("#Cust_TSNatrue").focus();
        return false;
    }
    if ($("#Cust_TSPhone").val().trim() == "") {
        $.ligerDialog.warn('投诉号码不能为空！');
        $("#Cust_TSPhone").focus();
        return false;
    }
    if ($("#Cust_KF_Remark").val().length > 600) {
        $.ligerDialog.warn('客服备注 最大长度不能超过600！');
        return;
    }
    return true;
}

var CustomMethod = {
    SaveQuitSuccess: function (text) {
        if (text == "true") {
            layer.msg('提交成功！');
            setTimeout(function () {
                parent.layer.closeAll('iframe');
            }, 2000);
        }
        else {
            layer.msg('提交失败！');
        }
    }, GetDataSuccess: function (text) {
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
        $("#Cust_State").val(Dataz.Rows[0].Cust_State);

        if (Dataz.Rows[0].Cust_UnOrder != "" && Dataz.Rows[0].Cust_UnOrder != null) {
            var Cust_UnOrder = (Dataz.Rows[0].Cust_UnOrder).substring(0, 10);
            $("#Cust_UnOrder").val(Cust_UnOrder);
        } else {
            $("#Cust_UnOrder").val("");
        }
        if (Dataz.Rows[0].Cust_OutDate != "" && Dataz.Rows[0].Cust_OutDate != null) {
            var Cust_OutDate = (Dataz.Rows[0].Cust_OutDate).substring(0, 10);
            $("#Cust_OutDate").val(Cust_OutDate);
        } else {
            $("#Cust_OutDate").val("");
        }
        $("#Cust_OutMoney").val(Dataz.Rows[0].Cust_OutMoney);
        if (Dataz.Rows[0].Cust_ReturnMan != "" && Dataz.Rows[0].Cust_ReturnMan != null) {
            $("#Cust_ReturnMan").val(Dataz.Rows[0].Cust_ReturnMan);
        } else {
            $("#Cust_ReturnMan").val("");
        }

        if (Dataz.Rows[0].Cust_IsLessMoney == "是") {
            $("#Cust_IsLessMoney").attr("checked", true);
        } else {
            $("#Cust_IsLessMoney").attr("checked", false);
        }

        if (Dataz.Rows[0].Cust_IsKeep == "是") {
            $("#Cust_IsKeep").attr("checked", true);
        } else {
            $("#Cust_IsKeep").attr("checked", false);
        }

        $("#Cust_TSNatrue").val(Dataz.Rows[0].Cust_TSNatrue);
        if (Dataz.Rows[0].Cust_TSNatrue == "恶劣投诉" || Dataz.Rows[0].Cust_TSNatrue == "重大投诉")
            $("#Cust_IsAddBlack").prop("checked", true);
        $("#Cust_TSSource").val(Dataz.Rows[0].Cust_TSSource);
        $("#Cust_TSPhone").val(Dataz.Rows[0].Cust_TSPhone);
        if (Dataz.Rows[0].Cust_KF_Remark != "" && Dataz.Rows[0].Cust_KF_Remark != null) {
            $("#Cust_KF_Remark").val(Dataz.Rows[0].Cust_KF_Remark);
        } else {
            $("#Cust_KF_Remark").val("");
        } 
    }, GetLastestState: function (text) {
        var dataState = eval("(" + text + ")");
        if (dataState.total > 0) {
            $("#sp_LastModify").html("<br/>" + dataState.Rows[0].User_Name + " 在" +
                dataState.Rows[0].CSH_Time + "修改客户状态：" + dataState.Rows[0].CSH_Remark);
        }
    }
}

//取消
function Close() {
    parent.layer.closeAll('iframe');
}
