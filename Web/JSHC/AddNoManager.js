var Address = "../";
var gridManager = null;
var toptoolbarManager = null;

$(function () {
    InitControl();
    GetArea1(); 
});

function InitControl() {
    toptoolbarManager = $("#toptoolbar").ligerToolBar({
        items: [
            { text: '添加', icon: 'add', click: Add },
            { line: true },
            {
                text: '退出', icon: 'logout', click: function (item) {
                    parent.layer.closeAll('iframe');
                }
            }
        ]
    }); 

    $("#Province").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value5",
        width: 148,
        cancelable: false,
        isMultiSelect: false
    });

    $("#City").ligerComboBox({
        data: "",
        textField: "LS_Name",
        valueField: "LS_Code",
        valueFieldID: "value6",
        width: 148,
        cancelable: false,
        isMultiSelect: false
    });

    $("#NmType").ligerComboBox({
        width: 153,
        data: [
                  { text: '移动', id: '1' },
                  { text: '电信', id: '2' },
                  { text: '联通', id: '3' }

        ]
    }); 
}

//省下拉框
function GetArea1() {
    var arr = new Array();
    arr[0] = '0';
    arr[1] = 'D0000001';
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全国\"}]")[0]);//追加全国选项
    }
    $("#Province").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value5",
        value: Data[0].SA_Code,
        width: 148,
        isMultiSelect: false,
        cancelable: false,
        onSelected: function (newvalue) {
            if (newvalue != null && newvalue != "") {
                $("#Inp_City").val("");
                $("#value6").val("");

                GetArea2(newvalue);
            } else {
                $("#City").ligerComboBox({
                    data: "",
                    textField: "SA_Name",
                    valueField: "SA_Code",
                    valueFieldID: "value6",
                    width: 148,
                    cancelable: false,
                    isMultiSelect: false
                });
            }
        }
    });
}

//市下拉框
function GetArea2(DataCode) {
    var arr = new Array();
    arr[0] = '1';
    arr[1] = DataCode;
    var Data = eval(ReturnValue("BLL_PublicInfo", "GetArea", arr, Address));
    if (Data.length > 0) {
        Data.splice(0, 0, eval("[{\"SA_Code\": \"\", \"SA_Name\": \"全部\"}]")[0]);//追加全部选项
    }
    $("#City").ligerComboBox({
        data: Data,
        textField: "SA_Name",
        valueField: "SA_Code",
        valueFieldID: "value6",
        value: Data[0].SA_Code,
        width: 148,
        isMultiSelect: false,
        cancelable: false
    });
}

// 添加
function Add() { 
    if ($("#NmPhone").val() == "") {
        layer.msg("请输入号段!");
        return;
    } else if ($("#City").val() == "" || $("#City").val() == "全部") {
         layer.msg("请完善城市信息!");
        return;
     } else if ($("#NmType").ligerComboBox().getText() == "") {
         layer.msg("请选择运营商!");
         return;
     } else {
         var arr = new Array();
         arr[0] = $("#NmPhone").val();
         arr[1] = $("#Province").ligerComboBox().getValue();
         arr[2] = $("#Province").ligerComboBox().getText();
         arr[3] = $("#City").ligerComboBox().getValue();
         arr[4] = $("#City").ligerComboBox().getText();
         arr[5] = $("#NmType").val();
         var result = ReturnValue("BLL_NoManager", "AddNoManager", arr, Address);
         if (result == "true") {
             layer.msg("添加成功!"); 
             setTimeout(function () {
                 parent.layer.closeAll('iframe');
             }, 2000);
         } else {
             layer.msg("添加失败!");
         }
     }
}