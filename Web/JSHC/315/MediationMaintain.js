var form;
var Address = "../../"; //当前的html与Aspx中PubForm的文件深度
var PageIndex = 1;
var PageNum = 20;
var MaxCount = 0;
var MaxPage = 0;
var Code = GetQueryString("Code");

$(function () {

    InitControl();
    InitData();

});

function InitControl() {

    $("#Inp_ComplaintTime").ligerDateEditor({ format: "yyyy-MM-dd", width: 200 });
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
                    { text: '保存', icon: 'save', click: SaveData },
                    { line: true },
                    { text: '退出', icon: 'logout', click: Logout }
        ]
    });
    $("#Inp_Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 200,
        cancelable: false,
        isMultiSelect: false
    });

    $("#Inp_City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 200,
        cancelable: false,
        isMultiSelect: false
    });

    $("#Inp_District").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value7",
        width: 200,
        cancelable: false,
        isMultiSelect: false
    });

}

//获取购买渠道
function GetComplaintchannel() {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = "Complaintchannel";
    arr[1] = "0";
    arr[2] = "0";
    var Data = eval(ReturnValue("BLL_Mediation", "GetParaType", arr, Address));

    $("#Inp_Complaintchannel").ligerComboBox({
        data: Data,
        textField: "CSet_Name",
        valueField: "CSet_Code",
        valueFieldID: "value_Complaintchannel",
        value: '',
        width: 200,
        isMultiSelect: false,
        cancelable: false,
    });
}

//获取投诉类型
function GetComplaintType() {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = "ComplaintType";
    arr[1] = "0";
    arr[2] = "0";
    var Data = eval(ReturnValue("BLL_Mediation", "GetParaType", arr, Address));
    
    $("#Inp_ComplaintType").ligerComboBox({
        data: Data,
        textField: "CSet_Name",
        valueField: "CSet_Code",
        valueFieldID: "value_ComplaintType",
        value: '',
        width: 200,
        cancelable: false,
        isMultiSelect: false,
        onSelected: function (newvalue) {
            $("#Inp_IndustryType").val("");
            $("#value_IndustryType").val("");
            if (newvalue != null && newvalue != "") {
                GetIndustryType(newvalue);
            } else {
                $("#Inp_IndustryType").ligerComboBox({
                    data: "",
                    textField: "CSet_Name",
                    valueField: "CSet_Code",
                    valueFieldID: "value_IndustryType",
                    width: 200,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

//获取类别
function GetIndustryType(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = DataCode;
    arr[1] = "1";
    arr[2] = "0";
    var Data = eval(ReturnValue("BLL_Mediation", "GetParaType", arr, Address));

    $("#Inp_IndustryType").ligerComboBox({
        data: Data,
        textField: "CSet_Name",
        valueField: "CSet_Code",
        valueFieldID: "value_IndustryType",
        value: '',
        width: 200,
        cancelable: false,
        isMultiSelect: false
    });
}

function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_Mediation", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择省份\"}]")[0]);//追加全国选项
    }
    $("#Inp_Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        value: Data[0].SA_Code,
        width: 160,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            PageIndex = 1;
            if (newvalue != null && newvalue != "") {

                $("#Inp_City").val("");
                $("#value6").val("");

                GetArea2(newvalue);
            } else {
                $("#Inp_City").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value6",
                    width: 160,
                    cancelable: false,
                    isMultiSelect: false
                });
                $("#Inp_District").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value7",
                    width: 160,
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
    var Data = eval(ReturnValue("BLL_Mediation", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择城市\"}]")[0]);//追加全部选项
    }
    $("#Inp_City").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data[0].SA_Code,
        width: 160,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            $("#Inp_District").val("");
            $("#value7").val("");
            if (newvalue != null && newvalue != "") {
                GetArea3(newvalue);
            } else {
                $("#Inp_District").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value7",
                    width: 160,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

function GetArea3(DataCode) {
    PageIndex = 1;
    var arr = new Array();
    arr[0] = '2';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_Mediation", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"请选择区县\"}]")[0]);//追加全部选项
    }
    $("#Inp_District").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value7",
        value: Data[0].SA_Code,
        width: 160,
        cancelable: false,
        isMultiSelect: false
    });
}

function InitData() {
    GetComplaintchannel();
    GetComplaintType();
    GetArea1();
    GetData();
}

