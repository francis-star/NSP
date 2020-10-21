var gridManager = null; //列表控件
var groupicon = "../JS/lib/ligerUI/skins/icons/communication.gif";
var form;
var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var Code = GetQueryString("Code");
var OldName = "";
var UserName = "";
var CityName = "";
var Cus_code = '';
var fromPage = GetQueryString("fromPage");//记录是从哪个画面过来的 CustomerReturns_A:修改
var updateLog = '';
var Ischeck = '';

$(function () {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '保存', icon: 'save', click: SaveData }
        ]
    });

    UserName = ReturnValue("BLL_CallCenterDts", "GetUserName", "", Address);

    InitControl();
    GetArea1();
    GetjfArea1();
    displayShowByFromPage();
    if (Code != "") {
        document.getElementById("SearchDiv").style.display = "none";
        document.getElementById("SearchDiv1").style.display = "block";
        GetData();
    } else {
        document.getElementById("SearchDiv").style.display = "block";
        document.getElementById("SearchDiv1").style.display = "none";
    }

    $("#ODDPhone").bind("keyup click", function () {
        if ($("#ODDPhone").val().length > 1) {
            var arr = new Array();
            arr[0] = $("#ODDPhone").val();
            var result = eval("(" + ReturnValue("BLL_CallCenterDts", "GetXFBPhone", arr, Address) + ")");
            var html = "";
            for (var i = 0; i < result.length; i++) {
                html += "<li>" + result[i].ODD_Phone + "</li>";
            }
            $("#search-result").html(html);
            $("#search-suggest").show().css({
                top: $("#ODDPhone").offset().top + 30,
                left: $("#ODDPhone").offset().left,
                position: 'absolute'
            });
        }
        liPlat = "1";
    });

    //查询名称
    $("#ODDName").bind("keyup click", function () {
        if ($("#ODDName").val().length > 1) {
            var arr = new Array();
            arr[0] = $("#ODDName").val();
            var result = eval("(" + ReturnValue("BLL_CallCenterDts", "GetXFBName", arr, Address) + ")");
            var html = "";
            for (var i = 0; i < result.length; i++) {
                html += "<li>" + result[i].ODD_Name + "</li>";
            }
            $("#search-result").html(html);
            $("#search-suggest").show().css({
                top: $("#ODDName").offset().top + 30,
                left: $("#ODDName").offset().left,
                position: 'absolute'
            });
        }
        liPlat = "2";
    });
    $(document).delegate('li', 'click', function () {
        var arr = new Array();
        arr[0] = $(this).text();
        var result;
        result = ReturnValue("BLL_BlackList", "IsBlackPhone", arr, Address);

        if (result != 'true') {
            if (liPlat == "1") {
                arr[0] = $(this).text();
                arr[1] = "";
                result = eval("(" + ReturnValue("BLL_CallCenterDts", "GetXFBComData", arr, Address) + ")");
            }
            else {
                arr[0] = "";
                arr[1] = $(this).text();
                result = eval("(" + ReturnValue("BLL_CallCenterDts", "GetXFBComData", arr, Address) + ")");
            }
            $("#CustName").val(result[0].ODD_Name);
            OldName = result[0].ODD_Name;
            CityName = result[0].OD_CityName;
            $("#CustPhone1").val(result[0].ODD_Phone);
            $("#Cust_IsBill").ligerComboBox({
                value: result[0].ODD_IsBill,
            });
            $("#Province").ligerComboBox({
                value: result[0].OD_ProvinceCode,
            });
            $("#City").ligerComboBox({
                value: result[0].OD_CityCode,
            });
            $("#jfProvince").ligerComboBox({
                value: result[0].OD_ProvinceCode,
            });
            $("#jfCity").ligerComboBox({
                value: result[0].OD_CityCode,
            });
            $("#Cust_BillMoney").val(result[0].OD_BillMoney);
            $("#Cust_BillNumber").val(result[0].ODD_Phone);
            $("#Cust_KFVoice").val(UserName + result[0].ODD_Phone);
            $("#Cust_Source").val(result[0].OD_Provider);
            $("#Cust_WH_Remark").val(result[0].ODD_Remark);
        }
        else {
            $.ligerDialog.warn('黑名单用户');
        }
    });
});

