﻿var Address = "../../";
var gridManager = null;
var toptoolbarManager = null;
var PageIndex = 1;
var PageNum = 50;
var MaxPage = 0;
var MaxCount = 0;
var Code = GetQueryString("Code");
var State = GetQueryString("State");
var CityCode = ''; //城市编码
var KeyName = ''; // 关键字
var MessageTwo = "";//短信内容二
var CustAddress = "";
var LinkPhone = "";
var BillNumber = "";

$(function () {
    InitControl();

    InitData();
    InitCount();
    if (State == "View") {
        $("#toptoolbar").attr("style", "display:none");
        $("#message").hide();
        $("#bodyInfo").attr("style", "max-height: 600px");
    }
    if (parseInt(MaxCount) <= 0) {
        $("#bodyInfo").attr("style", "max-height: 600px");
        $("#xsDiv").hide();
        $("#grid").hide();
    }
    $("#bodyInfo").css("overflow", "auto");
});

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '提交', icon: 'comment', click: Exam },
            { line: true },
            {
                text: '退出', icon: 'logout', click: function (item) {
                    parent.layer.closeAll('iframe');
                }
            }
        ]
    });

    InitGrid();
}

var search_result;
var all_MaxCount;

function InitCount() {
    var arr = new Array();
    arr[0] = Code;
    arr[1] = KeyName;
    arr[2] = LinkPhone;
    arr[3] = BillNumber;
    arr[4] = CustAddress;

    var Method = "GetXFBSimilarCount";
    var BLL = "BLL_CustomerService";
    var search_result = JSON.parse(ReturnValue(BLL, Method, arr, Address));
    all_MaxCount = search_result;
    $("#spanCount").html("相似客户：<a href='javascript:PageIndex=1;Search();'>联系电话(" + all_MaxCount + "条)</a>&nbsp;&nbsp;");
    $("a").click(function () {
        $(this).css('color', "orange");
        $("a").css({ "color": "orange" }).not($(this)).css({ "color": "blue" });
    });

    Search();
}

function getBlackInfo() {
    var backstr = "";
    var arr = new Array();
    arr[0] = $("#CustBillNumber").html(); 
    var search_result = JSON.parse(ReturnValue("BLL_BlackList", "IsBlackPhone", arr, Address));
    if (search_result == true)
        backstr = "<font color='red'>该客户为黑名单</font>";
    return backstr;
}

function InitData() {
    var arr = new Array();
    arr[0] = Code;
    var oneData = eval(ReturnValue("BLL_CustomerService", "GetXFBCustomerByCode", arr, Address));
    $("#CustOldName").html(oneData[0].Cust_OldName);
    $("#CustName").html(oneData[0].Cust_Name);
    //$("#CustNameKey").html(oneData[0].Cust_NameKey);
    $("#CustPhone").html(oneData[0].Cust_Phone);
    //$("#CustLinkman").html(oneData[0].Cust_Linkman);
    //$("#CustLinkPhone").html(oneData[0].Cust_LinkPhone);
    var addr = oneData[0].Cust_ProvinceName + oneData[0].Cust_CityName;// + oneData[0].Cust_CountyName;
    $("#CustAddress").html(addr);
    var jfaddr = oneData[0].Cust_BelongProvinceName + oneData[0].Cust_BelongCityName;// + oneData[0].Cust_CountyName;
    $("#CustjfAddress").html(jfaddr);
    $("#CustIsBill").html(oneData[0].Cust_IsBill);
    $("#CustBillMoney").html(oneData[0].Cust_BillMoney);
    $("#CustBillNumber").html(oneData[0].Cust_BillNumber);
    $("#CustNature").html(oneData[0].Cust_Nature);
    $("#CustKFVoice").html(oneData[0].Cust_KFVoice);
    $("#CustSource").html(oneData[0].Cust_Source);
    $("#CustWHRemark").html(oneData[0].Cust_WH_Remark);
    $("#txtMessage").val(oneData[0].SMSMsg);
    //MessageTwo = oneData[0].SMSMsgTwo;
    CityCode = oneData[0].Cust_CityCode;
    KeyName = "";//oneData[0].Cust_NameKey;
    CustAddress = oneData[0].Cust_ProvinceName + oneData[0].Cust_CityName + oneData[0].Cust_Address;
    LinkPhone = oneData[0].Cust_LinkPhone;
    BillNumber = oneData[0].Cust_BillNumber;
    $("#spanState").html(" " + oneData[0].Cust_State + "&nbsp;&nbsp;" + getBlackInfo());
}

