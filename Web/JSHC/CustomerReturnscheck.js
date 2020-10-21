var Address = "../";
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
    
    var arr = new Array();
    arr[0] = CustCode;
    var Result = eval("(" + ReturnValue("BLL_CustomerReturnsDts", "GetBilllist", arr, Address) + ")");
    gridManager = $("#grid").ligerGrid({
        columns: [
                    { display: '年度', name: 'BL_Year', minWidth: 90 },
                    { display: '总计', name: 'BL_TotalMoney', minWidth: 90 },
                    { display: '一月', name: 'BL_One', minWidth: 90 },
                    { display: '二月', name: 'BL_Two', minWidth: 90 },
                    { display: '三月', name: 'BL_Three', minWidth: 90 },
                    { display: '四月', name: 'BL_Four', minWidth: 90 },
                    { display: '五月', name: 'BL_Five', minWidth: 90 },
                    { display: '六月', name: 'BL_Six', minWidth: 90 },
                    { display: '七月', name: 'BL_Seven', minWidth: 90 },
                    { display: '八月', name: 'BL_Eight', minWidth: 90 },
                    { display: '九月', name: 'BL_Nine', minWidth: 90 },
                    { display: '十月', name: 'BL_Ten', minWidth: 90 },
                    { display: '十一月', name: 'BL_Eleven', minWidth: 90 },
                    { display: '十二月', name: 'BL_Twelve', minWidth: 90 }
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
}


//初始化数据
function InitData() {
    var arr = new Array();
    arr[0] = CustCode;
    ReturnMethod("BLL_CustomerReturnsDts", "GetDetails", arr, Address, "GetDataSuccess");

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