//20170426 add by zhangmin 客服中心修改根据页面传值显示不同
function displayShowByFromPage() {
    if (Code != "") {
        if (fromPage == 'CustomerReturns_A') {
            $('#Cust_BillNumber').attr('disabled', 'disabled');
            $('#CustPhone1').attr('disabled', 'disabled'); 
            //$('#CustAddress').removeAttr('disabled');
            document.getElementById("SearchDiv").style.display = "none";
            document.getElementById("SearchDiv1").style.display = "none";
            $('#spanIsView').css('display', 'inline');
            $('#td_Cust_IsView').css('display', 'inline');
            $('#div_BH_Remark').css('display', 'inline');
            $('#tr_Cust_WH_Remark').css('display', 'none');
            $('#tr_Cust_WH_UserName').css('display', '');

            var IsView = eval("[{ text: '否', id: '0' },{ text: '是', id: '1' }]");
            $("#Cust_IsView").ligerComboBox({
                data: IsView,
                textField: "text",
                valueField: "id",
                valueFieldID: "value10",
                value: IsView[1].id,
                width: 140,
                cancelable: false,
                isMultiSelect: false
            });
        }
    }
    $('#viewhistory').click(function () {
        var arr = new Array();
        arr[0] = Code;
        arr[1] = 1;
        arr[2] = 100;
        var Data = JSON.parse(ReturnValue("BLL_BillHistory", "GetBillHistoryInfo", arr, Address));
        var strHtml = '<div style="width:560px;margin:10px auto">';
        if (Data['total'] > 0) {
            $.each(Data['Rows'], function (id, obj) {
                strHtml += '<div style="margin-top:10px;">';
                strHtml += '<p style="margin-top:5px;"><span>' + obj.BH_User_Name + '</span>  <span>更新了' + obj.BH_Content + '</span>  (' + obj.BH_Time + ') </p>';
                strHtml += '<p style="margin-top:5px;"><span>修改原因：</span></p>';
                strHtml += '<p style="margin-top:5px;"><span>' + obj.BH_Remark + '</span></p>';
                strHtml += '</div>';
                strHtml += '<hr  style="margin-top:5px;height:1px;border:none; border-bottom:#808080 1px dashed" />';
            });
        } else {
            strHtml += '暂无数据！';
        }
        strHtml += '</div>';
        layer.open({
            type: 1,
            skin: 'layui-layer-rim', //加上边框
            area: ['600px', '440px'], //宽高
            title: '历史纪录',
            content: strHtml
        });
    });
}

function InitControl() {
    $("#Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 160,
        cancelable: false,
        isMultiSelect: false
    });
    $("#jfProvince").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value20",
        width: 160,
        cancelable: false,
        isMultiSelect: false
    });
    $("#jfCity").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value11",
        width: 160,
        cancelable: false,
        isMultiSelect: false
    });

    var IsBillData = eval("[{ text: '是', id: '是' },{ text: '否', id: '否' }]");
    $("#Cust_IsBill").ligerComboBox({
        data: IsBillData,
        textField: "text",
        valueField: "id",
        valueFieldID: "value8",
        value: IsBillData[0].id,
        width: 140,
        cancelable: false,
        isMultiSelect: false
    });

    var NatureData = eval("[{ text: '签约', id: '签约' },{ text: '外呼', id: '外呼' },{ text: '消协', id: '消协' },{ text: '套餐用户', id: '套餐用户' },{ text: '非套餐用户', id: '非套餐用户' },{ text: '外呼套餐用户', id: '外呼套餐用户'} ]");
    $("#Cust_Nature").ligerComboBox({
        data: NatureData,
        textField: "text",
        valueField: "id",
        valueFieldID: "value9",
        value: NatureData[1].id,
        width: 140,
        cancelable: false,
        isMultiSelect: false
    });
}