//初始化预审核
function InitGrid() {
    gridManager = $("#grid").ligerGrid({
        columns: [
            { display: '序号', name: 'rownumber', width: '6%' },
            { display: '来源', name: 'TableName', width: 50 },
            { display: '客户名称', name: 'Cust_Name', minWidth: 95 },
            { display: '客户关键字', name: 'Cust_NameKey', minWidth: 80 },
            { display: '联系电话', name: 'Cust_LinkPhone', width: '8%' },
            { display: '计费号码', name: 'Cust_BillNumber', width: '9%' },
            { display: '状态', name: 'Cust_State', width: '6%' },
            { display: '地址', name: 'Cust_Address', minWidth: 100 }
        ],
        pageSize: PageNum, width: '98.8%', height: '60%',
        pageSizeOptions: PageNum,
        enabledSort: false,
        onToFirst: onToFirst,
        onToPrev: onToPrev,
        onToNext: onToNext,
        onToLast: onToLast,
        isScroll: false
    });
}

// 审核
function Exam() {
    if (State == "Exam") {
        var IsChecked1 = $("#chk1").prop("checked");  // 发送短信

        if (IsChecked1) {
            if (!CreateAndSendMSG()) {
                return;
            }
        }
        var arr = new Array();
        arr[0] = Code;
        arr[1] = $('#message input[name="radAudit"]:checked ').val(); // 是否通过 
        if (arr[1] == "0") {
            arr[2] = $("#chk2").prop("checked") == true ? "1" : "0";  // 是否显示
        } else {
            if ($("#txtRetMsg").val() == "") {
                layer.msg("请输入退回原因!");
                return;
            }
            if ($("#txtRetMsg").val().length > 500) {
                layer.msg("退回原因 最大长度不能超过500！");
                return;
            }
            arr[2] = $("#txtRetMsg").val();       //退回内容
        }
        var result = ReturnValue("BLL_CustomerService", "PassXFB", arr, Address);
        if (result == "true") {
            if (arr[1] == "0") {
                layer.msg("审核成功");
                // 修改营销成功
                var arr1 = new Array();
                arr1[0] = Code;
                arr1[1] = $("#CustOldName").html();
                arr1[2] = $("#CustPhone").html();
                ReturnValue("BLL_CustomerService", "UpXFBAlreadyUse", arr1, Address);
            } else {
                layer.msg("退回成功");
            }

            setTimeout(function () {
                parent.layer.closeAll('iframe');
            }, 2000);
        } else {
            layer.msg("审核失败");
        }
    }
}

// 发送短信
function CreateAndSendMSG() {
    var result = false;
    $.ajax({
        url: Address + '../../Aspx/SendMsg.aspx?method=SendMsgsXFB&Message=' + $("#txtMessage").val() + '&Phone=' + $("#CustPhone").html(),
        cache: false,
        async: false,
        type: "POST",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        success: function (text) {
            if (text == "true") {
                result = true;
            }
            else {
                layer.msg('短信发送失败，请重新审核！');
                result = false;
            }
        }
    })
    return result;
}

function showXS(obj) {
    var para = $(obj).attr("src");
    if (State == "View") {
        $("#bodyInfo").attr("style", "max-height: 600px");
    }

    if (para == "../../Images/up.png") {
        $(obj).attr("src", "../../Images/down.png");
        $("#tdXS").attr("style", "border-bottom: 0px;");
        $("#grid").show();
    } else {
        $(obj).attr("src", "../../Images/up.png");
        $("#tdXS").attr("style", "border-bottom: 1px solid #d3d3d3;");
        $("#grid").hide();
    }
}

//退出
function Close() {
    window.close();
}

function onChange(obj) {
    if (obj.value == "0") {
        $("#trChckMesge").show();
        $("#trMessage").show();
        $("#trRetMesge").hide();
    } else {
        $("#trChckMesge").hide();
        $("#trMessage").hide();
        $("#trRetMesge").show();
        $("#chk1").prop("checked", false);
        $("#chk2").prop("checked", false);
    }
}

function Search() {
    var arr = new Array();
    arr[0] = Code;
    arr[1] = KeyName;
    arr[2] = PageIndex;
    arr[3] = PageNum;

    arr[4] = LinkPhone;
    arr[5] = BillNumber;
    arr[6] = CustAddress;

    var arrStr = arr;
    ReplaceStr(arrStr);
    var Method = "GetSimilarInfo";
    var BLL = "BLL_CustomerService";
    var search_result = JSON.parse(ReturnValue("BLL_CustomerService", "GetXFBSimilarInfo", arr, Address));
    MaxCount = all_MaxCount;
    MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
    gridManager = $("#grid").ligerGrid({
        //url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
        data: search_result
    });
}

function onToFirst() {
    PageIndex = 1;
    Search();
    return false;
}

//末页
function onToLast() {
    PageIndex = MaxPage;
    Search();
    return false;
}
//上一页
function onToPrev() {
    if (PageIndex - 1 > 0) {
        PageIndex = PageIndex - 1;
    } else {
        PageIndex = 1;
    }
    Search();
    return false;
}

//下一页
function onToNext() {
    if (PageIndex + 1 < MaxPage) {
        PageIndex = PageIndex + 1;
    } else {
        PageIndex = MaxPage;
    }
    Search();
    return false;
}