function GetData() {
    if ( Code != "") {
        var arr = new Array();
        arr[0] = " MD_Code='" + Code + "'";
        arr[1] = '';
        arr[2] = 1;
        arr[3] = 10;
        arr[4] = 0;
        var Data = JSON.parse(ReturnValue("BLL_Mediation", "GetMediation", arr, Address)).Rows;
        if (Data.length > 0) {
            $("#Inp_Complaintchannel").ligerComboBox({
                value: Data[0].MD_Complaintchannel_Value,
            });
            $("#Inp_ComplaintType").ligerComboBox({
                value: Data[0].MD_ComplaintType_Value,
            });
            $("#Inp_IndustryType").ligerComboBox({
                value: Data[0].MD_IndustryType_Value,
            });
            $("#Inp_Province").val(Data[0].MD_Province);
            $("#Inp_City").val(Data[0].MD_City);
            $("#Inp_District").val(Data[0].MD_District);
            $("#Inp_Title").val(Data[0].MD_Title);
            $("#Inp_ComplaintTime").val(Data[0].MD_ComplaintTime);
            $("#Inp_ProcessingUnit").val(Data[0].MD_ProcessingUnit);
            $("#Inp_CitationClause").val(Data[0].MD_CitationClause);
            $("#txtContent").val(unescape(Data[0].MD_CaseInfo));
            $("#txtProcessingProcedure").val(unescape(Data[0].MD_ProcessingProcedure));
            $("#txtCaseAnalysis").val(unescape(Data[0].MD_CaseAnalysis));
        }
    }
}

function SaveData() {
    if ($("#value_Complaintchannel").val() == '' || $("#Inp_Complaintchannel").val() == '') {
        $.ligerDialog.warn("投诉渠道不能为空!");
        return false;
    }
    if ($("#value_ComplaintType").val() == '' || $("#Inp_ComplaintType").val() == '') {
        $.ligerDialog.warn("投诉类型不能为空!");
        return false;
    }
    if ($("#Inp_ComplaintTime").val() == '') {
        $.ligerDialog.warn("投诉时间不能为空!");
        return false;
    }
    if ($("#value_IndustryType").val() == '' || $("#Inp_IndustryType").val() == '') {
        $.ligerDialog.warn("行业类型不能为空!");
        return false;
    }
    if ($("#Inp_ProcessingUnit").val() == '') {
        $.ligerDialog.warn("处理单位不能为空!");
        return false;
    }
   
    //if ($("#Inp_CitationClause").val() == '') {
    //    $.ligerDialog.warn("引用条款不能为空!");
    //    return false;
    //}
    var Inp_Title = $("#Inp_Title").val();
    if (Inp_Title.length > 50) {
        $.ligerDialog.warn("标题长度不能超过50个字!");
        return false;
    }
    if (Inp_Title.length == 0) {
        $.ligerDialog.warn("标题不能为空!");
        return false;
    }
    if (editor.getContent() == '')
    {
        $.ligerDialog.warn("案例内容不能为空!");
        return false;
    }
    if (editorProcessingProcedure.getContent() == '') {
        $.ligerDialog.warn("处理过程及结果不能为空!");
        return false;
    } 
    if (editorCaseAnalysis.getContent() == '') {
        $.ligerDialog.warn("案例评析不能为空!");
        return false;
    }
    if ($("#Inp_Province").val() == '') {
        $.ligerDialog.warn("省份不能为空!");
        return false;
    }
    var arr = new Array();
    arr[0] = Code;
    arr[1] = $("#value_Complaintchannel").val();
    arr[2] = $("#value_ComplaintType").val();
    arr[3] = $("#Inp_ComplaintTime").val();
    arr[4] = $("#value_IndustryType").val();
    arr[5] = $("#Inp_Province").val();
    arr[6] = $("#Inp_City").val();
    arr[7] = $("#Inp_District").val();
    arr[8] = $("#Inp_ProcessingUnit").val();
    arr[9] = $("#Inp_Title").val();
    arr[10] = $("#Inp_CitationClause").val();
    arr[11] = escape(editor.getContent());
    arr[12] = escape(editorProcessingProcedure.getContent());
    arr[13] = escape(editorCaseAnalysis.getContent());
    arr[14] = Code == "" ? 1 : 2;
    if (Code == "" || Code == "null" || Code == null) {
        ReturnMethod("BLL_Mediation", "OperateMediation", arr, Address, "InsertSuccess");
    } else {
        ReturnMethod("BLL_Mediation", "OperateMediation", arr, Address, "UpdateSuccess");
    }
}

function Logout() {
    parent.layer.closeAll('iframe');
}

//回调刷新列表页
function End() {
    window.close();
    var pWindow = window.dialogArguments; if (pWindow != null) {
        alert(parent);
        parent.pWindow.InitData();

    } else {
        window.opener.InitData();
    }
}

var CustomMethod = {
    InsertSuccess: function (text) {
        if (JSON.parse(text).Rows[0].msg == "1") {
            $.ligerDialog.success('添加成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        } else {
            $.ligerDialog.error("添加失败!");
        }
    },
    UpdateSuccess: function (text) {
        if (JSON.parse(text).Rows[0].msg == "1") {
            $.ligerDialog.success('修改成功!', '提示', function () {
                parent.layer.closeAll('iframe');
            });
        } else {
            $.ligerDialog.error("修改失败!");
        }
    }
}

function escapeHTML(str) {
    str = String(str).replace(/&/g, '&amp;').
      replace(/>/g, '&gt;').
      replace(/</g, '&lt;').
      replace(/"/g, '&quot;');
    return str;
}
function unescapeHTML(str) {
    str = String(str).replace(/&gt;/g, '>').
      replace(/&lt;/g, '<').
      replace(/&quot;/g, '"').
      replace(/&amp;/g, '&');
    return str;
}