function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_CallCenterDts", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择省\"}]")[0]);//追加全国选项
    }
    $("#Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        value: Data[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            PageIndex = 1;
            if (newvalue != null && newvalue != "") {
                $("#Inp_City").val("");
                $("#value6").val("");

                GetArea2(newvalue);
            } else {
                $("#City").ligerComboBox({
                    data: eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择市\"}]"),
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value6",
                    value: "",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

function GetjfArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_CallCenterDts", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择省\"}]")[0]);//追加全国选项
    }
    $("#jfProvince").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value20",
        value: Data[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            PageIndex = 1;
            if (newvalue != null && newvalue != "") {
                $("#value11").val("");

                GetjfArea2(newvalue);
            } else {
                $("#jfCity").ligerComboBox({
                    data: eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择市\"}]"),
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value11",
                    value: "",
                    width: 135,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

function GetArea2(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = '1';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_CallCenterDts", "GetArea", arr, Address));
    if (Data.length > 0 || DataCode == "") {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择市\"}]")[0]);//追加全国选项
    }
    $("#City").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {

        }
    });
}

function GetjfArea2(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = '1';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_CallCenterDts", "GetArea", arr, Address));
    if (Data.length > 0 || DataCode == "") {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择市\"}]")[0]);//追加全国选项
    }
    $("#jfCity").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value11",
        value: Data[0].SA_Code,
        width: 135,
        isMultiSelect: false,
        cancelable: false
    });
}

function GetData() {
    var arr = new Array();
    arr[0] = Code;
    var Data = eval(ReturnValue("BLL_CallCenterDts", "GetXFBCallCenterDts", arr, Address));
    if (Data.length > 0) {
        if (Data[0].Cust_Code != "") {
            $("#CustName").val(Data[0].Cust_Name);
            OldName = Data[0].Cust_Name;
            CityName = Data[0].Cust_CityName;
            //$("#CustNameKey").val(Data[0].Cust_NameKey);
            $("#CustPhone1").val(Data[0].Cust_Phone);
            //$("#Custlinkman").val(Data[0].Cust_Linkman);
            //$("#CustlinkPhone").val(Data[0].Cust_LinkPhone);
            $("#Province").ligerComboBox({
                value: Data[0].Cust_ProvinceCode,
            });
            $("#City").ligerComboBox({
                value: Data[0].Cust_CityCode,
            });
            $("#jfProvince").ligerComboBox({
                value: Data[0].Cust_BelongProvinceCode,
            });
            $("#jfCity").ligerComboBox({
                value: Data[0].Cust_BelongCityCode,
            });
            //$("#District").ligerComboBox({
            //    value: Data[0].Cust_CountyCode,
            //});
            //$("#CustAddress").val(Data[0].Cust_Address);
            $("#Cust_IsBill").ligerComboBox({
                value: Data[0].Cust_IsBill
            });
            $("#Cust_BillMoney").val(Data[0].Cust_BillMoney);
            $("#Cust_BillNumber").val(Data[0].Cust_BillNumber);
            $("#Cust_Nature").ligerComboBox({
                value: Data[0].Cust_Nature
            });
            $("#Cust_KFVoice").val(Data[0].Cust_KFVoice);
            $("#Cust_Source").val(Data[0].Cust_Source);
            $("#Cust_WH_Remark").val(Data[0].Cust_WH_Remark);
            $("#SP_State").html(Data[0].Cust_State);
            $("#Cust_WH_UserName").val(Data[0].Cust_WH_UserName);

            $("#Cust_IsView").ligerComboBox({
                value: Data[0].Cust_IsView
            });
            if (Data[0].Cust_State == "退回") {
                $("#SP_ReturnContent").html(Data[0].Cust_ReturnContent);
                document.getElementById("SearchDiv2").style.display = "block";
            }
        }
    }
}

