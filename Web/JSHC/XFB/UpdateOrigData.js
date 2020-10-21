var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var OrginData = "";;
var Code = GetQueryString("Code");

$(function () {
    InitData();
    InitControl();
});

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '修改', id: 'btnUpdate', icon: 'edit', click: UpdateDatas }
        ]
    });

    $("#Provider").html(unescape(OrginData[0].OD_Provider));
    $("#ProDate").html(unescape(OrginData[0].ProviderTime));

    GetArea1(OrginData[0].OD_ProvinceCode);
    GetArea2(OrginData[0].OD_ProvinceCode); 

    var strData = [{ 'text': '8' }];
    $("#Billing").ligerComboBox({
        data: strData,
        textField: "text",
        valueField: "text",
        valueFieldID: "value1",
        value: OrginData[0].OD_BillMoney,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
    });
}

function InitData() {
    var arr = new Array();
    arr[0] = Code;
    OrginData = eval(ReturnValue("BLL_OriginalData", "GetXFBOriginData", arr, Address));
}

//省下拉框
function GetArea1(PCode) {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择省\"}]")[0]);//追加全国选项
    }
    $("#Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        value: PCode,
        width: 135,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            $("#value6").val("");
            GetArea2(newvalue);
        }
    });
}

//市下拉框
function GetArea2(DataCode) {
    if (DataCode == "")
        $("#City").ligerComboBox({
            data: "",
            textField: "SA_Name",
            valueField: "SA_Code",
            valueFieldID: "value6",
            width: 135,
            cancelable: false,
            isMultiSelect: false
        });
    else {
        var arr = new Array();
        arr[0] = '1';
        arr[1] = DataCode;
        var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
        if (Data.length > 0) {
            Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"选择市\"}]")[0]);//追加全部选项
        }
        $("#City").ligerComboBox({
            data: Data,
            textField: "SA_Name",
            valueField: "SA_Code",
            valueFieldID: "value6",
            value: OrginData[0].OD_CityCode,
            width: 135,
            isMultiSelect: false,
            cancelable: false
        });
    }
}

function UpdateDatas() {
    if (validata()) {
        var arr = new Array();
        arr[0] = Code;
        arr[1] = $("#Billing").val();
        arr[2] = $("#Province").ligerComboBox().getValue();
        arr[3] = (arr[2] == "" ? "" : $("#City").ligerComboBox().getValue());
        ReturnMethod("BLL_OriginalData", "UpdateXFBDatas", arr, Address, "UpdateDatas");
    }
}

var CustomMethod = {
    UpdateDatas: function (text) {
        if (text == 'false') {
            $.ligerDialog.error('修改失败！');
        }
        else {
            $.ligerDialog.success('修改成功！', '', function () {
                parent.layer.closeAll('iframe');
            });
        }
    }
}

function validata() {
    //if ($("#City").ligerComboBox().getValue() == "") {
    //    $.ligerDialog.warn('请补填写完整信息！');
    //    $("#City").focus();
    //    return false;
    //}
    return true;
}