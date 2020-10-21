var Address = "../../";
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

var searchtype = "0";
var search_result;
var all_MaxCount;
var teliphoneMaxCount;
var addressMaxCount;
var billingnoMaxCount;
var keynameMaxCount;
var tsPhoneMaxCount;

function InitCount() {
    var arr = new Array();
    arr[0] = Code;
    arr[1] = KeyName;
    arr[2] = LinkPhone;
    arr[3] = BillNumber;
    arr[4] = CustAddress;

    var Method = "GetSSBSimilarInfoCount";
    var BLL = "BLL_CustomerService";
    var search_result = JSON.parse(ReturnValue(BLL, Method, arr, Address));
    all_MaxCount = search_result.AllCount;
    teliphoneMaxCount = search_result.PhoneCount;
    addressMaxCount = search_result.AddressCount;
    billingnoMaxCount = search_result.BillnoCount;
    keynameMaxCount = search_result.KeyCount;
    tsPhoneMaxCount = search_result.TsPhoneCount;
    $("#spanCount").html("相似客户：<a href='javascript:PageIndex=1;searchtype=0;Search();'>全部(" + all_MaxCount + "条)</a>&nbsp;&nbsp;" + getGoupInfo());
    $("a").click(function () {
        $(this).css('color', "orange");
        $("a").css({ "color": "orange" }).not($(this)).css({ "color": "blue" });
    });

    Search();
}