function SaveData() {
    if (VaildData() == true) {
        var arr = new Array();
        arr[0] = Code;
        arr[1] = $("#CustName").val();
        arr[2] = OldName;
        arr[3] = "";//$("#CustNameKey").val();
        arr[4] = $("#Cust_BillNumber").val();
        arr[5] = "";// $("#Custlinkman").val();
        arr[6] = $("#Cust_BillNumber").val();
        arr[7] = $("#value5").val();
        arr[8] = $("#Province").val();
        arr[9] = $("#value6").val();
        arr[10] = $("#City").val();
        arr[11] = "";//$("#value7").val();
        arr[12] = "";//$("#District").val();
        arr[13] = "";// $("#CustAddress").val();
        arr[14] = $("#value8").val();
        arr[15] = $("#Cust_BillMoney").val();
        arr[16] = $("#Cust_BillNumber").val();
        arr[17] = $("#value9").val();
        arr[18] = $("#Cust_KFVoice").val();
        arr[19] = $("#Cust_Source").val();
        arr[20] = $("#Cust_WH_Remark").val();
        arr[21] = $("#Cust_IsView").ligerComboBox().getValue();
        arr[22] = fromPage;
        arr[23] = 4;
        arr[24] = $("#Cust_WH_UserName").val();
        arr[25] = Ischeck;
        arr[26] = $("#value20").val();
        arr[27] = $("#jfProvince").val();
        arr[28] = $("#value11").val();
        arr[29] = $("#jfCity").val();
        //20170426 add by zhangmin 客服中心修改
        if (fromPage != 'CustomerReturns_A') {
            if (!validatejfhm($("#Cust_BillNumber").val(),Code))
                return false;
        }
        if (Code == "" || Code == "null" || Code == null) {
            ReturnMethod("BLL_CallCenterDts", "Update", arr, Address, "InsertSuccess");
        } else {
            updateLog = saveLog();
            if (updateLog == '') {
                $.ligerDialog.warn('资料没有变更！');
                return false;
            }
            ReturnMethod("BLL_CallCenterDts", "Update", arr, Address, "UpdateSuccess");
        }
    }
}