function getGoupInfo() {
    var backstr = "";
    if (parseInt(keynameMaxCount) > 0) {
        backstr += "<a href='javascript:PageIndex=1;searchtype=1;Search();'>名称包含关键字：(" + keynameMaxCount + "条)</a>&nbsp;&nbsp;";
    }
    if (parseInt(billingnoMaxCount) > 0) {
        backstr += "<a href='javascript:PageIndex=1;searchtype=2;Search();'>计费号码：(" + billingnoMaxCount + "条)</a>&nbsp;&nbsp;";
    }
    if (parseInt(addressMaxCount) > 0) {
        backstr += "<a href='javascript:PageIndex=1;searchtype=3;Search();'>地址：(" + addressMaxCount + "条)</a>&nbsp;&nbsp;";
    }
    if (parseInt(teliphoneMaxCount) > 0) {
        backstr += "<a href='javascript:PageIndex=1;searchtype=4;Search();'>联系电话：(" + teliphoneMaxCount + "条)</a>&nbsp;&nbsp;"
    }
    if (parseInt(tsPhoneMaxCount) > 0) {
        backstr += "<a href='javascript:PageIndex=1;searchtype=5;Search();'>投诉号码：(" + tsPhoneMaxCount + "条)</a>&nbsp;&nbsp;"
    }
    return backstr;
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
    var oneData = eval(ReturnValue("BLL_CustomerService", "GetSSBCustomerByCode", arr, Address));
    $("#CustOldName").html(oneData[0].Cust_OldName);
    $("#CustName").html(oneData[0].Cust_Name);
    $("#CustNameKey").html(oneData[0].Cust_NameKey);
    $("#CustPhone").html(oneData[0].Cust_Phone);
    $("#CustLinkman").html(oneData[0].Cust_Linkman);
    $("#CustLinkPhone").html(oneData[0].Cust_LinkPhone);
    var addr = oneData[0].Cust_ProvinceName + oneData[0].Cust_CityName + oneData[0].Cust_CountyName + " (" + oneData[0].Cust_Address + ")";
    $("#CustAddress").html(addr);
    var jfaddr = oneData[0].Cust_BelongProvinceName + oneData[0].Cust_BelongCityName;
    $("#CustjfAddress").html(jfaddr);
    $("#CustIsBill").html(oneData[0].Cust_IsBill);
    $("#CustBillMoney").html(oneData[0].Cust_BillMoney);
    $("#CustBillNumber").html(oneData[0].Cust_BillNumber);
    $("#CustNature").html(oneData[0].Cust_Nature);
    $("#CustKFVoice").html(oneData[0].Cust_KFVoice);
    $("#CustSource").html(oneData[0].Cust_Source);
    $("#CustWHRemark").html(oneData[0].Cust_WH_Remark);
    $("#txtMessage").val('');//oneData[0].SMSMsg 模板待定
    //MessageTwo = oneData[0].SMSMsgTwo;
    CityCode = oneData[0].Cust_CityCode;
    KeyName = oneData[0].Cust_NameKey;
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
            { display: '计费号码', name: 'Cust_BillNumber', width: '8%' },
            { display: '投诉号码', name: 'Cust_TSPhone', width: '8%' },
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

        if (IsChecked1 && $("#txtMessage").val() != '') {
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
        var result = ReturnValue("BLL_CustomerService", "PassSSB", arr, Address);
        if (result == "true") {
            if (arr[1] == "0") {
                layer.msg("审核成功");
                // 修改营销成功
                var arr1 = new Array();
                arr1[0] = Code;
                arr1[1] = $("#CustOldName").html();
                arr1[2] = $("#CustPhone").html();
                ReturnValue("BLL_CustomerService", "UpSSBAlreadyUse", arr1, Address);

                //审核成功后 向JSXFWQTDB库中的客户表添加信息 不同步开户
                //var arr2 = new Array();
                //arr2[0] = CityCode;
                //arr2[1] = $("#CustName").html();
                //arr2[2] = $("#CustAddress").html();
                //arr2[3] = $("#CustBillNumber").html();
                //arr2[4] = $("#CustLinkman").html();
                //arr2[5] = $("#CustLinkPhone").html();
                //ReturnValue("BLL_CustomerService", "AddOtherCustomer", arr2, Address);
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
        url: Address + '../../Aspx/SendMsg.aspx?method=SendMsgsET&Message=' + encodeURI($("#txtMessage").val()) + '&Phone=' + $("#CustLinkPhone").html(),
        cache: false,
        async: false,
        success: function (text) {
            if (text == "true") {
                result = true;
                //if (MessageTwo != "") {
                //    CreateAndSendMSGTwo();
                //}
            }
            else {
                layer.msg('短信发送失败，请重新审核！');
                result = false;
            }
        }
    })
    return result;
}

// 发送短信
function CreateAndSendMSGTwo() {
    $.ajax({
        url: Address + '../../Aspx/SendMsg.aspx?method=SendMsgsET&Message=' + MessageTwo + '&Phone=' + $("#CustLinkPhone").html(),
        cache: false,
        async: false,
        success: function (text) {
        }
    })
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
    if (searchtype == "0") {//全部
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
        var search_result = JSON.parse(ReturnValue("BLL_CustomerService", "GetSSBSimilarInfo", arr, Address));
        // alert(JSON.stringify(search_result));
        MaxCount = all_MaxCount;
        MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
        gridManager = $("#grid").ligerGrid({
            //url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
            data: search_result
        });
    }
    else if (searchtype == 1) {//名称包含关键字
        arr[0] = Code;
        arr[1] = KeyName;
        arr[2] = PageIndex;
        arr[3] = PageNum;
        var Method = "GetSSBNameContainsKeySimilarInfo";
        var BLL = "BLL_CustomerService";
        var search_result = JSON.parse(ReturnValue(BLL, Method, arr, Address));
        MaxCount = keynameMaxCount;
        MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
        gridManager = $("#grid").ligerGrid({
            //url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
            data: search_result
        });
    }
    else if (searchtype == 2) {//计费号码相
        arr[0] = Code;
        arr[1] = BillNumber;
        arr[2] = PageIndex;
        arr[3] = PageNum;
        var Method = "GetSSBBillNoSimilarInfo";
        var BLL = "BLL_CustomerService";
        var search_result = JSON.parse(ReturnValue(BLL, Method, arr, Address));
        MaxCount = billingnoMaxCount;
        MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
        gridManager = $("#grid").ligerGrid({
            //url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
            data: search_result
        });
    }
    else if (searchtype == 3) {//地址相似
        arr[0] = Code;
        arr[1] = CustAddress;
        arr[2] = PageIndex;
        arr[3] = PageNum;
        var Method = "GetSSBCustAddressSimilarInfo";
        var BLL = "BLL_CustomerService";
        var search_result = JSON.parse(ReturnValue(BLL, Method, arr, Address));
        MaxCount = addressMaxCount;
        MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
        gridManager = $("#grid").ligerGrid({
            //url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
            data: search_result
        });
    }
    else if (searchtype == 4) {//联系电话相似
        arr[0] = Code;
        arr[1] = LinkPhone;
        arr[2] = PageIndex;
        arr[3] = PageNum;
        var arrStr = arr;
        ReplaceStr(arrStr);
        var Method = "GetSSBCustPhoneSimilarInfo";
        var BLL = "BLL_CustomerService";
        var search_result = JSON.parse(ReturnValue(BLL, Method, arr, Address));
        MaxCount = teliphoneMaxCount;
        MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
        gridManager = $("#grid").ligerGrid({
            //url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
            data: search_result
        });
    }
    else {
        arr[0] = Code;
        arr[1] = LinkPhone;
        arr[2] = BillNumber;
        arr[3] = PageIndex;
        arr[4] = PageNum;
        var arrStr = arr;
        ReplaceStr(arrStr);
        var Method = "GetSSBCustTsPhoneSimilarInfo";
        var BLL = "BLL_CustomerService";
        var search_result = JSON.parse(ReturnValue(BLL, Method, arr, Address));
        MaxCount = tsPhoneMaxCount;
        MaxPage = (MaxCount % PageNum == 0 ? MaxCount / PageNum : Math.ceil((MaxCount / PageNum)));
        gridManager = $("#grid").ligerGrid({
            //url: "../Ashx/GetPublicData.ashx?Data=" + Method + "臡" + BLL + "臡" + encodeURI(arrStr)
            data: search_result
        });
    }
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