var CustomMethod = {
    InsertSuccess: function (text) {
        if (text == "true") {
            $.ligerDialog.success('添加成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        } else {
            $.ligerDialog.error(text);
        }
    },
    UpdateSuccess: function (text) {
        if (text == "true") {
            arr = new Array();
            arr[0] = Code;
            arr[1] = updateLog;
            arr[2] = $('#BH_Remark').val();
            var Data = eval(ReturnValue("BLL_CustomerService", "UpBillHistory", arr, Address));

            $.ligerDialog.success('修改成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        } else {
            if (fromPage == 'CustomerReturns_A') {
                $.ligerDialog.confirm(text, function (yes) {
                    if (yes) {
                        Ischeck = 'N';
                        SaveData();
                        Ischeck = '';
                    }
                });
            } else {
                $.ligerDialog.error(text);
            }
        }
    }
}

//add by paul 20170426 增加修改记录
function saveLog() {
    var msg = '';
    var arr = new Array();
    arr[0] = Code;
    var Data = eval(ReturnValue("BLL_CallCenterDts", "GetXFBCallCenterDts", arr, Address));
    if (Data.length > 0) {
        if (Data[0].Cust_Code != "") {
            Cus_code = Data[0].Cust_Code;
            if ($("#CustName").val() != Data[0].Cust_Name) {
                msg += ',客户名称';
            }
            OldName = Data[0].Cust_Name;
            CityName = Data[0].Cust_CityName;
            //if ($("#CustNameKey").val() != Data[0].Cust_NameKey) {
            //    msg += ',客户关键字';
            //}
            if ($("#CustPhone1").val() != Data[0].Cust_Phone) {
                msg += ',联系电话';
            }
            //if ($("#Custlinkman").val() != Data[0].Cust_Linkman) {
            //    msg += ',联系人';
            //}
            //if ($("#CustlinkPhone").val() != Data[0].Cust_LinkPhone) {
            //    msg += ',联系电话';
            //}
            if ($("#Province").val() != Data[0].Cust_ProvinceName) {
                msg += ',省份';
            }
            if ($("#City").val() != Data[0].Cust_CityName) {
                msg += ',城市';
            }
            //if ($("#District").val() != Data[0].Cust_CountyName) {
            //    msg += ',街道';
            //}
            //if ($("#CustAddress").val() != Data[0].Cust_Address) {
            //    msg += ',详细地址';
            //}
            if ($("#Cust_IsBill").val() != Data[0].Cust_IsBill) {
                msg += ',是否计费';
            }
            if ($("#Cust_IsView").ligerComboBox().getValue() != Data[0].Cust_IsView) {
                msg += ',是否显示';
            }
            if ($("#Cust_BillMoney").val() != Data[0].Cust_BillMoney) {
                msg += ',计费金额';
            }
            if ($("#Cust_BillNumber").val() != Data[0].Cust_BillNumber) {
                msg += ',计费号码';
            }
            if ($("#jfProvince").val() != Data[0].Cust_BelongProvinceName) {
                msg += ',归属地省份';
            }
            if ($("#jfCity").val() != Data[0].Cust_BelongCityName) {
                msg += ',归属地城市';
            }
            if ($("#Cust_Nature").val() != Data[0].Cust_Nature) {
                msg += ',客户性质';
            }
            if ($("#Cust_KFVoice").val() != Data[0].Cust_KFVoice) {
                msg += ',录音编码';
            }
            if ($("#Cust_Source").val() != Data[0].Cust_Source) {
                msg += ',数据来源';
            }
            if ($("#Cust_WH_UserName").val() != Data[0].Cust_WH_UserName) {
                msg += ',外呼人员';
            }
            if ($("#Cust_WH_Remark").val() != Data[0].Cust_WH_Remark) {
                msg += ',备注';
            }
        }
    }
    if (msg.indexOf(',') >= 0)
        msg = msg.substring(1);
    return msg;
}

//2017-04-25　陆明讴添加计费号验证
function validatejfhm(val,code) {
    var arr = new Array();
    arr[0] = val;
    arr[1] = code;
    var Data = JSON.parse(ReturnValue("BLL_CustomerReturnsDts", "GetXFBCustomerListByBillNumber", arr, Address));
    if (Data.length > 0) {
        $.ligerDialog.warn('该计费号码已存在！');
        return false;
    }
    else
        return true;
}

function Logout() {
    parent.layer.closeAll('iframe');
}

function VaildData() {
    if ($("#CustName").val() == "") {
        $.ligerDialog.warn('客户名称不能为空！');
        $("#CustName").focus();
        return false;
    }
    //if ($("#CustNameKey").val() == "") {
    //    $.ligerDialog.warn('客户关键字不能为空！');
    //    $("#CustNameKey").focus();
    //    return false;
    //}
    if ($("#CustPhone1").val() == "") {
        $.ligerDialog.warn('联系电话不能为空！');
        $("#CustPhone1").focus();
        return false;
    }
    //if ($("#Custlinkman").val() == "") {
    //    $.ligerDialog.warn('联系人不能为空！');
    //    $("#Custlinkman").focus();
    //    return false;
    //}
    //if ($("#CustlinkPhone").val() == "") {
    //    $.ligerDialog.warn('联系电话不能为空！');
    //    $("#CustlinkPhone").focus();
    //    return false;
    //}
    if ($("#Province").val() == "选择省" || $("#Province").val() == "") {
        $.ligerDialog.warn('省不能为空！');
        $("#Province").focus();
        return false;
    }
    if ($("#City").val() == "选择市" || $("#City").val() == "") {
        $.ligerDialog.warn('市不能为空！');
        $("#City").focus();
        return false;
    }
    if ($("#jfProvince").val() == "选择省" || $("#jfProvince").val() == "") {
        $.ligerDialog.warn('省不能为空！');
        $("#jfProvince").focus();
        return false;
    }
    if ($("#jfCity").val() == "选择市" || $("#jfCity").val() == "") {
        $.ligerDialog.warn('市不能为空！');
        $("#jfCity").focus();
        return false;
    }
    //if ($("#District").val() == "") {
    //    $.ligerDialog.warn('区县不能为空！');
    //    $("#District").focus();
    //    return false;
    //}
    if ($("#Cust_BillMoney").val() == "") {
        $.ligerDialog.warn('计费金额不能为空！');
        $("#Cust_BillMoney").focus();
        return false;
    }
    if ($("#Cust_BillNumber").val() == "") {
        $.ligerDialog.warn('计费号码不能为空！');
        $("#Cust_BillNumber").focus();
        return false;
    }
    if ($("#Cust_KFVoice").val() == "") {
        $.ligerDialog.warn('录音编码不能为空！');
        $("#Cust_KFVoice").focus();
        return false;
    }

    if (fromPage == 'CustomerReturns_A') {
        if ($("#Cust_WH_UserName").val() == "") {
            $.ligerDialog.warn('外呼人员不能为空！');
            $("#Cust_WH_UserName").focus();
            return false;
        }
        if ($("#BH_Remark").val() == "") {
            $.ligerDialog.warn('修改原因不能为空！');
            $("#BH_Remark").focus();
            return false;
        }
        if ($("#BH_Remark").val().length > 500) {
            $.ligerDialog.warn('退回原因 最大长度不能超过500！');
            $("#BH_Remark").focus();
            return false;
        }
    }
    else {
        if ($("#Cust_WH_Remark").val().length > 500) {
            $.ligerDialog.warn('备注 最大长度不能超过500！');
            $("#Cust_WH_Remark").focus();
            return false;
        }
    }
    return true;
}

function GetMapApiSearch() {
    if ($("#MapSearchStr").val().length < 1) {
        $("#MapSearchStr").val($("#CustName").val());
    }
    var arr = new Array();
    arr[0] = $("#MapSearchStr").val();
    arr[1] = CityName;
    var Data = eval("(" + ReturnValue("BLL_CallCenterDts", "GetSearchForBaiduAPI", arr, Address) + ")");
    var HtmlStr = "";
    if (Data.results.length > 0) {
        if (Data.results[0].name != "") {
            HtmlStr += "<table style=\"width: 100%; overflow: scroll;\" id=\"tab\">"
            for (var i = 0; i < Data.results.length; i++) {
                HtmlStr += "<tr>";
                HtmlStr += "<td style=\" border-bottom: 1px solid #999;\"><p>" + Data.results[i].name + "</p><p>" + Data.results[i].address + "</p></td>";
                HtmlStr += "</tr>";
            }
            HtmlStr += "</table>";
        } else {
            HtmlStr += "未查询到相关信息!";
            $("#MapSearchStr").val("");
            $("#MapAPISearch").html("")
            $(".div1").click(function () {
                $("#tc").attr("style", "display:none");
                $("#MapSearchStr").val("");
                $("#MapAPISearch").html("")
            })
        }
    } else {
        HtmlStr += "未查询到相关信息!";

        $(".div1").click(function () {
            $("#tc").attr("style", "display:none");
            $("#MapSearchStr").val("");
            $("#MapAPISearch").html("")
        })
    }
    $("#MapAPISearch").html(HtmlStr);
    $("#CustAddress").removeAttr("disabled");
    $(".gb").click(function () {
        $("#tc").attr("style", "display:none");
    })
    $("#tab tr td").click(function () {
        var text = $(this).children("p:eq(1)").text();
        $("#CustAddress").val(text);
        $("#MapSearchStr").val("");
        $("#MapAPISearch").html("");
        $("#tc").attr("style", "display:none");
    })
}